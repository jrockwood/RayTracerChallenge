// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ShapeTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests.Shapes
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;
    using RayTracerChallenge.Library.Shapes;

    public class ShapeTests
    {
        private sealed class TestShape : Shape
        {
            public TestShape(Matrix4x4? transform = null, Material? material = null)
                : base(transform, material)
            {
            }

            public Ray? SavedLocalRay { get; private set; }

            public override Shape WithTransform(Matrix4x4 value)
            {
                return new TestShape(value, Material);
            }

            public override Shape WithMaterial(Material value)
            {
                return new TestShape(Transform, value);
            }

            protected override IntersectionList LocalIntersect(Ray localRay)
            {
                SavedLocalRay = localRay;
                return new IntersectionList();
            }

            protected override Vector LocalNormalAt(Point localPoint)
            {
                return new Vector(localPoint.X, localPoint.Y, localPoint.Z);
            }
        }

        [Test]
        public void A_Shape_should_default_to_the_identity_matrix_for_the_transform()
        {
            var shape = new TestShape();
            shape.Transform.Should().Be(Matrix4x4.Identity);
        }

        [Test]
        public void Ctor_should_store_the_transform_and_material()
        {
            var shape = new TestShape(Matrix4x4.CreateTranslation(2, 3, 4), new Material(Colors.Red));
            shape.Transform.Should().Be(Matrix4x4.CreateTranslation(2, 3, 4));
            shape.Material.Color.Should().Be(Colors.Red);
        }

        [Test]
        public void Shape_should_have_a_default_material()
        {
            var shape = new TestShape();
            shape.Material.Should().BeEquivalentTo(new Material());
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

        [Test]
        public void NormalAt_should_calculate_the_normal_ona_translated_shape()
        {
            var shape = new TestShape(Matrix4x4.CreateTranslation(0, 1, 0));
            var normal = shape.NormalAt(new Point(0, 1.70711f, -0.70711f));
            normal.Should().Be(new Vector(0, 0.70711f, -0.70711f));
        }

        [Test]
        public void NormalAt_should_calculate_the_normal_on_a_transformed_shape()
        {
            var shape = new TestShape(Matrix4x4.CreateRotationZ(MathF.PI / 5).Scale(1, 0.5f, 1));
            var normal = shape.NormalAt(new Point(0, MathF.Sqrt(2) / 2, -MathF.Sqrt(2) / 2));
            normal.Should().Be(new Vector(0, 0.97014f, -0.24254f));
        }
    }
}
