// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="World.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using RayTracerChallenge.Library.Lights;
    using RayTracerChallenge.Library.Shapes;

    public class World
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public World(Light light, params Shape[] shapes)
            : this(light, shapes.ToImmutableArray())
        {
        }

        public World(Light light, ImmutableArray<Shape> shapes)
        {
            Light = light;
            Shapes = shapes;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public Light Light { get; }

        public World WithLight(Light value)
        {
            return new World(value, Shapes);
        }

        public ImmutableArray<Shape> Shapes { get; }

        public World WithShapes(ImmutableArray<Shape> value)
        {
            return new World(Light, value);
        }

        public World WithShapes(params Shape[] value)
        {
            return new World(Light, value);
        }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        /// <summary>
        /// Iterates over all of the shapes in the world and returns all of the hits for the specified ray.
        /// </summary>
        /// <param name="ray">The ray to use for hit testing.</param>
        /// <returns>The list of all intersections for the specified ray.</returns>
        public IntersectionList Intersect(Ray ray)
        {
            var hits = new List<Intersection>();

            foreach (Shape shape in Shapes)
            {
                IntersectionList shapeHits = shape.Intersect(ray);
                hits.AddRange(shapeHits);
            }

            return new IntersectionList(hits);
        }

        internal Color ShadeHit(IntersectionState state)
        {
            Color color = state.Shape.Material.CalculateLighting(Light, state.Point, state.Eye, state.Normal);
            return color;
        }

        /// <summary>
        /// Finds the color of the first shape that is hit for the given ray.
        /// </summary>
        /// <param name="ray">The ray to use for hit testing.</param>
        /// <returns>
        /// The color for the part of the shape that the ray hits, or <see cref="Colors.Black"/> if no shape is hit.
        /// </returns>
        public Color ColorAt(Ray ray)
        {
            IntersectionList intersections = Intersect(ray);
            Intersection? hit = intersections.Hit;

            if (hit == null)
            {
                return Colors.Black;
            }

            var state = IntersectionState.Create(hit, ray);
            Color color = ShadeHit(state);
            return color;
        }

        /// <summary>
        /// Creates a world used in unit tests. The light source is at (-10, 10, -10) and contains two concentric
        /// spheres, where the outermost is a unit sphere and the innermost has a radius of 0.5. Both lie at the origin.
        /// </summary>
        internal static World CreateDefaultWorld()
        {
            var light = new PointLight(new Point(-10, 10, -10), Colors.White);
            var sphere1 = new Sphere(
                material: new Material(new Color(0.8f, 1.0f, 0.6f), diffuse: 0.7f, specular: 0.2f));
            var sphere2 = new Sphere(Matrix4x4.CreateScaling(0.5f, 0.5f, 0.5f));

            return new World(light, sphere1, sphere2);
        }
    }
}
