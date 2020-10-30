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
                5.5, 6.5, 7.5, 8.5,
                9, 10, 11, 12,
                13.5, 14.5, 15.5, 16.5);

            matrix.M00.Should().Be(1);
            matrix.M01.Should().Be(2);
            matrix.M02.Should().Be(3);
            matrix.M03.Should().Be(4);

            matrix.M10.Should().Be(5.5);
            matrix.M11.Should().Be(6.5);
            matrix.M12.Should().Be(7.5);
            matrix.M13.Should().Be(8.5);

            matrix.M20.Should().Be(9);
            matrix.M21.Should().Be(10);
            matrix.M22.Should().Be(11);
            matrix.M23.Should().Be(12);

            matrix.M30.Should().Be(13.5);
            matrix.M31.Should().Be(14.5);
            matrix.M32.Should().Be(15.5);
            matrix.M33.Should().Be(16.5);
        }

        //// ===========================================================================================================
        //// Equality Tests
        //// ===========================================================================================================

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
                0.999999, 2.000001, 2.999999, 4.000001,
                4.999999, 6.000001, 6.999999, 8.000001,
                8.999999, 10.000001, 10.999999, 12.000001,
                12.999999, 14.000001, 14.999999, 16.000001);

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

        //// ===========================================================================================================
        //// IsIdentity Tests
        //// ===========================================================================================================

        [Test]
        public void IsIdentity_should_return_true_for_exact_equality()
        {
            Matrix4x4.Identity.IsIdentity.Should().BeTrue();
        }

        [Test]
        public void IsIdentity_should_return_true_for_approximate_equality()
        {
            var almostIdentity = new Matrix4x4(
                1.000001, 0, 0, 0,
                0, 0.99999, 0, 0,
                0, 0, 1, 0,
                0, 0, 0.000001, 1);

            almostIdentity.IsIdentity.Should().BeTrue();
        }

        [Test]
        public void IsIdentity_should_return_false_if_it_is_not_the_identity_matrix()
        {
            var notIdentity = new Matrix4x4(
                2, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1);

            notIdentity.IsIdentity.Should().BeFalse();
        }

        //// ===========================================================================================================
        //// ToString Tests
        //// ===========================================================================================================

        [Test]
        public void ToString_should_display_in_a_friendly_square()
        {
            new Matrix4x4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16).ToString()
                .Should()
                .Be("|1 2 3 4|\n|5 6 7 8|\n|9 10 11 12|\n|13 14 15 16|");
        }

        //// ===========================================================================================================
        //// Indexer Tests
        //// ===========================================================================================================

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

        //// ===========================================================================================================
        //// Multiplication Tests
        //// ===========================================================================================================

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

        //// ===========================================================================================================
        //// Transpose Tests
        //// ===========================================================================================================

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

        //// ===========================================================================================================
        //// Submatrix Tests
        //// ===========================================================================================================

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

        //// ===========================================================================================================
        //// Minor Tests
        //// ===========================================================================================================

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

        //// ===========================================================================================================
        //// Cofactor Tests
        //// ===========================================================================================================

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

        //// ===========================================================================================================
        //// Determinant Tests
        //// ===========================================================================================================

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

        //// ===========================================================================================================
        //// IsInvertible and Invert Tests
        //// ===========================================================================================================

        [Test]
        public void IsInvertible_should_return_true_if_the_determinant_is_non_zero()
        {
            var matrix = new Matrix4x4(
                6, 4, 4, 4,
                5, 5, 7, 6,
                4, -9, 3, -7,
                9, 1, 7, -6);

            matrix.Determinant.Should().Be(-2120);
            matrix.IsInvertible.Should().BeTrue();
        }

        [Test]
        public void IsInvertible_should_return_false_if_the_determinant_is_zero()
        {
            var matrix = new Matrix4x4(
                -4, 2, -2, -3,
                9, 6, 2, 6,
                0, -5, 1, -5,
                0, 0, 0, 0);

            matrix.Determinant.Should().Be(0);
            matrix.IsInvertible.Should().BeFalse();
        }

        [Test]
        public void Invert_should_return_the_inverse_of_a_matrix()
        {
            // Test 1
            var matrix = new Matrix4x4(
                -5, 2, 6, -8,
                1, -5, 1, 8,
                7, 7, -6, -7,
                1, -3, 7, 4);

            var invertedMatrix = new Matrix4x4(
                0.21805, 0.45113, 0.24060, -0.04511,
                -0.80827, -1.45677, -0.44361, 0.52068,
                -0.07895, -0.22368, -0.05263, 0.19737,
                -0.52256, -0.81391, -0.30075, 0.30639);

            matrix.Invert().Should().Be(invertedMatrix);

            // Test 2
            matrix = new Matrix4x4(
                8, -5, 9, 2,
                7, 5, 6, 1,
                -6, 0, 9, 6,
                -3, 0, -9, -4);

            invertedMatrix = new Matrix4x4(
                -0.15385, -0.15385, -0.28205, -0.53846,
                -0.07692, 0.12308, 0.02564, 0.03077,
                0.35897, 0.35897, 0.43590, 0.92308,
                -0.69231, -0.69231, -0.76923, -1.92308);

            matrix.Invert().Should().Be(invertedMatrix);

            // Test 3
            matrix = new Matrix4x4(
                9, 3, 0, 9,
                -5, -2, -6, -3,
                -4, 9, 6, 4,
                -7, 6, 6, 2);

            invertedMatrix = new Matrix4x4(
                -0.04074, -0.07778, 0.14444, -0.22222,
                -0.07778, 0.03333, 0.36667, -0.33333,
                -0.02901, -0.14630, -0.10926, 0.12963,
                0.17778, 0.06667, -0.26667, 0.33333);

            matrix.Invert().Should().Be(invertedMatrix);
        }

        [Test]
        public void Invert_should_throw_when_trying_to_invert_the_matrix_if_it_is_not_invertible()
        {
            var matrix = new Matrix4x4(
                -4, 2, -2, -3,
                9, 6, 2, 6,
                0, -5, 1, -5,
                0, 0, 0, 0);

            Action action = () => matrix.Invert();
            action.Should().ThrowExactly<InvalidOperationException>();
        }

        [Test]
        public void Invert_should_return_the_same_matrix_if_multiplying_a_product_by_its_inverse()
        {
            var a = new Matrix4x4(
                3, -9, 7, 3,
                3, -8, 2, -9,
                -4, 4, 4, 1,
                -6, 5, -1, 1);

            var b = new Matrix4x4(
                8, 2, 2, 2,
                3, -1, 7, 0,
                7, 0, 5, 4,
                6, -2, 0, 5);

            var c = a * b;
            (c * b.Invert()).Should().Be(a);
        }

        //// ===========================================================================================================
        //// CreateTranslation Tests
        //// ===========================================================================================================

        [Test]
        public void Multiplying_by_a_translation_matrix()
        {
            var transform = Matrix4x4.CreateTranslation(5, -3, 2);
            var p = new Point(-3, 4, 5);
            (transform * p).Should().Be(new Point(2, 1, 7));
        }

        [Test]
        public void Multiplying_by_the_inverse_of_a_translation_matrix()
        {
            var transform = Matrix4x4.CreateTranslation(5, -3, 2);
            var inverse = transform.Invert();
            var p = new Point(-3, 4, 5);
            (inverse * p).Should().Be(new Point(-8, 7, 3));
        }

        [Test]
        public void Translation_does_not_affect_vectors()
        {
            var transform = Matrix4x4.CreateTranslation(5, -3, 2);
            var v = new Vector(-3, 4, 5);
            (transform * v).Should().Be(v);
        }

        //// ===========================================================================================================
        //// CreateScaling Tests
        //// ===========================================================================================================

        [Test]
        public void CreateScaling_should_scale_a_point()
        {
            var transform = Matrix4x4.CreateScaling(2, 3, 4);
            var point = new Point(-4, 6, 8);
            (transform * point).Should().Be(new Point(-8, 18, 32));
        }

        [Test]
        public void CreateScaling_should_scale_a_vector()
        {
            var transform = Matrix4x4.CreateScaling(2, 3, 4);
            var vector = new Vector(-4, 6, 8);
            (transform * vector).Should().Be(new Vector(-8, 18, 32));
        }

        [Test]
        public void CreateScaling_should_scale_in_the_reverse_direction_by_multiplying_by_the_inverse_of_the_scaling_matrix()
        {
            var transform = Matrix4x4.CreateScaling(2, 3, 4);
            var inverse = transform.Invert();
            var vector = new Vector(-4, 6, 8);
            (inverse * vector).Should().Be(new Vector(-2, 2, 2));
        }

        [Test]
        public void CreateScaling_should_reflect_along_an_axis_by_scaling_a_negative_value()
        {
            var transform = Matrix4x4.CreateScaling(-1, 1, 1);
            var point = new Point(2, 3, 4);
            (transform * point).Should().Be(new Point(-2, 3, 4));
        }

        //// ===========================================================================================================
        //// CreateRotationX Tests
        //// ===========================================================================================================

        [Test]
        public void CreateRotationX_should_rotate_a_point_around_the_x_axis()
        {
            var point = new Point(0, 1, 0);
            var halfQuarter = Matrix4x4.CreateRotationX(Math.PI / 4);
            var fullQuarter = Matrix4x4.CreateRotationX(Math.PI / 2);

            (halfQuarter * point).Should().Be(new Point(0, Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
            (fullQuarter * point).Should().Be(new Point(0, 0, 1));
        }

        [Test]
        public void
            CreateRotationX_should_rotate_a_point_around_the_x_axis_in_the_opposite_direction_by_multiplying_by_the_inverse_matrix()
        {
            var point = new Point(0, 1, 0);
            var halfQuarter = Matrix4x4.CreateRotationX(Math.PI / 4);
            var inverse = halfQuarter.Invert();
            (inverse * point).Should().Be(new Point(0, Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2));
        }

        //// ===========================================================================================================
        //// CreateRotationY Tests
        //// ===========================================================================================================

        [Test]
        public void CreateRotationY_should_rotate_a_point_around_the_x_axis()
        {
            var point = new Point(0, 0, 1);
            var halfQuarter = Matrix4x4.CreateRotationY(Math.PI / 4);
            var fullQuarter = Matrix4x4.CreateRotationY(Math.PI / 2);

            (halfQuarter * point).Should().Be(new Point(Math.Sqrt(2) / 2, 0, Math.Sqrt(2) / 2));
            (fullQuarter * point).Should().Be(new Point(1, 0, 0));
        }

        //// ===========================================================================================================
        //// CreateRotationZ Tests
        //// ===========================================================================================================

        [Test]
        public void CreateRotationZ_should_rotate_a_point_around_the_x_axis()
        {
            var point = new Point(0, 1, 0);
            var halfQuarter = Matrix4x4.CreateRotationZ(Math.PI / 4);
            var fullQuarter = Matrix4x4.CreateRotationZ(Math.PI / 2);

            (halfQuarter * point).Should().Be(new Point(-Math.Sqrt(2) / 2, Math.Sqrt(2) / 2, 0));
            (fullQuarter * point).Should().Be(new Point(-1, 0, 0));
        }

        //// ===========================================================================================================
        //// CreateShearing Tests
        //// ===========================================================================================================

        [Test]
        public void CreateShearing_should_move_x_in_proportion_to_y()
        {
            var transform = Matrix4x4.CreateShearing(1, 0, 0, 0, 0, 0);
            var p = new Point(2, 3, 4);
            (transform * p).Should().Be(new Point(5, 3, 4));
        }

        [Test]
        public void CreateShearing_should_move_x_in_proportion_to_z()
        {
            var transform = Matrix4x4.CreateShearing(0, 1, 0, 0, 0, 0);
            var p = new Point(2, 3, 4);
            (transform * p).Should().Be(new Point(6, 3, 4));
        }

        [Test]
        public void CreateShearing_should_move_y_in_proportion_to_x()
        {
            var transform = Matrix4x4.CreateShearing(0, 0, 1, 0, 0, 0);
            var p = new Point(2, 3, 4);
            (transform * p).Should().Be(new Point(2, 5, 4));
        }

        [Test]
        public void CreateShearing_should_move_y_in_proportion_to_z()
        {
            var transform = Matrix4x4.CreateShearing(0, 0, 0, 1, 0, 0);
            var p = new Point(2, 3, 4);
            (transform * p).Should().Be(new Point(2, 7, 4));
        }

        [Test]
        public void CreateShearing_should_move_z_in_proportion_to_x()
        {
            var transform = Matrix4x4.CreateShearing(0, 0, 0, 0, 1, 0);
            var p = new Point(2, 3, 4);
            (transform * p).Should().Be(new Point(2, 3, 6));
        }

        [Test]
        public void CreateShearing_should_move_z_in_proportion_to_y()
        {
            var transform = Matrix4x4.CreateShearing(0, 0, 0, 0, 0, 1);
            var p = new Point(2, 3, 4);
            (transform * p).Should().Be(new Point(2, 3, 7));
        }

        //// ===========================================================================================================
        //// Chaining Transformations
        //// ===========================================================================================================

        [Test]
        public void Individual_transformations_are_applied_in_sequence()
        {
            var p = new Point(1, 0, 1);
            var rotation = Matrix4x4.CreateRotationX(Math.PI / 2);
            var scaling = Matrix4x4.CreateScaling(5, 5, 5);
            var translation = Matrix4x4.CreateTranslation(10, 5, 7);

            // apply rotation first
            var p2 = rotation * p;
            p2.Should().Be(new Point(1, -1, 0));

            // then apply scaling
            var p3 = scaling * p2;
            p3.Should().Be(new Point(5, -5, 0));

            // then apply translation
            var p4 = translation * p3;
            p4.Should().Be(new Point(15, 0, 7));
        }

        [Test]
        public void Chained_transformations_should_be_done_in_reverse_order()
        {
            var p = new Point(1, 0, 1);
            var rotation = Matrix4x4.CreateRotationX(Math.PI / 2);
            var scaling = Matrix4x4.CreateScaling(5, 5, 5);
            var translation = Matrix4x4.CreateTranslation(10, 5, 7);

            var transform = translation * scaling * rotation;
            (transform * p).Should().Be(new Point(15, 0, 7));
        }

        [Test]
        public void Chained_transformations_should_correctly_apply_using_the_fluent_API()
        {
            var p = new Point(1, 0, 1);
            var transform = Matrix4x4.Identity
                .RotateX(Math.PI / 2)
                .Scale(5, 5, 5)
                .Translate(10, 5, 7);
            (transform * p).Should().Be(new Point(15, 0, 7));
        }

        //// ===========================================================================================================
        //// CreateLookAt Tests
        //// ===========================================================================================================

        [Test]
        public void CreateLookAt_should_return_the_identity_matrix_for_the_default_orientation()
        {
            var transform = Matrix4x4.CreateLookAt(Point.Zero, new Point(0, 0, -1), Vector.UnitY);
            transform.Should().Be(Matrix4x4.Identity);
        }

        [Test]
        public void
            CreateLookAt_should_be_the_same_as_a_scaling_mirrored_transform_when_looking_in_the_positive_z_direction()
        {
            var transform = Matrix4x4.CreateLookAt(Point.Zero, new Point(0, 0, 1), Vector.UnitY);
            transform.Should().Be(Matrix4x4.CreateScaling(-1, 1, -1));
        }

        [Test]
        public void CreateLookAt_should_move_the_world_and_not_the_eye()
        {
            var transform = Matrix4x4.CreateLookAt(new Point(0, 0, 8), Point.Zero, Vector.UnitY);
            transform.Should().Be(Matrix4x4.CreateTranslation(0, 0, -8));
        }

        [Test]
        public void CreateLookAt_should_move_the_world_using_an_arbitrary_view_transformation()
        {
            var from = new Point(1, 3, 2);
            var to = new Point(4, -2, 8);
            var up = new Vector(1, 1, 0);
            var transform = Matrix4x4.CreateLookAt(from, to, up);

            // prettier-ignore
            var expected = new Matrix4x4(
                -0.50709, 0.50709, 0.67612, -2.36643,
                0.76772, 0.60609, 0.12122, -2.82843,
                -0.35857, 0.59761, -0.71714, 0.00000,
                0.00000, 0.00000, 0.00000, 1.00000);

            transform.Should().Be(expected);
        }
    }
}
