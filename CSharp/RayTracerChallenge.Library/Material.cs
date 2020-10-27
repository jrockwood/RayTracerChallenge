// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Material.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library
{
    using System;
    using RayTracerChallenge.Library.Lights;
    using RayTracerChallenge.Library.Patterns;
    using RayTracerChallenge.Library.Shapes;

    /// <summary>
    /// Immutable class representing a surface material.
    /// </summary>
    public sealed class Material
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        public static readonly Color DefaultColor = Colors.White;
        public const double DefaultAmbient = 0.1;
        public const double DefaultDiffuse = 0.9;
        public const double DefaultSpecular = 0.9;
        public const double DefaultShininess = 200.0;
        public const double DefaultReflective = 0;
        public const double DefaultTransparency = 0;
        public const double DefaultRefractiveIndex = VacuumRefractiveIndex;

        public const double VacuumRefractiveIndex = 1;
        public const double AirRefractiveIndex = 1.00029;
        public const double WaterRefractiveIndex = 1.333;
        public const double GlassRefractiveIndex = 1.52;
        public const double DiamondRefractiveIndex = 2.417;

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public Material(
            Color? color = null,
            Pattern? pattern = null,
            double ambient = DefaultAmbient,
            double diffuse = DefaultDiffuse,
            double specular = DefaultSpecular,
            double shininess = DefaultShininess,
            double reflective = DefaultReflective,
            double transparency = DefaultTransparency,
            double refractiveIndex = DefaultRefractiveIndex)
        {
            Color = color ?? DefaultColor;
            Pattern = pattern;
            Ambient = VerifyValue(ambient, nameof(ambient));
            Diffuse = VerifyValue(diffuse, nameof(diffuse));
            Specular = VerifyValue(specular, nameof(specular));
            Shininess = VerifyValue(shininess, nameof(shininess));
            Reflective = VerifyValue(reflective, nameof(reflective));
            Transparency = VerifyValue(transparency, nameof(transparency));
            RefractiveIndex = VerifyValue(refractiveIndex, nameof(refractiveIndex));
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public Color Color { get; }
        public Pattern? Pattern { get; }

        public double Ambient { get; }
        public double Diffuse { get; }
        public double Specular { get; }
        public double Shininess { get; }

        public double Reflective { get; }
        public double Transparency { get; }
        public double RefractiveIndex { get; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public Material WithColor(Color value)
        {
            return new Material(
                value,
                Pattern,
                Ambient,
                Diffuse,
                Specular,
                Shininess,
                Reflective,
                Transparency,
                RefractiveIndex);
        }

        public Material WithPattern(Pattern? value)
        {
            return new Material(
                Color,
                value,
                Ambient,
                Diffuse,
                Specular,
                Shininess,
                Reflective,
                Transparency,
                RefractiveIndex);
        }

        public Material WithAmbient(double value)
        {
            return new Material(
                Color,
                Pattern,
                value,
                Diffuse,
                Specular,
                Shininess,
                Reflective,
                Transparency,
                RefractiveIndex);
        }

        public Material WithDiffuse(double value)
        {
            return new Material(
                Color,
                Pattern,
                Ambient,
                value,
                Specular,
                Shininess,
                Reflective,
                Transparency,
                RefractiveIndex);
        }

        public Material WithSpecular(double value)
        {
            return new Material(
                Color,
                Pattern,
                Ambient,
                Diffuse,
                value,
                Shininess,
                Reflective,
                Transparency,
                RefractiveIndex);
        }

        public Material WithShininess(double value)
        {
            return new Material(
                Color,
                Pattern,
                Ambient,
                Diffuse,
                Specular,
                value,
                Reflective,
                Transparency,
                RefractiveIndex);
        }

        public Material WithReflective(double value)
        {
            return new Material(
                Color,
                Pattern,
                Ambient,
                Diffuse,
                Specular,
                Shininess,
                value,
                Transparency,
                RefractiveIndex);
        }

        public Material WithTransparency(double value)
        {
            return new Material(
                Color,
                Pattern,
                Ambient,
                Diffuse,
                Specular,
                Shininess,
                Reflective,
                value,
                RefractiveIndex);
        }

        public Material WithRefractiveIndex(double value)
        {
            return new Material(
                Color,
                Pattern,
                Ambient,
                Diffuse,
                Specular,
                Shininess,
                Reflective,
                Transparency,
                value);
        }

        public Color CalculateLighting(
            Shape shape,
            Light light,
            Point point,
            Vector eye,
            Vector normal,
            bool isInShadow)
        {
            // Get the color from the pattern if it exists.
            Color color = Pattern?.ColorOnShapeAt(shape, point) ?? Color;

            // Combine the surface color with the light's color/intensity.
            Color effectiveColor = color * light.Intensity;

            // Find the direction to the light source.
            Vector lightVector = (light.Position - point).Normalize();

            // Compute the ambient contribution.
            Color ambient = effectiveColor * Ambient;

            // lightDotNormal represents the cosine of the angle between the light vector and the normal
            // vector. A negative number means the light is on the other side of the surface.
            double lightDotNormal = lightVector.Dot(normal);

            Color diffuse;
            Color specular;

            if (lightDotNormal < 0)
            {
                diffuse = Colors.Black;
                specular = Colors.Black;
            }
            else
            {
                // Compute the diffuse contribution.
                diffuse = effectiveColor * Diffuse * lightDotNormal;

                // reflectDotEye represents the cosine of the angle between the reflection vector and the eye
                // vector. A negative number means the light reflects away from the eye.
                Vector reflectVector = lightVector.Negate().Reflect(normal);
                double reflectDotEye = reflectVector.Dot(eye);

                if (reflectDotEye <= 0)
                {
                    specular = Colors.Black;
                }
                else
                {
                    // Compute the specular contribution.
                    double factor = Math.Pow(reflectDotEye, Shininess);
                    specular = light.Intensity * Specular * factor;
                }
            }

            // Add the three contributions together to get the final shading.
            Color result = isInShadow ? ambient : ambient + diffuse + specular;
            return result;
        }

        private static double VerifyValue(double value, string argName)
        {
            if (value < 0)
            {
                throw new ArgumentException("Value cannot be negative", argName);
            }

            return value;
        }
    }
}
