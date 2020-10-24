// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Chapter5RedSphere.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Library.Scenes
{
    using System;
    using System.Threading;
    using RayTracerChallenge.Library;
    using RayTracerChallenge.Library.Shapes;

    public class Chapter5RedSphere : Scene
    {
        public Chapter5RedSphere()
            : base(
                "Chapter 5 - Red Sphere",
                "Renders a sphere without any lighting. Tests ray intersections with a sphere.",
                400,
                400)
        {
        }

        protected override Canvas Render(IProgress<RenderProgressStep> progress, CancellationToken cancellationToken)
        {
            var canvas = new MutableCanvas(CanvasWidth, CanvasHeight);

            var sphere = new Sphere();

            var rayOrigin = new Point(0, 0, -5);
            const double wallZ = 10;
            const double wallSize = 7.0;

            double pixelSize = wallSize / canvas.Width;
            const double halfWallSize = wallSize / 2;
            double totalPixels = canvas.Width * canvas.Height;

            // For each row of pixels in the canvas...
            for (int y = 0; y < canvas.Height; y++)
            {
                // Compute the world y coordinate (top = +half, bottom = -half).
                double worldY = halfWallSize - (pixelSize * y);

                // For each pixel in the row...
                for (int x = 0; x < canvas.Width; x++)
                {
                    // See if we should stop.
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return canvas.ToImmutable();
                    }

                    // Compute the world x coordinate (left = -half, right = +half).
                    double worldX = -halfWallSize + (pixelSize * x);

                    // Describe the point on the wall that the ray will target.
                    var position = new Point(worldX, worldY, wallZ);

                    // Cast the ray into the scene to see what it hits.
                    var ray = new Ray(rayOrigin, (position - rayOrigin).Normalize());
                    var intersections = sphere.Intersect(ray);

                    if (intersections.Hit != null)
                    {
                        canvas.SetPixel(x, y, Colors.Red);
                    }
                }

                // Report the progress.
                double pixelsRendered = (y * CanvasWidth) + CanvasWidth;
                int percentComplete = (int)Math.Round((pixelsRendered / totalPixels) * 100.0);
                progress.Report(new RenderProgressStep(percentComplete, y, canvas.GetRow(y)));
            }

            return canvas.ToImmutable();
        }
    }
}
