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
            var floor = new Plane(
                Matrix4x4.CreateRotationY(Math.PI / 4),
                new Material(pattern: new CheckerPattern(Colors.White, Colors.Black)).WithSpecular(0));

            var middle = new Sphere(
                Matrix4x4.CreateTranslation(-0.5, 1, 0.5),
                new Material(
                    pattern: new StripePattern(
                        new Color(0.8, 0.8, 1),
                        Colors.Blue,
                        Matrix4x4.CreateScaling(0.3, 0.3, 0.3)
                            .RotateZ(-Math.PI / 4)
                            .RotateY(-Math.PI / 6)
                            .Translate(0.4, 0, 0)),
                    diffuse: 0.7,
                    specular: 0.3));

            var right = new Sphere(
                Matrix4x4.CreateScaling(0.5, 0.5, 0.5).Translate(1.5, 0.5, -0.5),
                new Material(
                    pattern: new GradientPattern(
                        Colors.Red,
                        Colors.Yellow,
                        Matrix4x4.CreateRotationZ(-Math.PI / 4).RotateY(-Math.PI / 4)),
                    diffuse: 0.7,
                    specular: 0.3));

            var left = new Sphere(
                Matrix4x4.CreateScaling(0.33, 0.33, 0.33).Translate(-1.5, 0.33, -0.75),
                new Material(
                    pattern: new RingPattern(
                        new Color(0.8, 1, 0.8),
                        Colors.Green,
                        Matrix4x4.CreateRotationZ(Math.PI)),
                    diffuse: 0.7,
                    specular: 0.3));

            var light = new PointLight(new Point(-10, 10, -10), Colors.White);
            var world = new World(light, floor, middle, right, left);

            var cameraTransform = Matrix4x4.CreateLookAt(new Point(0, 1.5, -5), new Point(0, 1, 0), Vector.UnitY);
            var camera = new Camera(CanvasWidth, CanvasHeight, Math.PI / 3, cameraTransform);

            Canvas canvas = camera.Render(world, progress, cancellationToken);
            return canvas;
        }
    }
}
