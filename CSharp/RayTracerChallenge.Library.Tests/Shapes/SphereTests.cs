// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="SphereTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests.Shapes
{
    using FluentAssertions;
    using NUnit.Framework;
    using RayTracerChallenge.Library.Shapes;

    public class SphereTests
    {
        //// ===========================================================================================================
        //// Intersect
        //// ===========================================================================================================

        [Test]
        public void Intersect_should_intersect_at_two_points()
        {
            var ray = new Ray(new Point(0, 0, -5), Vector.UnitZ);
            var sphere = new Sphere();
            var points = sphere.Intersect(ray).Ts;
            points.Should().HaveCount(2).And.ContainInOrder(4.0f, 6.0f);
        }

        [Test]
        public void Intersect_should_set_the_shape_on_the_intersection()
        {
            var ray = new Ray(new Point(0, 1, -5), Vector.UnitZ);
            var s = new Sphere();
            var xs = s.Intersect(ray);
            xs.Shapes.Should().HaveCount(2).And.ContainInOrder(s, s);
        }

        [Test]
        public void Intersect_should_intersect_at_a_tangent()
        {
            var ray = new Ray(new Point(0, 1, -5), Vector.UnitZ);
            var s = new Sphere();
            var xs = s.Intersect(ray);
            xs.Ts.Should().HaveCount(2).And.ContainInOrder(5, 5);
        }

        [Test]
        public void Intersect_should_miss_correctly()
        {
            var ray = new Ray(new Point(0, 2, -5), Vector.UnitZ);
            var s = new Sphere();
            var xs = s.Intersect(ray);
            xs.Should().HaveCount(0);
        }

        [Test]
        public void Intersect_should_hit_twice_when_a_ray_originates_inside_a_sphere()
        {
            var ray = new Ray(Point.Zero, Vector.UnitZ);
            var s = new Sphere();
            var xs = s.Intersect(ray);
            xs.Ts.Should().HaveCount(2).And.ContainInOrder(-1.0f, 1.0f);
        }

        [Test]
        public void Intersect_should_hit_when_a_sphere_is_behind_a_ray()
        {
            var ray = new Ray(new Point(0, 0, 5), Vector.UnitZ);
            var s = new Sphere();
            var xs = s.Intersect(ray);
            xs.Ts.Should().HaveCount(2).And.ContainInOrder(-6.0f, -4.0f);
        }
    }
}
