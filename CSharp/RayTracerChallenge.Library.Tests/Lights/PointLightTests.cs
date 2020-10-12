// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="PointLightTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests.Lights
{
    using FluentAssertions;
    using NUnit.Framework;
    using RayTracerChallenge.Library.Lights;

    public class PointLightTests
    {
        [Test]
        public void A_point_light_has_a_position_and_intensity()
        {
            var intensity = Colors.White;
            var position = Point.Zero;
            var light = new PointLight(position, intensity);
            light.Position.Should().Be(position);
            light.Intensity.Should().Be(intensity);
        }
    }
}
