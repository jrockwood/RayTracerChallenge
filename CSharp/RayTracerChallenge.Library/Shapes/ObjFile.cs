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

        private ObjFile(
            int ignoredLineCount,
            IReadOnlyList<Point> vertices,
            IReadOnlyList<Vector> normals,
            IReadOnlyList<Group> groups)
        {
            IgnoredLineCount = ignoredLineCount;
            Vertices = vertices;
            Normals = normals;
            Groups = groups;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public int IgnoredLineCount { get; }

        public IReadOnlyList<Point> Vertices { get; }

        public IReadOnlyList<Vector> Normals { get; }

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
            var normals = new List<Vector>();
            var groups = new List<Group>();
            var currentGroup = new Group("Default");
            groups.Add(currentGroup);

            string[] lines = contents.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines.Where(s => !string.IsNullOrWhiteSpace(s)))
            {
                string[] args = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string command = args.Length > 0 ? args[0] : string.Empty;
                var arguments = args.Skip(1);

                switch (command)
                {
                    case "v":
                        vertices.Add(ParseVertex(arguments));
                        break;

                    case "vn":
                        normals.Add(ParseVertexNormal(arguments));
                        break;

                    case "f":
                        ParseTriangulatedFace(arguments, vertices, normals, currentGroup);
                        break;

                    case "g":
                        currentGroup = ParseGroup(arguments);
                        groups.Add(currentGroup);
                        break;

                    default:
                        ignoredLines++;
                        break;
                }
            }

            return new ObjFile(ignoredLines, vertices, normals, groups);
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

        /// <summary>
        /// Parses vertex line of the form 'v 1 2 3'.
        /// </summary>
        private static Point ParseVertex(IEnumerable<string> args)
        {
            (double x, double y, double z) = ParseTuple(args);
            return new Point(x, y, z);
        }

        /// <summary>
        /// Parses a vertex normal line of the form 'vn 1 2 3'.
        /// </summary>
        private static Vector ParseVertexNormal(IEnumerable<string> args)
        {
            (double x, double y, double z) = ParseTuple(args);
            return new Vector(x, y, z);
        }

        private static (double x, double y, double z) ParseTuple(IEnumerable<string> args)
        {
            string[] coords = args.ToArray();
            if (coords.Length < 3)
            {
                throw new InvalidOperationException();
            }

            double x = double.Parse(coords[0], CultureInfo.InvariantCulture);
            double y = double.Parse(coords[1], CultureInfo.InvariantCulture);
            double z = double.Parse(coords[2], CultureInfo.InvariantCulture);

            return (x, y, z);
        }

        /// <summary>
        /// Parses a face line of the form 'f 1 2 3 4 5' or 'f 1/2/3 2/3/4 3/4/5'.
        /// </summary>
        private static void ParseTriangulatedFace(
            IEnumerable<string> args,
            IReadOnlyList<Point> vertices,
            IReadOnlyList<Vector> normals,
            Group group)
        {
            string[] faceVertices = args.ToArray();
            if (faceVertices.Length < 3)
            {
                throw new InvalidOperationException(
                    $"Face needs at least three arguments: {string.Join(' ', faceVertices)}");
            }

            // If the extended syntax is used for at least one face vertex (with slashes), then all of them must use it.
            bool useExtendedSyntax = faceVertices.Any(x => x.Contains('/'));
            if (useExtendedSyntax && !faceVertices.All(x => x.Contains('/')))
            {
                throw new InvalidOperationException(
                    $"Face arguments must all be extended if one of them is: {string.Join(' ', faceVertices)}");
            }

            // Face indexes are all 1-based; convert to 0-based.
            int ConvertFaceIndex(string s)
            {
                if (!int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result))
                {
                    throw new InvalidOperationException($"Invalid face: {string.Join(' ', faceVertices)}");
                }

                return result - 1;
            }

            // Local function to parse an extended syntax face vertex of the form 1/2/3 or 1//3.
            (Point p, Vector n) ParseFaceVertex(string faceVertex)
            {
                string[] vertexInfo = faceVertex.Split('/', StringSplitOptions.None);
                if (vertexInfo.Length != 3)
                {
                    throw new InvalidOperationException($"Invalid face: {string.Join(' ', faceVertices)}");
                }

                // 1/2/3 -> 1 = vertex index, 2 = texture index, 3 = normal index
                int vertexIndex = ConvertFaceIndex(vertexInfo[0]);
                int normalIndex = ConvertFaceIndex(vertexInfo[2]);

                return (vertices[vertexIndex], normals[normalIndex]);
            }

            // Loop through all of the vertexes and triangulate if there are more than 3 vertexes.
            for (int i = 1; i < faceVertices.Length - 1; i++)
            {
                string faceVertex = faceVertices[i];

                // If the face has extended information of the form '1/2/3', then we want to create a SmoothTriangle with
                // the normals. Otherwise, it's just a plain triangle.
                Triangle triangle;
                if (useExtendedSyntax)
                {
                    (Point p1, Vector n1) = ParseFaceVertex(faceVertices[0]);
                    (Point p2, Vector n2) = ParseFaceVertex(faceVertices[i]);
                    (Point p3, Vector n3) = ParseFaceVertex(faceVertices[i + 1]);

                    triangle = new SmoothTriangle(p1, p2, p3, n1, n2, n3);
                }
                else
                {
                    var p1 = vertices[ConvertFaceIndex(faceVertices[0])];
                    var p2 = vertices[ConvertFaceIndex(faceVertices[i])];
                    var p3 = vertices[ConvertFaceIndex(faceVertices[i + 1])];
                    triangle = new Triangle(p1, p2, p3);
                }

                group.AddChild(triangle);
            }
        }

        private static Group ParseGroup(IEnumerable<string> args)
        {
            string groupName = args.Single();
            var group = new Group(groupName);
            return group;
        }
    }
}
