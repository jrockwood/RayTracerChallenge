// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Matrix4x4.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library
{
    using System;

    /// <summary>
    /// Represents an immutable 4x4 matrix.
    /// </summary>
    public sealed class Matrix4x4 : IEquatable<Matrix4x4>
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        public static readonly Matrix4x4 Identity = new Matrix4x4(
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1);

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public Matrix4x4(
            float m00, float m01, float m02, float m03,
            float m10, float m11, float m12, float m13,
            float m20, float m21, float m22, float m23,
            float m30, float m31, float m32, float m33)
        {
            M00 = m00;
            M01 = m01;
            M02 = m02;
            M03 = m03;

            M10 = m10;
            M11 = m11;
            M12 = m12;
            M13 = m13;

            M20 = m20;
            M21 = m21;
            M22 = m22;
            M23 = m23;

            M30 = m30;
            M31 = m31;
            M32 = m32;
            M33 = m33;
        }

        //// ===========================================================================================================
        //// Indexers
        //// ===========================================================================================================

        public float this[int row, int col] =>
            (row, col) switch
            {
                (0, 0) => M00,
                (0, 1) => M01,
                (0, 2) => M02,
                (0, 3) => M03,

                (1, 0) => M10,
                (1, 1) => M11,
                (1, 2) => M12,
                (1, 3) => M13,

                (2, 0) => M20,
                (2, 1) => M21,
                (2, 2) => M22,
                (2, 3) => M23,

                (3, 0) => M30,
                (3, 1) => M31,
                (3, 2) => M32,
                (3, 3) => M33,

                (_, _) => throw new IndexOutOfRangeException()
            };

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public float M00 { get; }
        public float M01 { get; }
        public float M02 { get; }
        public float M03 { get; }

        public float M10 { get; }
        public float M11 { get; }
        public float M12 { get; }
        public float M13 { get; }

        public float M20 { get; }
        public float M21 { get; }
        public float M22 { get; }
        public float M23 { get; }

        public float M30 { get; }
        public float M31 { get; }
        public float M32 { get; }
        public float M33 { get; }

        //// ===========================================================================================================
        //// Operators
        //// ===========================================================================================================

        public static Matrix4x4 operator *(Matrix4x4 a, Matrix4x4 b)
        {
            return new Matrix4x4(
                // First row
                (a.M00 * b.M00) + (a.M01 * b.M10) + (a.M02 * b.M20) + (a.M03 * b.M30),
                (a.M00 * b.M01) + (a.M01 * b.M11) + (a.M02 * b.M21) + (a.M03 * b.M31),
                (a.M00 * b.M02) + (a.M01 * b.M12) + (a.M02 * b.M22) + (a.M03 * b.M32),
                (a.M00 * b.M03) + (a.M01 * b.M13) + (a.M02 * b.M23) + (a.M03 * b.M33),

                // Second row
                (a.M10 * b.M00) + (a.M11 * b.M10) + (a.M12 * b.M20) + (a.M13 * b.M30),
                (a.M10 * b.M01) + (a.M11 * b.M11) + (a.M12 * b.M21) + (a.M13 * b.M31),
                (a.M10 * b.M02) + (a.M11 * b.M12) + (a.M12 * b.M22) + (a.M13 * b.M32),
                (a.M10 * b.M03) + (a.M11 * b.M13) + (a.M12 * b.M23) + (a.M13 * b.M33),

                // Third row
                (a.M20 * b.M00) + (a.M21 * b.M10) + (a.M22 * b.M20) + (a.M23 * b.M30),
                (a.M20 * b.M01) + (a.M21 * b.M11) + (a.M22 * b.M21) + (a.M23 * b.M31),
                (a.M20 * b.M02) + (a.M21 * b.M12) + (a.M22 * b.M22) + (a.M23 * b.M32),
                (a.M20 * b.M03) + (a.M21 * b.M13) + (a.M22 * b.M23) + (a.M23 * b.M33),

                // Fourth row
                (a.M30 * b.M00) + (a.M31 * b.M10) + (a.M32 * b.M20) + (a.M33 * b.M30),
                (a.M30 * b.M01) + (a.M31 * b.M11) + (a.M32 * b.M21) + (a.M33 * b.M31),
                (a.M30 * b.M02) + (a.M31 * b.M12) + (a.M32 * b.M22) + (a.M33 * b.M32),
                (a.M30 * b.M03) + (a.M31 * b.M13) + (a.M32 * b.M23) + (a.M33 * b.M33)

            );
        }

        public static Point operator *(Matrix4x4 matrix, Point point)
        {
            float x = (matrix.M00 * point.X) + (matrix.M01 * point.Y) + (matrix.M02 * point.Z) + matrix.M03;
            float y = (matrix.M10 * point.X) + (matrix.M11 * point.Y) + (matrix.M12 * point.Z) + matrix.M13;
            float z = (matrix.M20 * point.X) + (matrix.M21 * point.Y) + (matrix.M22 * point.Z) + matrix.M23;

            return new Point(x, y, z);
        }

        public static Vector operator *(Matrix4x4 matrix, Vector vector)
        {
            float x = (matrix.M00 * vector.X) + (matrix.M01 * vector.Y) + (matrix.M02 * vector.Z);
            float y = (matrix.M10 * vector.X) + (matrix.M11 * vector.Y) + (matrix.M12 * vector.Z);
            float z = (matrix.M20 * vector.X) + (matrix.M21 * vector.Y) + (matrix.M22 * vector.Z);

            return new Vector(x, y, z);
        }

        //// ===========================================================================================================
        //// Equality Members
        //// ===========================================================================================================

        public bool Equals(Matrix4x4? other)
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
                   M02.IsApproximatelyEqual(other.M02) &&
                   M03.IsApproximatelyEqual(other.M03) &&

                   M10.IsApproximatelyEqual(other.M10) &&
                   M11.IsApproximatelyEqual(other.M11) &&
                   M12.IsApproximatelyEqual(other.M12) &&
                   M13.IsApproximatelyEqual(other.M13) &&

                   M20.IsApproximatelyEqual(other.M20) &&
                   M21.IsApproximatelyEqual(other.M21) &&
                   M22.IsApproximatelyEqual(other.M22) &&
                   M23.IsApproximatelyEqual(other.M23) &&

                   M30.IsApproximatelyEqual(other.M30) &&
                   M31.IsApproximatelyEqual(other.M31) &&
                   M32.IsApproximatelyEqual(other.M32) &&
                   M33.IsApproximatelyEqual(other.M33);
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) || (obj is Matrix4x4 other && Equals(other));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                HashCode.Combine(M00.RoundToEpsilon(), M01.RoundToEpsilon(), M02.RoundToEpsilon(), M03.RoundToEpsilon()),
                HashCode.Combine(M10.RoundToEpsilon(), M11.RoundToEpsilon(), M12.RoundToEpsilon(), M13.RoundToEpsilon()),
                HashCode.Combine(M20.RoundToEpsilon(), M21.RoundToEpsilon(), M22.RoundToEpsilon(), M23.RoundToEpsilon()),
                HashCode.Combine(M30.RoundToEpsilon(), M31.RoundToEpsilon(), M32.RoundToEpsilon(), M33.RoundToEpsilon()));
        }

        public static bool operator ==(Matrix4x4? left, Matrix4x4? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Matrix4x4? left, Matrix4x4? right)
        {
            return !Equals(left, right);
        }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public override string ToString()
        {
            return $"|{M00} {M01} {M02} {M03}|\n|{M10} {M11} {M12} {M13}|\n|{M20} {M21} {M22} {M23}|\n|{M30} {M31} {M32} {M33}|";
        }

        public Matrix4x4 Transpose()
        {
            return new Matrix4x4(
                M00, M10, M20, M30,
                M01, M11, M21, M31,
                M02, M12, M22, M32,
                M03, M13, M23, M33);
        }
    }
}
