// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Matrix4x4Tests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;

    public class Matrix4x4Tests
    {
        [Test]
        public void A_3x3_matrix_should_store_the_rows_and_columns()
        {
            var matrix = new Matrix4x4(
                1, 2, 3, 4,
                5.5f, 6.5f, 7.5f, 8.5f,
                9, 10, 11, 12,
                13.5f, 14.5f, 15.5f, 16.5f);

            matrix.M00.Should().Be(1);
            matrix.M01.Should().Be(2);
            matrix.M02.Should().Be(3);
            matrix.M03.Should().Be(4);

            matrix.M10.Should().Be(5.5f);
            matrix.M11.Should().Be(6.5f);
            matrix.M12.Should().Be(7.5f);
            matrix.M13.Should().Be(8.5f);

            matrix.M20.Should().Be(9);
            matrix.M21.Should().Be(10);
            matrix.M22.Should().Be(11);
            matrix.M23.Should().Be(12);

            matrix.M30.Should().Be(13.5f);
            matrix.M31.Should().Be(14.5f);
            matrix.M32.Should().Be(15.5f);
            matrix.M33.Should().Be(16.5f);
        }

        [Test]
        public void Equality_should_be_true_for_identical_matrices()
        {
            var a = new Matrix4x4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
            var b = new Matrix4x4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
            a.Should().Be(b);
            (a == b).Should().BeTrue();
            (a != b).Should().BeFalse();
            a.GetHashCode().Should().Be(b.GetHashCode());
        }

        [Test]
        public void Equality_should_be_true_for_roughly_equivalent_matrices()
        {
            var a = new Matrix4x4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
            var b = new Matrix4x4(
                0.999999f, 2.000001f, 2.999999f, 4.000001f,
                4.999999f, 6.000001f, 6.999999f, 8.000001f,
                8.999999f, 10.000001f, 10.999999f, 12.000001f,
                12.999999f, 14.000001f, 14.999999f, 16.000001f);

            a.Should().Be(b);
            (a == b).Should().BeTrue();
            (a != b).Should().BeFalse();
            a.GetHashCode().Should().Be(b.GetHashCode());
        }

        [Test]
        public void Equality_should_be_false_for_different_matrices()
        {
            var a = new Matrix4x4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
            var b = new Matrix4x4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 140, 150, 160);
            a.Should().NotBe(b);
            (a == b).Should().BeFalse();
            (a != b).Should().BeTrue();
            a.GetHashCode().Should().NotBe(b.GetHashCode());
        }

        [Test]
        public void ToString_should_display_in_a_friendly_square()
        {
            new Matrix4x4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16).ToString()
                .Should()
                .Be("|1 2 3 4|\n|5 6 7 8|\n|9 10 11 12|\n|13 14 15 16|");
        }

        [Test]
        public void Indexer_should_return_each_value()
        {
            var matrix = new Matrix4x4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
            matrix[0, 0].Should().Be(1);
            matrix[0, 1].Should().Be(2);
            matrix[0, 2].Should().Be(3);
            matrix[0, 3].Should().Be(4);

            matrix[1, 0].Should().Be(5);
            matrix[1, 1].Should().Be(6);
            matrix[1, 2].Should().Be(7);
            matrix[1, 3].Should().Be(8);

            matrix[2, 0].Should().Be(9);
            matrix[2, 1].Should().Be(10);
            matrix[2, 2].Should().Be(11);
            matrix[2, 3].Should().Be(12);

            matrix[2, 0].Should().Be(9);
            matrix[2, 1].Should().Be(10);
            matrix[2, 2].Should().Be(11);
            matrix[2, 3].Should().Be(12);
        }

        [Test]
        public void Indexer_should_throw_if_out_of_bounds()
        {
            Action action = () => _ = Matrix3x3.Identity[3, 0];
            action.Should().Throw<IndexOutOfRangeException>();
        }
    }
}
