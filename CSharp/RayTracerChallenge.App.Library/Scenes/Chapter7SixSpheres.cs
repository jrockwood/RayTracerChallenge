// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Chapter7SixSpheres.cs" company="Justin Rockwood">
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

    public class Chapter7SixSpheres : ComplexScene
    {
        public Chapter7SixSpheres()
            : base(
                "Chapter 7 - Six Spheres",
                "Renders three flattened spheres as walls and a floor and three additional spheres of different sizes. Tests the camera and world.",
                400,
                200)
        {
        }

        protected override void CreateScene(out World world, out Camera camera)
        {
            var floor = new Sphere(
                Matrix4x4.CreateScaling(10, 0.01f, 10),
                new Material(new Color(1, 0.9f, 0.9f)).WithSpecular(0));

            var leftWall = new Sphere(
                Matrix4x4.CreateScaling(10, 0.01f, 10)
                    .RotateX(MathF.PI / 2)
                    .RotateY(-MathF.PI / 4)
                    .Translate(0, 0, 5),
                floor.Material

            );

            var rightWall = new Sphere(
                Matrix4x4.CreateScaling(10, 0.01f, 10)
                    .RotateX(MathF.PI / 2)
                    .RotateY(MathF.PI / 4)
                    .Translate(0, 0, 5),
                floor.Material

            );

            var middle = new Sphere(
                Matrix4x4.CreateTranslation(-0.5f, 1, 0.5f),
                new Material(new Color(0.1f, 1, 0.5f), diffuse: 0.7f, specular: 0.3f)

            );

            var right = new Sphere(
                Matrix4x4.CreateScaling(0.5f, 0.5f, 0.5f).Translate(1.5f, 0.5f, -0.5f),
                new Material(new Color(0.5f, 1, 0.1f), diffuse: 0.7f, specular: 0.3f)

            );

            var left = new Sphere(
                Matrix4x4.CreateScaling(0.33f, 0.33f, 0.33f).Translate(-1.5f, 0.33f, -0.75f),
                new Material(new Color(1, 0.8f, 0.1f), diffuse: 0.7f, specular: 0.3f)

            );

            var light = new PointLight(new Point(-10, 10, -10), Colors.White);
            world = new World(light, floor, leftWall, rightWall, middle, right, left);

            var cameraTransform = Matrix4x4.CreateLookAt(new Point(0, 1.5f, -5), new Point(0, 1, 0), Vector.UnitY);
            camera = new Camera(CanvasWidth, CanvasHeight, MathF.PI / 3, cameraTransform);
        }
    }
}
