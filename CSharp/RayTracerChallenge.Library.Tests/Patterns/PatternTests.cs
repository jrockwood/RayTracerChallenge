// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="PatternTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests.Patterns
{
    using FluentAssertions;
    using NUnit.Framework;
    using RayTracerChallenge.Library.Patterns;
    using RayTracerChallenge.Library.Shapes;

    public class PatternTests
    {
        public class TestPattern : Pattern
        {
            public TestPattern(Matrix4x4? transform = null)
                : base(transform)
            {
            }

            public override Color ColorAt(Point point)
            {
                return new Color(point.X, point.Y, point.Z);
            }
        }

        [Test]
        public void Ctor_should_default_the_transformation_matrix_to_the_identity_matrix()
        {
            var pattern = new TestPattern();
            pattern.Transform.Should().Be(Matrix4x4.Identity);
        }

        [Test]
        public void Ctor_should_store_the_transformation_matrix()
        {
            var pattern = new TestPattern(Matrix4x4.CreateTranslation(1, 2, 3));
            pattern.Transform.Should().Be(Matrix4x4.CreateTranslation(1, 2, 3));
        }

        [Test]
        public void ColorOnShapeAt_should_use_the_shape_transformation()
        {
            var shape = new Sphere(transform: Matrix4x4.CreateScaling(2, 2, 2));
            var pattern = new TestPattern();
            Color color = pattern.ColorOnShapeAt(shape, new Point(2, 3, 4));
            color.Should().Be(new Color(1, 1.5, 2));
        }

        [Test]
        public void ColorOnShapeAt_should_use_the_pattern_transformation()
        {
            var shape = new Sphere();
            var pattern = new TestPattern(Matrix4x4.CreateScaling(2, 2, 2));
            Color color = pattern.ColorOnShapeAt(shape, new Point(2, 3, 4));
            color.Should().Be(new Color(1, 1.5, 2));
        }

        [Test]
        public void ColorOnShapeAt_should_use_both_the_shape_and_pattern_transformation()
        {
            var shape = new Sphere(transform: Matrix4x4.CreateScaling(2, 2, 2));
            var pattern = new TestPattern(Matrix4x4.CreateTranslation(0.5, 1, 1.5));
            Color color = pattern.ColorOnShapeAt(shape, new Point(2.5, 3, 3.5));
            color.Should().Be(new Color(0.75, 0.5, 0.25));
        }
    }
}
