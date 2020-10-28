// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="IntersectionStateTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;
    using RayTracerChallenge.Library.Shapes;

    public class IntersectionStateTests
    {
        [Test]
        public void Precomputing_the_state_of_an_intersection()
        {
            var ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var shape = new Sphere();
            var hit = new Intersection(4, shape);
            var state = IntersectionState.Create(hit, ray, IntersectionList.Create(hit));
            state.T.Should().Be(hit.T);
            state.Shape.Should().Be(hit.Shape);
            state.Point.Should().Be(new Point(0, 0, -1));
            state.Eye.Should().Be(new Vector(0, 0, -1));
            state.Normal.Should().Be(new Vector(0, 0, -1));
        }

        [Test]
        public void Precomputing_the_hit_when_an_intersection_occurs_on_the_outside()
        {
            var ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var shape = new Sphere();
            var hit = new Intersection(4, shape);
            var state = IntersectionState.Create(hit, ray, IntersectionList.Create(hit));
            state.IsInside.Should().BeFalse();
        }

        [Test]
        public void Precomputing_the_hit_when_an_intersection_occurs_on_the_inside()
        {
            var ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
            var shape = new Sphere();
            var hit = new Intersection(1, shape);
            var state = IntersectionState.Create(hit, ray, IntersectionList.Create(hit));
            state.Point.Should().Be(new Point(0, 0, 1));
            state.Eye.Should().Be(new Vector(0, 0, -1));
            state.IsInside.Should().BeTrue();

            // Normal would have been (0, 0, 1) but is inverted!
            state.Normal.Should().Be(new Vector(0, 0, -1));
        }

        [Test]
        public void Precomputing_the_hit_should_offset_the_point()
        {
            var ray = new Ray(new Point(0, 0, -5), Vector.UnitZ);
            var shape = new Sphere(Matrix4x4.CreateTranslation(0, 0, 1));
            var hit = new Intersection(5, shape);
            var state = IntersectionState.Create(hit, ray, IntersectionList.Create(hit));

            state.OverPoint.Z.Should().BeLessThan(-NumberExtensions.Epsilon / 2);
            state.Point.Z.Should().BeGreaterThan(state.OverPoint.Z);
        }

        /// <summary>
        /// Create a plane and position a ray above it, slanting downward at a 45 degree angle. Position the
        /// intersection on the plane, and compute the reflection vector.
        /// </summary>
        [Test]
        public void Precomputing_the_reflection_vector()
        {
            var shape = new Plane();
            var ray = new Ray(new Point(0, 1, -1), new Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
            var hit = new Intersection(Math.Sqrt(2), shape);
            var state = IntersectionState.Create(hit, ray, IntersectionList.Create(hit));
            state.Reflection.Should().Be(new Vector(0, Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
        }

        [Test]
        public void Create_should_find_n1_and_n2_refraction_vectors_at_various_intersections()
        {
            // We'll set up a scene with three glass spheres, A, B, and C. Sphere A contains both B and C
            // completely and B is to the left of C and overlaps. We shoot a ray through the middle, which
            // will have 6 intersections and calculate the n1 and n2 values for each intersection.

            var testCases = new[]
            {
                (index: 0, n1: 1.0, n2: 1.5),
                (index: 1, n1: 1.5, n2: 2.0),
                (index: 2, n1: 2.0, n2: 2.5),
                (index: 3, n1: 2.5, n2: 2.5),
                (index: 4, n1: 2.5, n2: 1.5),
                (index: 5, n1: 1.5, n2: 1.0),
            };

            static void Test((int index, double n1, double n2) testCase)
            {
                var a = Sphere.CreateGlassSphere()
                    .WithTransform(Matrix4x4.CreateScaling(2, 2, 2))
                    .WithMaterial(m => m.WithRefractiveIndex(1.5));

                var b = Sphere.CreateGlassSphere()
                    .WithTransform(Matrix4x4.CreateTranslation(0, 0, -0.25))
                    .WithMaterial(m => m.WithRefractiveIndex(2.0));

                var c = Sphere.CreateGlassSphere()
                    .WithTransform(Matrix4x4.CreateTranslation(0, 0, 0.25))
                    .WithMaterial(m => m.WithRefractiveIndex(2.5));

                var ray = new Ray(new Point(0, 0, -4), Vector.UnitZ);
                var intersections = IntersectionList.Create(
                    new Intersection(2, a),
                    new Intersection(2.75, b),
                    new Intersection(3.25, c),
                    new Intersection(4.75, b),
                    new Intersection(5.25, c),
                    new Intersection(6, a));

                (int index, double n1, double n2) = testCase;
                var state = IntersectionState.Create(intersections[index], ray, intersections);
                state.N1.Should().Be(n1, $"for test case {index} for n1");
                state.N2.Should().Be(n2, $"for test case {index} for n2");
            }

            foreach (var testCase in testCases)
            {
                Test(testCase);
            }
        }

        [Test]
        public void Create_should_set_UnderPoint_to_just_below_the_surface()
        {
            var ray = new Ray(new Point(0, 0, -5), Vector.UnitZ);
            var shape = Sphere.CreateGlassSphere().WithTransform(Matrix4x4.CreateTranslation(0, 0, 1));
            var hit = new Intersection(5, shape);
            var state = IntersectionState.Create(hit, ray, IntersectionList.Create(hit));
            state.UnderPoint.Z.Should().BeGreaterThan(NumberExtensions.Epsilon / 2);
            state.Point.Z.Should().BeLessThan(state.UnderPoint.Z);
        }

        [Test]
        public void Schlick_should_return_the_reflectance_under_total_internal_reflection()
        {
            var shape = Sphere.CreateGlassSphere();
            var ray = new Ray(new Point(0, 0, Math.Sqrt(2) / 2), Vector.UnitY);
            var intersections = IntersectionList.Create(
                new Intersection(-Math.Sqrt(2) / 2, shape),
                new Intersection(Math.Sqrt(2) / 2, shape));
            var state = IntersectionState.Create(intersections[1], ray, intersections);
            double reflectance = state.Schlick();
            reflectance.Should().Be(1.0);
        }

        [Test]
        public void Schlick_should_return_the_reflectance_with_a_perpendicular_viewing_angle()
        {
            var shape = Sphere.CreateGlassSphere();
            var ray = new Ray(new Point(0, 0, 0), Vector.UnitY);
            var intersections = IntersectionList.Create(
                new Intersection(-1, shape),
                new Intersection(1, shape));
            var state = IntersectionState.Create(intersections[1], ray, intersections);
            double reflectance = state.Schlick();
            reflectance.Should().BeApproximately(0.04, NumberExtensions.Epsilon);
        }

        [Test]
        public void Schlick_should_return_the_reflectance_with_a_small_angle_and_n2_greater_than_n1()
        {
            var shape = Sphere.CreateGlassSphere();
            var ray = new Ray(new Point(0, 0.99, -2), Vector.UnitZ);
            var intersections = IntersectionList.Create(new Intersection(1.8589, shape));
            var state = IntersectionState.Create(intersections[0], ray, intersections);
            double reflectance = state.Schlick();
            reflectance.Should().BeApproximately(0.48873, NumberExtensions.Epsilon);
        }
    }
}
