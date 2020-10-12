// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Chapter6ShadedSphere.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Scenes
{
    using System;
    using RayTracerChallenge.Library;
    using RayTracerChallenge.Library.Lights;
    using RayTracerChallenge.Library.Shapes;

    public class Chapter6ShadedSphere : Scene
    {
        public Chapter6ShadedSphere()
            : base("Chapter 6 - Shaded Sphere", "Renders a shaded sphere. Tests materials and lighting.")
        {
        }

        public override int RequestedWidth => 400;
        public override int RequestedHeight => 400;

        protected override void RenderToCanvas(Canvas canvas)
        {
            var sphere = new Sphere(material: new Material(new Color(1, 0.2f, 1)));
            var lightPosition = new Point(-10, 10, -10);
            var lightColor = Colors.White;
            var light = new PointLight(lightPosition, lightColor);

            var rayOrigin = new Point(0, 0, -5);
            const int wallZ = 10;
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
                    IntersectionList intersections = sphere.Intersect(ray);
                    Intersection? hit = intersections.Hit;

                    if (hit != null)
                    {
                        Point point = ray.PositionAt(hit.T);
                        Vector normal = hit.Shape.NormalAt(point);
                        Vector eye = ray.Direction.Negate();
                        Color color = hit.Shape.Material.CalculateLighting(light, point, eye, normal);
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
