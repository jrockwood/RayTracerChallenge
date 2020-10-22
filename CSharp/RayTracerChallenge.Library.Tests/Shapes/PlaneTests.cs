// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="PlaneTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests.Shapes
{
    using FluentAssertions;
    using NUnit.Framework;
    using RayTracerChallenge.Library.Shapes;

    public class PlaneTests
    {
        [Test]
        public void LocalNormalAt_should_return_the_same_normal_at_every_point()
        {
            var plane = new Plane();
            var n1 = plane.LocalNormalAt(new Point(0, 0, 0));
            var n2 = plane.LocalNormalAt(new Point(10, 0, -10));
            var n3 = plane.LocalNormalAt(new Point(-5, 0, 150));

            n1.Should().Be(Vector.UnitY);
            n2.Should().Be(Vector.UnitY);
            n3.Should().Be(Vector.UnitY);
        }

        [Test]
        public void LocalIntersect_should_not_intersect_with_a_ray_parallel_to_the_plane()
        {
            var plane = new Plane();
            var ray = new Ray(new Point(0, 10, 0), Vector.UnitZ);
            var intersections = plane.LocalIntersect(ray);
            intersections.Should().BeEmpty();
        }

        [Test]
        public void LocalIntersect_should_not_intersect_with_a_coplanar_ray()
        {
            var plane = new Plane();
            var ray = new Ray(Point.Zero, Vector.UnitZ);
            var intersections = plane.LocalIntersect(ray);
            intersections.Should().BeEmpty();
        }

        [Test]
        public void LocalIntersect_should_intersect_with_a_ray_from_above()
        {
            var plane = new Plane();
            var ray = new Ray(new Point(0, 1, 0), new Vector(0, -1, 0));
            var intersections = plane.LocalIntersect(ray);
            intersections.Should().HaveCount(1);
            intersections[0].T.Should().Be(1);
            intersections[0].Shape.Should().BeSameAs(plane);
        }

        [Test]
        public void LocalIntersect_should_intersect_with_a_ray_from_below()
        {
            var plane = new Plane();
            var ray = new Ray(new Point(0, -1, 0), new Vector(0, 1, 0));
            var intersections = plane.LocalIntersect(ray);
            intersections.Should().HaveCount(1);
            intersections[0].T.Should().Be(1);
            intersections[0].Shape.Should().BeSameAs(plane);
        }
    }
}
