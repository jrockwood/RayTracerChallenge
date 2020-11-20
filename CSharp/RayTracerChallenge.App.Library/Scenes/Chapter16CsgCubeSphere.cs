// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ClassNamePlaceholder.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Library.Scenes
{
    using System;
    using RayTracerChallenge.Library;
    using RayTracerChallenge.Library.Lights;
    using RayTracerChallenge.Library.Patterns;
    using RayTracerChallenge.Library.Shapes;

    public class Chapter16CsgCubeSphere : Scene
    {
        public Chapter16CsgCubeSphere()
            : base(
                "Chapter 16 - CSG Cube and Sphere",
                "Renders a CSG (Constructive Solid Geometry) shape consisting of a cube and sphere, with union, intersection, and difference.",
                600,
                400)
        {
        }

        protected override Canvas Render(CameraRenderOptions options)
        {
            // Shapes
            var floorCeiling = new Cube(
                "FloorCeiling",
                Matrix4x4.CreateTranslation(0, 1, 0).Scale(20, 7, 20),
                new Material(
                    pattern: new CheckerPattern(
                        Colors.Black,
                        Colors.DarkGray,
                        Matrix4x4.CreateScaling(0.07, 0.07, 0.07)),
                    ambient: 0.25,
                    diffuse: 0.7,
                    specular: 0.9,
                    shininess: 300,
                    reflective: 0.1));

            var walls = new Cube(
                "Walls",
                Matrix4x4.CreateScaling(10, 10, 10),
                new Material(
                    pattern: new CheckerPattern(
                        new Color(0.4863, 0.3765, 0.2941),
                        new Color(0.3725, 0.2902, 0.2275),
                        Matrix4x4.CreateScaling(0.05, 20, 0.05)),
                    ambient: 0.1,
                    diffuse: 0.7,
                    specular: 0.9,
                    shininess: 300,
                    reflective: 0.1));

            var csgUnion = new CsgShape(
                "Union",
                CsgOperation.Union,
                new Cube(
                    "YellowCube",
                    Matrix4x4.CreateTranslation(0, 1.5, 0),
                    new Material(Colors.Yellow, diffuse: 0.7, specular: 0.3)),
                new Sphere(
                    "RedSphere",
                    Matrix4x4.CreateTranslation(0.6, 2, -0.6),
                    new Material(Colors.Red, diffuse: 0.7, specular: 0.9)),
                transform: Matrix4x4.CreateRotationY(-Math.PI / 3).Translate(-4, 0, -2));

            var csgIntersection = new CsgShape(
                "Intersection",
                CsgOperation.Intersection,
                new Cube(
                    "YellowCube",
                    Matrix4x4.CreateTranslation(0, 1.5, 0),
                    new Material(Colors.Yellow, diffuse: 0.7, specular: 0.3)),
                new Sphere(
                    "RedSphere",
                    Matrix4x4.CreateTranslation(0.6, 2, -0.6),
                    new Material(Colors.Red, diffuse: 0.7, specular: 0.9)),
                transform: Matrix4x4.CreateRotationY(-Math.PI / 2).Translate(0, 0, -2));

            var csgDifference = new CsgShape(
                "Difference",
                CsgOperation.Difference,
                new Cube(
                    "YellowCube",
                    Matrix4x4.CreateTranslation(0, 1.5, 0),
                    new Material(Colors.Yellow, diffuse: 0.7, specular: 0.3)),
                new Sphere(
                    "RedSphere",
                    Matrix4x4.CreateTranslation(0.6, 2, -0.6),
                    new Material(Colors.Red, diffuse: 0.7, specular: 0)),
                transform: Matrix4x4.CreateRotationY(-Math.PI / 6).Translate(3, 0, 2));

            // Lights, camera, action.
            var light = new PointLight(new Point(8, 8, -5), Colors.White);
            var world = new World(light, floorCeiling, walls, csgUnion, csgIntersection, csgDifference);

            var cameraTransform = Matrix4x4.CreateLookAt(new Point(8, 6, -8), new Point(0, 3, 0), Vector.UnitY);
            var camera = new Camera(CanvasWidth, CanvasHeight, fieldOfView: 0.785, cameraTransform);

            Canvas canvas = camera.Render(world, options);
            return canvas;
        }
    }
}
