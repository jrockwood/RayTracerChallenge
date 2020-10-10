// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Matrix2x2Tests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;

    public class Matrix2x2Tests
    {
        [Test]
        public void A_2x2_matrix_should_store_the_rows_and_columns()
        {
            var matrix = new Matrix2x2(1, 2, 3.5f, 4.5f);

            matrix.M00.Should().Be(1);
            matrix.M01.Should().Be(2);
            matrix.M10.Should().Be(3.5f);
            matrix.M11.Should().Be(4.5f);
        }

        [Test]
        public void Equality_should_be_true_for_identical_matrices()
        {
            var a = new Matrix2x2(1, 2, 3, 4);
            var b = new Matrix2x2(1, 2, 3, 4);
            a.Should().Be(b);
            (a == b).Should().BeTrue();
            (a != b).Should().BeFalse();
            a.GetHashCode().Should().Be(b.GetHashCode());
        }

        [Test]
        public void Equality_should_be_true_for_roughly_equivalent_matrices()
        {
            var a = new Matrix2x2(1, 2, 3, 4);
            var b = new Matrix2x2(0.999999f, 2.000001f, 2.999999f, 4.000001f);
            a.Should().Be(b);
            (a == b).Should().BeTrue();
            (a != b).Should().BeFalse();
            a.GetHashCode().Should().Be(b.GetHashCode());
        }

        [Test]
        public void Equality_should_be_false_for_different_matrices()
        {
            var a = new Matrix2x2(1, 2, 3, 4);
            var b = new Matrix2x2(10, 20, 30, 40);
            a.Should().NotBe(b);
            (a == b).Should().BeFalse();
            (a != b).Should().BeTrue();
            a.GetHashCode().Should().NotBe(b.GetHashCode());
        }

        [Test]
        public void ToString_should_display_in_a_friendly_square()
        {
            new Matrix2x2(1, 2, 3, 4).ToString().Should().Be("|1 2|\n|3 4|");
            new Matrix2x2(1.1f, 2.22f, 3.333f, 4.4444f).ToString().Should().Be("|1.1 2.22|\n|3.333 4.4444|");
        }

        [Test]
        public void Indexer_should_return_each_value()
        {
            var matrix = new Matrix2x2(1, 2, 3, 4);
            matrix[0, 0].Should().Be(1);
            matrix[0, 1].Should().Be(2);
            matrix[1, 0].Should().Be(3);
            matrix[1, 1].Should().Be(4);
        }

        [Test]
        public void Indexer_should_throw_if_out_of_bounds()
        {
            Action action = () => _ = Matrix2x2.Identity[2, 0];
            action.Should().Throw<IndexOutOfRangeException>();
        }
    }
}
