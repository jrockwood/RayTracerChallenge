// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ShapeTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests.Shapes
{
    using FluentAssertions;
    using NUnit.Framework;
    using RayTracerChallenge.Library.Shapes;

    public class ShapeTests
    {
        private sealed class TestShape : Shape
        {
            public TestShape(Matrix4x4? transform = null)
                : base(transform)
            {
            }

            public Ray? SavedLocalRay { get; private set; }

            protected override IntersectionList LocalIntersect(Ray localRay)
            {
                SavedLocalRay = localRay;
                return new IntersectionList();
            }

            public override Shape WithTransform(Matrix4x4 value)
            {
                return new TestShape(value);
            }
        }

        [Test]
        public void A_Shape_should_default_to_the_identity_matrix_for_the_transform()
        {
            var shape = new TestShape();
            shape.Transform.Should().Be(Matrix4x4.Identity);
        }

        [Test]
        public void Ctor_should_store_the_transform()
        {
            var shape = new TestShape(Matrix4x4.CreateTranslation(2, 3, 4));
            shape.Transform.Should().Be(Matrix4x4.CreateTranslation(2, 3, 4));
        }

        [Test]
        public void Intersect_should_translate_world_coordinates_into_local_coordinates()
        {
            var ray = new Ray(new Point(0, 0, -5), Vector.UnitZ);
            var shape = new TestShape(Matrix4x4.CreateScaling(2, 2, 2));
            shape.Intersect(ray);
            shape.SavedLocalRay!.Origin.Should().Be(new Point(0, 0, -2.5f));
            shape.SavedLocalRay!.Direction.Should().Be(new Vector(0, 0, 0.5f));

            shape = (TestShape)shape.WithTransform(Matrix4x4.CreateTranslation(5, 0, 0));
            shape.Intersect(ray);
            shape.SavedLocalRay!.Origin.Should().Be(new Point(-5, 0, -5));
            shape.SavedLocalRay!.Direction.Should().Be(new Vector(0, 0, 1));
        }
    }
}
