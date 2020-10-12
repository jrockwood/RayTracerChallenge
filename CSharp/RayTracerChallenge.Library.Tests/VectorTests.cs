// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="VectorTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;

    public class VectorTests
    {
        //// ===========================================================================================================
        //// Ctor Tests
        //// ===========================================================================================================

        [Test]
        public void Vectors_have_x_y_and_z_coordinates()
        {
            var vector = new Vector(1f, 2f, 3f);
            vector.X.Should().Be(1);
            vector.Y.Should().Be(2);
            vector.Z.Should().Be(3);
        }

        //// ===========================================================================================================
        //// Equality Tests
        //// ===========================================================================================================

        [Test]
        public void Equality_should_be_equal_for_two_vectors_that_are_approximately_equal()
        {
            var v1 = new Vector(0.123456f, 0.123456f, 0.123456f);
            var v2 = new Vector(0.123457f, 0.123457f, 0.123457f);
            v1.Should().Be(v2);
            v1.GetHashCode().Should().Be(v2.GetHashCode());
            (v1 == v2).Should().BeTrue();
            (v1 != v2).Should().BeFalse();
        }

        //// ===========================================================================================================
        //// Mathematical Operations Tests
        //// ===========================================================================================================

        [Test]
        public void Adding_two_vectors()
        {
            var v1 = new Vector(3, -2, 5);
            var v2 = new Vector(-2, 3, 1);
            (v1 + v2).Should().Be(new Vector(1, 1, 6));
        }

        [Test]
        public void Adding_a_point_to_a_vector()
        {
            var v = new Vector(3, -2, 5);
            var p = new Point(-2, 3, 1);
            (v + p).Should().Be(new Point(1, 1, 6));
        }

        [Test]
        public void Subtracting_two_vectors()
        {
            var v1 = new Vector(3, 2, 1);
            var v2 = new Vector(5, 6, 7);
            (v1 - v2).Should().Be(new Vector(-2, -4, -6));
        }

        [Test]
        public void Subtracting_a_vector_from_the_zero_vector()
        {
            var zero = Vector.Zero;
            var v = new Vector(1, -2, 3);
            (zero - v).Should().Be(new Vector(-1, 2, -3));
        }

        [Test]
        public void Negating_a_vector()
        {
            var v = new Vector(1, -2, 3);
            v.Negate().Should().Be(new Vector(-1, 2, -3));
        }

        [Test]
        public void Multiplying_a_vector_by_a_scalar()
        {
            var v = new Vector(1, -2, 3);
            (v * 3.5f).Should().Be(new Vector(3.5f, -7, 10.5f));
        }

        [Test]
        public void Multiplying_a_vector_by_a_fraction()
        {
            var v = new Vector(1, -2, 3);
            (v * 0.5f).Should().Be(new Vector(0.5f, -1, 1.5f));
        }

        [Test]
        public void Dividing_a_vector_by_a_scalar()
        {
            var v = new Vector(1, -2, 3);
            (v / 2f).Should().Be(new Vector(0.5f, -1, 1.5f));
        }

        //// ===========================================================================================================
        //// Magnitude Tests
        //// ===========================================================================================================

        [Test]
        public void Computing_the_magnitude_of_unit_vector_x()
        {
            Vector.UnitX.Magnitude.Should().Be(1);
        }

        [Test]
        public void Computing_the_magnitude_of_unit_vector_y()
        {
            Vector.UnitY.Magnitude.Should().Be(1);
        }

        [Test]
        public void Computing_the_magnitude_of_unit_vector_z()
        {
            Vector.UnitZ.Magnitude.Should().Be(1);
        }

        [Test]
        public void Computing_the_magnitude_of_a_positive_vector()
        {
            new Vector(1, 2, 3).Magnitude.Should().Be(MathF.Sqrt(14));
        }

        [Test]
        public void Computing_the_magnitude_of_a_negative_vector()
        {
            new Vector(-1, -2, -3).Magnitude.Should().Be(MathF.Sqrt(14));
        }

        //// ===========================================================================================================
        //// Normalize Tests
        //// ===========================================================================================================

        [Test]
        public void Normalizing_a_vector_gives_a_unit_vector()
        {
            new Vector(4, 0, 0).Normalize().Should().Be(Vector.UnitX);
            new Vector(1, 2, 3).Normalize().Should().Be(new Vector(0.26726f, 0.53452f, 0.80178f));
        }

        [Test]
        public void The_magnitude_of_a_normalized_vector_should_be_one()
        {
            new Vector(1, 2, 3).Normalize().Magnitude.Should().BeApproximately(1, NumberExtensions.Epsilon);
        }

        //// ===========================================================================================================
        //// Dot and Cross Product Tests
        //// ===========================================================================================================

        [Test]
        public void The_dot_product_of_two_vectors()
        {
            var a = new Vector(1, 2, 3);
            var b = new Vector(2, 3, 4);
            a.Dot(b).Should().Be(20);
        }

        [Test]
        public void The_cross_product_of_two_vectors()
        {
            var a = new Vector(1, 2, 3);
            var b = new Vector(2, 3, 4);
            a.Cross(b).Should().Be(new Vector(-1, 2, -1));
            b.Cross(a).Should().Be(new Vector(1, -2, 1));
        }

        //// ===========================================================================================================
        //// Reflect Tests
        //// ===========================================================================================================

        [Test]
        public void Reflect_should_reflect_a_vector_approaching_at_45_degrees()
        {
            var vector = new Vector(1, -1, 0);
            var normal = Vector.UnitY;
            var reflection = vector.Reflect(normal);
            reflection.Should().Be(new Vector(1, 1, 0));
        }

        [Test]
        public void Reflect_should_reflect_a_vector_off_a_slanted_surface()
        {
            var vector = new Vector(0, -1, 0);
            var normal = new Vector(MathF.Sqrt(2) / 2, MathF.Sqrt(2) / 2, 0);
            var reflection = vector.Reflect(normal);
            reflection.Should().Be(new Vector(1, 0, 0));
        }
    }
}
