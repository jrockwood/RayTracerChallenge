// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Chapter10PerlinPattern.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Library.Scenes
{
    using System;
    using System.Threading;
    using RayTracerChallenge.Library;
    using RayTracerChallenge.Library.Lights;
    using RayTracerChallenge.Library.Patterns;
    using RayTracerChallenge.Library.Shapes;

    public class Chapter10PerlinPattern : Scene
    {
        public Chapter10PerlinPattern()
            : base(
                "Chapter 10 - Perlin Pattern",
                "Renders spheres on a plane using Perlin patterns.",
                800,
                400)
        {
        }

        protected override Canvas Render(IProgress<RenderProgressStep> progress, CancellationToken cancellationToken)
        {
            // Patterns
            var map1 = new ColorMap(new ColorMapEntry(0, Colors.White));
            var floorPattern = new PerlinPattern(map1, transform: Matrix4x4.CreateTranslation(-1000, 0, 0));

            // Shapes
            var floor = new Plane(
                Matrix4x4.CreateRotationY(Math.PI / 4),
                new Material(pattern: floorPattern, specular: 0));

            var left = new Sphere(
                Matrix4x4.CreateScaling(0.33, 0.33, 0.33).Translate(-2, 0.33, -0.75),
                new Material(pattern: floorPattern, diffuse: 0.7, specular: 0.3));

            var middle = new Sphere(
                Matrix4x4.CreateTranslation(-0.5, 1, 0.5),
                new Material(pattern: floorPattern, diffuse: 0.7, specular: 0.3));

            var right = new Sphere(
                Matrix4x4.CreateScaling(0.5, 0.5, 0.5).Translate(1.5, 0.5, -0.5),
                new Material(pattern: floorPattern, diffuse: 0.7, specular: 0.3));

            var light = new PointLight(new Point(-10, 10, -10), Colors.White);
            var world = new World(light, floor, left, middle, right);

            var cameraTransform = Matrix4x4.CreateLookAt(new Point(0, 1.5, -5), new Point(0, 1, 0), Vector.UnitY);
            var camera = new Camera(CanvasWidth, CanvasHeight, Math.PI / 3, cameraTransform);

            Canvas canvas = camera.Render(world, progress, cancellationToken);
            return canvas;
        }
    }
}
