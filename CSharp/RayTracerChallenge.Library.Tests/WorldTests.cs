// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="WorldTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using NUnit.Framework;
    using RayTracerChallenge.Library.Lights;
    using RayTracerChallenge.Library.Shapes;
    using RayTracerChallenge.Library.Tests.Patterns;

    public class WorldTests
    {
        [Test]
        public void Ctor_should_store_the_light_source_and_the_shapes()
        {
            var light = new PointLight(new Point(-10, 10, -10), Colors.White);
            var sphere1 = new Sphere(
                material: new Material(new Color(0.8, 1.0, 0.6), diffuse: 0.7, specular: 0.2));
            var sphere2 = new Sphere(transform: Matrix4x4.CreateScaling(0.5, 0.5, 0.5));
            var world = new World(light, sphere1, sphere2);

            world.Lights.Should().HaveCount(1).And.ContainInOrder(light);
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
            var state = IntersectionState.Create(intersection, ray, new IntersectionList(intersection));
            Color color = world.ShadeHit(state);
            color.Should().Be(new Color(0.38066, 0.47583, 0.2855));
        }

        [Test]
        public void ShadeHit_should_shade_an_intersection_from_the_inside()
        {
            var world = World.CreateDefaultWorld().ChangeLights(new PointLight(new Point(0, 0.25, 0), Colors.White));
            var ray = new Ray(new Point(0, 0, 0), Vector.UnitZ);
            Shape shape = world.Shapes[1];
            var intersection = new Intersection(0.5, shape);
            var state = IntersectionState.Create(intersection, ray, new IntersectionList(intersection));
            Color color = world.ShadeHit(state);
            color.Should().Be(new Color(0.90498, 0.90498, 0.90498));
        }

        [Test]
        public void ShadeHit_should_shade_an_intersection_in_shadow()
        {
            var sphere1 = new Sphere();
            var sphere2 = new Sphere(transform: Matrix4x4.CreateTranslation(0, 0, 10));
            var world = World.CreateDefaultWorld()
                .ChangeLights(new PointLight(new Point(0, 0, -10), Colors.White))
                .AddShapes(sphere1, sphere2);

            var ray = new Ray(new Point(0, 0, 5), Vector.UnitZ);
            var intersection = new Intersection(4, sphere2);
            var state = IntersectionState.Create(intersection, ray, new IntersectionList(intersection));

            Color color = world.ShadeHit(state);
            color.Should().Be(new Color(0.1, 0.1, 0.1));
        }

        /// <summary>
        /// Add a reflective plane to the default scene, just below the spheres, and orient a ray so it strikes the
        /// plane, reflects upward, and hits the outermost sphere.
        /// </summary>
        [Test]
        public void ShadeHit_should_calculate_the_reflected_color_for_a_reflective_material()
        {
            var world = World.CreateDefaultWorld();
            var floor = new Plane("Floor", Matrix4x4.CreateTranslation(0, -1, 0), new Material(reflective: 0.5));
            world = world.AddShapes(floor);

            var ray = new Ray(new Point(0, 0, -3), new Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
            var intersection = new Intersection(Math.Sqrt(2), floor);
            var state = IntersectionState.Create(intersection, ray, new IntersectionList(intersection));

            var color = world.ShadeHit(state);
            color.Should().Be(new Color(0.87675, 0.92434, 0.82917));
        }

        /// <summary>
        /// Add a glass floor to the default world, positioned just below the two default spheres, and add a new,
        /// colored sphere below the floor. Cast a ray diagonally toward the floor, with the expectation that it will
        /// refract and eventually strike the colored ball. Because the plan is only semitransparent, the resulting
        /// color should combine the refracted color of the ball and the color of the plane.
        /// </summary>
        [Test]
        public void ShadeHit_should_calculate_the_shade_color_with_a_transparent_material()
        {
            var floor = new Plane(
                "Floor",
                Matrix4x4.CreateTranslation(0, -1, 0),
                new Material(transparency: 0.5, refractiveIndex: 1.5));

            var ball = new Sphere(
                "s",
                Matrix4x4.CreateTranslation(0, -3.5, -0.5),
                new Material(Colors.Red, ambient: 0.5));
            var world = World.CreateDefaultWorld().AddShapes(floor, ball);

            var ray = new Ray(new Point(0, 0, -3), new Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
            var intersections = new IntersectionList((Math.Sqrt(2), floor));
            var state = IntersectionState.Create(intersections[0], ray, intersections);
            Color color = world.ShadeHit(state, 5);
            color.Should().Be(new Color(0.93642, 0.68642, 0.68642));
        }

        [Test]
        public void ShadeHit_should_calculate_the_shade_color_with_a_reflective_and_transparent_material()
        {
            var floor = new Plane(
                "Floor",
                Matrix4x4.CreateTranslation(0, -1, 0),
                new Material(reflective: 0.5, transparency: 0.5, refractiveIndex: 1.5));

            var ball = new Sphere(
                "Ball",
                Matrix4x4.CreateTranslation(0, -3.5, -0.5),
                new Material(Colors.Red, ambient: 0.5));
            var world = World.CreateDefaultWorld().AddShapes(floor, ball);

            var ray = new Ray(new Point(0, 0, -3), new Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
            var intersections = new IntersectionList((Math.Sqrt(2), floor));
            var state = IntersectionState.Create(intersections[0], ray, intersections);
            Color color = world.ShadeHit(state, 5);
            color.Should().Be(new Color(0.93391, 0.69643, 0.69243));
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
            Shape outer = world.Shapes[0].ChangeMaterial(m => m.WithAmbient(1));
            Shape inner = world.Shapes[1].ChangeMaterial(m => m.WithAmbient(1));

            var ray = new Ray(new Point(0, 0, 0.75), -Vector.UnitZ);
            Color color = world.ColorAt(ray);
            color.Should().Be(inner.Material.Color);
        }

        [Test]
        public void ColorAt_should_avoid_infinite_recursion_with_two_parallel_mirrors()
        {
            var lower = new Plane("Lower", Matrix4x4.CreateTranslation(0, -1, 0), new Material(reflective: 1));
            var upper = new Plane("Upper", Matrix4x4.CreateTranslation(0, 1, 0), new Material(reflective: 1));
            var world = World.CreateDefaultWorld()
                .ChangeLights(new PointLight(Point.Zero, Colors.White))
                .ChangeShapes(lower, upper);
            var ray = new Ray(Point.Zero, Vector.UnitY);
            Color color = world.ColorAt(ray);
            color.Should().Be(new Color(11.4, 11.4, 11.4));
        }

        //// ===========================================================================================================
        //// IsShadowed Tests
        //// ===========================================================================================================

        [Test]
        public void IsShadowed_should_return_false_when_nothing_is_collinear_with_the_point_and_the_light()
        {
            var world = World.CreateDefaultWorld();
            var p = new Point(0, 10, 0);
            world.IsShadowed(p, world.Lights.Single()).Should().BeFalse();
        }

        [Test]
        public void IsShadowed_should_return_true_when_an_object_is_between_the_point_and_the_light()
        {
            var world = World.CreateDefaultWorld();
            var p = new Point(10, -10, 10);
            world.IsShadowed(p, world.Lights.Single()).Should().BeTrue();
        }

        [Test]
        public void IsShadowed_should_return_false_when_an_object_is_behind_the_light()
        {
            var world = World.CreateDefaultWorld();
            var p = new Point(-20, 20, -20);
            world.IsShadowed(p, world.Lights.Single()).Should().BeFalse();
        }

        [Test]
        public void IsShadowed_should_return_false_when_an_object_is_behind_the_point()
        {
            var world = World.CreateDefaultWorld();
            var p = new Point(-2, 2, -2);
            world.IsShadowed(p, world.Lights.Single()).Should().BeFalse();
        }

        //// ===========================================================================================================
        //// ReflectedColor Tests
        //// ===========================================================================================================

        [Test]
        public void ReflectedColor_should_return_black_for_the_reflected_color_of_a_non_reflective_surface()
        {
            var world = World.CreateDefaultWorld();
            var ray = new Ray(Point.Zero, Vector.UnitZ);
            var shape = world.Shapes[1].ChangeMaterial(m => m.WithAmbient(1));

            var intersection = new Intersection(1, shape);
            var state = IntersectionState.Create(intersection, ray, new IntersectionList(intersection));
            Color color = world.ReflectedColor(state);
            color.Should().Be(Colors.Black);
        }

        /// <summary>
        /// Add a reflective plane to the default scene, just below the spheres, and orient a ray so it strikes the
        /// plane, reflects upward, and hits the outermost sphere.
        /// </summary>
        [Test]
        public void ReflectedColor_should_calculate_the_reflected_color_for_a_reflective_material()
        {
            var world = World.CreateDefaultWorld();
            var floor = new Plane("Floor", Matrix4x4.CreateTranslation(0, -1, 0), new Material(reflective: 0.5));
            world = world.AddShapes(floor);

            var ray = new Ray(new Point(0, 0, -3), new Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
            var intersection = new Intersection(Math.Sqrt(2), floor);
            var state = IntersectionState.Create(intersection, ray, new IntersectionList(intersection));

            Color color = world.ReflectedColor(state);
            color.Should().Be(new Color(0.190332, 0.23791, 0.14274));
        }

        [Test]
        public void ReflectedColor_should_only_allow_a_maximum_recursion_depth()
        {
            var world = World.CreateDefaultWorld();
            var floor = new Plane("Floor", Matrix4x4.CreateTranslation(0, -1, 0), new Material(reflective: 0.5));
            world = world.AddShapes(floor);

            var ray = new Ray(new Point(0, 0, -3), new Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
            var intersection = new Intersection(Math.Sqrt(2), floor);
            var state = IntersectionState.Create(intersection, ray, new IntersectionList(intersection));

            Color color = world.ReflectedColor(state, 0);
            color.Should().Be(Colors.Black);
        }

        //// ===========================================================================================================
        //// RefractedColor Tests
        //// ===========================================================================================================

        [Test]
        public void RefractedColor_should_return_black_for_the_refracted_color_with_an_opaque_surface()
        {
            var world = World.CreateDefaultWorld();
            var shape = world.Shapes[0];
            var ray = new Ray(new Point(0, 0, -5), Vector.UnitZ);
            var intersections = new IntersectionList((4, shape), (6, shape));
            var state = IntersectionState.Create(intersections[0], ray, intersections);
            Color color = world.RefractedColor(state, 5);
            color.Should().Be(Colors.Black);
        }

        [Test]
        public void RefractedColor_should_return_the_refracted_color_at_the_maximum_recursive_depth()
        {
            var world = World.CreateDefaultWorld();
            var shape = world.Shapes[0].ChangeMaterial(m => m.WithTransparency(1.0).WithRefractiveIndex(1.5));
            var ray = new Ray(new Point(0, 0, -5), Vector.UnitZ);
            var intersections = new IntersectionList((4, shape), (6, shape));
            var state = IntersectionState.Create(intersections[0], ray, intersections);
            Color color = world.RefractedColor(state, 0);
            color.Should().Be(Colors.Black);
        }

        [Test]
        public void RefractedColor_should_return_black_for_the_refracted_color_under_total_internal_reflection()
        {
            var world = World.CreateDefaultWorld();
            var shape = world.Shapes[0].ChangeMaterial(m => m.WithTransparency(1.0).WithRefractiveIndex(1.5));
            var ray = new Ray(new Point(0, 0, Math.Sqrt(2) / 2), Vector.UnitY);
            var intersections = new IntersectionList((-Math.Sqrt(2) / 2, shape), (Math.Sqrt(2) / 2, shape));

            // Note this time we're inside the sphere, so we need to look at intersections[1], not 0.
            var state = IntersectionState.Create(intersections[1], ray, intersections);
            Color color = world.RefractedColor(state, 5);
            color.Should().Be(Colors.Black);
        }

        [Test]
        public void RefractedColor_should_return_the_refracted_color_with_a_refracted_ray()
        {
            var world = World.CreateDefaultWorld();
            var a = world.Shapes[0].ChangeMaterial(m => m.WithAmbient(1).WithPattern(new PatternTests.TestPattern()));
            var b = world.Shapes[1].ChangeMaterial(m => m.WithTransparency(1).WithRefractiveIndex(1.5));

            var ray = new Ray(new Point(0, 0, 0.1), Vector.UnitY);
            var intersections = new IntersectionList((-0.9899, a), (-0.4899, b), (0.4899, b), (0.9899, a));
            var state = IntersectionState.Create(intersections[2], ray, intersections);
            Color color = world.RefractedColor(state, 5);
            color.Should().Be(new Color(0, 0.99887, 0.04722));
        }
    }
}
