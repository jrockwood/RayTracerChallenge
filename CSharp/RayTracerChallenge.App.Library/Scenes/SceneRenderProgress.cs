// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="SceneRenderProgress.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Library.Scenes
{
    using System.Windows.Media.Imaging;

    public sealed class SceneRenderProgress
    {
        public SceneRenderProgress(int percentComplete, BitmapSource renderedBitmap)
        {
            PercentComplete = percentComplete;
            RenderedBitmap = renderedBitmap;
        }

        public int PercentComplete { get; }
        public BitmapSource RenderedBitmap { get; }
    }
}
