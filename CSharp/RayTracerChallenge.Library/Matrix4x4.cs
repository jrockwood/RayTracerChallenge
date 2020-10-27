// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Matrix4x4.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library
{
    using System;
    using System.Diagnostics.CodeAnalysis;

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
            double m00, double m01, double m02, double m03,
            double m10, double m11, double m12, double m13,
            double m20, double m21, double m22, double m23,
            double m30, double m31, double m32, double m33)
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

        public double this[int row, int col] =>
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

        public double M00 { get; }
        public double M01 { get; }
        public double M02 { get; }
        public double M03 { get; }

        public double M10 { get; }
        public double M11 { get; }
        public double M12 { get; }
        public double M13 { get; }

        public double M20 { get; }
        public double M21 { get; }
        public double M22 { get; }
        public double M23 { get; }

        public double M30 { get; }
        public double M31 { get; }
        public double M32 { get; }
        public double M33 { get; }

        public double Determinant
        {
            get
            {
                double result = 0;

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
            return a.Multiply(b);
        }

        public Matrix4x4 Multiply(Matrix4x4 b)
        {
            return new Matrix4x4(
                // First row
                (M00 * b.M00) + (M01 * b.M10) + (M02 * b.M20) + (M03 * b.M30),
                (M00 * b.M01) + (M01 * b.M11) + (M02 * b.M21) + (M03 * b.M31),
                (M00 * b.M02) + (M01 * b.M12) + (M02 * b.M22) + (M03 * b.M32),
                (M00 * b.M03) + (M01 * b.M13) + (M02 * b.M23) + (M03 * b.M33),

                // Second row
                (M10 * b.M00) + (M11 * b.M10) + (M12 * b.M20) + (M13 * b.M30),
                (M10 * b.M01) + (M11 * b.M11) + (M12 * b.M21) + (M13 * b.M31),
                (M10 * b.M02) + (M11 * b.M12) + (M12 * b.M22) + (M13 * b.M32),
                (M10 * b.M03) + (M11 * b.M13) + (M12 * b.M23) + (M13 * b.M33),

                // Third row
                (M20 * b.M00) + (M21 * b.M10) + (M22 * b.M20) + (M23 * b.M30),
                (M20 * b.M01) + (M21 * b.M11) + (M22 * b.M21) + (M23 * b.M31),
                (M20 * b.M02) + (M21 * b.M12) + (M22 * b.M22) + (M23 * b.M32),
                (M20 * b.M03) + (M21 * b.M13) + (M22 * b.M23) + (M23 * b.M33),

                // Fourth row
                (M30 * b.M00) + (M31 * b.M10) + (M32 * b.M20) + (M33 * b.M30),
                (M30 * b.M01) + (M31 * b.M11) + (M32 * b.M21) + (M33 * b.M31),
                (M30 * b.M02) + (M31 * b.M12) + (M32 * b.M22) + (M33 * b.M32),
                (M30 * b.M03) + (M31 * b.M13) + (M32 * b.M23) + (M33 * b.M33)

            );
        }

        public static Point operator *(Matrix4x4 matrix, Point point)
        {
            return matrix.Multiply(point);
        }

        public Point Multiply(Point point)
        {
            double x = (M00 * point.X) + (M01 * point.Y) + (M02 * point.Z) + M03;
            double y = (M10 * point.X) + (M11 * point.Y) + (M12 * point.Z) + M13;
            double z = (M20 * point.X) + (M21 * point.Y) + (M22 * point.Z) + M23;

            return new Point(x, y, z);
        }

        public static Vector operator *(Matrix4x4 matrix, Vector vector)
        {
            return matrix.Multiply(vector);
        }

        public Vector Multiply(Vector vector)
        {
            double x = (M00 * vector.X) + (M01 * vector.Y) + (M02 * vector.Z);
            double y = (M10 * vector.X) + (M11 * vector.Y) + (M12 * vector.Z);
            double z = (M20 * vector.X) + (M21 * vector.Y) + (M22 * vector.Z);

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

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public Matrix4x4 Invert()
        {
            // The following is copied from the .NET implementation
            // (https://referencesource.microsoft.com/#System.Numerics/System/Numerics/Matrix4x4.cs). In performance
            // testing, this method was a bottleneck, so this implementation is used, which is much faster, although
            // much harder to read.

            //                                       -1
            // If you have matrix M, inverse Matrix M   can compute
            //
            //     -1       1
            //    M   = --------- A
            //            det(M)
            //
            // A is adjugate (adjoint) of M, where,
            //
            //      T
            // A = C
            //
            // C is Cofactor matrix of M, where,
            //           i + j
            // C   = (-1)      * det(M  )
            //  ij                    ij
            //
            //     [ a b c d ]
            // M = [ e f g h ]
            //     [ i j k l ]
            //     [ m n o p ]
            //
            // First Row
            //           2 | f g h |
            // C   = (-1)  | j k l | = + ( f ( kp - lo ) - g ( jp - ln ) + h ( jo - kn ) )
            //  11         | n o p |
            //
            //           3 | e g h |
            // C   = (-1)  | i k l | = - ( e ( kp - lo ) - g ( ip - lm ) + h ( io - km ) )
            //  12         | m o p |
            //
            //           4 | e f h |
            // C   = (-1)  | i j l | = + ( e ( jp - ln ) - f ( ip - lm ) + h ( in - jm ) )
            //  13         | m n p |
            //
            //           5 | e f g |
            // C   = (-1)  | i j k | = - ( e ( jo - kn ) - f ( io - km ) + g ( in - jm ) )
            //  14         | m n o |
            //
            // Second Row
            //           3 | b c d |
            // C   = (-1)  | j k l | = - ( b ( kp - lo ) - c ( jp - ln ) + d ( jo - kn ) )
            //  21         | n o p |
            //
            //           4 | a c d |
            // C   = (-1)  | i k l | = + ( a ( kp - lo ) - c ( ip - lm ) + d ( io - km ) )
            //  22         | m o p |
            //
            //           5 | a b d |
            // C   = (-1)  | i j l | = - ( a ( jp - ln ) - b ( ip - lm ) + d ( in - jm ) )
            //  23         | m n p |
            //
            //           6 | a b c |
            // C   = (-1)  | i j k | = + ( a ( jo - kn ) - b ( io - km ) + c ( in - jm ) )
            //  24         | m n o |
            //
            // Third Row
            //           4 | b c d |
            // C   = (-1)  | f g h | = + ( b ( gp - ho ) - c ( fp - hn ) + d ( fo - gn ) )
            //  31         | n o p |
            //
            //           5 | a c d |
            // C   = (-1)  | e g h | = - ( a ( gp - ho ) - c ( ep - hm ) + d ( eo - gm ) )
            //  32         | m o p |
            //
            //           6 | a b d |
            // C   = (-1)  | e f h | = + ( a ( fp - hn ) - b ( ep - hm ) + d ( en - fm ) )
            //  33         | m n p |
            //
            //           7 | a b c |
            // C   = (-1)  | e f g | = - ( a ( fo - gn ) - b ( eo - gm ) + c ( en - fm ) )
            //  34         | m n o |
            //
            // Fourth Row
            //           5 | b c d |
            // C   = (-1)  | f g h | = - ( b ( gl - hk ) - c ( fl - hj ) + d ( fk - gj ) )
            //  41         | j k l |
            //
            //           6 | a c d |
            // C   = (-1)  | e g h | = + ( a ( gl - hk ) - c ( el - hi ) + d ( ek - gi ) )
            //  42         | i k l |
            //
            //           7 | a b d |
            // C   = (-1)  | e f h | = - ( a ( fl - hj ) - b ( el - hi ) + d ( ej - fi ) )
            //  43         | i j l |
            //
            //           8 | a b c |
            // C   = (-1)  | e f g | = + ( a ( fk - gj ) - b ( ek - gi ) + c ( ej - fi ) )
            //  44         | i j k |
            //
            // Cost of operation
            // 53 adds, 104 multiplies, and 1 div.

            double a = M00, b = M01, c = M02, d = M03;
            double e = M10, f = M11, g = M12, h = M13;
            double i = M20, j = M21, k = M22, l = M23;
            double m = M30, n = M31, o = M32, p = M33;

            double kp_lo = (k * p) - (l * o);
            double jp_ln = (j * p) - (l * n);
            double jo_kn = (j * o) - (k * n);
            double ip_lm = (i * p) - (l * m);
            double io_km = (i * o) - (k * m);
            double in_jm = (i * n) - (j * m);

            double a11 = +(((f * kp_lo) - (g * jp_ln)) + (h * jo_kn));
            double a12 = -(((e * kp_lo) - (g * ip_lm)) + (h * io_km));
            double a13 = +(((e * jp_ln) - (f * ip_lm)) + (h * in_jm));
            double a14 = -(((e * jo_kn) - (f * io_km)) + (g * in_jm));

            double det = (a * a11) + (b * a12) + (c * a13) + (d * a14);

            if (Math.Abs(det) < double.Epsilon)
            {
                throw new InvalidOperationException("The matrix is not invertible");
            }

            double invDet = 1.0 / det;

            double result_M00 = a11 * invDet;
            double result_M10 = a12 * invDet;
            double result_M20 = a13 * invDet;
            double result_M30 = a14 * invDet;

            double result_M01 = -(((b * kp_lo) - (c * jp_ln)) + (d * jo_kn)) * invDet;
            double result_M11 = +(((a * kp_lo) - (c * ip_lm)) + (d * io_km)) * invDet;
            double result_M21 = -(((a * jp_ln) - (b * ip_lm)) + (d * in_jm)) * invDet;
            double result_M31 = +(((a * jo_kn) - (b * io_km)) + (c * in_jm)) * invDet;

            double gp_ho = (g * p) - (h * o);
            double fp_hn = (f * p) - (h * n);
            double fo_gn = (f * o) - (g * n);
            double ep_hm = (e * p) - (h * m);
            double eo_gm = (e * o) - (g * m);
            double en_fm = (e * n) - (f * m);

            double result_M02 = +(((b * gp_ho) - (c * fp_hn)) + (d * fo_gn)) * invDet;
            double result_M12 = -(((a * gp_ho) - (c * ep_hm)) + (d * eo_gm)) * invDet;
            double result_M22 = +(((a * fp_hn) - (b * ep_hm)) + (d * en_fm)) * invDet;
            double result_M32 = -(((a * fo_gn) - (b * eo_gm)) + (c * en_fm)) * invDet;

            double gl_hk = (g * l) - (h * k);
            double fl_hj = (f * l) - (h * j);
            double fk_gj = (f * k) - (g * j);
            double el_hi = (e * l) - (h * i);
            double ek_gi = (e * k) - (g * i);
            double ej_fi = (e * j) - (f * i);

            double result_M03 = -(((b * gl_hk) - (c * fl_hj)) + (d * fk_gj)) * invDet;
            double result_M13 = +(((a * gl_hk) - (c * el_hi)) + (d * ek_gi)) * invDet;
            double result_M23 = -(((a * fl_hj) - (b * el_hi)) + (d * ej_fi)) * invDet;
            double result_M33 = +(((a * fk_gj) - (b * ek_gi)) + (c * ej_fi)) * invDet;

            return new Matrix4x4(
                result_M00, result_M01, result_M02, result_M03,
                result_M10, result_M11, result_M12, result_M13,
                result_M20, result_M21, result_M22, result_M23,
                result_M30, result_M31, result_M32, result_M33);
        }

        public static Matrix4x4 CreateTranslation(double x, double y, double z)
        {
            return new Matrix4x4(
                1, 0, 0, x,
                0, 1, 0, y,
                0, 0, 1, z,
                0, 0, 0, 1);
        }

        public Matrix4x4 Translate(double x, double y, double z)
        {
            return CreateTranslation(x, y, z) * this;
        }

        public static Matrix4x4 CreateScaling(double x, double y, double z)
        {
            return new Matrix4x4(
                x, 0, 0, 0,
                0, y, 0, 0,
                0, 0, z, 0,
                0, 0, 0, 1);
        }

        public Matrix4x4 Scale(double x, double y, double z)
        {
            return CreateScaling(x, y, z) * this;
        }

        public static Matrix4x4 CreateRotationX(double radians)
        {
            double cos = Math.Cos(radians);
            double sin = Math.Sin(radians);

            return new Matrix4x4(
                1, 0, 0, 0,
                0, cos, -sin, 0,
                0, sin, cos, 0,
                0, 0, 0, 1);
        }

        public Matrix4x4 RotateX(double radians)
        {
            return CreateRotationX(radians) * this;
        }

        public static Matrix4x4 CreateRotationY(double radians)
        {
            double cos = Math.Cos(radians);
            double sin = Math.Sin(radians);

            return new Matrix4x4(
                cos, 0, sin, 0,
                0, 1, 0, 0,
                -sin, 0, cos, 0,
                0, 0, 0, 1);
        }

        public Matrix4x4 RotateY(double radians)
        {
            return CreateRotationY(radians) * this;
        }

        public static Matrix4x4 CreateRotationZ(double radians)
        {
            double cos = Math.Cos(radians);
            double sin = Math.Sin(radians);

            return new Matrix4x4(
                cos, -sin, 0, 0,
                sin, cos, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1);
        }

        public Matrix4x4 RotateZ(double radians)
        {
            return CreateRotationZ(radians) * this;
        }

        public static Matrix4x4 CreateShearing(double xy, double xz, double yx, double yz, double zx, double zy)
        {
            return new Matrix4x4(
                1, xy, xz, 0,
                yx, 1, yz, 0,
                zx, zy, 1, 0,
                0, 0, 0, 1);
        }

        public Matrix4x4 Shear(double xy, double xz, double yx, double yz, double zx, double zy)
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
