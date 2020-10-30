// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="World.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library
{
    using System;
    using System.Collections.Generic;
    using RayTracerChallenge.Library.Lights;
    using RayTracerChallenge.Library.Shapes;

    /// <summary>
    /// Represents a world with lights and shapes.
    /// </summary>
    public class World
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        public const int MaxRecursion = 5;

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public World(Light light, params Shape[] shapes)
            : this(light, (IEnumerable<Shape>)shapes)
        {
        }

        public World(Light light, IEnumerable<Shape> shapes)
        {
            Light = light;
            Shapes = new List<Shape>(shapes);
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public Light Light { get; set; }

        public List<Shape> Shapes { get; set; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public World ChangeLight(Light value)
        {
            Light = value;
            return this;
        }

        public World AddShapes(params Shape[] shapes)
        {
            Shapes.AddRange(shapes);
            return this;
        }

        public World ChangeShapes(params Shape[] shapes)
        {
            Shapes = new List<Shape>(shapes);
            return this;
        }

        /// <summary>
        /// Iterates over all of the shapes in the world and returns all of the hits for the specified ray.
        /// </summary>
        /// <param name="ray">The ray to use for hit testing.</param>
        /// <returns>The list of all intersections for the specified ray.</returns>
        public IntersectionList Intersect(Ray ray)
        {
            var hits = new IntersectionList();

            foreach (Shape shape in Shapes)
            {
                IntersectionList shapeHits = shape.Intersect(ray);
                hits.AddRange(shapeHits);
            }

            return hits;
        }

        internal Color ShadeHit(IntersectionState state, int maxRecursion = MaxRecursion)
        {
            bool isShadowed = IsShadowed(state.OverPoint);

            Color surfaceColor = state.Shape.Material.CalculateLighting(
                state.Shape,
                Light,
                state.OverPoint,
                state.Eye,
                state.Normal,
                isShadowed);

            Color reflectedColor = ReflectedColor(state, maxRecursion);
            Color refractedColor = RefractedColor(state, maxRecursion);

            Material material = state.Shape.Material;
            if (material.Reflective > 0 && material.Transparency > 0)
            {
                double reflectance = state.Schlick();
                return surfaceColor + (reflectedColor * reflectance) + (refractedColor * (1 - reflectance));
            }

            return surfaceColor + reflectedColor + refractedColor;
        }

        /// <summary>
        /// Finds the color of the first shape that is hit for the given ray.
        /// </summary>
        /// <param name="ray">The ray to use for hit testing.</param>
        /// <param name="maxRecursion">The maximum number of additional rays to use for reflection.</param>
        /// <returns>
        /// The color for the part of the shape that the ray hits, or <see cref="Colors.Black"/> if no shape is hit.
        /// </returns>
        public Color ColorAt(Ray ray, int maxRecursion = MaxRecursion)
        {
            IntersectionList intersections = Intersect(ray);
            Intersection? hit = intersections.Hit;

            if (hit == null)
            {
                return Colors.Black;
            }

            var state = IntersectionState.Create(hit, ray, intersections);
            Color color = ShadeHit(state, maxRecursion);
            return color;
        }

        public bool IsShadowed(Point point)
        {
            Vector pointToLightVector = Light.Position - point;
            double distance = pointToLightVector.Magnitude;
            Vector direction = pointToLightVector.Normalize();

            var ray = new Ray(point, direction);
            IntersectionList intersections = Intersect(ray);
            Intersection? hit = intersections.Hit;

            return hit != null && hit.T < distance && !hit.Shape.IsShadowHidden;
        }

        public Color ReflectedColor(IntersectionState state, int maxRecursion = MaxRecursion)
        {
            if (maxRecursion <= 0 || state.Shape.Material.Reflective == 0)
            {
                return Colors.Black;
            }

            var reflectedRay = new Ray(state.OverPoint, state.Reflection);
            Color color = ColorAt(reflectedRay, maxRecursion - 1);
            Color reflectedColor = color * state.Shape.Material.Reflective;
            return reflectedColor;
        }

        public Color RefractedColor(IntersectionState state, int maxRecursion = MaxRecursion)
        {
            if (maxRecursion <= 0 || state.Shape.Material.Transparency == 0)
            {
                return Colors.Black;
            }

            // Find the ratio of the first index of refraction to the second. This is inverted from the definition of
            // Snell's Law, which is sin(theta_i) / sin(theta_t) = n2 / n1.
            double nRatio = state.N1 / state.N2;

            // cos(theta_i) is the same as the dot product of the two vectors.
            double cosI = state.Eye.Dot(state.Normal);

            // Find sin(theta_t)^2 via trigonometric identity.
            double sin2T = nRatio * nRatio * (1 - (cosI * cosI));

            // Total internal reflection - there is no refraction.
            if (sin2T > 1)
            {
                return Colors.Black;
            }

            // Find cos(theta_t) via trigonometric identity.
            double cosT = Math.Sqrt(1.0 - sin2T);

            // Compute the direction of the refracted ray.
            Vector direction = (state.Normal * ((nRatio * cosI) - cosT)) - (state.Eye * nRatio);

            // Create the refracted ray.
            var refractedRay = new Ray(state.UnderPoint, direction);

            // Find the color of the refracted ray, making sure to multiply by the transparency value to account for any opacity.
            Color color = ColorAt(refractedRay, maxRecursion - 1) * state.Shape.Material.Transparency;
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
                material: new Material(new Color(0.8, 1.0, 0.6), diffuse: 0.7, specular: 0.2));
            var sphere2 = new Sphere(Matrix4x4.CreateScaling(0.5, 0.5, 0.5));

            return new World(light, sphere1, sphere2);
        }
    }
}
