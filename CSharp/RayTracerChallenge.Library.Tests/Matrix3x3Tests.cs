// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Matrix3x3Tests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;

    public class Matrix3x3Tests
    {
        [Test]
        public void A_3x3_matrix_should_store_the_rows_and_columns()
        {
            var matrix = new Matrix3x3(
                1, 2, 3,
                4.5f, 5.5f, 6.5f,
                7, 8, 9);

            matrix.M00.Should().Be(1);
            matrix.M01.Should().Be(2);
            matrix.M02.Should().Be(3);

            matrix.M10.Should().Be(4.5f);
            matrix.M11.Should().Be(5.5f);
            matrix.M12.Should().Be(6.5f);

            matrix.M20.Should().Be(7);
            matrix.M21.Should().Be(8);
            matrix.M22.Should().Be(9);
        }

        [Test]
        public void Equality_should_be_true_for_identical_matrices()
        {
            var a = new Matrix3x3(1, 2, 3, 4, 5, 6, 7, 8, 9);
            var b = new Matrix3x3(1, 2, 3, 4, 5, 6, 7, 8, 9);
            a.Should().Be(b);
            (a == b).Should().BeTrue();
            (a != b).Should().BeFalse();
            a.GetHashCode().Should().Be(b.GetHashCode());
        }

        [Test]
        public void Equality_should_be_true_for_roughly_equivalent_matrices()
        {
            var a = new Matrix3x3(1, 2, 3, 4, 5, 6, 7, 8, 9);
            var b = new Matrix3x3(
                0.999999f, 2.000001f, 2.999999f,
                4.000001f, 4.999999f, 6.000001f,
                6.999999f, 8.000001f, 8.999999f);

            a.Should().Be(b);
            (a == b).Should().BeTrue();
            (a != b).Should().BeFalse();
            a.GetHashCode().Should().Be(b.GetHashCode());
        }

        [Test]
        public void Equality_should_be_false_for_different_matrices()
        {
            var a = new Matrix3x3(1, 2, 3, 4, 5, 6, 7, 8, 9);
            var b = new Matrix3x3(1, 2, 3, 4, 5, 6, 70, 80, 90);
            a.Should().NotBe(b);
            (a == b).Should().BeFalse();
            (a != b).Should().BeTrue();
            a.GetHashCode().Should().NotBe(b.GetHashCode());
        }

        [Test]
        public void ToString_should_display_in_a_friendly_square()
        {
            new Matrix3x3(1, 2, 3, 4, 5, 6, 7, 8, 9).ToString().Should().Be("|1 2 3|\n|4 5 6|\n|7 8 9|");
        }

        [Test]
        public void Indexer_should_return_each_value()
        {
            var matrix = new Matrix3x3(1, 2, 3, 4, 5, 6, 7, 8, 9);
            matrix[0, 0].Should().Be(1);
            matrix[0, 1].Should().Be(2);
            matrix[0, 2].Should().Be(3);

            matrix[1, 0].Should().Be(4);
            matrix[1, 1].Should().Be(5);
            matrix[1, 2].Should().Be(6);

            matrix[2, 0].Should().Be(7);
            matrix[2, 1].Should().Be(8);
            matrix[2, 2].Should().Be(9);
        }

        [Test]
        public void Indexer_should_throw_if_out_of_bounds()
        {
            Action action = () => _ = Matrix3x3.Identity[3, 0];
            action.Should().Throw<IndexOutOfRangeException>();
        }
    }
}
