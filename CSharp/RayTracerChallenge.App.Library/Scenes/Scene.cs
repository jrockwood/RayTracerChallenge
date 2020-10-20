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
    using RayTracerChallenge.Library;

    /// <summary>
    /// Abstract base class for a scenes that can render to a <see cref="Canvas"/>. Inherit from <see
    /// cref="SimpleScene"/> if you don't use a <see cref="World"/> or <see cref="Camera"/> and rendering should take
    /// less than 1-2 seconds. Inherit from <see cref="ComplexScene"/> for all other cases.
    /// </summary>
    public abstract class Scene
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private int _highestPercentageReached;
        private IProgress<SceneRenderProgress>? _progress;
        private CancellationToken? _cancellationToken;

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

        protected Canvas? CurrentlyRenderingCanvas { get; private set; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public async Task<Canvas> RenderAsync(
            IProgress<SceneRenderProgress>? progress = null,
            CancellationToken cancellationToken = default)
        {
            _progress = progress;
            _cancellationToken = cancellationToken;

            try
            {
                var canvas = new Canvas(CanvasWidth, CanvasHeight);
                CurrentlyRenderingCanvas = canvas;
                await Task.Run(() => RenderToCanvas(canvas, cancellationToken), cancellationToken);
                ReportProgress(100);

                return canvas;
            }
            finally
            {
                _progress = null;
                _cancellationToken = null;
                CurrentlyRenderingCanvas = null;
            }
        }

        protected abstract void RenderToCanvas(Canvas canvas, CancellationToken cancellationToken = default);

        protected void ReportProgress(int percentComplete)
        {
            if (percentComplete <= _highestPercentageReached)
            {
                return;
            }

            _highestPercentageReached = percentComplete;

            if (CurrentlyRenderingCanvas == null)
            {
                throw new InvalidOperationException("Should not be reporting progress when there is no current canvas");
            }

            _progress?.Report(new SceneRenderProgress(percentComplete, CurrentlyRenderingCanvas));
        }

        /// <summary>
        /// Reports percent complete to the background worker so that it can change the UI.
        /// </summary>
        /// <param name="x">The x coordinate of the pixel that was just rendered.</param>
        /// <param name="y">The y coordinate of the pixel that was just rendered.</param>
        protected void ReportProgress(int x, int y)
        {
            if (CurrentlyRenderingCanvas == null)
            {
                return;
            }

            double totalPixels = CurrentlyRenderingCanvas.Width * CurrentlyRenderingCanvas.Height;
            double percentComplete = ((y * CurrentlyRenderingCanvas.Width) + x) / totalPixels;
            percentComplete *= 100;

            ReportProgress((int)percentComplete);
        }
    }
}
