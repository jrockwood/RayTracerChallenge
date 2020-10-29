// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Intersection.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library
{
    using System;
    using RayTracerChallenge.Library.Shapes;

    /// <summary>
    /// Represents an immutable intersection between a ray and a <see cref="Shape"/>.
    /// </summary>
    public sealed class Intersection : IEquatable<Intersection>, IComparable<Intersection>
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public Intersection(double t, Shape shape)
        {
            T = t;
            Shape = shape;
        }

        public Intersection((double t, Shape shape) tuple)
            : this(tuple.t, tuple.shape)
        {
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public double T { get; }
        public Shape Shape { get; }

        //// ===========================================================================================================
        //// Operators
        //// ===========================================================================================================

        public static implicit operator Intersection((double t, Shape shape) tuple)
        {
            return new Intersection(tuple.t, tuple.shape);
        }

        //// ===========================================================================================================
        //// Equality Members
        //// ===========================================================================================================

        public bool Equals(Intersection? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return T.Equals(other.T) && Shape.Equals(other.Shape);
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) || (obj is Intersection other && Equals(other));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(T, Shape);
        }

        public static bool operator ==(Intersection? left, Intersection? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Intersection? left, Intersection? right)
        {
            return !Equals(left, right);
        }

        //// ===========================================================================================================
        //// IComparable Members
        //// ===========================================================================================================

        public int CompareTo(Intersection? other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (other is null)
            {
                return 1;
            }

            return T.CompareTo(other.T);
        }
    }
}
