// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="NumberExtensions.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library
{
    using System;

    /// <summary>
    /// Contains extension methods for numbers.
    /// </summary>
    public static class NumberExtensions
    {
        public const double Epsilon = 0.00001;

        public static bool IsApproximatelyEqual(this double number, double other)
        {
            return Math.Abs(number - other) < Epsilon;
        }

        public static double RoundToEpsilon(this double number)
        {
            return Math.Round(number, digits: 5);
        }
    }
}
