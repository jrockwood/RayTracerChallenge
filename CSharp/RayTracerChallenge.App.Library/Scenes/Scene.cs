// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Scene.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Library.Scenes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using RayTracerChallenge.App.Library.Extensions;
    using RayTracerChallenge.App.Library.SystemServices;
    using RayTracerChallenge.Library;

    /// <summary>
    /// Abstract base class for a scenes that can render to a <see cref="Canvas"/>.
    /// </summary>
    public abstract class Scene
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private List<RenderProgressStep> _pendingUpdates = new List<RenderProgressStep>();
        private IProgress<SceneRenderProgress>? _progress;
        private WriteableBitmap? _bitmap;
        private int _totalPixelsRendered;
        private IDispatcherTimer? _updateTimer;

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        protected Scene(string title, string description, int canvasWidth, int canvasHeight)
        {
            Title = title;
            Description = description;
            CanvasWidth = canvasWidth;
            CanvasHeight = canvasHeight;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public string Title { get; }
        public string Description { get; }

        public int CanvasWidth { get; }
        public int CanvasHeight { get; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public async Task<BitmapSource> RenderAsync(
            double dpiX,
            double dpiY,
            IProgress<SceneRenderProgress>? progress = null,
            CancellationToken cancellationToken = default,
            Func<IDispatcherTimer>? createTimerFunc = null)
        {
            _progress = progress;
            _bitmap = new WriteableBitmap(CanvasWidth, CanvasHeight, dpiX, dpiY, PixelFormats.Bgr24, null);
            _totalPixelsRendered = 0;

            // Create and setup the timer that will be used for updating the bitmap and progress.
            _updateTimer = createTimerFunc?.Invoke() ?? new WrappedDispatcherTimer();
            _updateTimer.Interval = TimeSpan.FromMilliseconds(250);
            _updateTimer.Tick += OnUpdateTimerTick;
            _updateTimer.IsEnabled = true;

            try
            {
                // Render the scene.
                var renderProgress = new Progress<RenderProgressStep>(OnRenderProgress);
                Canvas canvas = await Task.Run(() => Render(renderProgress, cancellationToken), cancellationToken);

                // Turn off the timer.
                _updateTimer.IsEnabled = true;

                // Don't worry if we have pending updates at this point. We'll just do a complete rewrite of the bitmap.
                canvas.RenderToWriteableBitmap(_bitmap);
                ReportProgress();

                return _bitmap;
            }
            finally
            {
                // Clean up the timer.
                _updateTimer.IsEnabled = false;
                _updateTimer.Tick -= OnUpdateTimerTick;
                _updateTimer = null;

                // Clean up other state variables that we used during rendering.
                _pendingUpdates.Clear();
                _progress = null;
                _bitmap = null;
            }
        }

        protected abstract Canvas Render(IProgress<RenderProgressStep> progress, CancellationToken cancellationToken);

        private void ReportProgress()
        {
            if (_progress == null || _bitmap == null)
            {
                throw new InvalidOperationException("Shouldn't be reporting progress when nothing is rendering.");
            }

            // Calculate the percent complete.
            double totalPixels = CanvasWidth * CanvasHeight;
            int percentComplete = (int)Math.Round((_totalPixelsRendered / totalPixels) * 100.0);

            _progress.Report(new SceneRenderProgress(percentComplete, _bitmap));
        }

        private void OnRenderProgress(RenderProgressStep progressStep)
        {
            if (_bitmap == null)
            {
                throw new InvalidOperationException("Shouldn't be reporting progress when nothing is rendering.");
            }

            _pendingUpdates.Add(progressStep);
        }

        /// <summary>
        /// Called as part of a timer to process all of the pending updates to the bitmap.
        /// </summary>
        private void OnUpdateTimerTick(object? sender, EventArgs e)
        {
            if (_updateTimer == null)
            {
                throw new InvalidOperationException("Should not be processing updates when the timer is null.");
            }

            // Turn off the timer until we're done processing this batch.
            _updateTimer.IsEnabled = false;

            try
            {
                List<RenderProgressStep> updates = SwapPendingUpdatesList();
                if (updates.Count > 0)
                {
                    UpdateBitmap(updates);
                }
            }
            finally
            {
                // Turn the timer back on.
                _updateTimer.IsEnabled = true;
            }

            ReportProgress();
        }

        /// <summary>
        /// Make a copy of the update list and clear the existing one so that we don't have to worry about thread
        /// synchronization (adding to the update list while we're processing it).
        /// </summary>
        /// <returns>The update list to process.</returns>
        private List<RenderProgressStep> SwapPendingUpdatesList()
        {
            List<RenderProgressStep> current = _pendingUpdates;
            var empty = new List<RenderProgressStep>();

            // Make the first attempt to swap the current update list with an empty one.
            if (Interlocked.CompareExchange(ref _pendingUpdates, empty, current) != current)
            {
                // Use a lightweight spin wait to keep trying until we get the list.
                var spinWait = new SpinWait();

                do
                {
                    spinWait.SpinOnce();
                    current = _pendingUpdates;
                } while (Interlocked.CompareExchange(ref _pendingUpdates, empty, current) != current);
            }

            return current;
        }

        /// <summary>
        /// Updates the bitmap with the cached updates.
        /// </summary>
        /// <param name="updates">The updates containing which pixels to set.</param>
        private void UpdateBitmap(IReadOnlyCollection<RenderProgressStep> updates)
        {
            if (_bitmap == null)
            {
                throw new InvalidOperationException("Should not be processing updates when the bitmap is null.");
            }

            // Lock the bitmap so that we can do all of the updates at once.
            _bitmap.Lock();

            try
            {
                int bytesPerPixel = _bitmap.Format.BitsPerPixel / 8;

                // Get a pointer to the back buffer.
                IntPtr backBuffer = _bitmap.BackBuffer;

                // Loop through the updates and set each pixel's color.
                foreach (RenderProgressStep renderProgress in updates)
                {
                    // Convert the pixel's color to rgb format.
                    int rgb = renderProgress.PixelColor.ToRgb();

                    // Get the memory location of the pixel.
                    IntPtr p = backBuffer +
                               (renderProgress.PixelY * _bitmap.BackBufferStride) +
                               (renderProgress.PixelX * bytesPerPixel);

                    // Set the pixel in memory
                    unsafe
                    {
                        *(int*)p = rgb;
                    }
                }

                // Calculate the update rectangle for the bitmap.
                int minX = updates.Min(p => p.PixelX);
                int maxX = updates.Max(p => p.PixelX);
                int minY = updates.Min(p => p.PixelY);
                int maxY = updates.Max(p => p.PixelY);
                int width = (maxX - minX) + 1;
                int height = (maxY - minY) + 1;
                var rect = new Int32Rect(minX, minY, width, height);

                // Tell the bitmap what changed.
                _bitmap.AddDirtyRect(rect);
            }
            finally
            {
                // Unlock the bitmap so that it can be displayed in the UI.
                _bitmap.Unlock();
            }

            // Update the number of pixels processed.
            Interlocked.Add(ref _totalPixelsRendered, updates.Count);
        }
    }
}
