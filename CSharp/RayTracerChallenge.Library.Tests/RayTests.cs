// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="RayTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests
{
    using FluentAssertions;
    using NUnit.Framework;

    public class RayTests
    {
        [Test]
        public void Creating_and_querying_a_ray()
        {
            var origin = new Point(1, 2, 3);
            var direction = new Vector(4, 5, 6);
            var ray = new Ray(origin, direction);

            ray.Origin.Should().Be(origin);
            ray.Direction.Should().Be(direction);
        }

        [Test]
        public void Computing_a_point_from_a_distance()
        {
            var ray = new Ray(new Point(2, 3, 4), new Vector(1, 0, 0));
            ray.PositionAt(0).Should().Be(new Point(2, 3, 4));
            ray.PositionAt(1).Should().Be(new Point(3, 3, 4));
            ray.PositionAt(-1).Should().Be(new Point(1, 3, 4));
            ray.PositionAt(2.5f).Should().Be(new Point(4.5f, 3, 4));
        }

        [Test]
        public void Transform_should_translate_a_ray()
        {
            var ray = new Ray(new Point(1, 2, 3), Vector.UnitY);
            var m = Matrix4x4.CreateTranslation(3, 4, 5);
            var r2 = ray.Transform(m);
            r2.Origin.Should().Be(new Point(4, 6, 8));
            r2.Direction.Should().Be(Vector.UnitY);
        }

        [Test]
        public void Scaling_a_ray()
        {
            var ray = new Ray(new Point(1, 2, 3), Vector.UnitY);
            var m = Matrix4x4.CreateScaling(2, 3, 4);
            var r2 = ray.Transform(m);
            r2.Origin.Should().Be(new Point(2, 6, 12));
            r2.Direction.Should().Be(new Vector(0, 3, 0));
        }
    }
}
