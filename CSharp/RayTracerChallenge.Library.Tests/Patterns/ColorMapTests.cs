// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorMapTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests.Patterns
{
    using FluentAssertions;
    using NUnit.Framework;
    using RayTracerChallenge.Library.Patterns;

    public class ColorMapTests
    {
        [Test]
        public void Ctor_Add_and_AddRange_should_add_the_entries_in_order_of_key_using_stable_sort()
        {
            var map = new ColorMap(
                new ColorMapEntry(0.5, Colors.Red),
                new ColorMapEntry(0.2, Colors.Green),
                new ColorMapEntry(0.199999, Colors.Blue));

            map.Entries.Should()
                .HaveCount(3)
                .And.ContainInOrder(
                    new ColorMapEntry(0.2, Colors.Green),
                    new ColorMapEntry(0.199999, Colors.Blue),
                    new ColorMapEntry(0.5, Colors.Red));
        }

        [Test]
        public void GetColor_should_return_black_for_an_empty_map()
        {
            var map = new ColorMap();
            map.GetColor(0).Should().Be(Colors.Black);
        }

        [Test]
        public void GetColor_should_interpolate_between_black_and_the_entries()
        {
            var map = new ColorMap(new ColorMapEntry(0.5, Colors.White), new ColorMapEntry(0.75, Colors.Red));
            map.GetColor(0).Should().Be(Colors.Black);
            map.GetColor(0.1).Should().Be(new Color(0.2, 0.2, 0.2));
            map.GetColor(0.3).Should().Be(new Color(0.6, 0.6, 0.6));
            map.GetColor(0.5).Should().Be(Colors.White);
            map.GetColor(0.65).Should().Be(new Color(1, 0.4, 0.4));
            map.GetColor(0.75).Should().Be(Colors.Red);
            map.GetColor(0.85).Should().Be(new Color(0.6, 0, 0));
            map.GetColor(1).Should().Be(Colors.Black);
        }
    }
}
