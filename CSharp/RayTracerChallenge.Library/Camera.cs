// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Camera.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class Camera
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private readonly double _halfWidth;
        private readonly double _halfHeight;

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
            Point pixel = Transform.Invert() * new Point(worldX, worldY, -1);
            Point origin = Transform.Invert() * new Point(0, 0, 0);
            Vector direction = (pixel - origin).Normalize();

            return new Ray(origin, direction);
        }

        /// <summary>
        /// Uses the camera to render an image of the specified world.
        /// </summary>
        /// <param name="world">The world to render.</param>
        /// <param name="progress">Provides a way to communicate progress.</param>
        /// <param name="cancellationToken">
        /// Optional <see cref="CancellationToken"/> within the rendering loop to see if the rendering should be cancelled.
        /// </param>
        /// <returns>A <see cref="Canvas"/> containing the rendered image.</returns>
        public Canvas Render(
            World world,
            IProgress<RenderProgressStep>? progress = null,
            CancellationToken cancellationToken = default)
        {
            int width = CanvasWidth;
            int height = CanvasHeight;
            int totalPixels = width * height;
            int totalRenderedPixels = 0;
            var canvas = new MutableCanvas(width, height);

            // Run the loop in parallel for each row.
            try
            {
                var parallelOptions = new ParallelOptions { CancellationToken = cancellationToken };
                Parallel.For(
                    0,
                    height,
                    parallelOptions,
                    (int y, ParallelLoopState state) =>
                    {
                        for (int x = 0; x < width; x++)
                        {
                            // Check for cancellation.
                            if (state.ShouldExitCurrentIteration)
                            {
                                return;
                            }

                            // Run the ray tracing algorithm to get the color of the pixel.
                            Ray ray = RayForPixel(x, y);
                            Color color = world.ColorAt(ray);

                            canvas.SetPixel(x, y, color);
                        }

                        if (progress == null)
                        {
                            return;
                        }

                        // Update the number of pixels processed so we can calculate the progress.
                        int renderedPixelCount = Interlocked.Add(ref totalRenderedPixels, width);

                        // Report the progress.
                        int percentComplete = (int)Math.Round((renderedPixelCount / (double)totalPixels) * 100.0);
                        var progressStep = new RenderProgressStep(percentComplete, y, canvas.GetRow(y));

                        progress.Report(progressStep);
                    });
            }
            catch (OperationCanceledException)
            {
                // Do nothing.
            }

            // Create a canvas from the pixel data.
            return canvas.ToImmutable();
        }
    }
}
