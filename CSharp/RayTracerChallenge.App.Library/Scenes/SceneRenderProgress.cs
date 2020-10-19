// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="SceneRenderProgress.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Library.Scenes
{
    using RayTracerChallenge.Library;

    public sealed class SceneRenderProgress
    {
        public SceneRenderProgress(int percentComplete, Canvas canvas)
        {
            PercentComplete = percentComplete;
            Canvas = canvas;
        }

        public int PercentComplete { get; }
        public Canvas Canvas { get; }
    }
}
