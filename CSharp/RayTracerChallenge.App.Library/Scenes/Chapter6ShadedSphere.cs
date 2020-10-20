// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Chapter6ShadedSphere.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Library.Scenes
{
    using RayTracerChallenge.Library;
    using RayTracerChallenge.Library.Lights;
    using RayTracerChallenge.Library.Shapes;

    public class Chapter6ShadedSphere : SimpleScene
    {
        public Chapter6ShadedSphere()
            : base("Chapter 6 - Shaded Sphere", "Renders a shaded sphere. Tests materials and lighting.", 400, 400)
        {
        }

        protected override void RenderToCanvas(Canvas canvas)
        {
            var sphere = new Sphere(material: new Material(new Color(1, 0.2, 1)));
            var lightPosition = new Point(-10, 10, -10);
            var lightColor = Colors.White;
            var light = new PointLight(lightPosition, lightColor);

            var rayOrigin = new Point(0, 0, -5);
            const int wallZ = 10;
            const double wallSize = 7.0;

            double pixelSize = wallSize / canvas.Width;
            const double halfWallSize = wallSize / 2;

            // For each row of pixels in the canvas...
            for (int y = 0; y < canvas.Height; y++)
            {
                // Compute the world y coordinate (top = +half, bottom = -half).
                double worldY = halfWallSize - (pixelSize * y);

                // For each pixel in the row...
                for (int x = 0; x < canvas.Width; x++)
                {
                    // See if we should stop.
                    if (ShouldCancel)
                    {
                        return;
                    }

                    // Compute the world x coordinate (left = -half, right = +half).
                    double worldX = -halfWallSize + (pixelSize * x);

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
                        Color color = hit.Shape.Material.CalculateLighting(light, point, eye, normal, false);
                        canvas.SetPixel(x, y, color);
                    }

                    // Report the progress.
                    ReportProgress(x, y);
                }
            }
        }
    }
}
