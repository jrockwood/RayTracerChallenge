// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="CubeTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests.Shapes
{
    using FluentAssertions;
    using NUnit.Framework;
    using RayTracerChallenge.Library.Shapes;

    public class CubeTests
    {
        [Test]
        public void LocalIntersect_should_intersect()
        {
            var testCases = new[]
            {
                (desc: "+x", origin: new Point(5, 0.5, 0), direction: new Vector(-1, 0, 0), t1: 4, t2: 6),
                (desc: "-x", origin: new Point(-5, 0.5, 0), direction: new Vector(1, 0, 0), t1: 4, t2: 6),
                (desc: "+y", origin: new Point(0.5, 5, 0), direction: new Vector(0, -1, 0), t1: 4, t2: 6),
                (desc: "-y", origin: new Point(0.5, -5, 0), direction: new Vector(0, 1, 0), t1: 4, t2: 6),
                (desc: "+z", origin: new Point(0.5, 0, 5), direction: new Vector(0, 0, -1), t1: 4, t2: 6),
                (desc: "-z", origin: new Point(0.5, 0, -5), direction: new Vector(0, 0, 1), t1: 4, t2: 6),
                (desc: "inside", origin: new Point(0, 0.5, 0), direction: new Vector(0, 0, 1), t1: -1, t2: 1),
            };

            static void Test((string desc, Point origin, Vector direction, double t1, double t2) testCase)
            {
                (string desc, Point origin, Vector direction, double t1, double t2) = testCase;

                var cube = new Cube();
                var ray = new Ray(origin, direction);
                IntersectionList intersections = cube.LocalIntersect(ray);
                intersections.Ts.Should().HaveCount(2).And.ContainInOrder(new[] { t1, t2 }, $"for test case '{desc}'");
            }

            foreach (var testCase in testCases)
            {
                Test(testCase);
            }
        }

        [Test]
        public void LocalIntersect_should_miss_properly()
        {
            var testCases = new[]
            {
                new Ray(new Point(-2, 0, 0), new Vector(0.2673, 0.5345, 0.8018)),
                new Ray(new Point(0, -2, 0), new Vector(0.8018, 0.2673, 0.5345)),
                new Ray(new Point(0, 0, -2), new Vector(0.5345, 0.8018, 0.2673)),
                new Ray(new Point(2, 0, 2),  new Vector(0, 0, -1)),
                new Ray(new Point(0, 2, 2),  new Vector(0, -1, 0)),
                new Ray(new Point(2, 2, 0),  new Vector(-1, 0, 0)),
            };

            static void Test(Ray ray)
            {
                var cube = new Cube();
                IntersectionList intersections = cube.LocalIntersect(ray);
                intersections.Should().BeEmpty();
            }

            foreach (Ray testCase in testCases)
            {
                Test(testCase);
            }
        }

        [Test]
        public void LocalNormalAt_should_calculate_the_normal_on_the_surface_of_the_cube()
        {
            var testCases = new[]
            {
                (point: new Point(1, 0.5, -0.8), normal: new Vector(1, 0, 0)),
                (point: new Point(-1, -0.2, 0.9), normal: new Vector(-1, 0, 0)),
                (point: new Point(-0.4, 1, -0.1), normal: new Vector(0, 1, 0)),
                (point: new Point(0.3, -1, -0.7), normal: new Vector(0, -1, 0)),
                (point: new Point(-0.6, 0.3, 1), normal: new Vector(0, 0, 1)),
                (point: new Point(0.4, 0.4, -1), normal: new Vector(0, 0, -1)),
                (point: new Point(1, 1, 1), normal: new Vector(1, 0, 0)),
                (point: new Point(-1, -1, -1), normal: new Vector(-1, 0, 0)),
            };

            static void Test((Point point, Vector normal) testCase)
            {
                (Point point, Vector expectedNormal) = testCase;
                var cube = new Cube();
                Vector normal = cube.LocalNormalAt(point);
                normal.Should().Be(expectedNormal);
            }

            foreach (var testCase in testCases)
            {
                Test(testCase);
            }
        }
    }
}
