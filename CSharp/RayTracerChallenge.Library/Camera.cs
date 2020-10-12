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

        private readonly float _halfWidth;
        private readonly float _halfHeight;

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public Camera(int canvasWidth, int canvasHeight, float fieldOfView, Matrix4x4? transform = null)
        {
            CanvasWidth = canvasWidth;
            CanvasHeight = canvasHeight;
            FieldOfView = fieldOfView;
            Transform = transform ?? Matrix4x4.Identity;

            // Calculate the pixel size and store some other calculations used in RayForPixel.
            float halfView = MathF.Tan(fieldOfView / 2);
            float aspect = (float)canvasWidth / canvasHeight;

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
        public float FieldOfView { get; }

        /// <summary>
        /// Gets a matrix describing how the world should be oriented relative to the camera.
        /// </summary>
        public Matrix4x4 Transform { get; }

        /// <summary>
        /// Gets the size in world-space units of the pixels on the canvas.
        /// </summary>
        public float PixelSize { get; }

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
            float offsetX = (x + 0.5f) * PixelSize;
            float offsetY = (y + 0.5f) * PixelSize;

            // The untransformed coordinates of the pixel in world space. (Remember that the camera looks
            // towards -z, so +x is to the *left*).
            float worldX = _halfWidth - offsetX;
            float worldY = _halfHeight - offsetY;

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
        /// <returns>A <see cref="Canvas"/> containing the rendered image.</returns>
        public Canvas Render(World world)
        {
            var canvas = new Canvas(CanvasWidth, CanvasHeight);

            for (int y = 0; y < CanvasHeight; y++)
            {
                for (int x = 0; x < CanvasWidth; x++)
                {
                    Ray ray = RayForPixel(x, y);
                    Color color = world.ColorAt(ray);
                    canvas.SetPixel(x, y, color);
                }
            }

            return canvas;
        }
    }
}
