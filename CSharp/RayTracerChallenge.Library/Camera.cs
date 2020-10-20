// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Camera.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library
{
    using System;

    public class Camera
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private readonly double _halfWidth;
        private readonly double _halfHeight;

        private int _renderPercentComplete;
        private Canvas? _currentRenderingCanvas;

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public Camera(int canvasWidth, int canvasHeight, double fieldOfView, Matrix4x4? transform = null)
        {
            CanvasWidth = canvasWidth;
            CanvasHeight = canvasHeight;
            FieldOfView = fieldOfView;
            Transform = transform ?? Matrix4x4.Identity;

            // Calculate the pixel size and store some other calculations used in RayForPixel.
            double halfView = Math.Tan(fieldOfView / 2);
            double aspect = (double)canvasWidth / canvasHeight;

            if (aspect >= 1)
            {
                _halfWidth = halfView;
                _halfHeight = halfView / aspect;
            }
            else
            {
                _halfWidth = halfView * aspect;
                _halfHeight = halfView;
            }

            PixelSize = (_halfWidth * 2) / canvasWidth;
        }

        //// ===========================================================================================================
        //// Events
        //// ===========================================================================================================

        public event EventHandler<RenderPercentCompleteEventArgs>? RenderPercentCompleteChanged;

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        /// <summary>
        /// Gets the horizontal size (in pixels) of the canvas that the picture will be rendered to.
        /// </summary>
        public int CanvasWidth { get; }

        /// <summary>
        /// Gets the vertical size (in pixels) of the canvas that the picture will be rendered to.
        /// </summary>
        public int CanvasHeight { get; }

        /// <summary>
        /// Gets an angle the describes how much the camera can see. When the field of view is small, the view will be
        /// "zoomed in," magnifying a smaller area of the scene.
        /// </summary>
        public double FieldOfView { get; }

        /// <summary>
        /// Gets a matrix describing how the world should be oriented relative to the camera.
        /// </summary>
        public Matrix4x4 Transform { get; }

        /// <summary>
        /// Gets the size in world-space units of the pixels on the canvas.
        /// </summary>
        public double PixelSize { get; }

        /// <summary>
        /// Gets the current render percent complete.
        /// </summary>
        public int RenderPercentComplete
        {
            get => _renderPercentComplete;
            set
            {
                if (_renderPercentComplete == value)
                {
                    return;
                }

                if (_currentRenderingCanvas != null)
                {
                    RenderPercentCompleteChanged?.Invoke(
                        this,
                        new RenderPercentCompleteEventArgs(value, _currentRenderingCanvas));
                }

                _renderPercentComplete = value;
            }
        }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        /// <summary>
        /// Returns a new ray that starts at the camera and passes through the indicated pixel on the canvas.
        /// </summary>
        /// <param name="x">The canvas x position.</param>
        /// <param name="y">The canvas y position.</param>
        public Ray RayForPixel(int x, int y)
        {
            // The offset from the edge of the canvas to the pixel's center.
            double offsetX = (x + 0.5) * PixelSize;
            double offsetY = (y + 0.5) * PixelSize;

            // The untransformed coordinates of the pixel in world space. (Remember that the camera looks
            // towards -z, so +x is to the *left*).
            double worldX = _halfWidth - offsetX;
            double worldY = _halfHeight - offsetY;

            // Using the camera matrix, transform the canvas point and the origin, and then compute the
            // ray's direction vector. (Remember that the canvas is at z=-1).
            Point pixel = Transform.Invert().Multiply(new Point(worldX, worldY, -1));
            Point origin = Transform.Invert().Multiply(new Point(0, 0, 0));
            Vector direction = pixel.Subtract(origin).Normalize();

            return new Ray(origin, direction);
        }

        /// <summary>
        /// Uses the camera to render an image of the specified world.
        /// </summary>
        /// <param name="world">The world to render.</param>
        /// <param name="shouldCancelFunc">
        /// Optional function that is called during the rendering loop to see if the rendering should be cancelled.
        /// </param>
        /// <returns>A <see cref="Canvas"/> containing the rendered image.</returns>
        public Canvas Render(World world, Func<bool>? shouldCancelFunc = null)
        {
            var canvas = new Canvas(CanvasWidth, CanvasHeight);
            RenderToCanvas(canvas, world, shouldCancelFunc);
            return canvas;
        }

        /// <summary>
        /// Uses the camera to render an image of the specified world.
        /// </summary>
        /// <param name="canvas">
        /// The canvas to render to. It should be the same size as <see cref="CanvasWidth"/> and <see cref="CanvasHeight"/>
        /// </param>
        /// <param name="world">The world to render.</param>
        /// <param name="shouldCancelFunc">
        /// Optional function that is called during the rendering loop to see if the rendering should be cancelled.
        /// </param>
        /// <returns>A <see cref="Canvas"/> containing the rendered image.</returns>
        public void RenderToCanvas(Canvas canvas, World world, Func<bool>? shouldCancelFunc = null)
        {
            // Make sure the canvas is the same size as this camera.
            if (canvas.Width != CanvasWidth || canvas.Height != CanvasHeight)
            {
                throw new ArgumentException("Canvas must be the same size as the camera", nameof(canvas));
            }

            // Cache the canvas so that it can be passed in the RenderPercentCompleteChanged event handler
            _currentRenderingCanvas = canvas;

            int totalPixels = CanvasWidth * CanvasHeight;

            for (int y = 0; y < CanvasHeight; y++)
            {
                // Check for cancellation.
                if (shouldCancelFunc?.Invoke() == true)
                {
                    break;
                }

                for (int x = 0; x < CanvasWidth; x++)
                {
                    // Check for cancellation.
                    if (shouldCancelFunc?.Invoke() == true)
                    {
                        break;
                    }

                    Ray ray = RayForPixel(x, y);
                    Color color = world.ColorAt(ray);
                    canvas.SetPixel(x, y, color);

                    // Report the progress.
                    RenderPercentComplete = (int)((((y * CanvasWidth) + x) / (double)totalPixels) * 100.0);
                }
            }

            // Report that we're done.
            RenderPercentComplete = 100;

            // Clear the cached canvas.
            _currentRenderingCanvas = null;
        }
    }
}
