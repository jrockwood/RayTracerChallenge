// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="RingPatternTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests.Patterns
{
    using FluentAssertions;
    using NUnit.Framework;
    using RayTracerChallenge.Library.Patterns;

    public class RingPatternTests
    {
        [Test]
        public void ColorAt_should_extend_in_both_x_and_z()
        {
            var pattern = new RingPattern(Colors.White, Colors.Black);
            pattern.ColorAt(new Point(0, 0, 0)).Should().Be(Colors.White);
            pattern.ColorAt(new Point(1, 0, 0)).Should().Be(Colors.Black);
            pattern.ColorAt(new Point(0, 0, 1)).Should().Be(Colors.Black);

            // 0.708 is just slightly more than sqrt(2)/2
            pattern.ColorAt(new Point(0.708, 0, 0.708)).Should().Be(Colors.Black);
        }
    }
}
