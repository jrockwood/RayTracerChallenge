// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="GradientPatternTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests.Patterns
{
    using FluentAssertions;
    using NUnit.Framework;
    using RayTracerChallenge.Library.Patterns;

    public class GradientPatternTests
    {
        [Test]
        public void ColorAt_should_linearly_interpolate_between_colors()
        {
            var pattern = new GradientPattern(Colors.White, Colors.Black);
            pattern.ColorAt(new Point(0, 0, 0)).Should().Be(Colors.White);
            pattern.ColorAt(new Point(0.25, 0, 0)).Should().Be(new Color(0.75, 0.75, 0.75));
            pattern.ColorAt(new Point(0.5, 0, 0)).Should().Be(new Color(0.5, 0.5, 0.5));
            pattern.ColorAt(new Point(0.75, 0, 0)).Should().Be(new Color(0.25, 0.25, 0.25));
        }
    }
}
