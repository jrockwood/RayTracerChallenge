// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="WorldTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests
{
    using FluentAssertions;
    using NUnit.Framework;
    using RayTracerChallenge.Library.Lights;
    using RayTracerChallenge.Library.Shapes;

    public class WorldTests
    {
        [Test]
        public void Ctor_should_store_the_light_source_and_the_shapes()
        {
            var light = new PointLight(new Point(-10, 10, -10), Colors.White);
            var sphere1 = new Sphere(
                material: new Material(new Color(0.8, 1.0, 0.6), diffuse: 0.7, specular: 0.2));
            var sphere2 = new Sphere(Matrix4x4.CreateScaling(0.5, 0.5, 0.5));
            var world = new World(light, sphere1, sphere2);

            world.Light.Should().Be(light);
            world.Shapes.Should().HaveCount(2).And.ContainInOrder(sphere1, sphere2);
        }

        //// ===========================================================================================================
        //// Intersect Tests
        //// ===========================================================================================================

        [Test]
        public void Intersect_should_intersect_with_both_spheres_when_looking_at_the_origin()
        {
            var world = World.CreateDefaultWorld();
            var ray = new Ray(new Point(0, 0, -5), Vector.UnitZ);
            IntersectionList intersections = world.Intersect(ray);
            intersections.Ts.Should().HaveCount(4).And.ContainInOrder(4, 4.5, 5.5, 6);
        }

        //// ===========================================================================================================
        //// ShadeHit Tests
        //// ===========================================================================================================

        [Test]
        public void ShadeHit_should_shade_an_intersection()
        {
            var world = World.CreateDefaultWorld();
            var ray = new Ray(new Point(0, 0, -5), Vector.UnitZ);
            Shape shape = world.Shapes[0];
            var intersection = new Intersection(4, shape);
            var state = IntersectionState.Create(intersection, ray);
            Color color = world.ShadeHit(state);
            color.Should().Be(new Color(0.38066, 0.47583, 0.2855));
        }

        [Test]
        public void ShadeHit_should_shade_an_intersection_from_the_inside()
        {
            var world = World.CreateDefaultWorld().WithLight(new PointLight(new Point(0, 0.25, 0), Colors.White));
            var ray = new Ray(new Point(0, 0, 0), Vector.UnitZ);
            Shape shape = world.Shapes[1];
            var intersection = new Intersection(0.5, shape);
            var state = IntersectionState.Create(intersection, ray);
            Color color = world.ShadeHit(state);
            color.Should().Be(new Color(0.90498, 0.90498, 0.90498));
        }

        [Test]
        public void ShadeHit_should_shade_an_intersection_in_shadow()
        {
            var sphere1 = new Sphere();
            var sphere2 = new Sphere(Matrix4x4.CreateTranslation(0, 0, 10));
            var world = World.CreateDefaultWorld()
                .WithLight(new PointLight(new Point(0, 0, -10), Colors.White))
                .WithAddedShapes(sphere1, sphere2);

            var ray = new Ray(new Point(0, 0, 5), Vector.UnitZ);
            var intersection = new Intersection(4, sphere2);
            var state = IntersectionState.Create(intersection, ray);

            Color color = world.ShadeHit(state);
            color.Should().Be(new Color(0.1, 0.1, 0.1));
        }

        //// ===========================================================================================================
        //// ColorAt Tests
        //// ===========================================================================================================

        [Test]
        public void ColorAt_should_return_black_when_a_ray_misses()
        {
            var world = World.CreateDefaultWorld();
            var ray = new Ray(new Point(0, 0, -5), Vector.UnitY);
            Color color = world.ColorAt(ray);
            color.Should().Be(Colors.Black);
        }

        [Test]
        public void ColorAt_should_return_the_color_when_a_ray_hits()
        {
            var world = World.CreateDefaultWorld();
            var ray = new Ray(new Point(0, 0, -5), Vector.UnitZ);
            Color color = world.ColorAt(ray);
            color.Should().Be(new Color(0.38066, 0.47583, 0.2855));
        }

        [Test]
        public void ColorAt_should_return_the_color_with_an_intersection_behind_the_ray()
        {
            var world = World.CreateDefaultWorld();
            Shape outer = world.Shapes[0].WithMaterial(m => m.WithAmbient(1));
            Shape inner = world.Shapes[1].WithMaterial(m => m.WithAmbient(1));
            world = world.WithShapes(outer, inner);

            var ray = new Ray(new Point(0, 0, 0.75), -Vector.UnitZ);
            Color color = world.ColorAt(ray);
            color.Should().Be(inner.Material.Color);
        }

        //// ===========================================================================================================
        //// IsShadowed Tests
        //// ===========================================================================================================

        [Test]
        public void IsShadowed_should_return_false_when_nothing_is_collinear_with_the_point_and_the_light()
        {
            var world = World.CreateDefaultWorld();
            var p = new Point(0, 10, 0);
            world.IsShadowed(p).Should().BeFalse();
        }

        [Test]
        public void IsShadowed_should_return_true_when_an_object_is_between_the_point_and_the_light()
        {
            var world = World.CreateDefaultWorld();
            var p = new Point(10, -10, 10);
            world.IsShadowed(p).Should().BeTrue();
        }

        [Test]
        public void IsShadowed_should_return_false_when_an_object_is_behind_the_light()
        {
            var world = World.CreateDefaultWorld();
            var p = new Point(-20, 20, -20);
            world.IsShadowed(p).Should().BeFalse();
        }

        [Test]
        public void IsShadowed_should_return_false_when_an_object_is_behind_the_point()
        {
            var world = World.CreateDefaultWorld();
            var p = new Point(-2, 2, -2);
            world.IsShadowed(p).Should().BeFalse();
        }
    }
}
