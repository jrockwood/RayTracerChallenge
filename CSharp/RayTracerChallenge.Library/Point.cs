﻿// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Point.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library
{
    using System;

    public readonly struct Point : IEquatable<Point>
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        public static readonly Point Zero = new Point(0);

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public Point(float value)
        {
            X = Y = Z = value;
        }

        public Point(float x, float y, float z)
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

        //// ===========================================================================================================
        //// Operators
        //// ===========================================================================================================

        public static Point operator +(Point point, Vector vector)
        {
            return new Point(point.X + vector.X, point.Y + vector.Y, point.Z + vector.Z);
        }

        public static Point operator +(Vector vector, Point point)
        {
            return new Point(point.X + vector.X, point.Y + vector.Y, point.Z + vector.Z);
        }

        public static Vector operator -(Point p1, Point p2)
        {
            return new Vector(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        }

        public static Point operator -(Point point, Vector vector)
        {
            return new Point(point.X - vector.X, point.Y - vector.Y, point.Z - vector.Z);
        }

        //// ===========================================================================================================
        //// Equality Methods and Operators
        //// ===========================================================================================================

        public bool Equals(Point other)
        {
            return X.IsApproximatelyEqual(other.X) && Y.IsApproximatelyEqual(other.Y) && Z.IsApproximatelyEqual(other.Z);
        }

        public override bool Equals(object? obj)
        {
            return obj is Point other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X.RoundToEpsilon(), Y.RoundToEpsilon(), Z.RoundToEpsilon());
        }

        public static bool operator ==(Point left, Point right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Point left, Point right)
        {
            return !left.Equals(right);
        }
    }
}
