// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Canvas.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library
{
    using System;
    using System.Linq;

    /// <summary>
    /// Represents a canvas that contains a color for every pixel.
    /// </summary>
    public class Canvas
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private readonly Color[] _pixels;

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public Canvas(int width, int height)
        {
            Width = width;
            Height = height;

            _pixels = Enumerable.Repeat(Colors.Black, width * height).ToArray();
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public int Width { get; }
        public int Height { get; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public Color GetPixel(int x, int y)
        {
            int index = CalculateIndex(x, y);
            return _pixels[index];
        }

        public void SetPixel(int x, int y, Color color)
        {
            int index = CalculateIndex(x, y);
            _pixels[index] = color;
        }

        public void FillRect(int top, int left, int bottom, int right, Color color)
        {
            for (int x = Math.Max(0, left); x <= Math.Min(right, Width - 1); x++)
            {
                for (int y = Math.Max(0, top); y <= Math.Min(bottom, Height - 1); y++)
                {
                    SetPixel(x, y, color);
                }
            }
        }

        private int CalculateIndex(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                throw new IndexOutOfRangeException($"Index is outside the bounds of the canvas: x={x}, y={y}");
            }

            return (y * Width) + x;
        }
    }
}
