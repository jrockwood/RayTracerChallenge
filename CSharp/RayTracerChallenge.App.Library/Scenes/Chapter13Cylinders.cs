// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Chapter13Cylinders.cs" company="Justin Rockwood">
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

    public class Chapter13Cylinders : Scene
    {
        public Chapter13Cylinders()
            : base(
                "Chapter 13 - Cylinders",
                "Renders several cylinders, both open and closed.",
                800,
                400)
        {
        }

        protected override Canvas Render(CameraRenderOptions options)
        {
            // Shapes
            var floor = new Plane(material: new Material(
                pattern: new CheckerPattern(
                    Colors.Gray,
                    Colors.DarkGray,
                    Matrix4x4.CreateScaling(0.25, 0.25, 0.25).RotateY(0.3)),
                ambient: 0.2,
                diffuse: 0.9,
                specular: 0));

            var blueCylinder = new Cylinder(
                minimumY: 0,
                maximumY: 0.75,
                isClosed: true,
                Matrix4x4.CreateScaling(0.5, 1, 0.5).Translate(-1, 0, 1),
                new Material(new Color(0, 0, 0.6), diffuse: 0.1, specular: 0.9, shininess: 300, reflective: 0.9));

            // Concentric Cylinders
            var outer = new Cylinder(
                minimumY: 0,
                maximumY: 0.2,
                transform: Matrix4x4.CreateScaling(0.8, 1, 0.8).Translate(1, 0, 0),
                material: new Material(
                    new Color(1, 1, 0.3),
                    ambient: 0.1,
                    diffuse: 0.8,
                    specular: 0.9,
                    shininess: 300));

            var middle = new Cylinder(
                minimumY: 0,
                maximumY: 0.3,
                transform: Matrix4x4.CreateScaling(0.6, 1, 0.6).Translate(1, 0, 0),
                material: new Material(
                    new Color(1, 0.9, 0.4),
                    ambient: 0.1,
                    diffuse: 0.8,
                    specular: 0.9,
                    shininess: 300));

            var inner = new Cylinder(
                minimumY: 0,
                maximumY: 0.4,
                transform: Matrix4x4.CreateScaling(0.4, 1, 0.4).Translate(1, 0, 0),
                material: new Material(
                    new Color(1, 0.8, 0.5),
                    ambient: 0.1,
                    diffuse: 0.8,
                    specular: 0.9,
                    shininess: 300));

            var solidInner = new Cylinder(
                minimumY: 0,
                maximumY: 0.5,
                isClosed: true,
                transform: Matrix4x4.CreateScaling(0.2, 1, 0.2).Translate(1, 0, 0),
                material: new Material(
                    new Color(1, 0.7, 0.6),
                    ambient: 0.1,
                    diffuse: 0.8,
                    specular: 0.9,
                    shininess: 300));

            // Decorative Cylinders
            var red = new Cylinder(
                minimumY: 0,
                maximumY: 0.3,
                isClosed: true,
                transform: Matrix4x4.CreateScaling(0.05, 1, 0.05).Translate(0, 0, -0.75),
                material: new Material(Colors.Red, ambient: 0.1, diffuse: 0.9, specular: 0.9, shininess: 300));

            var yellow = new Cylinder(
                minimumY: 0,
                maximumY: 0.3,
                isClosed: true,
                transform: Matrix4x4.CreateScaling(0.05, 1, 0.05)
                    .Translate(0, 0, 1.5)
                    .RotateY(-0.15)
                    .Translate(0, 0, -2.25),
                material: new Material(Colors.Yellow, ambient: 0.1, diffuse: 0.9, specular: 0.9, shininess: 300));

            var green = new Cylinder(
                minimumY: 0,
                maximumY: 0.3,
                isClosed: true,
                transform: Matrix4x4.CreateScaling(0.05, 1, 0.05)
                    .Translate(0, 0, 1.5)
                    .RotateY(-0.3)
                    .Translate(0, 0, -2.25),
                material: new Material(Colors.Green, ambient: 0.1, diffuse: 0.9, specular: 0.9, shininess: 300));

            var cyan = new Cylinder(
                minimumY: 0,
                maximumY: 0.3,
                isClosed: true,
                transform: Matrix4x4.CreateScaling(0.05, 1, 0.05)
                    .Translate(0, 0, 1.5)
                    .RotateY(-0.45)
                    .Translate(0, 0, -2.25),
                material: new Material(Colors.Cyan, ambient: 0.1, diffuse: 0.9, specular: 0.9, shininess: 300));

            var glass = new Cylinder(
                minimumY: 0.0001,
                maximumY: 0.5,
                isClosed: true,
                transform: Matrix4x4.CreateScaling(0.33, 1, 0.33).Translate(0, 0, -1.5),
                material: new Material(
                    new Color(0.25, 0, 0),
                    diffuse: 0.1,
                    specular: 0.9,
                    shininess: 300,
                    reflective: 0.9,
                    transparency: 0.9,
                    refractiveIndex: 1.5));

            // Lights, camera, action.
            var light = new PointLight(new Point(1, 6.9, -4.9), Colors.White);
            var world = new World(light, floor, blueCylinder,
                outer, middle, inner, solidInner,
                red, yellow, green, cyan, glass);

            var cameraTransform = Matrix4x4.CreateLookAt(new Point(8, 3.5, -9), new Point(0, 0.3, 0), Vector.UnitY);
            var camera = new Camera(CanvasWidth, CanvasHeight, fieldOfView: 0.314, cameraTransform);

            Canvas canvas = camera.Render(world, options);
            return canvas;
        }
    }
}
