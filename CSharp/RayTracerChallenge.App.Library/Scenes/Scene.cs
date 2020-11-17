// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Scene.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Library.Scenes
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using RayTracerChallenge.App.Library.Extensions;
    using RayTracerChallenge.Library;
    using Color = RayTracerChallenge.Library.Color;

    /// <summary>
    /// Abstract base class for a scenes that can render to a <see cref="Canvas"/>.
    /// </summary>
    public abstract class Scene
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private IProgress<SceneRenderProgress>? _progress;
        private WriteableBitmap? _bitmap;

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
            int maxDegreeOfParallelism = -1,
            IProgress<SceneRenderProgress>? progress = null,
            CancellationToken cancellationToken = default)
        {
            _progress = progress;
            _bitmap = new WriteableBitmap(CanvasWidth, CanvasHeight, dpiX, dpiY, PixelFormats.Bgr24, null);

            try
            {
                // Render the scene.
                var renderProgress = new Progress<RenderProgressStep>(OnRenderProgress);
                var options = new CameraRenderOptions(maxDegreeOfParallelism, renderProgress, cancellationToken);
                Canvas canvas = await Task.Run(() => Render(options), cancellationToken);
                canvas.RenderToWriteableBitmap(_bitmap);
                ReportProgress(100);

                return _bitmap;
            }
            finally
            {
                _progress = null;
                _bitmap = null;
            }
        }

        protected abstract Canvas Render(CameraRenderOptions options);

        private void ReportProgress(int percentComplete)
        {
            if (_progress == null || _bitmap == null)
            {
                throw new InvalidOperationException("Shouldn't be reporting progress when nothing is rendering.");
            }

            _progress.Report(new SceneRenderProgress(percentComplete, _bitmap));
        }

        private void OnRenderProgress(RenderProgressStep progressStep)
        {
            if (_bitmap == null)
            {
                throw new InvalidOperationException("Shouldn't be reporting progress when nothing is rendering.");
            }

            UpdateBitmap(progressStep);
            ReportProgress(progressStep.PercentComplete);
        }

        /// <summary>
        /// Updates the bitmap with the cached updates.
        /// </summary>
        /// <param name="renderProgressStep">The update containing which pixels to set.</param>
        private void UpdateBitmap(RenderProgressStep renderProgressStep)
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

                // Get a pointer to the back buffer for the corresponding row.
                IntPtr backBuffer = _bitmap.BackBuffer + (renderProgressStep.Row * _bitmap.BackBufferStride);

                // Loop through each pixel in the render progress step and set the corresponding color in the bitmap.
                foreach (Color color in renderProgressStep.Pixels)
                {
                    // Convert the pixel's color to rgb format.
                    int rgb = color.ToRgb();

                    // Set the pixel in memory
                    unsafe
                    {
                        *(int*)backBuffer = rgb;
                    }

                    // Increment the buffer pointer
                    backBuffer += bytesPerPixel;
                }

                // Calculate the update rectangle for the bitmap.
                var rect = new Int32Rect(0, renderProgressStep.Row, _bitmap.PixelWidth, 1);

                // Tell the bitmap what changed.
                _bitmap.AddDirtyRect(rect);
            }
            finally
            {
                // Unlock the bitmap so that it can be displayed in the UI.
                _bitmap.Unlock();
            }
        }
    }
}
