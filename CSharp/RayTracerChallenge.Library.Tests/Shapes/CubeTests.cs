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
        [TestCase("+x", 5, 0.5, 0, -1, 0, 0, 4, 6)]
        [TestCase("-x", -5, 0.5, 0, 1, 0, 0, 4, 6)]
        [TestCase("+y", 0.5, 5, 0, 0, -1, 0, 4, 6)]
        [TestCase("-y", 0.5, -5, 0, 0, 1, 0, 4, 6)]
        [TestCase("+z", 0.5, 0, 5, 0, 0, -1, 4, 6)]
        [TestCase("-z", 0.5, 0, -5, 0, 0, 1, 4, 6)]
        [TestCase("inside", 0, 0.5, 0, 0, 0, 1, -1, 1)]
        public void LocalIntersect_should_intersect(
            string description,
            double originX,
            double originY,
            double originZ,
            double directionX,
            double directionY,
            double directionZ,
            double t1,
            double t2)
        {
            var cube = new Cube();
            var ray = new Ray(new Point(originX, originY, originZ), new Vector(directionX, directionY, directionZ));
            IntersectionList intersections = cube.LocalIntersect(ray);
            intersections.Ts.Should().HaveCount(2).And.ContainInOrder(new[] { t1, t2 }, $"for test case '{description}'");
        }

        [Test]
        [TestCase(-2, 0, 0, 0.2673, 0.5345, 0.8018)]
        [TestCase(0, -2, 0, 0.8018, 0.2673, 0.5345)]
        [TestCase(0, 0, -2, 0.5345, 0.8018, 0.2673)]
        [TestCase(2, 0, 2, 0, 0, -1)]
        [TestCase(0, 2, 2, 0, -1, 0)]
        [TestCase(2, 2, 0, -1, 0, 0)]
        public void LocalIntersect_should_miss_properly(double px, double py, double pz, double vx, double vy, double vz)
        {
            var cube = new Cube();
            var ray = new Ray(new Point(px, py, pz), new Vector(vx, vy, vz));
            IntersectionList intersections = cube.LocalIntersect(ray);
            intersections.Should().BeEmpty();
        }

        [Test]
        [TestCase(1, 0.5, -0.8, 1, 0, 0)]
        [TestCase(-1, -0.2, 0.9, -1, 0, 0)]
        [TestCase(-0.4, 1, -0.1, 0, 1, 0)]
        [TestCase(0.3, -1, -0.7, 0, -1, 0)]
        [TestCase(-0.6, 0.3, 1, 0, 0, 1)]
        [TestCase(0.4, 0.4, -1, 0, 0, -1)]
        [TestCase(1, 1, 1, 1, 0, 0)]
        [TestCase(-1, -1, -1, -1, 0, 0)]
        public void LocalNormalAt_should_calculate_the_normal_on_the_surface_of_the_cube(
            double px,
            double py,
            double pz,
            double nx,
            double ny,
            double nz)
        {
            var cube = new Cube();
            Vector normal = cube.LocalNormalAt(new Point(px, py, pz));
            normal.Should().Be(new Vector(nx, ny, nz));
        }
    }
}
