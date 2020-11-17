// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Chapter15Triangles.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Library.Scenes
{
    using RayTracerChallenge.Library;
    using RayTracerChallenge.Library.Lights;
    using RayTracerChallenge.Library.Patterns;
    using RayTracerChallenge.Library.Shapes;

    public class Chapter15Triangles : Scene
    {
        public Chapter15Triangles()
            : base(
                "Chapter 15 - Triangles",
                "Renders a scene with a bunch of triangles.",
                800,
                600)
        {
        }

        protected override Canvas Render(CameraRenderOptions options)
        {
            // Floor
            var floor = new Plane(
                transform: Matrix4x4.CreateTranslation(0, -NumberExtensions.Epsilon, 0),
                material: new Material(
                    pattern: new CheckerPattern(
                        Colors.Gray,
                        Colors.DarkGray,
                        Matrix4x4.CreateScaling(0.25, 0.25, 0.25)),
                    ambient: 0.2,
                    diffuse: 0.9,
                    specular: 0));

            var pyramid = CreatePyramid();
            pyramid.Material = new Material(Colors.Yellow, ambient: 0.2, diffuse: 0.9, specular: 0);

            // Lights, camera, action.
            var light = new PointLight(new Point(1, 7, -5), Colors.White);
            var world = new World(light, floor, pyramid);

            var cameraTransform = Matrix4x4.CreateLookAt(new Point(0, 3.5, -9), new Point(0, 0.3, 0), Vector.UnitY);
            var camera = new Camera(CanvasWidth, CanvasHeight, fieldOfView: 0.314, cameraTransform);

            Canvas canvas = camera.Render(world, options);
            return canvas;
        }

        private static Group CreatePyramid()
        {
            var pyramid = new Group("Pyramid");
            var bottom = new Triangle(new Point(-0.5, 0, 0), new Point(0, 0, -1), new Point(0.5, 0, 0));
            var backSide = new Triangle(new Point(-0.5, 0, 0), new Point(0, 1, -0.5), new Point(0.5, 0, 0));
            var leftSide = new Triangle(new Point(-0.5, 0, 0), new Point(0, 1, -0.5), new Point(0, 0, -1));
            var rightSide = new Triangle(new Point(0, 0, -1), new Point(0, 1, -0.5), new Point(0.5, 0, 0));
            pyramid.AddChildren(bottom, backSide, leftSide, rightSide);

            return pyramid;
        }
    }
}
