// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Scene.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Scenes
{
    using RayTracerChallenge.Library;

    public abstract class Scene
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        protected Scene(string title, string description)
        {
            Title = title;
            Description = description;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public string Title { get; }
        public string Description { get; }

        public abstract int RequestedWidth { get; }
        public abstract int RequestedHeight { get; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public abstract void Render(Canvas canvas);
    }
}
