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
    using System.IO;
    using System.Linq;

    public class ObjFile
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        private ObjFile(int ignoredLineCount, IReadOnlyList<Point> vertices, IReadOnlyList<Group> groups)
        {
            IgnoredLineCount = ignoredLineCount;
            Vertices = vertices;
            Groups = groups;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public int IgnoredLineCount { get; }

        public IReadOnlyList<Point> Vertices { get; }

        public Group DefaultGroup => Groups[0];

        public IReadOnlyList<Group> Groups { get; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public static ObjFile Parse(Stream stream)
        {
            using var reader = new StreamReader(stream, leaveOpen: true);
            string contents = reader.ReadToEnd();
            ObjFile result = Parse(contents);
            return result;
        }

        public static ObjFile Parse(string contents)
        {
            int ignoredLines = 0;
            var vertices = new List<Point>();
            var groups = new List<Group>();
            var currentGroup = new Group("Default");
            groups.Add(currentGroup);

            string[] lines = contents.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines.Where(s => !string.IsNullOrWhiteSpace(s)))
            {
                string command = line.Length >= 2 ? line.Substring(0, 2) : string.Empty;
                string arguments = line.Length > 2 ? line.Substring(2) : string.Empty;

                switch (command)
                {
                    case "v ":
                        vertices.Add(ParsePoint(arguments));
                        break;

                    case "f ":
                        ParseTriangulatedFace(arguments, vertices, currentGroup);
                        break;

                    case "g ":
                        currentGroup = ParseGroup(arguments);
                        groups.Add(currentGroup);
                        break;

                    default:
                        ignoredLines++;
                        break;
                }
            }

            return new ObjFile(ignoredLines, vertices, groups);
        }

        public Group? FindGroupByName(string name)
        {
            return Groups.FirstOrDefault(g => g.Name == name);
        }

        public Group ToGroup()
        {
            var nonEmptyGroups = Groups.Where(g => g.Children.Count != 0).ToList();

            if (nonEmptyGroups.Count == 1)
            {
                return nonEmptyGroups[0];
            }

            var group = new Group();
            nonEmptyGroups.ForEach(group.AddChild);
            return group;
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
            string[] faceIndexStrings = contents.Split(' ', StringSplitOptions.RemoveEmptyEntries);
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

        private static Group ParseGroup(string contents)
        {
            string groupName = contents;
            var group = new Group(groupName);
            return group;
        }
    }
}
