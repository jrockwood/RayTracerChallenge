// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="BoundingBoxTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests.Shapes
{
    using FluentAssertions;
    using NUnit.Framework;
    using RayTracerChallenge.Library.Shapes;

    public class BoundingBoxTests
    {
        [Test]
        public void BoundingBox_contains_two_points_that_describe_the_bounds()
        {
            var box = new BoundingBox(new Point(-1, -1, -1), new Point(1, 1, 1));
            box.MinPoint.Should().Be(new Point(-1, -1, -1));
            box.MaxPoint.Should().Be(new Point(1, 1, 1));
        }

        [Test]
        public void BoundingBox_should_calculate_the_min_and_max_points_from_a_series_of_points()
        {
            var box = new BoundingBox(new Point(1, -2, 3), new Point(0, 0, 0), new Point(10, -3, 0));
            box.MinPoint.Should().Be(new Point(0, -3, 0));
            box.MaxPoint.Should().Be(new Point(10, 0, 3));
        }
    }
}
