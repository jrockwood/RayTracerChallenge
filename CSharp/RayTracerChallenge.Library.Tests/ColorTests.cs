// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests
{
    using FluentAssertions;
    using NUnit.Framework;

    public class ColorTests
    {
        [Test]
        public void Colors_are_red_green_blue_tuples()
        {
            var color = new Color(-0.5, 0.4, 1.7);
            color.Red.Should().Be(-0.5);
            color.Green.Should().Be(0.4);
            color.Blue.Should().Be(1.7);
        }

        [Test]
        public void Equality_should_be_equal_for_two_colors_that_are_approximately_equal()
        {
            var c1 = new Color(0.123456, 0.123456, 0.123456);
            var c2 = new Color(0.123457, 0.123457, 0.123457);
            c1.Should().Be(c2);
            c1.GetHashCode().Should().Be(c2.GetHashCode());
            (c1 == c2).Should().BeTrue();
            (c1 != c2).Should().BeFalse();
        }

        [Test]
        public void Adding_colors()
        {
            var c1 = new Color(0.9, 0.6, 0.75);
            var c2 = new Color(0.7, 0.1, 0.25);
            (c1 + c2).Should().Be(new Color(1.6, 0.7, 1.0));
        }

        [Test]
        public void Subtracting_colors()
        {
            var c1 = new Color(0.9, 0.6, 0.75);
            var c2 = new Color(0.7, 0.1, 0.25);
            (c1 - c2).Should().Be(new Color(0.2, 0.5, 0.5));
        }

        [Test]
        public void Multiplying_a_color_by_a_scalar()
        {
            var c = new Color(0.2, 0.3, 0.4);
            (c * 2).Should().Be(new Color(0.4, 0.6, 0.8));
            (c * 2d).Should().Be(new Color(0.4, 0.6, 0.8));
        }

        [Test]
        public void Multiplying_colors()
        {
            var c1 = new Color(1, 0.2, 0.4);
            var c2 = new Color(0.9, 1, 0.1);
            (c1 * c2).Should().Be(new Color(0.9, 0.2, 0.04));
        }
    }
}
