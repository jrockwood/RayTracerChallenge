// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="IntersectionListTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests
{
    using FluentAssertions;
    using NUnit.Framework;
    using RayTracerChallenge.Library.Shapes;

    public class IntersectionListTests
    {
        [Test]
        public void Ctor_should_store_the_intersections_in_sorted_order()
        {
            var s = new Sphere();
            var list = IntersectionList.Create(new Intersection(30, s), new Intersection(-1, s));
            list.Ts.Should().HaveCount(2).And.ContainInOrder(-1, 30);
        }

        [Test]
        public void Add_should_return_a_new_list_in_sorted_order()
        {
            var s = new Sphere();
            var list = IntersectionList.Create(new Intersection(30, s), new Intersection(-1, s));
            list = list.Add(new Intersection(10, s), new Intersection(-300, s));
            list.Ts.Should().HaveCount(4).And.ContainInOrder(-300, -1, 10, 30);
        }

        //// ===========================================================================================================
        //// Hit Tests
        //// ===========================================================================================================

        [Test]
        public void Hit_should_return_the_smallest_t_when_all_intersections_have_positive_t()
        {
            var s = new Sphere();
            var i1 = new Intersection(1, s);
            var i2 = new Intersection(2, s);
            var list = IntersectionList.Create(i1, i2);
            list.Hit.Should().Be(i1);
        }

        [Test]
        public void Hit_should_return_the_smallest_non_negative_t_when_some_intersections_have_negative_t()
        {
            var s = new Sphere();
            var i1 = new Intersection(-1, s);
            var i2 = new Intersection(2, s);
            var list = IntersectionList.Create(i1, i2);
            list.Hit.Should().Be(i2);
        }

        [Test]
        public void Hit_should_return_null_when_all_intersections_have_negative_t()
        {
            var s = new Sphere();
            var i1 = new Intersection(-2, s);
            var i2 = new Intersection(-1, s);
            var list = IntersectionList.Create(i1, i2);
            list.Hit.Should().BeNull();
        }

        [Test]
        public void Hit_should_return_the_lowest_non_negative_intersection()
        {
            var s = new Sphere();
            var i1 = new Intersection(5, s);
            var i2 = new Intersection(7, s);
            var i3 = new Intersection(-3, s);
            var i4 = new Intersection(2, s);
            var list = IntersectionList.Create(i1, i2, i3, i4);
            list.Hit.Should().Be(i4);
        }
    }
}
