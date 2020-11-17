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

        [Test]
        public void
            An_intersection_record_may_have_u_and_v_properties_to_help_identify_where_the_intersection_occurred()
        {
            var triangle = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));
            var intersection = new Intersection(3.5, triangle, 0.2, 0.4);
            intersection.U.Should().Be(0.2);
            intersection.V.Should().Be(0.4);
        }
    }
}
