// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="SolidColorPatternTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests.Patterns
{
    using FluentAssertions;
    using NUnit.Framework;
    using RayTracerChallenge.Library.Patterns;

    public class SolidColorPatternTests
    {
        [Test]
        public void Ctor_should_store_the_color()
        {
            var pattern = new SolidColorPattern(Colors.Red);
            pattern.Color.Should().Be(Colors.Red);
        }

        [Test]
        public void ColorAt_should_return_the_same_color_at_every_point()
        {
            var pattern = new SolidColorPattern(Colors.Cyan);
            pattern.ColorAt(new Point(0, 0, 0)).Should().Be(Colors.Cyan);
            pattern.ColorAt(new Point(0, 1, 0)).Should().Be(Colors.Cyan);
            pattern.ColorAt(new Point(0, 0, 1)).Should().Be(Colors.Cyan);
            pattern.ColorAt(new Point(2, 2, 2)).Should().Be(Colors.Cyan);
        }
    }
}
