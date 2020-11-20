// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Chapter15Teapots.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Library.Scenes
{
    using System;
    using System.IO;
    using System.Text;
    using RayTracerChallenge.Library;
    using RayTracerChallenge.Library.Lights;
    using RayTracerChallenge.Library.Patterns;
    using RayTracerChallenge.Library.Shapes;

    public class Chapter15Teapots : Scene
    {
        public Chapter15Teapots()
            : base(
                "Chapter 15 - Teapots (Low and High Resolution)",
                "Renders two teapots using triangles. The left one uses a low-resolution mesh and the one on the right uses a high-resolution mesh.",
                600,
                400)
        {
        }

        protected override Canvas Render(CameraRenderOptions options)
        {
            static string ReadObjData(string fileName) => File.ReadAllText(
                Path.Combine("Scenes", "Objects", fileName),
                Encoding.ASCII);

            // Floor
            var floorMaterial = new Material(
                pattern: new CheckerPattern(Colors.White * 0.35, Colors.White * 0.4),
                ambient: 1,
                diffuse: 0,
                specular: 0,
                reflective: 0.1);

            var floor = new Plane("Floor", material: floorMaterial);
            var wall = new Plane(
                "Wall",
                Matrix4x4.CreateRotationX(Math.PI / 2).Translate(0, 0, -10),
                floorMaterial.WithReflective(0));

            // Left Teapot
            string lowResOnlyTrianglesData = ReadObjData("teapot-low-only-triangles.obj");
            var leftTeapotFile = ObjFile.Parse(lowResOnlyTrianglesData);
            var leftTeapot = leftTeapotFile.ToGroup()
                .ChangeTransform(
                    Matrix4x4.CreateScaling(0.3, 0.3, 0.3)
                        .RotateX(-Math.PI / 2)
                        .RotateY((Math.PI * 23) / 22)
                        .Translate(7, 0, 3))
                .ChangeMaterial(new Material(new Color(1, 0.3, 0.2), shininess: 5, specular: 0.4));

            // Right Teapot
            string lowResData = ReadObjData("teapot-low.obj");
            var rightTeapotFile = ObjFile.Parse(lowResData);
            var rightTeapot = rightTeapotFile.ToGroup()
                .ChangeTransform(
                    Matrix4x4.CreateScaling(0.3, 0.3, 0.3)
                        .RotateX(-Math.PI / 2)
                        .RotateY((-Math.PI * 46) / 22)
                        .Translate(-7, 0, 3))
                .ChangeMaterial(new Material(new Color(0.3, 0.2, 1), shininess: 5, specular: 0.4));

            // Center Teapot (hi-res)
            string hiResData = ReadObjData("teapot.obj");
            var hiResTeapotFile = ObjFile.Parse(hiResData);
            var hiResTeapot = hiResTeapotFile.ToGroup()
                .ChangeTransform(
                    Matrix4x4.CreateScaling(0.4, 0.4, 0.4).RotateX(-Math.PI / 2).RotateY(-Math.PI).Translate(0, 0, -5))
                .ChangeMaterial(new Material(new Color(0.3, 1, 0.2), shininess: 5, specular: 0.4, reflective: 0.5));

            // Lights, camera, action.
            var light1 = new PointLight(new Point(50, 100, 20), Colors.Gray);
            var light2 = new PointLight(new Point(2, 50, 100), Colors.Gray);
            var world = new World(
                lights: new[] { light1, light2 },
                shapes: new Shape[] { floor, wall, leftTeapot, rightTeapot, hiResTeapot });

            var cameraTransform = Matrix4x4.CreateLookAt(
                new Point(0, 7, 13),
                new Point(0, 1, 0),
                Vector.UnitY);
            var camera = new Camera(CanvasWidth, CanvasHeight, fieldOfView: 1.5, cameraTransform);

            Canvas canvas = camera.Render(world, options);
            return canvas;
        }
    }
}
