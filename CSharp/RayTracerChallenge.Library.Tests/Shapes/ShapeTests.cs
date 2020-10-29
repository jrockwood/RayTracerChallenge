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
        public void Shape_should_default_to_having_a_null_parent()
        {
            var shape = new TestShape();
            shape.Parent.Should().BeNull();
        }

        [Test]
        public void Intersect_should_translate_world_coordinates_into_local_coordinates()
        {
            var ray = new Ray(new Point(0, 0, -5), Vector.UnitZ);
            var shape = new TestShape(Matrix4x4.CreateScaling(2, 2, 2));
            shape.Intersect(ray);
            shape.SavedLocalRay!.Origin.Should().Be(new Point(0, 0, -2.5));
            shape.SavedLocalRay!.Direction.Should().Be(new Vector(0, 0, 0.5));

            shape = (TestShape)shape.WithTransform(Matrix4x4.CreateTranslation(5, 0, 0));
            shape.Intersect(ray);
            shape.SavedLocalRay!.Origin.Should().Be(new Point(-5, 0, -5));
            shape.SavedLocalRay!.Direction.Should().Be(new Vector(0, 0, 1));
        }

        [Test]
        public void NormalAt_should_calculate_the_normal_ona_translated_shape()
        {
            var shape = new TestShape(Matrix4x4.CreateTranslation(0, 1, 0));
            var normal = shape.NormalAt(new Point(0, 1.70711, -0.70711));
            normal.Should().Be(new Vector(0, 0.70711, -0.70711));
        }

        [Test]
        public void NormalAt_should_calculate_the_normal_on_a_transformed_shape()
        {
            var shape = new TestShape(Matrix4x4.CreateRotationZ(Math.PI / 5).Scale(1, 0.5, 1));
            var normal = shape.NormalAt(new Point(0, Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2));
            normal.Should().Be(new Vector(0, 0.97014, -0.24254));
        }

        [Test]
        public void Converting_a_point_from_world_to_object_space()
        {
            var g1 = new Group(Matrix4x4.CreateRotationY(Math.PI / 2));
            var g2 = new Group(Matrix4x4.CreateScaling(2, 2, 2));
            g1.AddChild(g2);
            var s = new Sphere(Matrix4x4.CreateTranslation(5, 0, 0));
            g2.AddChild(s);

            Point p = s.WorldToObject(new Point(-2, 0, -10));
            p.Should().Be(new Point(0, 0, -1));
        }

        [Test]
        public void Converting_a_normal_from_object_to_world_space()
        {
            var g1 = new Group(Matrix4x4.CreateRotationY(Math.PI / 2));
            var g2 = new Group(Matrix4x4.CreateScaling(1, 2, 3));
            g1.AddChild(g2);
            var s = new Sphere(Matrix4x4.CreateTranslation(5, 0, 0));
            g2.AddChild(s);

            Vector normal = s.NormalToWorld(new Vector(Math.Sqrt(3) / 3, Math.Sqrt(3) / 3, Math.Sqrt(3) / 3));
            normal.Should().Be(new Vector(0.28571, 0.42857, -0.85714));
        }

        [Test]
        public void Finding_the_normal_on_a_child_object()
        {
            var g1 = new Group(Matrix4x4.CreateRotationY(Math.PI / 2));
            var g2 = new Group(Matrix4x4.CreateScaling(1, 2, 3));
            g1.AddChild(g2);
            var s = new Sphere(Matrix4x4.CreateTranslation(5, 0, 0));
            g2.AddChild(s);

            Vector normal = s.NormalAt(new Point(1.7321, 1.1547, -5.5774));
            normal.Should().Be(new Vector(0.2857, 0.42854, -0.85716));
        }
    }

    public sealed class TestShape : Shape
    {
        public TestShape(Matrix4x4? transform = null, Material? material = null)
            : base(transform, material)
        {
        }

        public Ray? SavedLocalRay { get; private set; }

        protected internal override IntersectionList LocalIntersect(Ray localRay)
        {
            SavedLocalRay = localRay;
            return IntersectionList.Empty;
        }

        protected internal override Vector LocalNormalAt(Point localPoint)
        {
            return new Vector(localPoint.X, localPoint.Y, localPoint.Z);
        }
    }
}
