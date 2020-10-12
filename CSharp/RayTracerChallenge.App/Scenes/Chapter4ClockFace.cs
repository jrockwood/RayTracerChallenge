// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Chapter4ClockFace.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Scenes
{
    using System;
    using RayTracerChallenge.Library;

    public sealed class Chapter4ClockFace : SimpleScene
    {
        public Chapter4ClockFace()
            : base(
                "Chapter 4 - Clock Face",
                "Draws dots representing a clock face. Tests matrix transformations",
                400,
                400)
        {
        }

        protected override void RenderToCanvas(Canvas canvas)
        {
            int centerX = canvas.Width / 2;
            int centerY = canvas.Height / 2;
            float clockRadius = MathF.Min(canvas.Width * (3f / 8), canvas.Height * (3f / 8));

            var color = Colors.Cyan;
            const int pixelBorderSize = 4;

            // The clock is oriented along the y axis, which means when looking at it face-on you're looking
            // towards the negative y axis and the clock face sits on the x-z plane.
            var twelve = new Point(0, 0, 1);
            const float rotationAngle = (2 * MathF.PI) / 12;

            for (int hour = 0; hour < 12; hour++)
            {
                // Rotate the twelve point around the y axis.
                // Scale by the clock radius.
                // Translate to the center of the canvas.
                var transform = Matrix4x4.CreateRotationY(hour * rotationAngle);
                Point hourPoint = transform * twelve;
                int x = centerX + (int)MathF.Round(hourPoint.X * clockRadius);
                int y = centerY - (int)MathF.Round(hourPoint.Z * clockRadius);
                canvas.FillRect(
                    top: y - pixelBorderSize,
                    left: x - pixelBorderSize,
                    bottom: y + pixelBorderSize,
                    right: x + pixelBorderSize,
                    color);
            }
        }
    }
}
