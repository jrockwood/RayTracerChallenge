// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckerPatternTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests.Patterns
{
    using FluentAssertions;
    using NUnit.Framework;
    using RayTracerChallenge.Library.Patterns;

    public class CheckerPatternTests
    {
        [Test]
        public void ColorAt_should_repeat_in_x()
        {
            var pattern = new CheckerPattern(Colors.White, Colors.Black);
            pattern.ColorAt(new Point(0, 0, 0)).Should().Be(Colors.White);
            pattern.ColorAt(new Point(0.99, 0, 0)).Should().Be(Colors.White);
            pattern.ColorAt(new Point(1.01, 0, 0)).Should().Be(Colors.Black);
        }

        [Test]
        public void ColorAt_should_repeat_in_y()
        {
            var pattern = new CheckerPattern(Colors.White, Colors.Black);
            pattern.ColorAt(new Point(0, 0, 0)).Should().Be(Colors.White);
            pattern.ColorAt(new Point(0, 0.99, 0)).Should().Be(Colors.White);
            pattern.ColorAt(new Point(0, 1.01, 0)).Should().Be(Colors.Black);
        }

        [Test]
        public void ColorAt_should_repeat_in_z()
        {
            var pattern = new CheckerPattern(Colors.White, Colors.Black);
            pattern.ColorAt(new Point(0, 0, 0)).Should().Be(Colors.White);
            pattern.ColorAt(new Point(0, 0, 0.99)).Should().Be(Colors.White);
            pattern.ColorAt(new Point(0, 0, 1.01)).Should().Be(Colors.Black);
        }
    }
}
