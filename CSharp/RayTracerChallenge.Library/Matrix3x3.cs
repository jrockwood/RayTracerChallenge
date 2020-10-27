// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Matrix3x3.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library
{
    using System;

    /// <summary>
    /// Represents an immutable 3x3 matrix.
    /// </summary>
    public readonly struct Matrix3x3 : IEquatable<Matrix3x3>
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        public static readonly Matrix3x3 Zero = new Matrix3x3(0, 0, 0, 0, 0, 0, 0, 0, 0);

        public static readonly Matrix3x3 Identity = new Matrix3x3(
            1, 0, 0,
            0, 1, 0,
            0, 0, 1);

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public Matrix3x3(
            double m00, double m01, double m02,
            double m10, double m11, double m12,
            double m20, double m21, double m22)
        {
            M00 = m00;
            M01 = m01;
            M02 = m02;

            M10 = m10;
            M11 = m11;
            M12 = m12;

            M20 = m20;
            M21 = m21;
            M22 = m22;
        }

        //// ===========================================================================================================
        //// Indexers
        //// ===========================================================================================================

        public double this[int row, int col] =>
            (row, col) switch
            {
                (0, 0) => M00,
                (0, 1) => M01,
                (0, 2) => M02,

                (1, 0) => M10,
                (1, 1) => M11,
                (1, 2) => M12,

                (2, 0) => M20,
                (2, 1) => M21,
                (2, 2) => M22,
                (_, _) => throw new IndexOutOfRangeException()
            };

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public double M00 { get; }
        public double M01 { get; }
        public double M02 { get; }

        public double M10 { get; }
        public double M11 { get; }
        public double M12 { get; }

        public double M20 { get; }
        public double M21 { get; }
        public double M22 { get; }

        public double Determinant
        {
            get
            {
                double result = 0;
                for (int column = 0; column <= 2; column++)
                {
                    result += this[0, column] * Cofactor(0, column);
                }

                return result;
            }
        }

        //// ===========================================================================================================
        //// Equality Members
        //// ===========================================================================================================

        public bool Equals(Matrix3x3 other)
        {
            return M00.IsApproximatelyEqual(other.M00) &&
                   M01.IsApproximatelyEqual(other.M01) &&
                   M02.IsApproximatelyEqual(other.M02) &&

                   M10.IsApproximatelyEqual(other.M10) &&
                   M11.IsApproximatelyEqual(other.M11) &&
                   M12.IsApproximatelyEqual(other.M12) &&

                   M20.IsApproximatelyEqual(other.M20) &&
                   M21.IsApproximatelyEqual(other.M21) &&
                   M22.IsApproximatelyEqual(other.M22);
        }

        public override bool Equals(object? obj)
        {
            return obj is Matrix3x3 other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                HashCode.Combine(M00.RoundToEpsilon(), M01.RoundToEpsilon(), M02.RoundToEpsilon()),
                HashCode.Combine(M10.RoundToEpsilon(), M11.RoundToEpsilon(), M12.RoundToEpsilon()),
                HashCode.Combine(M20.RoundToEpsilon(), M21.RoundToEpsilon(), M22.RoundToEpsilon()));
        }

        public static bool operator ==(Matrix3x3? left, Matrix3x3? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Matrix3x3? left, Matrix3x3? right)
        {
            return !Equals(left, right);
        }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public override string ToString()
        {
            return $"|{M00} {M01} {M02}|\n|{M10} {M11} {M12}|\n|{M20} {M21} {M22}|";
        }

        public Matrix2x2 Submatrix(int rowToRemove, int columnToRemove)
        {
            int row0 = rowToRemove == 0 ? 1 : 0;
            int row1 = rowToRemove < 2 ? 2 : 1;
            int col0 = columnToRemove == 0 ? 1 : 0;
            int col1 = columnToRemove < 2 ? 2 : 1;

            return new Matrix2x2(this[row0, col0], this[row0, col1], this[row1, col0], this[row1, col1]);
        }

        internal double Minor(int row, int column)
        {
            var submatrix = Submatrix(row, column);
            return submatrix.Determinant;
        }

        internal double Cofactor(int row, int column)
        {
            double minor = Minor(row, column);

            // If row + column is odd, then we need to negate the minor.
            if ((row + column) % 2 == 1)
            {
                minor = -minor;
            }

            return minor;
        }
    }
}
