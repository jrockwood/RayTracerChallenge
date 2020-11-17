// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Chapter15TeapotLow.cs" company="Justin Rockwood">
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

    public class Chapter15TeapotLow : Scene
    {
        public Chapter15TeapotLow()
            : base(
                "Chapter 15 - Teapot (Low Resolution)",
                "Renders a teapot using triangles.",
                600,
                400)
        {
        }

        protected override Canvas Render(CameraRenderOptions options)
        {
            // Floor
            var floorMaterial = new Material(
                pattern: new CheckerPattern(Colors.White * 0.35, Colors.White * 0.4),
                ambient: 1,
                diffuse: 0,
                specular: 0,
                reflective: 0.1);

            var floor = new Plane(material: floorMaterial);
            var wall = new Plane(
                Matrix4x4.CreateRotationX(Math.PI / 2).Translate(0, 0, -10),
                floorMaterial.WithReflective(0));

            // Teapot 1
            var lowPolyMaterial = new Material(new Color(1, 0.3, 0.2), shininess: 5, specular: 0.4);
            string teapotData = File.ReadAllText(
                Path.Combine("Scenes", "Objects", "teapot-low-only-triangles.obj"),
                Encoding.ASCII);

            var teapot1File = ObjFile.Parse(teapotData);
            var teapot1 = teapot1File.ToGroup();
            teapot1.Transform = Matrix4x4.CreateScaling(0.3, 0.3, 0.3)
                .RotateX(-Math.PI / 2)
                .RotateY((Math.PI * 23) / 22)
                .Translate(7, 0, 3);
            teapot1.Material = lowPolyMaterial;

            var teapot2File = ObjFile.Parse(teapotData);
            var teapot2 = teapot2File.ToGroup();
            teapot2.Transform = Matrix4x4.CreateScaling(0.3, 0.3, 0.3)
                .RotateX(-Math.PI / 2)
                .RotateY((-Math.PI * 46) / 22)
                .Translate(-7, 0, 3);
            teapot2.Material = lowPolyMaterial;

            // Lights, camera, action.
            var light1 = new PointLight(new Point(50, 100, 20), Colors.Gray);
            var light2 = new PointLight(new Point(2, 50, 100), Colors.Gray);
            var world = new World(lights: new[] { light1, light2 }, shapes: new Shape[] { floor, wall, teapot1, teapot2 });

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
