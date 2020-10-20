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
                4.5, 5.5, 6.5,
                7, 8, 9);

            matrix.M00.Should().Be(1);
            matrix.M01.Should().Be(2);
            matrix.M02.Should().Be(3);

            matrix.M10.Should().Be(4.5);
            matrix.M11.Should().Be(5.5);
            matrix.M12.Should().Be(6.5);

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
                0.999999, 2.000001, 2.999999,
                4.000001, 4.999999, 6.000001,
                6.999999, 8.000001, 8.999999);

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

        [Test]
        public void Submatrix_should_remove_the_specified_row_and_column_and_return_a_2x2_matrix()
        {
            var matrix = new Matrix3x3(
                1, 5, 0,
                -3, 2, 7,
                0, 6, -3);

            matrix.Submatrix(0, 0).Should().Be(new Matrix2x2(2, 7, 6, -3));
            matrix.Submatrix(0, 1).Should().Be(new Matrix2x2(-3, 7, 0, -3));
            matrix.Submatrix(0, 2).Should().Be(new Matrix2x2(-3, 2, 0, 6));

            matrix.Submatrix(1, 0).Should().Be(new Matrix2x2(5, 0, 6, -3));
            matrix.Submatrix(1, 1).Should().Be(new Matrix2x2(1, 0, 0, -3));
            matrix.Submatrix(1, 2).Should().Be(new Matrix2x2(1, 5, 0, 6));

            matrix.Submatrix(2, 0).Should().Be(new Matrix2x2(5, 0, 2, 7));
            matrix.Submatrix(2, 1).Should().Be(new Matrix2x2(1, 0, -3, 7));
            matrix.Submatrix(2, 2).Should().Be(new Matrix2x2(1, 5, -3, 2));
        }

        [Test]
        public void Minor_should_return_the_minor_of_the_matrix_which_is_the_determinant_of_the_submatrix()
        {
            var matrix = new Matrix3x3(
                3, 5, 0,
                2, -1, -7,
                6, -1, 5);

            matrix.Minor(0, 0).Should().Be(-12);
            matrix.Minor(0, 1).Should().Be(52);
            matrix.Minor(0, 2).Should().Be(4);

            matrix.Minor(1, 0).Should().Be(25);
            matrix.Minor(1, 1).Should().Be(15);
            matrix.Minor(1, 2).Should().Be(-33);

            matrix.Minor(2, 0).Should().Be(-35);
            matrix.Minor(2, 1).Should().Be(-21);
            matrix.Minor(2, 2).Should().Be(-13);
        }

        [Test]
        public void Cofactor_should_return_the_cofactor_of_the_matrix()
        {
            var matrix = new Matrix3x3(
                3, 5, 0,
                2, -1, -7,
                6, -1, 5);

            matrix.Cofactor(0, 0).Should().Be(-12);
            matrix.Cofactor(0, 1).Should().Be(-52);
            matrix.Cofactor(0, 2).Should().Be(4);

            matrix.Cofactor(1, 0).Should().Be(-25);
            matrix.Cofactor(1, 1).Should().Be(15);
            matrix.Cofactor(1, 2).Should().Be(33);

            matrix.Cofactor(2, 0).Should().Be(-35);
            matrix.Cofactor(2, 1).Should().Be(21);
            matrix.Cofactor(2, 2).Should().Be(-13);
        }

        [Test]
        public void Determinant_should_return_the_determinant_of_the_matrix()
        {
            var matrix = new Matrix3x3(
                1, 2, 6,
                -5, 8, -4,
                2, 6, 4);

            matrix.Determinant.Should().Be(-196);
        }
    }
}
