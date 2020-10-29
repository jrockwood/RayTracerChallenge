// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Chapter12TableInARoom.cs" company="Justin Rockwood">
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

    public class Chapter12TableInARoom : Scene
    {
        public Chapter12TableInARoom()
            : base(
                "Chapter 12 - Cubes",
                "Renders a table in a room, made up of only cubes.",
                900,
                450)
        {
        }

        protected override Canvas Render(IProgress<RenderProgressStep> progress, CancellationToken cancellationToken)
        {
            // Colors
            var tableLegColor = new Color(0.5529, 0.4235, 0.3255);

            // Shapes
            var floorCeiling = new Cube(
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

            var tableTop = new Cube(
                Matrix4x4.CreateScaling(3, 0.1, 2).Translate(0, 3.1, 0),
                new Material(
                    pattern: new StripePattern(
                        tableLegColor,
                        new Color(0.6588, 0.5098, 0.4000),
                        Matrix4x4.CreateRotationY(0.1).Scale(0.05, 0.05, 0.05)),
                    ambient: 0.1,
                    diffuse: 0.7,
                    specular: 0.9,
                    shininess: 300,
                    reflective: 0.2));

            var leg1 = new Cube(
                Matrix4x4.CreateScaling(0.1, 1.5, 0.1).Translate(2.7, 1.5, -1.7),
                new Material(tableLegColor, ambient: 0.2, diffuse: 0.7));

            var leg2 = new Cube(
                Matrix4x4.CreateScaling(0.1, 1.5, 0.1).Translate(2.7, 1.5, 1.7),
                new Material(tableLegColor, ambient: 0.2, diffuse: 0.7));

            var leg3 = new Cube(
                Matrix4x4.CreateScaling(0.1, 1.5, 0.1).Translate(-2.7, 1.5, -1.7),
                new Material(tableLegColor, ambient: 0.2, diffuse: 0.7));

            var leg4 = new Cube(
                Matrix4x4.CreateScaling(0.1, 1.5, 0.1).Translate(-2.7, 1.5, 1.7),
                new Material(tableLegColor, ambient: 0.2, diffuse: 0.7));

            var glassCube = new Cube(
                Matrix4x4.CreateScaling(0.25, 0.25, 0.25).RotateY(0.2).Translate(0, 3.45001, 0),
                new Material(
                    new Color(1, 1, 0.8),
                    ambient: 0,
                    diffuse: 0.3,
                    specular: 0.9,
                    shininess: 300,
                    reflective: 0.7,
                    transparency: 0.7,
                    refractiveIndex: 1.5),
                isShadowHidden: true);

            var littleCube1 = new Cube(
                Matrix4x4.CreateScaling(0.15, 0.15, 0.15).RotateY(-0.4).Translate(1, 3.35, -0.9),
                new Material(new Color(1, 0.5, 0.5), diffuse: 0.4, reflective: 0.6));

            var littleCube2 = new Cube(
                Matrix4x4.CreateScaling(0.15, 0.07, 0.15).RotateY(0.4).Translate(-1.5, 3.27, 0.3),
                new Material(new Color(1, 1, 0.5)));

            var littleCube3 = new Cube(
                Matrix4x4.CreateScaling(0.2, 0.05, 0.05).RotateY(0.4).Translate(0, 3.25, 1),
                new Material(new Color(0.5, 1, 0.5)));

            var littleCube4 = new Cube(
                Matrix4x4.CreateScaling(0.05, 0.2, 0.05).RotateY(0.8).Translate(-0.6, 3.4, -1),
                new Material(new Color(0.5, 0.5, 1)));

            var littleCube5 = new Cube(
                Matrix4x4.CreateScaling(0.05, 0.2, 0.05).RotateY(0.8).Translate(2, 3.4, 1),
                new Material(new Color(0.5, 1, 1)));

            var frame1 = new Cube(
                Matrix4x4.CreateScaling(0.05, 1, 1).Translate(-10, 4, 1),
                new Material(new Color(0.7098, 0.2471, 0.2196), diffuse: 0.6));

            var frame2 = new Cube(
                Matrix4x4.CreateScaling(0.05, 0.4, 0.4).Translate(-10, 3.4, 2.7),
                new Material(new Color(0.2667, 0.2706, 0.6902), diffuse: 0.6));

            var frame3 = new Cube(
                Matrix4x4.CreateScaling(0.05, 0.4, 0.4).Translate(-10, 4.6, 2.7),
                new Material(new Color(0.3098, 0.5961, 0.3098), diffuse: 0.6));

            var mirrorFrame = new Cube(
                Matrix4x4.CreateScaling(5, 1.5, 0.05).Translate(-2, 3.5, 9.95),
                new Material(new Color(0.3882, 0.2627, 0.1882), diffuse: 0.7));

            var mirror = new Cube(
                Matrix4x4.CreateScaling(4.8, 1.4, 0.06).Translate(-2, 3.5, 9.95),
                new Material(Colors.Black, ambient: 0, diffuse: 0, specular: 1, shininess: 300, reflective: 1));

            // Lights, camera, action.
            var light = new PointLight(new Point(0, 6.9, -5), new Color(1, 1, 0.9));
            var world = new World(
                light,
                floorCeiling, walls,
                tableTop, leg1, leg2, leg3, leg4,
                glassCube, littleCube1, littleCube2, littleCube3, littleCube4, littleCube5,
                frame1, frame2, frame3,
                mirrorFrame, mirror);

            var cameraTransform = Matrix4x4.CreateLookAt(new Point(8, 6, -8), new Point(0, 3, 0), Vector.UnitY);
            var camera = new Camera(CanvasWidth, CanvasHeight, fieldOfView: 0.785, cameraTransform);

            Canvas canvas = camera.Render(world, progress, cancellationToken);
            return canvas;
        }
    }
}
