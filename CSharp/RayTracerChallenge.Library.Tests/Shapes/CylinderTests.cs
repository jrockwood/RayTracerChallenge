// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="CylinderTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests.Shapes
{
    using System.Linq;
    using FluentAssertions;
    using NUnit.Framework;
    using RayTracerChallenge.Library.Shapes;

    public class CylinderTests
    {
        [Test]
        public void A_ray_misses_a_cylinder()
        {
            var testCases = new[]
            {
                // Sits on the surface of the cylinder and points along the +y axis.
                new Ray(new Point(1, 0, 0), Vector.UnitY),

                // Sits inside the cylinder and point along the +y axis.
                new Ray(new Point(0, 0, 0), Vector.UnitY),

                // Positioned outside the cylinder and oriented askew from all axes.
                new Ray(new Point(0, 0, -5), new Vector(1, 1, 1)),
            };

            foreach (Ray testCase in testCases)
            {
                var cylinder = new Cylinder();
                IntersectionList intersections = cylinder.LocalIntersect(testCase);
                intersections.Should().BeEmpty();
            }
        }

        [Test]
        public void A_ray_strikes_a_cylinder()
        {
            var testCases = new[]
            {
                // Strikes the cylinder on a tangent.
                (origin: new Point(1, 0, -5), direction: Vector.UnitZ, t1: 5, t2: 5),

                // Intersects the cylinder perpendicularly through the middle.
                (origin: new Point(0, 0, -5), direction: Vector.UnitZ, t1: 4, t2: 6),

                // Skewed so that the ray strikes the cylinder at an angle.
                (origin: new Point(0.5, 0, -5), direction: new Vector(0.1, 1, 1), t1: 6.80798, t2: 7.08872),
            };

            foreach ((Point origin, Vector vector, double t1, double t2) in testCases)
            {
                var cylinder = new Cylinder();
                var direction = vector.Normalize();
                var ray = new Ray(origin, direction);
                IntersectionList intersections = cylinder.LocalIntersect(ray);
                intersections.Ts.Select(x => x.RoundToEpsilon()).Should().HaveCount(2).And.ContainInOrder(t1, t2);
            }
        }

        [Test]
        public void Normal_vector_on_a_cylinder()
        {
            var testCases = new[]
            {
                (point: new Point(1, 0, 0), normal: new Vector(1, 0, 0)),
                (point: new Point(0, 5, -1), normal: new Vector(0, 0, -1)),
                (point: new Point(0, -2, 1), normal: new Vector(0, 0, 1)),
                (point: new Point(-1, 1, 0), normal: new Vector(-1, 0, 0)),
            };

            foreach ((Point point, Vector vector) in testCases)
            {
                var cylinder = new Cylinder();
                Vector normal = cylinder.LocalNormalAt(point);
                normal.Should().Be(vector);
            }
        }

        [Test]
        public void The_default_minimum_and_maximum_for_a_cylinder()
        {
            var cylinder = new Cylinder();
            cylinder.MinimumY.Should().Be(double.NegativeInfinity);
            cylinder.MaximumY.Should().Be(double.PositiveInfinity);
        }

        [Test]
        public void Intersecting_a_constrained_cylinder()
        {
            var testCases = new[]
            {
                // Cast a ray diagonally from inside the cylinder, with the ray escaping without intersecting.
                (point: new Point(0, 1.5, 0), direction: new Vector(0.1, 1, 0), count: 0),

                // Cast a ray perpendicular to the y axis, but from above and below the cylinder.
                (point: new Point(0, 3, -5), direction: new Vector(0, 0, 1), count: 0),
                (point: new Point(0, 0, -5), direction: new Vector(0, 0, 1), count: 0),

                // Cast rays perpendicular to the y axis, but at exactly the minimum and maximum.
                (point: new Point(0, 2, -5), direction: new Vector(0, 0, 1), count: 0),
                (point: new Point(0, 1, -5), direction: new Vector(0, 0, 1), count: 0),

                // Cast a perpendicular ray through the middle and get two intersections.
                (point: new Point(0, 1.5, -2), direction: new Vector(0, 0, 1), count: 2),
            };

            foreach ((Point point, Vector vector, int count) in testCases)
            {
                var cylinder = new Cylinder(minimumY: 1, maximumY: 2);
                var direction = vector.Normalize();
                var ray = new Ray(point, direction);
                IntersectionList intersections = cylinder.LocalIntersect(ray);
                intersections.Should().HaveCount(count);
            }
        }

        [Test]
        public void The_default_closed_value_for_a_cylinder()
        {
            var cylinder = new Cylinder();
            cylinder.IsClosed.Should().BeFalse();
        }

        [Test]
        public void Intersecting_the_caps_of_a_closed_cylinder()
        {
            var testCases = new[]
            {
                // The ray starts above the cylinder and points down through the middle, along the y axis.
                (point: new Point(0, 3, 0), direction: new Vector(0, -1, 0), count: 2),

                // The ray starts above and below the cylinder and casts through it, intersecting an end cap and side.
                (point: new Point(0, 3, -2), direction: new Vector(0, -1, 2), count: 2),
                (point: new Point(0, 0, -2), direction: new Vector(0, 1, 2), count: 2),

                // Corner cases: the ray originates above and below the cylinder, intersecting and end cap, but they
                // exit the cylinder at the point where the other end cap intersects the side of the cylinder.
                (point: new Point(0, 4, -2), direction: new Vector(0, -1, 1), count: 2),
                (point: new Point(0, -1, -2), direction: new Vector(0, 1, 1), count: 2),
            };

            foreach ((Point point, Vector vector, int count) in testCases)
            {
                var cylinder = new Cylinder(minimumY: 1, maximumY: 2, isClosed: true);
                var direction = vector.Normalize();
                var ray = new Ray(point, direction);
                IntersectionList intersections = cylinder.LocalIntersect(ray);
                intersections.Should().HaveCount(count);
            }
        }

        [Test]
        public void The_normal_vector_on_a_cylinder_s_end_caps()
        {
            var testCases = new[]
            {
                (point: new Point(0, 1, 0), normal: new Vector(0, -1, 0)),
                (point: new Point(0.5, 1, 0), normal: new Vector(0, -1, 0)),
                (point: new Point(0, 1, 0.5), normal: new Vector(0, -1, 0)),
                (point: new Point(0, 2, 0), normal: new Vector(0, 1, 0)),
                (point: new Point(0.5, 2, 0), normal: new Vector(0, 1, 0)),
                (point: new Point(0, 2, 0.5), normal: new Vector(0, 1, 0)),
            };

            foreach ((Point point, Vector expectedNormal) in testCases)
            {
                var cylinder = new Cylinder(minimumY: 1, maximumY: 2, isClosed: true);
                var normal = cylinder.LocalNormalAt(point);
                normal.Should().Be(expectedNormal);
            }
        }
    }
}
