// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Chapter11Reflections.cs" company="Justin Rockwood">
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

    public class Chapter11Reflections : Scene
    {
        public Chapter11Reflections()
            : base(
                "Chapter 11 - Reflections",
                "Renders spheres with various transparency and reflection values",
                800,
                600)
        {
        }

        protected override Canvas Render(CameraRenderOptions options)
        {
            // Materials
            var wallMaterial = new Material(
                pattern: new StripePattern(
                    new Color(0.45, 0.45, 0.45),
                    new Color(0.55, 0.55, 0.55),
                    Matrix4x4.CreateScaling(0.25, 0.25, 0.25).RotateY(Math.PI / 2)),
                ambient: 0,
                diffuse: 0.4,
                specular: 0,
                reflective: 0.3);

            // Shapes
            var floor = new Plane(
                Matrix4x4.CreateRotationY(0.31415),
                new Material(
                    pattern: new CheckerPattern(new Color(0.35, 0.35, 0.35), new Color(0.65, 0.65, 0.65)),
                    specular: 0,
                    reflective: 0.4));

            var ceiling = new Plane(
                Matrix4x4.CreateTranslation(0, 5, 0),
                new Material(new Color(0.8, 0.8, 0.8), ambient: 0.3, specular: 0));

            var westWall = new Plane(
                Matrix4x4.CreateRotationY(Math.PI / 2).RotateZ(Math.PI / 2).Translate(-5, 0, 0),
                wallMaterial);

            var eastWall = new Plane(
                Matrix4x4.CreateRotationY(Math.PI / 2).RotateZ(Math.PI / 2).Translate(5, 0, 0),
                wallMaterial);

            var northWall = new Plane(Matrix4x4.CreateRotationX(Math.PI / 2).Translate(0, 0, 5), wallMaterial);
            var southWall = new Plane(Matrix4x4.CreateRotationX(Math.PI / 2).Translate(0, 0, -5), wallMaterial);

            // Background balls
            var backBall1 = new Sphere(
                Matrix4x4.CreateScaling(0.4, 0.4, 0.4).Translate(4.6, 0.4, 1),
                new Material(new Color(0.8, 0.5, 0.3), shininess: 50));

            var backBall2 = new Sphere(
                Matrix4x4.CreateScaling(0.3, 0.3, 0.3).Translate(4.7, 0.3, 0.4),
                new Material(new Color(0.9, 0.4, 0.5), shininess: 50));

            var backBall3 = new Sphere(
                Matrix4x4.CreateScaling(0.5, 0.5, 0.5).Translate(-1, 0.5, 4.5),
                new Material(new Color(0.4, 0.9, 0.6), shininess: 50));

            var backBall4 = new Sphere(
                Matrix4x4.CreateScaling(0.3, 0.3, 0.3).Translate(-1.7, 0.3, 4.7),
                new Material(new Color(0.4, 0.6, 0.9), shininess: 50));

            // Foreground balls
            var redSphere = new Sphere(
                Matrix4x4.CreateTranslation(-0.6, 1, 0.6),
                new Material(new Color(1, 0.3, 0.2), specular: 0.4, shininess: 5));

            var blueGlassSphere = new Sphere(
                Matrix4x4.CreateScaling(0.7, 0.7, 0.7).Translate(0.6, 0.7, -0.6),
                new Material(
                    new Color(0, 0, 0.2),
                    ambient: 0,
                    diffuse: 0.4,
                    specular: 0.9,
                    shininess: 300,
                    reflective: 0.9,
                    transparency: 0.9,
                    refractiveIndex: 1.5));

            var greenGlassSphere = new Sphere(
                Matrix4x4.CreateScaling(0.5, 0.5, 0.5).Translate(-0.7, 0.5, -0.8),
                new Material(
                    new Color(0, 0.2, 0),
                    ambient: 0,
                    diffuse: 0.4,
                    specular: 0.9,
                    shininess: 300,
                    reflective: 0.9,
                    transparency: 0.9,
                    refractiveIndex: 1.5));

            // Lights, camera, action.
            var light = new PointLight(new Point(-4.9, 4.9, -1), Colors.White);
            var world = new World(
                light,
                floor, ceiling, westWall, eastWall, northWall, southWall,
                backBall1, backBall2, backBall3, backBall4,
                redSphere, blueGlassSphere, greenGlassSphere);

            var cameraTransform = Matrix4x4.CreateLookAt(
                new Point(-2.6, 1.5, -3.9),
                new Point(-0.6, 1, -0.8),
                Vector.UnitY);
            var camera = new Camera(CanvasWidth, CanvasHeight, fieldOfView: 1.152, cameraTransform);

            Canvas canvas = camera.Render(world, options);
            return canvas;
        }
    }
}
