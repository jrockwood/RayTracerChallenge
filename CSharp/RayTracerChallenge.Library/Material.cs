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

    /// <summary>
    /// Immutable class representing a surface material.
    /// </summary>
    public sealed class Material
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        public static readonly Color DefaultColor = Colors.White;
        public const float DefaultAmbient = 0.1f;
        public const float DefaultDiffuse = 0.9f;
        public const float DefaultSpecular = 0.9f;
        public const float DefaultShininess = 200.0f;

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public Material(
            Color? color = null,
            float ambient = DefaultAmbient,
            float diffuse = DefaultDiffuse,
            float specular = DefaultSpecular,
            float shininess = DefaultShininess)
        {
            Color = color ?? DefaultColor;
            Ambient = VerifyValue(ambient, nameof(ambient));
            Diffuse = VerifyValue(diffuse, nameof(diffuse));
            Specular = VerifyValue(specular, nameof(specular));
            Shininess = VerifyValue(shininess, nameof(shininess));
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public Color Color { get; }
        public float Ambient { get; }
        public float Diffuse { get; }
        public float Specular { get; }
        public float Shininess { get; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public Material WithColor(Color value)
        {
            return new Material(value, Ambient, Diffuse, Specular, Shininess);
        }

        public Material WithAmbient(float value)
        {
            return new Material(Color, value, Diffuse, Specular, Shininess);
        }

        public Material WithDiffuse(float value)
        {
            return new Material(Color, Ambient, value, Specular, Shininess);
        }

        public Material WithSpecular(float value)
        {
            return new Material(Color, Ambient, Diffuse, value, Shininess);
        }

        public Material WithShininess(float value)
        {
            return new Material(Color, Ambient, Diffuse, Specular, value);
        }

        public Color CalculateLighting(Light light, Point point, Vector eye, Vector normal, bool isInShadow)
        {
            // Combine the surface color with the light's color/intensity.
            Color effectiveColor = Color * light.Intensity;

            // Find the direction to the light source.
            Vector lightVector = (light.Position - point).Normalize();

            // Compute the ambient contribution.
            Color ambient = effectiveColor * Ambient;

            // lightDotNormal represents the cosine of the angle between the light vector and the normal
            // vector. A negative number means the light is on the other side of the surface.
            float lightDotNormal = lightVector.Dot(normal);

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
                float reflectDotEye = reflectVector.Dot(eye);

                if (reflectDotEye <= 0)
                {
                    specular = Colors.Black;
                }
                else
                {
                    // Compute the specular contribution.
                    float factor = MathF.Pow(reflectDotEye, Shininess);
                    specular = light.Intensity * Specular * factor;
                }
            }

            // Add the three contributions together to get the final shading.
            Color result = isInShadow ? ambient : ambient + diffuse + specular;
            return result;
        }

        private static float VerifyValue(float value, string argName)
        {
            if (value < 0)
            {
                throw new ArgumentException("Value cannot be negative", argName);
            }

            return value;
        }
    }
}
