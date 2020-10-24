// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Chapter10Patterns.cs" company="Justin Rockwood">
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

    public class Chapter10Patterns : Scene
    {
        public Chapter10Patterns()
            : base(
                "Chapter 10 - Patterns",
                "Renders three spheres on a plane with different patterns. Tests the Striped, Gradient, Ring, and Checker patterns.",
                400,
                200)
        {
        }

        protected override Canvas Render(IProgress<RenderProgressStep> progress, CancellationToken cancellationToken)
        {
            // Patterns
            var checkerPattern = new CheckerPattern(Colors.White, Colors.Red);

            var stripePattern = new StripePattern(
                new GradientPattern(new Color(0.8, 0.8, 1), Colors.Blue),
                new GradientPattern(Colors.Blue, new Color(0.8, 0.8, 1)),
                Matrix4x4.CreateScaling(0.25, 0.25, 0.25)
                    .RotateZ(-Math.PI / 4)
                    .RotateY(-Math.PI / 6)
                    .Translate(0.4, 0, 0));

            var gradientPattern = new RadialGradientPattern(
                Colors.Green,
                Colors.Yellow,
                Matrix4x4.Identity);

            var ringPattern = new RingPattern(
                Colors.Red,
                Colors.White,
                Matrix4x4.CreateRotationZ(-Math.PI / 2).RotateY(-Math.PI / 2));

            // Shapes
            var floor = new Plane(
                Matrix4x4.CreateRotationY(Math.PI / 4),
                new Material(pattern: gradientPattern, specular: 0));

            var left1 = new Sphere(
                Matrix4x4.CreateScaling(0.33, 0.33, 0.33).Translate(-2, 0.33, -0.75),
                new Material(pattern: ringPattern, diffuse: 0.7, specular: 0.3));

            var left2 = new Sphere(
                Matrix4x4.CreateScaling(0.4, 0.4, 0.4).Translate(-1, 0.4, -0.75),
                new Material(pattern: checkerPattern, diffuse: 0.7, specular: 0.3));

            var middle = new Sphere(
                Matrix4x4.CreateTranslation(-0.5, 1, 0.5),
                new Material(pattern: stripePattern, diffuse: 0.7, specular: 0.3));

            var right = new Sphere(
                Matrix4x4.CreateScaling(0.5, 0.5, 0.5).Translate(1.5, 0.5, -0.5),
                new Material(pattern: gradientPattern, diffuse: 0.7, specular: 0.3));

            var light = new PointLight(new Point(-10, 10, -10), Colors.White);
            var world = new World(light, floor, left1, left2, middle, right);

            var cameraTransform = Matrix4x4.CreateLookAt(new Point(0, 1.5, -5), new Point(0, 1, 0), Vector.UnitY);
            var camera = new Camera(CanvasWidth, CanvasHeight, Math.PI / 3, cameraTransform);

            Canvas canvas = camera.Render(world, progress, cancellationToken);
            return canvas;
        }
    }
}
