// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Chapter14Groups.cs" company="Justin Rockwood">
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
    using RayTracerChallenge.Library.Shapes;

    public class Chapter14Groups : Scene
    {
        public Chapter14Groups()
            : base(
                "Chapter 14 - Groups",
                "Renders three UFO-like groups.",
                600,
                200)
        {
        }

        protected override Canvas Render(IProgress<RenderProgressStep> progress, CancellationToken cancellationToken)
        {
            // Shapes
            var backdrop = new Plane(
                Matrix4x4.CreateRotationX(Math.PI / 2).Translate(0, 0, 100),
                new Material(Colors.White, ambient: 1, diffuse: 0, specular: 0));

            var ufo1 = CreateUfo()
                .ChangeTransform(Matrix4x4.CreateRotationY(0.1745).RotateX(0.4363).Translate(-2.8, 0, 0))
                .ChangeMaterial(
                    new Material(new Color(0.9, 0.2, 0.4), ambient: 0.2, diffuse: 0.8, specular: 0.7, shininess: 20));

            var ufo2 = CreateUfo()
                .ChangeTransform(Matrix4x4.CreateRotationY(0.1745))
                .ChangeMaterial(
                    new Material(new Color(0.2, 0.9, 0.6), ambient: 0.2, diffuse: 0.8, specular: 0.7, shininess: 20));

            var ufo3 = CreateUfo()
                .ChangeTransform(Matrix4x4.CreateRotationY(-0.1745).RotateX(-0.4363).Translate(2.8, 0, 0))
                .ChangeMaterial(
                    new Material(new Color(0.2, 0.3, 1), ambient: 0.2, diffuse: 0.8, specular: 0.7, shininess: 20));

            // Lights
            var light1 = new PointLight(new Point(10_000, 10_000, -10_000), Colors.DarkGray);
            var light2 = new PointLight(new Point(-10_000, 10_000, -10_000), Colors.DarkGray);
            var light3 = new PointLight(new Point(10_000, -10_000, -10_000), Colors.DarkGray);
            var light4 = new PointLight(new Point(-10_000, -10_000, -10_000), Colors.DarkGray);

            // Create the world.
            var world = new World(new[] { light1, light2, light3, light4 }, new[] { backdrop, ufo1, ufo2, ufo3 });

            // Create the camera.
            var cameraTransform = Matrix4x4.CreateLookAt(new Point(0, 0, -9), new Point(0, 0, 0), Vector.UnitY);
            var camera = new Camera(CanvasWidth, CanvasHeight, fieldOfView: 0.9, cameraTransform);

            Canvas canvas = camera.Render(world, progress, cancellationToken);
            return canvas;
        }

        private static Group CreateLeg(Matrix4x4 transform)
        {
            var end = new Sphere(Matrix4x4.CreateScaling(0.25, 0.25, 0.25).Translate(0, 0, -1));
            var edge = new Cylinder(
                minimumY: 0,
                maximumY: 1,
                transform: Matrix4x4.CreateScaling(0.25, 1, 0.25)
                    .RotateZ(-Math.PI / 2)
                    .RotateY(-Math.PI / 6)
                    .Translate(0, 0, -1));

            return new Group(transform, null, end, edge);
        }

        private static Group CreateCap()
        {
            var transform = Matrix4x4.CreateScaling(0.24606, 1.37002, 0.24606).RotateX(-0.7854);
            var group = new Group();

            for (int i = 0; i < 6; i++)
            {
                var cone = new Cone(minimumY: -1, maximumY: 0, transform: transform.RotateY(i * (Math.PI / 3)));
                group.AddChild(cone);
            }

            return group;
        }

        private static Group CreateUfo()
        {
            var group = new Group();

            for (int i = 0; i < 6; i++)
            {
                var leg = CreateLeg(Matrix4x4.CreateRotationY(i * (Math.PI / 3)));
                group.AddChild(leg);
            }

            var topCap = CreateCap().ChangeTransform(Matrix4x4.CreateTranslation(0, 1, 0));
            var bottomCap = CreateCap().ChangeTransform(Matrix4x4.CreateTranslation(0, 1, 0).RotateX(3.1416));
            group.AddChildren(topCap, bottomCap);

            return group;
        }
    }
}
