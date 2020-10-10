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
        public const float Epsilon = 0.00001f;

        public static bool IsApproximatelyEqual(this float number, float other)
        {
            return MathF.Abs(number - other) < Epsilon;
        }

        public static float RoundToEpsilon(this float number)
        {
            return MathF.Round(number, digits: 5);
        }
    }
}
