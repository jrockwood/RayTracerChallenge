// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ComplexScene.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Library.Scenes
{
    using RayTracerChallenge.Library;

    /// <summary>
    /// Abstract base class for a scene that uses a <see cref="World"/> and <see cref="Camera"/> and that could take
    /// several seconds, minutes, or hours to render.
    /// </summary>
    public abstract class ComplexScene : Scene
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private World? _world;
        private Camera? _camera;

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        protected ComplexScene(string title, string description, int canvasWidth, int canvasHeight)
            : base(title, description, canvasWidth, canvasHeight)
        {
        }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        protected abstract void CreateScene(out World world, out Camera camera);

        protected override void RenderToCanvas(Canvas canvas)
        {
            // Create the scene if necessary.
            if (_world == null || _camera == null)
            {
                CreateScene(out _world, out _camera);
            }

            // Render
            _camera.RenderPercentCompleteChanged += CameraOnRenderPercentCompleteChanged;
            try
            {
                _camera.RenderToCanvas(canvas, _world, shouldCancelFunc: () => Worker?.CancellationPending ?? false);
            }
            finally
            {
                _camera.RenderPercentCompleteChanged -= CameraOnRenderPercentCompleteChanged;
            }
        }

        private void CameraOnRenderPercentCompleteChanged(object? sender, RenderPercentCompleteEventArgs e)
        {
            ReportProgress(e.PercentComplete);
        }
    }
}
