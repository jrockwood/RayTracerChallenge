// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="IntersectionTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests
{
    using FluentAssertions;
    using NUnit.Framework;
    using RayTracerChallenge.Library.Shapes;

    public class IntersectionTests
    {
        [Test]
        public void An_intersection_encapsulates_t_and_a_shape()
        {
            var s = new Sphere();
            var i = new Intersection(3.5, s);
            i.T.Should().Be(3.5);
            i.Shape.Should().Be(s);
        }
    }
}
