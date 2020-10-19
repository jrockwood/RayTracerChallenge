// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="CanvasExtensions.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Library.Extensions
{
    using System;
    using System.Windows;
    using System.Windows.Media.Imaging;
    using RayTracerChallenge.Library;
    using Color = RayTracerChallenge.Library.Color;

    internal static class CanvasExtensions
    {
        public static void RenderToWriteableBitmap(this Canvas canvas, WriteableBitmap bitmap)
        {
            int bytesPerPixel = bitmap.Format.BitsPerPixel / 8;
            if (bitmap.Format.BitsPerPixel < 24)
            {
                throw new ArgumentException("Bitmap must have at least 24 bits per pixel", nameof(bitmap));
            }

            if (bitmap.Format.BitsPerPixel % 8 != 0)
            {
                throw new ArgumentException("Bitmap must have a format where the bits per pixel is divisible by 8");
            }

            int width = canvas.Width;
            int height = canvas.Height;

            // Reserve the back buffer for updates.
            bitmap.Lock();

            try
            {
                unsafe
                {
                    // Get a pointer to the back buffer.
                    IntPtr backBuffer = bitmap.BackBuffer;

                    // Loop through the canvas data an fill in the bitmap.
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            // Get the pixel's color and convert it to rgb format.
                            Color pixel = canvas.GetPixel(x, y);
                            int rgb = pixel.ToRgb();

                            // Set the writeable bitmap's pixel.
                            IntPtr p = backBuffer + (y * bitmap.BackBufferStride) + (x * bytesPerPixel);
                            *(int*)p = rgb;
                        }
                    }
                }

                // Tell the bitmap what changed.
                bitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));
            }
            finally
            {
                bitmap.Unlock();
            }
        }
    }
}
