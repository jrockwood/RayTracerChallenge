// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="PerlinTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests.Patterns
{
    using FluentAssertions;
    using NUnit.Framework;
    using RayTracerChallenge.Library.Patterns;

    public class PerlinTests
    {
        [Test]
        public void CalculatePerlin_should_return_the_same_value_for_a_series_of_x_values()
        {
            var perlin = new Perlin();
            double p1 = perlin.CalculatePerlin(0, 0, 0);
            double p2 = perlin.CalculatePerlin(1, 0, 0);
            double p3 = perlin.CalculatePerlin(2, 0, 0);
            double p4 = perlin.CalculatePerlin(3, 0, 0);

            p1.Should().Be(p2);
            p1.Should().Be(p3);
            p1.Should().Be(p4);
        }
    }
}
