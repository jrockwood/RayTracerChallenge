// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderSceneCommand.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Library.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Media.Imaging;
    using RayTracerChallenge.App.Library.Scenes;
    using RayTracerChallenge.Library;

    /// <summary>
    /// Renders a <see cref="Scene"/> to a <see cref="Canvas"/> and associated <see cref="BitmapSource"/>.
    /// </summary>
    public class RenderSceneCommand : BaseAsyncCommand
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private Scene? _scene;
        private Canvas? _renderedCanvas;

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public RenderSceneCommand(Scene? scene = null)
        {
            Scene = scene;
        }

        //// ===========================================================================================================
        //// Events
        //// ===========================================================================================================

        public event EventHandler<SceneRenderProgress>? RenderProgressChanged;

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public bool IsRendering { get; private set; }

        public Scene? Scene
        {
            get => _scene;
            set
            {
                SetProperty(ref _scene, value);
                IsEnabled = value != null;
            }
        }

        public Canvas? RenderedCanvas
        {
            get => _renderedCanvas;
            private set => SetProperty(ref _renderedCanvas, value);
        }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        protected override async Task ExecuteAsyncImpl(
            IProgress<int>? progress = null,
            CancellationToken cancellationToken = default)
        {
            if (Scene == null)
            {
                throw new InvalidOperationException("Scene cannot be null");
            }

            IsRendering = true;
            RenderedCanvas = null;
            try
            {
                var renderProgress = new Progress<SceneRenderProgress>(p =>
                {
                    RenderedCanvas = p.Canvas;
                    progress?.Report(p.PercentComplete);
                    RenderProgressChanged?.Invoke(this, p);
                });

                RenderedCanvas = await Scene.RenderAsync(renderProgress, cancellationToken);
                RenderProgressChanged?.Invoke(this, new SceneRenderProgress(100, RenderedCanvas));
            }
            finally
            {
                IsRendering = false;
            }
        }
    }
}
