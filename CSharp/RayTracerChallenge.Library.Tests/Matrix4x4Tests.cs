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
                0.21805f, 0.45113f, 0.24060f, -0.04511f,
                -0.80827f, -1.45677f, -0.44361f, 0.52068f,
                -0.07895f, -0.22368f, -0.05263f, 0.19737f,
                -0.52256f, -0.81391f, -0.30075f, 0.30639f);

            matrix.Invert().Should().Be(invertedMatrix);

            // Test 2
            matrix = new Matrix4x4(
                8, -5, 9, 2,
                7, 5, 6, 1,
                -6, 0, 9, 6,
                -3, 0, -9, -4);

            invertedMatrix = new Matrix4x4(
                -0.15385f, -0.15385f, -0.28205f, -0.53846f,
                -0.07692f, 0.12308f, 0.02564f, 0.03077f,
                0.35897f, 0.35897f, 0.43590f, 0.92308f,
                -0.69231f, -0.69231f, -0.76923f, -1.92308f);

            matrix.Invert().Should().Be(invertedMatrix);

            // Test 3
            matrix = new Matrix4x4(
                9, 3, 0, 9,
                -5, -2, -6, -3,
                -4, 9, 6, 4,
                -7, 6, 6, 2);

            invertedMatrix = new Matrix4x4(
                -0.04074f, -0.07778f, 0.14444f, -0.22222f,
                -0.07778f, 0.03333f, 0.36667f, -0.33333f,
                -0.02901f, -0.14630f, -0.10926f, 0.12963f,
                0.17778f, 0.06667f, -0.26667f, 0.33333f);

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
            var halfQuarter = Matrix4x4.CreateRotationX(MathF.PI / 4);
            var fullQuarter = Matrix4x4.CreateRotationX(MathF.PI / 2);

            (halfQuarter * point).Should().Be(new Point(0, MathF.Sqrt(2) / 2, MathF.Sqrt(2) / 2));
            (fullQuarter * point).Should().Be(new Point(0, 0, 1));
        }

        [Test]
        public void
            CreateRotationX_should_rotate_a_point_around_the_x_axis_in_the_opposite_direction_by_multiplying_by_the_inverse_matrix()
        {
            var point = new Point(0, 1, 0);
            var halfQuarter = Matrix4x4.CreateRotationX(MathF.PI / 4);
            var inverse = halfQuarter.Invert();
            (inverse * point).Should().Be(new Point(0, MathF.Sqrt(2) / 2, -MathF.Sqrt(2) / 2));
        }

        //// ===========================================================================================================
        //// CreateRotationY Tests
        //// ===========================================================================================================

        [Test]
        public void CreateRotationY_should_rotate_a_point_around_the_x_axis()
        {
            var point = new Point(0, 0, 1);
            var halfQuarter = Matrix4x4.CreateRotationY(MathF.PI / 4);
            var fullQuarter = Matrix4x4.CreateRotationY(MathF.PI / 2);

            (halfQuarter * point).Should().Be(new Point(MathF.Sqrt(2) / 2, 0, MathF.Sqrt(2) / 2));
            (fullQuarter * point).Should().Be(new Point(1, 0, 0));
        }

        //// ===========================================================================================================
        //// CreateRotationZ Tests
        //// ===========================================================================================================

        [Test]
        public void CreateRotationZ_should_rotate_a_point_around_the_x_axis()
        {
            var point = new Point(0, 1, 0);
            var halfQuarter = Matrix4x4.CreateRotationZ(MathF.PI / 4);
            var fullQuarter = Matrix4x4.CreateRotationZ(MathF.PI / 2);

            (halfQuarter * point).Should().Be(new Point(-MathF.Sqrt(2) / 2, MathF.Sqrt(2) / 2, 0));
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
            var rotation = Matrix4x4.CreateRotationX(MathF.PI / 2);
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
            var rotation = Matrix4x4.CreateRotationX(MathF.PI / 2);
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
                .RotateX(MathF.PI / 2)
                .Scale(5, 5, 5)
                .Translate(10, 5, 7);
            (transform * p).Should().Be(new Point(15, 0, 7));
        }
    }
}
