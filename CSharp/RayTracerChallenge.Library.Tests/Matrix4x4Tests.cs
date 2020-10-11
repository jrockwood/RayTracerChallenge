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

        [Test]
        public void Multiplying_two_matrices()
        {
            var matrix1 = new Matrix4x4(
                1, 2, 3, 4,
                5, 6, 7, 8,
                9, 8, 7, 6,
                5, 4, 3, 2);

            var matrix2 = new Matrix4x4(
                -2, 1, 2, 3,
                3, 2, 1, -1,
                4, 3, 6, 5,
                1, 2, 7, 8);

            (matrix1 * matrix2).Should().Be(
                new Matrix4x4(
                    20, 22, 50, 48,
                    44, 54, 114, 108,
                    40, 58, 110, 102,
                    16, 26, 46, 42));
        }

        [Test]
        public void Multiplying_a_matrix_and_a_point()
        {
            var matrix = new Matrix4x4(
                1, 2, 3, 4,
                2, 4, 4, 2,
                8, 6, 4, 1,
                0, 0, 0, 1);

            var point = new Point(1, 2, 3);
            (matrix * point).Should().Be(new Point(18, 24, 33));
        }

        [Test]
        public void Multiplying_a_matrix_and_a_vector()
        {
            var matrix = new Matrix4x4(
                1, 2, 3, 4,
                2, 4, 4, 2,
                8, 6, 4, 1,
                0, 0, 0, 1);

            var vector = new Vector(1, 2, 3);
            (matrix * vector).Should().Be(new Vector(14, 22, 32));
        }

        [Test]
        public void Multiplying_a_matrix_by_the_identity_matrix_should_return_the_same_matrix()
        {
            var matrix = new Matrix4x4(
                0, 1, 2, 4,
                1, 2, 4, 8,
                2, 4, 8, 16,
                4, 8, 16, 32);

            (matrix * Matrix4x4.Identity).Should().Be(matrix);
        }

        [Test]
        public void Multiplying_the_identity_matrix_by_a_point_should_return_the_same_point()
        {
            var p = new Point(1, 2, 3);
            (Matrix4x4.Identity * p).Should().Be(p);
        }

        [Test]
        public void Multiplying_the_identity_matrix_by_a_vector_should_return_the_same_vector()
        {
            var v = new Vector(1, 2, 3);
            (Matrix4x4.Identity * v).Should().Be(v);
        }

        [Test]
        public void Transpose_should_swap_the_rows_and_columns()
        {
            var matrix = new Matrix4x4(
                0, 9, 3, 0,
                9, 8, 0, 8,
                1, 8, 5, 3,
                0, 0, 5, 8);

            var expectedTransposed = new Matrix4x4(
                0, 9, 1, 0,
                9, 8, 8, 0,
                3, 0, 5, 5,
                0, 8, 3, 8);

            matrix.Transpose().Should().Be(expectedTransposed);
        }

        [Test]
        public void Transposing_the_identity_matrix_should_return_the_identity_matrix()
        {
            Matrix4x4.Identity.Transpose().Should().Be(Matrix4x4.Identity);
        }

        [Test]
        public void Submatrix_should_remove_the_specified_row_and_column_and_return_a_3x3_matrix()
        {
            var matrix = new Matrix4x4(
                -6, 1, 1, 6,
                -8, 5, 8, 6,
                -1, 0, 8, 2,
                -7, 1, -1, 1);

            matrix.Submatrix(0, 0).Should().Be(new Matrix3x3(5, 8, 6, 0, 8, 2, 1, -1, 1));
            matrix.Submatrix(0, 1).Should().Be(new Matrix3x3(-8, 8, 6, -1, 8, 2, -7, -1, 1));
            matrix.Submatrix(0, 2).Should().Be(new Matrix3x3(-8, 5, 6, -1, 0, 2, -7, 1, 1));
            matrix.Submatrix(0, 3).Should().Be(new Matrix3x3(-8, 5, 8, -1, 0, 8, -7, 1, -1));

            matrix.Submatrix(1, 0).Should().Be(new Matrix3x3(1, 1, 6, 0, 8, 2, 1, -1, 1));
            matrix.Submatrix(1, 1).Should().Be(new Matrix3x3(-6, 1, 6, -1, 8, 2, -7, -1, 1));
            matrix.Submatrix(1, 2).Should().Be(new Matrix3x3(-6, 1, 6, -1, 0, 2, -7, 1, 1));
            matrix.Submatrix(1, 3).Should().Be(new Matrix3x3(-6, 1, 1, -1, 0, 8, -7, 1, -1));

            matrix.Submatrix(2, 0).Should().Be(new Matrix3x3(1, 1, 6, 5, 8, 6, 1, -1, 1));
            matrix.Submatrix(2, 1).Should().Be(new Matrix3x3(-6, 1, 6, -8, 8, 6, -7, -1, 1));
            matrix.Submatrix(2, 2).Should().Be(new Matrix3x3(-6, 1, 6, -8, 5, 6, -7, 1, 1));
            matrix.Submatrix(2, 3).Should().Be(new Matrix3x3(-6, 1, 1, -8, 5, 8, -7, 1, -1));

            matrix.Submatrix(3, 0).Should().Be(new Matrix3x3(1, 1, 6, 5, 8, 6, 0, 8, 2));
            matrix.Submatrix(3, 1).Should().Be(new Matrix3x3(-6, 1, 6, -8, 8, 6, -1, 8, 2));
            matrix.Submatrix(3, 2).Should().Be(new Matrix3x3(-6, 1, 6, -8, 5, 6, -1, 0, 2));
            matrix.Submatrix(3, 3).Should().Be(new Matrix3x3(-6, 1, 1, -8, 5, 8, -1, 0, 8));
        }

        [Test]
        public void Minor_should_return_the_minor_of_the_matrix_which_is_the_determinant_of_the_submatrix()
        {
            var matrix = new Matrix4x4(
                3, 5, 0, 4,
                2, -1, -7, 9,
                6, -1, 5, 2,
                3, 7, -4, 6);

            matrix.Minor(0, 0).Should().Be(-457);
            matrix.Minor(0, 1).Should().Be(-65);
            matrix.Minor(0, 2).Should().Be(395);
            matrix.Minor(0, 3).Should().Be(-416);

            matrix.Minor(1, 0).Should().Be(66);
            matrix.Minor(1, 1).Should().Be(-42);
            matrix.Minor(1, 2).Should().Be(-30);
            matrix.Minor(1, 3).Should().Be(102);

            matrix.Minor(2, 0).Should().Be(182);
            matrix.Minor(2, 1).Should().Be(34);
            matrix.Minor(2, 2).Should().Be(-64);
            matrix.Minor(2, 3).Should().Be(94);
        }

        [Test]
        public void Cofactor_should_return_the_minor_of_the_matrix_which_is_the_determinant_of_the_submatrix()
        {
            var matrix = new Matrix4x4(
                3, 5, 0, 4,
                2, -1, -7, 9,
                6, -1, 5, 2,
                3, 7, -4, 6);

            matrix.Cofactor(0, 0).Should().Be(-457);
            matrix.Cofactor(0, 1).Should().Be(65);
            matrix.Cofactor(0, 2).Should().Be(395);
            matrix.Cofactor(0, 3).Should().Be(416);

            matrix.Cofactor(1, 0).Should().Be(-66);
            matrix.Cofactor(1, 1).Should().Be(-42);
            matrix.Cofactor(1, 2).Should().Be(30);
            matrix.Cofactor(1, 3).Should().Be(102);

            matrix.Cofactor(2, 0).Should().Be(182);
            matrix.Cofactor(2, 1).Should().Be(-34);
            matrix.Cofactor(2, 2).Should().Be(-64);
            matrix.Cofactor(2, 3).Should().Be(-94);
        }

        [Test]
        public void Determinant_should_return_the_determinant_of_the_matrix()
        {
            var matrix = new Matrix4x4(
                -2, -8, 3, 5,
                -3, 1, 7, 3,
                1, 2, -9, 6,
                -6, 7, 7, -9);

            matrix.Determinant.Should().Be(-4071);
        }
    }
}
