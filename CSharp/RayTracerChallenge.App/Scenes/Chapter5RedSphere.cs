﻿// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Chapter5RedSphere.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Scenes
{
    using System;
    using RayTracerChallenge.Library;
    using RayTracerChallenge.Library.Shapes;

    public class Chapter5RedSphere : Scene
    {
        public Chapter5RedSphere()
            : base("Chapter 5 - Red Sphere", "Renders a sphere without any lighting. Tests ray intersections with a sphere.")
        {
        }

        public override int RequestedWidth => 400;
        public override int RequestedHeight => 400;

        protected override void RenderToCanvas(Canvas canvas)
        {
            var color = Colors.Red;
            var sphere = new Sphere();

            var rayOrigin = new Point(0, 0, -5);
            const float wallZ = 10f;
            const float wallSize = 7.0f;

            float pixelSize = wallSize / canvas.Width;
            const float halfWallSize = wallSize / 2;

            // For each row of pixels in the canvas...
            for (int y = 0; y < canvas.Height; y++)
            {
                // Compute the world y coordinate (top = +half, bottom = -half).
                float worldY = halfWallSize - (pixelSize * y);

                // For each pixel in the row...
                for (int x = 0; x < canvas.Width; x++)
                {
                    // See if we should stop.
                    if (ShouldCancel)
                    {
                        return;
                    }

                    // Compute the world x coordinate (left = -half, right = +half).
                    float worldX = -halfWallSize + (pixelSize * x);

                    // Describe the point on the wall that the ray will target.
                    var position = new Point(worldX, worldY, wallZ);

                    // Cast the ray into the scene to see what it hits.
                    var ray = new Ray(rayOrigin, (position - rayOrigin).Normalize());
                    var intersections = sphere.Intersect(ray);

                    if (intersections.Hit != null)
                    {
                        canvas.SetPixel(x, y, color);
                    }

                    // Report the progress.
                    int percentComplete = (int)Math.Round(
                        (((y * canvas.Height) + x) / ((float)canvas.Width * canvas.Height)) * 100f);
                    ReportProgress(percentComplete);
                }
            }
        }
    }
}
