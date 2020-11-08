// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjFile.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.Library.Shapes
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class ObjFile
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        private ObjFile(int ignoredLineCount, IReadOnlyList<Point> vertices, Group defaultGroup)
        {
            IgnoredLineCount = ignoredLineCount;
            Vertices = vertices;
            DefaultGroup = defaultGroup;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public int IgnoredLineCount { get; }

        public IReadOnlyList<Point> Vertices { get; }

        public Group DefaultGroup { get; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public static ObjFile Parse(string contents)
        {
            int ignoredLines = 0;
            var vertices = new List<Point>();
            var group = new Group();

            string[] lines = contents.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines.Where(s => !string.IsNullOrWhiteSpace(s)))
            {
                switch (line.Substring(0, 2))
                {
                    case "v ":
                        vertices.Add(ParsePoint(line.Substring(2)));
                        break;

                    case "f ":
                        ParseTriangulatedFace(line.Substring(2), vertices, group);
                        break;

                    default:
                        ignoredLines++;
                        break;
                }
            }

            return new ObjFile(ignoredLines, vertices, group);
        }

        private static Point ParsePoint(string contents)
        {
            string[] points = contents.Split(' ');
            if (points.Length < 3)
            {
                throw new InvalidOperationException();
            }

            double p1 = double.Parse(points[0], CultureInfo.InvariantCulture);
            double p2 = double.Parse(points[1], CultureInfo.InvariantCulture);
            double p3 = double.Parse(points[2], CultureInfo.InvariantCulture);

            return new Point(p1, p2, p3);
        }

        private static void ParseTriangulatedFace(string contents, IReadOnlyList<Point> vertices, Group group)
        {
            string[] faceIndexStrings = contents.Split(' ');
            if (faceIndexStrings.Length < 3)
            {
                throw new InvalidOperationException();
            }

            // Face indexes are all 1-based; convert to 0-based.
            int[] faceIndexes = faceIndexStrings.Select(x => int.Parse(x, CultureInfo.InvariantCulture) - 1).ToArray();

            for (int i = 1; i < faceIndexes.Length - 1; i++)
            {
                var triangle = new Triangle(
                    vertices[faceIndexes[0]],
                    vertices[faceIndexes[i]],
                    vertices[faceIndexes[i + 1]]);

                group.AddChild(triangle);
            }
        }
    }
}
