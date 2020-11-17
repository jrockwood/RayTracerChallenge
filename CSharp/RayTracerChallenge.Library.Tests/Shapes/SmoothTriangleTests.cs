// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="SmoothTriangleTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests.Shapes
{
    using FluentAssertions;
    using NUnit.Framework;
    using RayTracerChallenge.Library.Shapes;

    public class SmoothTriangleTests
    {
        private readonly SmoothTriangle _triangle = new SmoothTriangle(
            new Point(0, 1, 0),
            new Point(-1, 0, 0),
            new Point(1, 0, 0),
            new Vector(0, 1, 0),
            new Vector(-1, 0, 0),
            new Vector(1, 0, 0));

        [Test]
        public void Ctor_should_store_the_values()
        {
            _triangle.P1.Should().Be(new Point(0, 1, 0));
            _triangle.P2.Should().Be(new Point(-1, 0, 0));
            _triangle.P3.Should().Be(new Point(1, 0, 0));

            _triangle.N1.Should().Be(new Vector(0, 1, 0));
            _triangle.N2.Should().Be(new Vector(-1, 0, 0));
            _triangle.N3.Should().Be(new Vector(1, 0, 0));
        }

        [Test]
        public void When_intersecting_triangles_preserve_the_u_and_v_values_in_the_resulting_intersection()
        {
            var ray = new Ray(new Point(-0.2, 0.3, -2), Vector.UnitZ);
            var intersections = _triangle.LocalIntersect(ray);
            intersections[0].U.Should().BeApproximately(0.45, NumberExtensions.Epsilon);
            intersections[0].V.Should().BeApproximately(0.25, NumberExtensions.Epsilon);
        }

        [Test]
        public void When_computing_the_normal_vector_use_u_and_v_to_interpolate_the_normal()
        {
            var intersection = new Intersection(1, _triangle, 0.45, 0.25);
            var normal = _triangle.NormalAt(Point.Zero, intersection);
            normal.Should().Be(new Vector(-0.5547, 0.83205, 0));
        }

        [Test]
        public void The_hit_should_be_preserved_when_calculating_the_intersection_state()
        {
            var hit = new Intersection(1, _triangle, 0.45, 0.25);
            var ray = new Ray(new Point(-0.2, 0.3, -2), Vector.UnitZ);
            var intersections = new IntersectionList(hit);
            var state = IntersectionState.Create(hit, ray, intersections);
            state.Normal.Should().Be(new Vector(-0.5547, 0.83205, 0));
        }
    }
}
