// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Color.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library
{
    using System;

    public readonly struct Color : IEquatable<Color>
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public Color(double red, double green, double blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public double Red { get; }
        public double Green { get; }
        public double Blue { get; }

        //// ===========================================================================================================
        //// Operators
        //// ===========================================================================================================

        public static Color operator +(Color c1, Color c2)
        {
            return new Color(c1.Red + c2.Red, c1.Green + c2.Green, c1.Blue + c2.Blue);
        }

        public static Color operator -(Color c1, Color c2)
        {
            return new Color(c1.Red - c2.Red, c1.Green - c2.Green, c1.Blue - c2.Blue);
        }

        public static Color operator *(Color c, int scalar)
        {
            return new Color(c.Red * scalar, c.Green * scalar, c.Blue * scalar);
        }

        public static Color operator *(Color c, double scalar)
        {
            return new Color(c.Red * scalar, c.Green * scalar, c.Blue * scalar);
        }

        public static Color operator *(Color c1, Color c2)
        {
            return new Color(c1.Red * c2.Red, c1.Green * c2.Green, c1.Blue * c2.Blue);
        }

        //// ===========================================================================================================
        //// Equality Methods and Operators
        //// ===========================================================================================================

        public bool Equals(Color other)
        {
            return Red.IsApproximatelyEqual(other.Red) &&
                   Green.IsApproximatelyEqual(other.Green) &&
                   Blue.IsApproximatelyEqual(other.Blue);
        }

        public override bool Equals(object? obj)
        {
            return obj is Color other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Red.RoundToEpsilon(), Green.RoundToEpsilon(), Blue.RoundToEpsilon());
        }

        public static bool operator ==(Color left, Color right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Color left, Color right)
        {
            return !left.Equals(right);
        }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public override string ToString()
        {
            return $"Color: Red={Red}, Green={Green}, Blue={Blue}";
        }
    }

    public static class Colors
    {
        public static readonly Color White = new Color(1, 1, 1);
        public static readonly Color Black = new Color(0, 0, 0);

        public static readonly Color LightGray = new Color(0.75, 0.75, 0.75);
        public static readonly Color Gray = new Color(0.5, 0.5, 0.5);

        public static readonly Color Red = new Color(1, 0, 0);
        public static readonly Color Green = new Color(0, 1, 0);
        public static readonly Color Blue = new Color(0, 0, 1);

        public static readonly Color Yellow = new Color(1, 1, 0);
        public static readonly Color Magenta = new Color(1, 0, 1);
        public static readonly Color Cyan = new Color(0, 1, 1);
    }
}
