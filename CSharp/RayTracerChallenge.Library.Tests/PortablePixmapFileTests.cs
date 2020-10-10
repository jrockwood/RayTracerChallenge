// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="PortablePixmapFileTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using FluentAssertions;
    using NUnit.Framework;

    public class PortablePixmapFileTests
    {
        private async Task<string> GetPpmContents(PortablePixmapFile file)
        {
            await using var stream = new MemoryStream();
            await file.SerializeAsync(stream);
            stream.Position = 0;
            using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
            string contents = await reader.ReadToEndAsync();
            return contents;
        }

        [Test]
        public async Task Constructing_the_PPM_header()
        {
            var file = new PortablePixmapFile(5, 3, Array.Empty<Color>());
            string contents = await GetPpmContents(file);
            contents.Split('\n').Take(3).Should().HaveCount(3).And.ContainInOrder("P3", "5 3", "255");
        }

        [Test]
        public async Task Constructing_the_PPM_pixel_data()
        {
            const int width = 5;
            const int height = 3;

            var pixels = new Color[width * height];
            var file = new PortablePixmapFile(width, height, pixels);
            pixels[0] = new Color(1.5f, 0f, 0f);
            pixels[(1 * width) + 2] = new Color(0f, 0.5f, 0f);
            pixels[(2 * width) + 4] = new Color(-0.5f, 0f, 1f);

            string contents = await GetPpmContents(file);
            contents.Split('\n')
                .Skip(3)
                .Should()
                .HaveCount(2)
                .And.ContainInOrder(
                    "255 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 128 0 0 0 0 0 0 0 0 0 0",
                    "0 0 0 0 0 0 0 0 0 0 0 255");
        }

        [Test]
        public async Task Splitting_long_lines_in_PPM_files()
        {
            const int width = 10;
            const int height = 2;

            var pixels = Enumerable.Repeat(new Color(1f, 0.8f, 0.6f), width * height).ToArray();
            var file = new PortablePixmapFile(10, 2, pixels);
            string contents = await GetPpmContents(file);
            contents.Split('\n')
                .Skip(3)
                .Should()
                .HaveCount(4)
                .And.ContainInOrder(
                    "255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204",
                    "153 255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255",
                    "204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204 153",
                    "255 204 153 255 204 153 255 204 153");
        }

        [Test]
        public async Task PPM_files_are_terminated_by_a_newline_character()
        {
            var file = new PortablePixmapFile(5, 3, Array.Empty<Color>());
            string contents = await GetPpmContents(file);
            contents.EndsWith('\n');
        }
    }
}
