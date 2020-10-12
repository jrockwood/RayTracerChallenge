﻿// ---------------------------------------------------------------------------------------------------------------------
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

        public float Determinant
        {
            get
            {
                float result = 0;

                for (int column = 0; column <= 3; column++)
                {
                    result += this[0, column] * Cofactor(0, column);
                }

                return result;
            }
        }

        public bool IsInvertible => Determinant != 0;

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

        public Matrix3x3 Submatrix(int rowToRemove, int columnToRemove)
        {
            int row0 = rowToRemove == 0 ? 1 : 0;
            int row1 = rowToRemove <= 1 ? 2 : 1;
            int row2 = rowToRemove <= 2 ? 3 : 2;
            int col0 = columnToRemove == 0 ? 1 : 0;
            int col1 = columnToRemove <= 1 ? 2 : 1;
            int col2 = columnToRemove <= 2 ? 3 : 2;

            // prettier-ignore
            return new Matrix3x3(
                this[row0, col0], this[row0, col1], this[row0, col2],
                this[row1, col0], this[row1, col1], this[row1, col2],
                this[row2, col0], this[row2, col1], this[row2, col2]);
        }

        internal float Minor(int row, int column)
        {
            var submatrix = Submatrix(row, column);
            return submatrix.Determinant;
        }

        internal float Cofactor(int row, int column)
        {
            float minor = Minor(row, column);

            // If row + column is odd, then we need to negate the minor.
            if ((row + column) % 2 == 1)
            {
                minor = -minor;
            }

            return minor;
        }

        public Matrix4x4 Invert()
        {
            if (!IsInvertible)
            {
                throw new InvalidOperationException("The matrix is not invertible");
            }

            // Initialize the new matrix.
            float[][] newMatrix = new float[4][];
            for (int row = 0; row < 4; row++)
            {
                newMatrix[row] = new float[4];
            }

            // Calculate the determinant outside of the loop.
            float determinant = Determinant;

            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    float c = Cofactor(row, col);

                    // Note the "col, row" here instead of "row, col", which transposes the matrix.
                    newMatrix[col][row] = c / determinant;
                }
            }

            return new Matrix4x4(
                newMatrix[0][0], newMatrix[0][1], newMatrix[0][2], newMatrix[0][3],
                newMatrix[1][0], newMatrix[1][1], newMatrix[1][2], newMatrix[1][3],
                newMatrix[2][0], newMatrix[2][1], newMatrix[2][2], newMatrix[2][3],
                newMatrix[3][0], newMatrix[3][1], newMatrix[3][2], newMatrix[3][3]);
        }

        public static Matrix4x4 CreateTranslation(float x, float y, float z)
        {
            return new Matrix4x4(
                1, 0, 0, x,
                0, 1, 0, y,
                0, 0, 1, z,
                0, 0, 0, 1);
        }

        public Matrix4x4 Translate(float x, float y, float z)
        {
            return CreateTranslation(x, y, z) * this;
        }

        public static Matrix4x4 CreateScaling(float x, float y, float z)
        {
            return new Matrix4x4(
                x, 0, 0, 0,
                0, y, 0, 0,
                0, 0, z, 0,
                0, 0, 0, 1);
        }

        public Matrix4x4 Scale(float x, float y, float z)
        {
            return CreateScaling(x, y, z) * this;
        }

        public static Matrix4x4 CreateRotationX(float radians)
        {
            float cos = MathF.Cos(radians);
            float sin = MathF.Sin(radians);

            return new Matrix4x4(
                1, 0, 0, 0,
                0, cos, -sin, 0,
                0, sin, cos, 0,
                0, 0, 0, 1);
        }

        public Matrix4x4 RotateX(float radians)
        {
            return CreateRotationX(radians) * this;
        }

        public static Matrix4x4 CreateRotationY(float radians)
        {
            float cos = MathF.Cos(radians);
            float sin = MathF.Sin(radians);

            return new Matrix4x4(
                cos, 0, sin, 0,
                0, 1, 0, 0,
                -sin, 0, cos, 0,
                0, 0, 0, 1);
        }

        public Matrix4x4 RotateY(float radians)
        {
            return CreateRotationY(radians) * this;
        }

        public static Matrix4x4 CreateRotationZ(float radians)
        {
            float cos = MathF.Cos(radians);
            float sin = MathF.Sin(radians);

            return new Matrix4x4(
                cos, -sin, 0, 0,
                sin, cos, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1);
        }

        public Matrix4x4 RotateZ(float radians)
        {
            return CreateRotationZ(radians) * this;
        }

        public static Matrix4x4 CreateShearing(float xy, float xz, float yx, float yz, float zx, float zy)
        {
            return new Matrix4x4(
                1, xy, xz, 0,
                yx, 1, yz, 0,
                zx, zy, 1, 0,
                0, 0, 0, 1);
        }

        public Matrix4x4 Shear(float xy, float xz, float yx, float yz, float zx, float zy)
        {
            return CreateShearing(xy, xz, yx, yz, zx, zy) * this;
        }

        /// <summary>
        /// Creates a matrix transformation for the view.
        /// </summary>
        /// <param name="from">The position of the eye (camera).</param>
        /// <param name="to">The destination point that the eye (camera) is looking at.</param>
        /// <param name="up">The direction of "up" from the eye's (camera's) point of view.</param>
        public static Matrix4x4 CreateLookAt(Point from, Point to, Vector up)
        {
            Vector forward = to.Subtract(from).Normalize();
            Vector upNormalized = up.Normalize();
            Vector left = forward.Cross(upNormalized);
            Vector trueUp = left.Cross(forward);

            var orientation = new Matrix4x4(
                left.X, left.Y, left.Z, 0,
                trueUp.X, trueUp.Y, trueUp.Z, 0,
                -forward.X, -forward.Y, -forward.Z, 0,
                0, 0, 0, 1);

            Matrix4x4 result = orientation * CreateTranslation(-from.X, -from.Y, -from.Z);
            return result;
        }
    }
}
