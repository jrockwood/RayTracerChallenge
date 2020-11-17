// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Chapter07SixSpheres.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Library.Scenes
{
    using System;
    using RayTracerChallenge.Library;
    using RayTracerChallenge.Library.Lights;
    using RayTracerChallenge.Library.Shapes;

    public class Chapter07SixSpheres : Scene
    {
        public Chapter07SixSpheres()
            : base(
                "Chapter 7 - Six Spheres",
                "Renders three flattened spheres as walls and a floor and three additional spheres of different sizes. Tests the camera and world.",
                800,
                400)
        {
        }

        protected override Canvas Render(CameraRenderOptions options)
        {
            var floor = new Sphere(
                Matrix4x4.CreateScaling(10, 0.01, 10),
                new Material(new Color(1, 0.9, 0.9)).WithSpecular(0));

            var leftWall = new Sphere(
                Matrix4x4.CreateScaling(10, 0.01, 10)
                    .RotateX(Math.PI / 2)
                    .RotateY(-Math.PI / 4)
                    .Translate(0, 0, 5),
                floor.Material

            );

            var rightWall = new Sphere(
                Matrix4x4.CreateScaling(10, 0.01, 10)
                    .RotateX(Math.PI / 2)
                    .RotateY(Math.PI / 4)
                    .Translate(0, 0, 5),
                floor.Material

            );

            var middle = new Sphere(
                Matrix4x4.CreateTranslation(-0.5, 1, 0.5),
                new Material(new Color(0.1, 1, 0.5), diffuse: 0.7, specular: 0.3)

            );

            var right = new Sphere(
                Matrix4x4.CreateScaling(0.5, 0.5, 0.5).Translate(1.5, 0.5, -0.5),
                new Material(new Color(0.5, 1, 0.1), diffuse: 0.7, specular: 0.3)

            );

            var left = new Sphere(
                Matrix4x4.CreateScaling(0.33, 0.33, 0.33).Translate(-1.5, 0.33, -0.75),
                new Material(new Color(1, 0.8, 0.1), diffuse: 0.7, specular: 0.3)

            );

            var light = new PointLight(new Point(-10, 10, -10), Colors.White);
            var world = new World(light, floor, leftWall, rightWall, middle, right, left);

            var cameraTransform = Matrix4x4.CreateLookAt(new Point(0, 1.5, -5), new Point(0, 1, 0), Vector.UnitY);
            var camera = new Camera(CanvasWidth, CanvasHeight, Math.PI / 3, cameraTransform);

            Canvas canvas = camera.Render(world, options);
            return canvas;
        }
    }
}
