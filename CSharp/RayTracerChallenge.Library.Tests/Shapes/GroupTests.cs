// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="GroupTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests.Shapes
{
    using FluentAssertions;
    using NUnit.Framework;
    using RayTracerChallenge.Library.Shapes;

    public class GroupTests
    {
        [Test]
        public void Creating_a_new_group()
        {
            var group = new Group();
            group.Transform.Should().Be(Matrix4x4.Identity);
            group.Children.Should().BeEmpty();
        }

        [Test]
        public void Adding_a_child_to_a_group()
        {
            var group = new Group();
            var shape = new TestShape();
            group.AddChild(shape);
            group.Children.Should().NotBeEmpty();
            group.Children.Should().Contain(shape);
            shape.Parent.Should().Be(group);
        }

        [Test]
        public void Intersecting_a_ray_with_an_empty_group()
        {
            var group = new Group();
            var ray = new Ray(Point.Zero, Vector.UnitZ);
            var intersections = group.LocalIntersect(ray);
            intersections.Should().BeEmpty();
        }

        [Test]
        public void Intersecting_a_ray_with_a_nonempty_group()
        {
            var group = new Group();
            var s1 = new Sphere();
            var s2 = new Sphere(Matrix4x4.CreateTranslation(0, 0, -3));
            var s3 = new Sphere(Matrix4x4.CreateTranslation(5, 0, 0));
            group.AddChildren(s1, s2, s3);

            var ray = new Ray(new Point(0, 0, -5), Vector.UnitZ);
            var intersections = group.LocalIntersect(ray);
            intersections.Shapes.Should().HaveCount(4).And.ContainInOrder(s2, s2, s1, s1);
        }

        [Test]
        public void Intersecting_a_transformed_group()
        {
            var group = new Group(Matrix4x4.CreateScaling(2, 2, 2));
            var s = new Sphere(Matrix4x4.CreateTranslation(5, 0, 0));
            group.AddChild(s);
            var ray = new Ray(new Point(10, 0, -10), Vector.UnitZ);
            var intersections = group.Intersect(ray);
            intersections.Should().HaveCount(2);
        }
    }
}
