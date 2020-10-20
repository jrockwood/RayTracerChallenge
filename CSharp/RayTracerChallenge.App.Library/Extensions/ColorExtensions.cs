// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorExtensions.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Library.Extensions
{
    using System;
    using RayTracerChallenge.Library;

    internal static class ColorExtensions
    {
        public static int ToRgb(this Color color)
        {
            Color scaled = color * 255;
            byte r = (byte)Math.Clamp((int)Math.Round(scaled.Red), 0, 255);
            byte g = (byte)Math.Clamp((int)Math.Round(scaled.Green), 0, 255);
            byte b = (byte)Math.Clamp((int)Math.Round(scaled.Blue), 0, 255);

            return (r << 16) | (g << 8) | (b << 0);
        }
    }
}
