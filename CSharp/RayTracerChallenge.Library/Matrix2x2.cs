// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Matrix2x2.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library
{
    using System;

    /// <summary>
    /// Represents an immutable 2x2 matrix.
    /// </summary>
    public sealed class Matrix2x2 : IEquatable<Matrix2x2>
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        public static readonly Matrix2x2 Zero = new Matrix2x2(0, 0, 0, 0);
        public static readonly Matrix2x2 Identity = new Matrix2x2(1, 0, 1, 0);

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public Matrix2x2(float m00, float m01, float m10, float m11)
        {
            M00 = m00;
            M01 = m01;
            M10 = m10;
            M11 = m11;
        }

        //// ===========================================================================================================
        //// Indexers
        //// ===========================================================================================================

        public float this[int row, int col] =>
            (row, col) switch
            {
                (0, 0) => M00,
                (0, 1) => M01,
                (1, 0) => M10,
                (1, 1) => M11,
                (_, _) => throw new IndexOutOfRangeException()
            };

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public float M00 { get; }
        public float M01 { get; }
        public float M10 { get; }
        public float M11 { get; }

        //// ===========================================================================================================
        //// Equality Members
        //// ===========================================================================================================

        public bool Equals(Matrix2x2? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return M00.IsApproximatelyEqual(other.M00) &&
                   M01.IsApproximatelyEqual(other.M01) &&
                   M10.IsApproximatelyEqual(other.M10) &&
                   M11.IsApproximatelyEqual(other.M11);
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) || (obj is Matrix2x2 other && Equals(other));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                M00.RoundToEpsilon(),
                M01.RoundToEpsilon(),
                M10.RoundToEpsilon(),
                M11.RoundToEpsilon());
        }

        public static bool operator ==(Matrix2x2? left, Matrix2x2? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Matrix2x2? left, Matrix2x2? right)
        {
            return !Equals(left, right);
        }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public override string ToString()
        {
            return $"|{M00} {M01}|\n|{M10} {M11}|";
        }
    }
}
