﻿// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="CanvasTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests
{
    using System.Collections.Immutable;
    using System.Linq;
    using FluentAssertions;
    using NUnit.Framework;

    public class CanvasTests
    {
        [Test]
        public void Creating_a_canvas()
        {
            var canvas = new Canvas(10, 20, Enumerable.Repeat(Colors.Black, 10 * 20).ToImmutableArray());
            canvas.Width.Should().Be(10);
            canvas.Height.Should().Be(20);

            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    canvas.GetPixel(x, y).Should().Be(Colors.Black);
                }
            }
        }
    }
}
