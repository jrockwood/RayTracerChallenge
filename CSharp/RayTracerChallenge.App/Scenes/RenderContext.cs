// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderContext.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Scenes
{
    using System;
    using RayTracerChallenge.Library;

    public class RenderContext
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private int _progress;

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public RenderContext(Canvas canvas)
        {
            Canvas = canvas;
        }

        //// ===========================================================================================================
        //// Events
        //// ===========================================================================================================

        public event EventHandler? ProgressChanged;

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public Canvas Canvas { get; }

        public int Progress
        {
            get => _progress;
            set
            {
                if (_progress != value)
                {
                    _progress = value;

                    ProgressChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}
