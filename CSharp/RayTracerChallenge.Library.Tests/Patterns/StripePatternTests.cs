// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="StripePatternTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests.Patterns
{
    using FluentAssertions;
    using NUnit.Framework;
    using RayTracerChallenge.Library.Patterns;

    public class StripePatternTests
    {
        [Test]
        public void Ctor_should_store_the_two_colors_for_the_stripe()
        {
            var pattern = new StripePattern(Colors.White, Colors.Black);
            pattern.Pattern1.Should().BeEquivalentTo(new SolidColorPattern(Colors.White));
            pattern.Pattern2.Should().BeEquivalentTo(new SolidColorPattern(Colors.Black));
        }

        [Test]
        public void ColorAt_should_return_the_same_color_for_every_y_coordinate()
        {
            var pattern = new StripePattern(Colors.White, Colors.Black);
            pattern.ColorAt(new Point(0, 0, 0)).Should().Be(Colors.White);
            pattern.ColorAt(new Point(0, 1, 0)).Should().Be(Colors.White);
            pattern.ColorAt(new Point(0, 2, 0)).Should().Be(Colors.White);
        }

        [Test]
        public void ColorAt_should_return_the_same_color_for_every_z_coordinate()
        {
            var pattern = new StripePattern(Colors.White, Colors.Black);
            pattern.ColorAt(new Point(0, 0, 0)).Should().Be(Colors.White);
            pattern.ColorAt(new Point(0, 0, 1)).Should().Be(Colors.White);
            pattern.ColorAt(new Point(0, 0, 2)).Should().Be(Colors.White);
        }

        [Test]
        public void ColorAt_should_alternate_colors_with_x_coordinates()
        {
            var pattern = new StripePattern(Colors.White, Colors.Black);
            pattern.ColorAt(new Point(0, 0, 0)).Should().Be(Colors.White);
            pattern.ColorAt(new Point(0.9, 0, 0)).Should().Be(Colors.White);
            pattern.ColorAt(new Point(1, 0, 0)).Should().Be(Colors.Black);
            pattern.ColorAt(new Point(-0.1, 0, 0)).Should().Be(Colors.Black);
            pattern.ColorAt(new Point(-1.1, 0, 0)).Should().Be(Colors.White);
        }
    }
}
