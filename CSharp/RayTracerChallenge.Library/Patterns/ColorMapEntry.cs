// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorMapEntry.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Patterns
{
    using System;

    public sealed class ColorMapEntry : IEquatable<ColorMapEntry>
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public ColorMapEntry(double key, Color color)
        {
            if (key < 0 || key > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(key), key, "Key must be between 0 and 1, inclusive.");
            }

            Key = key;
            Color = color;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public double Key { get; }
        public Color Color { get; }

        //// ===========================================================================================================
        //// Equality Members
        //// ===========================================================================================================

        public bool Equals(ColorMapEntry? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Key.IsApproximatelyEqual(other.Key) && Color.Equals(other.Color);
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) || (obj is ColorMapEntry other && Equals(other));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Key.RoundToEpsilon(), Color);
        }

        public static bool operator ==(ColorMapEntry? left, ColorMapEntry? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ColorMapEntry? left, ColorMapEntry? right)
        {
            return !Equals(left, right);
        }
    }
}
