// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="CsgShapeTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests.Shapes
{
    using FluentAssertions;
    using NUnit.Framework;
    using RayTracerChallenge.Library.Shapes;

    public class CsgShapeTests
    {
        [Test]
        public void A_CSG_shape_is_composed_of_an_operation_and_two_operand_shapes()
        {
            var sphere = new Sphere();
            var cube = new Cube();
            var csg = new CsgShape(CsgOperation.Union, sphere, cube);
            csg.Operation.Should().Be(CsgOperation.Union);
            csg.Left.Should().Be(sphere);
            csg.Right.Should().Be(cube);

            sphere.Parent.Should().Be(csg);
            cube.Parent.Should().Be(csg);
        }

        [Test]
        [TestCase(true, true, true, false)]
        [TestCase(true, true, false, true)]
        [TestCase(true, false, true, false)]
        [TestCase(true, false, false, true)]
        [TestCase(false, true, true, false)]
        [TestCase(false, true, false, false)]
        [TestCase(false, false, true, true)]
        [TestCase(false, false, false, true)]
        public void A_CSG_union_preserves_all_intersections_on_the_exterior_of_both_shapes(
            bool isLeftHit,
            bool isWithinLeft,
            bool isWithinRight,
            bool expectedResult)
        {
            CsgShape.IsIntersectionAllowed(
                    CsgOperation.Union,
                    isLeftHit,
                    isWithinLeft,
                    isWithinRight)
                .Should()
                .Be(expectedResult);
        }

        [Test]
        [TestCase(true, true, true, true)]
        [TestCase(true, true, false, false)]
        [TestCase(true, false, true, true)]
        [TestCase(true, false, false, false)]
        [TestCase(false, true, true, true)]
        [TestCase(false, true, false, true)]
        [TestCase(false, false, true, false)]
        [TestCase(false, false, false, false)]
        public void A_CSG_intersect_preserves_all_intersections_where_both_shapes_overlap(
            bool isLeftHit,
            bool isWithinLeft,
            bool isWithinRight,
            bool expectedResult)
        {
            CsgShape.IsIntersectionAllowed(
                    CsgOperation.Intersection,
                    isLeftHit,
                    isWithinLeft,
                    isWithinRight)
                .Should()
                .Be(expectedResult);
        }

        [Test]
        [TestCase(true, true, true, false)]
        [TestCase(true, true, false, true)]
        [TestCase(true, false, true, false)]
        [TestCase(true, false, false, true)]
        [TestCase(false, true, true, true)]
        [TestCase(false, true, false, true)]
        [TestCase(false, false, true, false)]
        [TestCase(false, false, false, false)]
        public void A_CSG_difference_preserves_all_intersections_not_exclusively_inside_the_object_on_the_right(
            bool isLeftHit,
            bool isWithinLeft,
            bool isWithinRight,
            bool expectedResult)
        {
            CsgShape.IsIntersectionAllowed(
                    CsgOperation.Difference,
                    isLeftHit,
                    isWithinLeft,
                    isWithinRight)
                .Should()
                .Be(expectedResult);
        }

        [Test]
        [TestCase(CsgOperation.Union, 0, 3)]
        [TestCase(CsgOperation.Intersection, 1, 2)]
        [TestCase(CsgOperation.Difference, 0, 1)]
        public void
            Given_a_set_of_intersections_produce_a_subset_of_only_those_intersections_that_conform_to_the_operation_of_the_current_CSG_object(
                CsgOperation operation,
                int intersectionIndex1,
                int intersectionIndex2)
        {
            var left = new Sphere();
            var right = new Cube();
            var csg = new CsgShape(operation, left, right);
            var intersections = new IntersectionList((1, left), (2, right), (3, left), (4, right));
            var filtered = csg.FilterIntersections(intersections);
            filtered.Should()
                .HaveCount(2)
                .And.ContainInOrder(intersections[intersectionIndex1], intersections[intersectionIndex2]);
        }

        [Test]
        public void A_ray_misses_a_CSG_object()
        {
            var csg = new CsgShape(CsgOperation.Union, new Sphere(), new Cube());
            var ray = new Ray(new Point(0, 2, -5), new Vector(0, 0, 1));
            var intersections = csg.LocalIntersect(ray);
            intersections.Should().BeEmpty();
        }

        [Test]
        public void A_ray_hits_a_CSG_object()
        {
            var left = new Sphere();
            var right = new Sphere(transform: Matrix4x4.CreateTranslation(0, 0, 0.5));
            var csg = new CsgShape(CsgOperation.Union, left, right);
            var ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var intersections = csg.LocalIntersect(ray);
            intersections.Should().HaveCount(2);
            intersections.Ts.Should().ContainInOrder(4, 6.5);
            intersections.Shapes.Should().ContainInOrder(left, right);
        }
    }
}
