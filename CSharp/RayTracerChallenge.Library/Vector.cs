// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Vector.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library
{
    using System;

    public readonly struct Vector : IEquatable<Vector>
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        public static readonly Vector Zero = new Vector(0);
        public static readonly Vector UnitX = new Vector(1, 0, 0);
        public static readonly Vector UnitY = new Vector(0, 1, 0);
        public static readonly Vector UnitZ = new Vector(0, 0, 1);

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public Vector(float value)
        {
            X = Y = Z = value;
        }

        public Vector(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public float X { get; }
        public float Y { get; }
        public float Z { get; }

        public float Magnitude => MathF.Sqrt(MathF.Pow(X, 2) + MathF.Pow(Y, 2) + MathF.Pow(Z, 2));

        //// ===========================================================================================================
        //// Operators
        //// ===========================================================================================================

        public static Vector operator -(Vector v)
        {
            return v.Negate();
        }

        public Vector Negate()
        {
            return new Vector(-X, -Y, -Z);
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            return v1.Add(v2);
        }

        public Vector Add(Vector v)
        {
            return new Vector(X + v.X, Y + v.Y, Z + v.Z);
        }

        public static Point operator +(Vector vector, Point point)
        {
            return vector.Add(point);
        }

        public Point Add(Point point)
        {
            return new Point(X + point.X, Y + point.Y, Z + point.Z);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            return v1.Subtract(v2);
        }

        public Vector Subtract(Vector v)
        {
            return new Vector(X - v.X, Y - v.Y, Z - v.Z);
        }

        public static Vector operator *(Vector v, float scalar)
        {
            return v.Multiply(scalar);
        }

        public Vector Multiply(float scalar)
        {
            return new Vector(X * scalar, Y * scalar, Z * scalar);
        }

        public static Vector operator /(Vector v, float scalar)
        {
            return v.Divide(scalar);
        }

        public Vector Divide(float scalar)
        {
            return new Vector(X / scalar, Y / scalar, Z / scalar);
        }

        //// ===========================================================================================================
        //// Equality Methods and Operators
        //// ===========================================================================================================

        public bool Equals(Vector other)
        {
            return X.IsApproximatelyEqual(other.X) && Y.IsApproximatelyEqual(other.Y) && Z.IsApproximatelyEqual(other.Z);
        }

        public override bool Equals(object? obj)
        {
            return obj is Vector other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X.RoundToEpsilon(), Y.RoundToEpsilon(), Z.RoundToEpsilon());
        }

        public static bool operator ==(Vector left, Vector right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vector left, Vector right)
        {
            return !left.Equals(right);
        }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public override string ToString()
        {
            return $"<{X}, {Y}, {Z}>";
        }

        public Vector Normalize()
        {
            float magnitude = Magnitude;
            return new Vector(X / magnitude, Y / magnitude, Z / magnitude);
        }

        public float Dot(Vector other)
        {
            return (X * other.X) + (Y * other.Y) + (Z * other.Z);
        }

        public Vector Cross(Vector other)
        {
            return new Vector(
                (Y * other.Z) - (Z * other.Y),
                (Z * other.X) - (X * other.Z),
                (X * other.Y) - (Y * other.X));
        }

        public Vector Reflect(Vector normal)
        {
            Vector reflection = this - (normal * 2 * Dot(normal));
            return reflection;
        }
    }
}
