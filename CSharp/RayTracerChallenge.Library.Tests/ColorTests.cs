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
            var color = new Color(-0.5f, 0.4f, 1.7f);
            color.Red.Should().Be(-0.5f);
            color.Green.Should().Be(0.4f);
            color.Blue.Should().Be(1.7f);
        }

        [Test]
        public void Equality_should_be_equal_for_two_colors_that_are_approximately_equal()
        {
            var c1 = new Color(0.123456f, 0.123456f, 0.123456f);
            var c2 = new Color(0.123457f, 0.123457f, 0.123457f);
            c1.Should().Be(c2);
            c1.GetHashCode().Should().Be(c2.GetHashCode());
            (c1 == c2).Should().BeTrue();
            (c1 != c2).Should().BeFalse();
        }

        [Test]
        public void Adding_colors()
        {
            var c1 = new Color(0.9f, 0.6f, 0.75f);
            var c2 = new Color(0.7f, 0.1f, 0.25f);
            (c1 + c2).Should().Be(new Color(1.6f, 0.7f, 1.0f));
        }

        [Test]
        public void Subtracting_colors()
        {
            var c1 = new Color(0.9f, 0.6f, 0.75f);
            var c2 = new Color(0.7f, 0.1f, 0.25f);
            (c1 - c2).Should().Be(new Color(0.2f, 0.5f, 0.5f));
        }

        [Test]
        public void Multiplying_a_color_by_a_scalar()
        {
            var c = new Color(0.2f, 0.3f, 0.4f);
            (c * 2).Should().Be(new Color(0.4f, 0.6f, 0.8f));
            (c * 2f).Should().Be(new Color(0.4f, 0.6f, 0.8f));
        }

        [Test]
        public void Multiplying_colors()
        {
            var c1 = new Color(1f, 0.2f, 0.4f);
            var c2 = new Color(0.9f, 1f, 0.1f);
            (c1 * c2).Should().Be(new Color(0.9f, 0.2f, 0.04f));
        }
    }
}
