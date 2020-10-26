// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Chapter08ShadowPuppets.cs" company="Justin Rockwood">
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

    public class Chapter08ShadowPuppets : Scene
    {
        public Chapter08ShadowPuppets()
            : base(
                "Chapter 8 - Shadow Puppets",
                "Renders a bunch of spheres that cast a shadow in the shape of a dog head.",
                400,
                200)
        {
        }

        protected override Canvas Render(IProgress<RenderProgressStep> progress, CancellationToken cancellationToken)
        {
            var sphereMaterial = new Material(ambient: 0.2, diffuse: 0.8, specular: 0.3, shininess: 200);
            var wristMaterial = sphereMaterial.WithColor(new Color(0.1, 1, 1));
            var palmMaterial = sphereMaterial.WithColor(new Color(0.1, 0.1, 1));
            var thumbMaterial = palmMaterial;
            var indexMaterial = sphereMaterial.WithColor(new Color(1, 1, 0.1));
            var middleMaterial = sphereMaterial.WithColor(new Color(0.1, 1, 0.5));
            var ringMaterial = sphereMaterial.WithColor(new Color(0.1, 1, 0.1));
            var pinkyMaterial = sphereMaterial.WithColor(new Color(0.1, 0.5, 1));

            var backdrop = new Sphere(
                Matrix4x4.CreateScaling(200, 200, 0.01).Translate(0, 0, 20),
                new Material(Colors.White, ambient: 0, diffuse: 0.5, specular: 0));

            var wrist = new Sphere(
                Matrix4x4.CreateScaling(3, 3, 3).Translate(-4, 0, -21).RotateZ(Math.PI / 4),
                wristMaterial);
            var palm = new Sphere(Matrix4x4.CreateScaling(4, 3, 3).Translate(0, 0, -15), palmMaterial);
            var thumb = new Sphere(Matrix4x4.CreateScaling(1, 3, 1).Translate(-2, 2, -16), thumbMaterial);
            var index = new Sphere(Matrix4x4.CreateScaling(3, 0.75, 0.75).Translate(3, 2, -22), indexMaterial);
            var middle = new Sphere(Matrix4x4.CreateScaling(3, 0.75, 0.75).Translate(4, 1, -19), middleMaterial);
            var ring = new Sphere(Matrix4x4.CreateScaling(3, 0.75, 0.75).Translate(4, 0, -18), ringMaterial);
            var pinky = new Sphere(
                Matrix4x4.CreateScaling(2.5, 0.6, 0.6)
                    .Translate(1, 0, 0)
                    .RotateZ(Math.PI / 10)
                    .Translate(3, -1.5, -20),
                pinkyMaterial);

            var light = new PointLight(new Point(0, 0, -100), Colors.White);
            var world = new World(light, backdrop, wrist, palm, thumb, index, middle, ring, pinky);

            var cameraTransform = Matrix4x4.CreateLookAt(new Point(40, 0, -70), new Point(0, 0, -5), Vector.UnitY);
            var camera = new Camera(CanvasWidth, CanvasHeight, 0.524, cameraTransform);

            Canvas canvas = camera.Render(world, progress, cancellationToken);
            return canvas;
        }
    }
}
