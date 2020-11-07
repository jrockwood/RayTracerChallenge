// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="TriangleTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests.Shapes
{
    using FluentAssertions;
    using NUnit.Framework;
    using RayTracerChallenge.Library.Shapes;

    public class TriangleTests
    {
        [Test]
        public void Constructing_a_triangle()
        {
            var p1 = new Point(0, 1, 0);
            var p2 = new Point(-1, 0, 0);
            var p3 = new Point(1, 0, 0);
            var triangle = new Triangle(p1, p2, p3);
            triangle.P1.Should().Be(p1);
            triangle.P2.Should().Be(p2);
            triangle.P3.Should().Be(p3);

            triangle.E1.Should().Be(new Vector(-1, -1, 0));
            triangle.E2.Should().Be(new Vector(1, -1, 0));
            triangle.Normal.Should().Be(new Vector(0, 0, -1));
        }

        [Test]
        public void Finding_the_normal_on_a_triangle()
        {
            var triangle = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));
            var n1 = triangle.LocalNormalAt(new Point(0, 0.5, 0));
            var n2 = triangle.LocalNormalAt(new Point(-0.5, 0.75, 0));
            var n3 = triangle.LocalNormalAt(new Point(0.5, 0.25, 0));
            n1.Should().Be(triangle.Normal);
            n2.Should().Be(triangle.Normal);
            n3.Should().Be(triangle.Normal);
        }

        [Test]
        public void Intersecting_a_ray_parallel_to_the_triangle()
        {
            var triangle = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));
            var ray = new Ray(new Point(0, -1, -2), new Vector(0, 1, 0));
            var intersections = triangle.LocalIntersect(ray);
            intersections.Should().BeEmpty();
        }

        [Test]
        public void A_ray_misses_the_p1_p3_edge()
        {
            var triangle = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));
            var ray = new Ray(new Point(1, 1, -2), new Vector(0, 0, 1));
            var intersections = triangle.LocalIntersect(ray);
            intersections.Should().BeEmpty();
        }

        [Test]
        public void A_ray_misses_the_p1_p2_edge()
        {
            var triangle = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));
            var ray = new Ray(new Point(-1, 1, -2), new Vector(0, 0, 1));
            var intersections = triangle.LocalIntersect(ray);
            intersections.Should().BeEmpty();
        }

        [Test]
        public void A_ray_misses_the_p2_p3_edge()
        {
            var triangle = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));
            var ray = new Ray(new Point(0, -1, -2), new Vector(0, 0, 1));
            var intersections = triangle.LocalIntersect(ray);
            intersections.Should().BeEmpty();
        }

        [Test]
        public void A_ray_strikes_a_triangle()
        {
            var triangle = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));
            var ray = new Ray(new Point(0, 0.5, -2), new Vector(0, 0, 1));
            var intersections = triangle.LocalIntersect(ray);
            intersections.Ts.Should().HaveCount(1).And.ContainInOrder(2);
        }
    }
}
