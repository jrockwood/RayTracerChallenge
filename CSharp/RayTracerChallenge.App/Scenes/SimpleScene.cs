// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleScene.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Scenes
{
    using RayTracerChallenge.Library;

    /// <summary>
    /// Abstract base class for a simple scene that doesn't use a <see cref="World"/> or <see cref="Camera"/> and should
    /// take less than 1-2 seconds to render. Inherit from <see cref="ComplexScene"/> for most scenes.
    /// </summary>
    public abstract class SimpleScene : Scene
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        protected SimpleScene(string title, string description, int canvasWidth, int canvasHeight)
            : base(title, description, canvasWidth, canvasHeight)
        {
        }
    }
}
