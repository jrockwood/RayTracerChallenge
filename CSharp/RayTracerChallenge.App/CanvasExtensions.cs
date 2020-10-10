// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="CanvasExtensions.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;
    using RayTracerChallenge.Library;
    using Color = RayTracerChallenge.Library.Color;

    public static class CanvasExtensions
    {
        public static Bitmap ToBitmap(this Canvas canvas)
        {
            // Create a new bitmap that is the same size as the canvas with 24bpp.
            var bitmap = new Bitmap(canvas.Width, canvas.Height, PixelFormat.Format24bppRgb);

            // Lock the bitmap's bits.
            BitmapData bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite,
                bitmap.PixelFormat);

            // Get the address of the first line in the bitmap.
            IntPtr ptr = bitmapData.Scan0;

            // Create an array to hold all of the pixel data.
            int bytes = Math.Abs(bitmapData.Stride) * bitmap.Height;
            byte[] rgbValues = new byte[bytes];

            // Local function to convert a Color3 component (from 0-1) to a byte (0-255)
            static byte ConvertColorComponent(float scaledComponent) =>
                (byte)Math.Clamp((int)MathF.Round(scaledComponent), 0, 255);

            int rgbValuesIndex = 0;
            for (int y = 0; y < canvas.Height; y++)
            {
                for (int x = 0; x < canvas.Width; x++)
                {
                    Color scaledColor = canvas.GetPixel(x, y) * 255;
                    byte red = ConvertColorComponent(scaledColor.Red);
                    byte green = ConvertColorComponent(scaledColor.Green);
                    byte blue = ConvertColorComponent(scaledColor.Blue);

                    rgbValues[rgbValuesIndex++] = red;
                    rgbValues[rgbValuesIndex++] = green;
                    rgbValues[rgbValuesIndex++] = blue;
                }
            }

            // Copy the rgb values back to the bitmap.
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits and return the bitmap.
            bitmap.UnlockBits(bitmapData);
            return bitmap;
        }
    }
}
