// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="IntersectionStateTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests
{
    using FluentAssertions;
    using NUnit.Framework;
    using RayTracerChallenge.Library.Shapes;

    public class IntersectionStateTests
    {
        [Test]
        public void Precomputing_the_state_of_an_intersection()
        {
            var ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var shape = new Sphere();
            var intersection = new Intersection(4, shape);
            var state = IntersectionState.Create(intersection, ray);
            state.T.Should().Be(intersection.T);
            state.Shape.Should().Be(intersection.Shape);
            state.Point.Should().Be(new Point(0, 0, -1));
            state.Eye.Should().Be(new Vector(0, 0, -1));
            state.Normal.Should().Be(new Vector(0, 0, -1));
        }

        [Test]
        public void Precomputing_the_hit_when_an_intersection_occurs_on_the_outside()
        {
            var ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var shape = new Sphere();
            var intersection = new Intersection(4, shape);
            var state = IntersectionState.Create(intersection, ray);
            state.IsInside.Should().BeFalse();
        }

        [Test]
        public void Precomputing_the_hit_when_an_intersection_occurs_on_the_inside()
        {
            var ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
            var shape = new Sphere();
            var intersection = new Intersection(1, shape);
            var state = IntersectionState.Create(intersection, ray);
            state.Point.Should().Be(new Point(0, 0, 1));
            state.Eye.Should().Be(new Vector(0, 0, -1));
            state.IsInside.Should().BeTrue();

            // Normal would have been (0, 0, 1) but is inverted!
            state.Normal.Should().Be(new Vector(0, 0, -1));
        }
    }
}
