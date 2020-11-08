// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjFileTests.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Tests.Shapes
{
    using System;
    using System.IO;
    using System.Text;
    using FluentAssertions;
    using NUnit.Framework;
    using RayTracerChallenge.Library.Shapes;

    public class ObjFileTests
    {
        [Test]
        public void Parse_should_silently_ignore_any_unrecognized_statements()
        {
            const string gibberish = @"
There was a young lady named Bright
who traveled much faster than light.
She set out one day
in a relative way,
and came back the previous night.";

            var objFile = ObjFile.Parse(gibberish);
            objFile.IgnoredLineCount.Should().Be(5);
        }

        [Test]
        public void Parse_should_process_vertex_data_from_the_given_input()
        {
            const string contents = @"
v -1 1 0
v -1.0000 0.5000 0.0000
v 1 0 0
v 1 1 0";

            var objFile = ObjFile.Parse(contents);
            objFile.Vertices.Should()
                .HaveCount(4)
                .And.ContainInOrder(
                    new Point(-1, 1, 0),
                    new Point(-1, 0.5, 0),
                    new Point(1, 0, 0),
                    new Point(1, 1, 0));
        }

        [Test]
        public void Parse_should_process_triangle_data_from_the_given_input()
        {
            const string contents = @"
v -1 1 0
v -1 0 0
v 1 0 0
v 1 1 0

f 1 2 3
f 1 3 4";

            var objFile = ObjFile.Parse(contents);
            Group g = objFile.DefaultGroup;
            var t1 = (Triangle)g.Children[0];
            var t2 = (Triangle)g.Children[1];

            t1.P1.Should().Be(objFile.Vertices[0]);
            t1.P2.Should().Be(objFile.Vertices[1]);
            t1.P3.Should().Be(objFile.Vertices[2]);

            t2.P1.Should().Be(objFile.Vertices[0]);
            t2.P2.Should().Be(objFile.Vertices[2]);
            t2.P3.Should().Be(objFile.Vertices[3]);
        }

        [Test]
        public void Parse_should_process_and_triangulate_polygonal_data_from_the_given_input()
        {
            const string contents = @"
v -1 1 0
v -1 0 0
v 1 0 0
v 1 1 0
v 0 2 0

f 1 2 3 4 5";

            var objFile = ObjFile.Parse(contents);
            Group g = objFile.DefaultGroup;
            var t1 = (Triangle)g.Children[0];
            var t2 = (Triangle)g.Children[1];
            var t3 = (Triangle)g.Children[2];

            t1.P1.Should().Be(objFile.Vertices[0]);
            t1.P2.Should().Be(objFile.Vertices[1]);
            t1.P3.Should().Be(objFile.Vertices[2]);

            t2.P1.Should().Be(objFile.Vertices[0]);
            t2.P2.Should().Be(objFile.Vertices[2]);
            t2.P3.Should().Be(objFile.Vertices[3]);

            t3.P1.Should().Be(objFile.Vertices[0]);
            t3.P2.Should().Be(objFile.Vertices[3]);
            t3.P3.Should().Be(objFile.Vertices[4]);
        }

        [Test]
        public void Parse_should_recognize_a_group_statement_and_add_subsequent_triangles_to_the_named_group()
        {
            const string contents = @"
v -1 1 0
v -1 0 0
v 1 0 0
v 1 1 0

g FirstGroup
f 1 2 3
g SecondGroup
f 1 3 4";

            using var stream = new MemoryStream();
            using var writer = new StreamWriter(stream, Encoding.ASCII, leaveOpen: true) { AutoFlush = true };
            writer.Write(contents);
            stream.Position = 0;

            var objFile = ObjFile.Parse(stream);
            Group group1 = objFile.FindGroupByName("FirstGroup") ?? throw new InvalidOperationException();
            Group group2 = objFile.FindGroupByName("SecondGroup") ?? throw new InvalidOperationException();

            var t1 = (Triangle)group1.Children[0];
            var t2 = (Triangle)group2.Children[0];

            t1.P1.Should().Be(objFile.Vertices[0]);
            t1.P2.Should().Be(objFile.Vertices[1]);
            t1.P3.Should().Be(objFile.Vertices[2]);

            t2.P1.Should().Be(objFile.Vertices[0]);
            t2.P2.Should().Be(objFile.Vertices[2]);
            t2.P3.Should().Be(objFile.Vertices[3]);
        }
    }
}
