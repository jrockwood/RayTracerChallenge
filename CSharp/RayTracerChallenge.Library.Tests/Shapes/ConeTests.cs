// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ConeTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests.Shapes
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using NUnit.Framework;
    using RayTracerChallenge.Library.Shapes;

    public class ConeTests
    {
        [Test]
        public void Intersecting_a_cone_with_a_ray()
        {
            var testCases = new[]
            {
                (origin: new Point(0, 0, -5), direction: new Vector(0, 0, 1), t1: 5, t2: 5),
                (origin: new Point(0, 0, -5), direction: new Vector(1, 1, 1), t1: 8.66025, t2: 8.66025),
                (origin: new Point(1, 1, -5), direction: new Vector(-0.5, -1, 1), t1: 4.55006, t2: 49.44994),
            };

            foreach ((Point origin, Vector vector, double t1, double t2) in testCases)
            {
                var cone = new Cone();
                var direction = vector.Normalize();
                var ray = new Ray(origin, direction);
                IntersectionList intersections = cone.LocalIntersect(ray);
                intersections.Ts.Select(x => x.RoundToEpsilon()).Should().HaveCount(2).And.ContainInOrder(t1, t2);
            }
        }

        [Test]
        public void Intersecting_a_cone_with_a_ray_parallel_to_one_of_its_halves()
        {
            var cone = new Cone();
            var direction = new Vector(0, 1, 1).Normalize();
            var ray = new Ray(new Point(0, 0, -1), direction);
            IntersectionList intersections = cone.LocalIntersect(ray);
            intersections.Ts.Select(x => x.RoundToEpsilon()).Should().HaveCount(1).And.ContainInOrder(0.35355);
        }

        [Test]
        public void Intersecting_a_cone_end_caps()
        {
            var testCases = new[]
            {
                (origin: new Point(0, 0, -5), direction: new Vector(0, 1, 0), count: 0),
                (origin: new Point(0, 0, -0.25), direction: new Vector(0, 1, 1), count: 2),
                (origin: new Point(0, 0, -0.25), direction: new Vector(0, 1, 0), count: 4),
            };

            foreach ((Point origin, Vector vector, int count) in testCases)
            {
                var cone = new Cone(minimumY: -0.5, maximumY: 0.5, isClosed: true);
                var direction = vector.Normalize();
                var ray = new Ray(origin, direction);
                IntersectionList intersections = cone.LocalIntersect(ray);
                intersections.Should().HaveCount(count);
            }
        }

        [Test]
        public void Computing_the_normal_vector_on_a_cone()
        {
            var testCases = new[]
            {
                (point: new Point(0, 0, 0), normal: new Vector(0, 0, 0)),
                (point: new Point(1, 1, 1), normal: new Vector(1, -Math.Sqrt(2), 1)),
                (point: new Point(-1, -1, 0), normal: new Vector(-1, 1, 0)),
            };

            foreach ((Point point, Vector expectedNormal) in testCases)
            {
                var cone = new Cone();
                var normal = cone.LocalNormalAt(point);
                normal.Should().Be(expectedNormal);
            }
        }
    }
}
