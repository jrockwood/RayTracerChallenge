// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="PointTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests
{
    using FluentAssertions;
    using NUnit.Framework;

    public class PointTests
    {
        [Test]
        public void Points_have_x_y_and_z_coordinates()
        {
            var point = new Point(1f, 2f, 3f);
            point.X.Should().Be(1);
            point.Y.Should().Be(2);
            point.Z.Should().Be(3);
        }

        [Test]
        public void Equality_should_be_equal_for_two_points_that_are_approximately_equal()
        {
            var p1 = new Point(0.123456f, 0.123456f, 0.123456f);
            var p2 = new Point(0.123457f, 0.123457f, 0.123457f);
            p1.Should().Be(p2);
            p1.GetHashCode().Should().Be(p2.GetHashCode());
            (p1 == p2).Should().BeTrue();
            (p1 != p2).Should().BeFalse();
        }

        [Test]
        public void Add_a_vector_to_a_point()
        {
            var point = new Point(3, -2, 5);
            var vector = new Vector(-2, 3, 1);
            (point + vector).Should().Be(new Point(1, 1, 6));
            (vector + point).Should().Be(point + vector);
        }

        [Test]
        public void Subtracting_two_points_gives_a_vector()
        {
            var p1 = new Point(3, 2, 1);
            var p2 = new Point(5, 6, 7);
            (p1 - p2).Should().Be(new Vector(-2, -4, -6));
        }

        [Test]
        public void Subtracting_a_vector_from_a_point()
        {
            var p = new Point(3, 2, 1);
            var v = new Vector(5, 6, 7);
            (p - v).Should().Be(new Point(-2, -4, -6));
        }
    }
}
