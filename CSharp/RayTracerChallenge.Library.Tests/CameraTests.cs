// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="CameraTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;

    public class CameraTests
    {
        [Test]
        public void A_camera_should_store_size_field_of_view_and_a_transform()
        {
            var camera = new Camera(160, 120, MathF.PI / 2);
            camera.CanvasWidth.Should().Be(160);
            camera.CanvasHeight.Should().Be(120);
            camera.FieldOfView.Should().Be(MathF.PI / 2);
            camera.Transform.Should().Be(Matrix4x4.Identity);
        }

        //// ===========================================================================================================
        //// PixelSize Tests
        //// ===========================================================================================================

        [Test]
        public void PixelSize_should_calculate_correctly_for_a_horizontal_canvas()
        {
            var camera = new Camera(200, 125, MathF.PI / 2);
            camera.PixelSize.Should().Be(0.01f);
        }

        [Test]
        public void PixelSize_should_calculate_correctly_for_a_vertical_canvas()
        {
            var camera = new Camera(125, 200, MathF.PI / 2);
            camera.PixelSize.Should().Be(0.01f);
        }

        //// ===========================================================================================================
        //// RayForPixel Tests
        //// ===========================================================================================================

        [Test]
        public void RayForPixel_should_construct_a_ray_through_the_center_of_the_canvas()
        {
            var camera = new Camera(201, 101, MathF.PI / 2);
            Ray ray = camera.RayForPixel(100, 50);
            ray.Origin.Should().Be(Point.Zero);
            ray.Direction.Should().Be(new Vector(0, 0, -1));
        }

        [Test]
        public void RayForPixel_should_construct_a_ray_through_a_corner_of_the_canvas()
        {
            var camera = new Camera(201, 101, MathF.PI / 2);
            Ray ray = camera.RayForPixel(0, 0);
            ray.Origin.Should().Be(Point.Zero);
            ray.Direction.Should().Be(new Vector(0.66519f, 0.33259f, -0.66851f));
        }

        [Test]
        public void RayForPixel_should_construct_a_raw_when_the_camera_is_transformed()
        {
            var transform = Matrix4x4.CreateRotationY(MathF.PI / 4) * Matrix4x4.CreateTranslation(0, -2, 5);
            var camera = new Camera(201, 101, MathF.PI / 2, transform);
            Ray ray = camera.RayForPixel(100, 50);
            ray.Origin.Should().Be(new Point(0, 2, -5));
            ray.Direction.Should().Be(new Vector(MathF.Sqrt(2) / 2, 0, -MathF.Sqrt(2) / 2));
        }

        //// ===========================================================================================================
        //// Render Tests
        //// ===========================================================================================================

        [Test]
        public void Render_should_render_a_world_with_a_camera()
        {
            var world = World.CreateDefaultWorld();
            var cameraTransform = Matrix4x4.CreateLookAt(new Point(0, 0, -5), Point.Zero, Vector.UnitY);
            var camera = new Camera(11, 11, MathF.PI / 2, cameraTransform);
            Canvas canvas = camera.Render(world);
            canvas.GetPixel(5, 5).Should().Be(new Color(0.38066f, 0.47583f, 0.2855f));
        }
    }
}
