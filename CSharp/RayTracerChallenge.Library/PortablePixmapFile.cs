// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="PortablePixmapFile.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a Portable Pixmap (PPM) file, which is a textual representation of a bitmap.
    /// </summary>
    public class PortablePixmapFile
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public PortablePixmapFile(int width, int height, IReadOnlyList<Color> pixels)
        {
            Width = width;
            Height = height;
            Pixels = pixels;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public int Width { get; }
        public int Height { get; }
        public IReadOnlyList<Color> Pixels { get; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public async Task SerializeAsync(Stream stream)
        {
            await using var writer = new StreamWriter(
                stream,
                new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
                leaveOpen: true)
            {
                NewLine = "\n"
            };

            await WriteHeader(writer);
            await WritePixelData(writer);
            await writer.FlushAsync();
        }

        private async Task WriteHeader(TextWriter writer)
        {
            // Write the magic number.
            await writer.WriteLineAsync("P3");

            // Write the width and height.
            await writer.WriteLineAsync($"{Width} {Height}");

            // Write the maximum color value, inclusive.
            await writer.WriteLineAsync("255");
        }

        private async Task WritePixelData(TextWriter writer)
        {
            var builder = new LineBuilder();
            foreach (Color pixel in Pixels)
            {
                builder.AppendColor(pixel);
            }

            string pixelDataLines = builder.ToString();
            await writer.WriteAsync(pixelDataLines);
        }

        //// ===========================================================================================================
        //// Classes
        //// ===========================================================================================================

        private sealed class LineBuilder
        {
            private const int MaxLineLength = 70;
            private readonly StringBuilder _lineBuilder = new StringBuilder(MaxLineLength + 10);
            private readonly List<string> _lines = new List<string>();

            public void AppendColor(Color color)
            {
                Color scaledColor = color * 255;
                AppendColorComponent(scaledColor.Red)
                    .AppendColorComponent(scaledColor.Green)
                    .AppendColorComponent(scaledColor.Blue);
            }

            public override string ToString()
            {
                return string.Join('\n', _lines) + '\n' + _lineBuilder;
            }

            private LineBuilder AppendColorComponent(float scaledComponent)
            {
                string clamped = Math.Clamp((int)Math.Round(scaledComponent), 0, 255)
                    .ToString(CultureInfo.InvariantCulture);

                if (_lineBuilder.Length + clamped.Length + 1 > MaxLineLength)
                {
                    _lines.Add(_lineBuilder.ToString());
                    _lineBuilder.Clear().Append(clamped);
                }
                else
                {
                    if (_lineBuilder.Length > 0)
                    {
                        _lineBuilder.Append(" ");
                    }

                    _lineBuilder.Append(clamped);
                }

                return this;
            }
        }
    }
}
