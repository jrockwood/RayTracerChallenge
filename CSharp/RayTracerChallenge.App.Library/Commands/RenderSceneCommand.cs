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

        public const double DefaultDpi = 96.0;

        private Scene? _scene;
        private BitmapSource? _renderedBitmap;
        private double _dpiX;
        private double _dpiY;

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public RenderSceneCommand(double dpiX = DefaultDpi, double dpiY = DefaultDpi, Scene? scene = null)
        {
            _dpiX = dpiX;
            _dpiY = dpiY;
            _scene = scene;
            IsEnabled = scene != null;
        }

        //// ===========================================================================================================
        //// Events
        //// ===========================================================================================================

        public event EventHandler<SceneRenderProgress>? RenderProgressChanged;

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public bool IsRendering { get; private set; }

        public double DpiX
        {
            get => _dpiX;
            set
            {
                ValidateNotRendering();
                SetProperty(ref _dpiX, value);
            }
        }

        public double DpiY
        {
            get => _dpiY;
            set
            {
                ValidateNotRendering();
                SetProperty(ref _dpiY, value);
            }
        }

        public Scene? Scene
        {
            get => _scene;
            set
            {
                ValidateNotRendering();

                if (SetProperty(ref _scene, value))
                {
                    IsEnabled = value != null;
                    RenderedBitmap = null;
                }
            }
        }

        public BitmapSource? RenderedBitmap
        {
            get => _renderedBitmap;
            private set => SetProperty(ref _renderedBitmap, value);
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
            RenderedBitmap = null;
            try
            {
                var renderProgress = new Progress<SceneRenderProgress>(p =>
                {
                    RenderedBitmap = p.RenderedBitmap;
                    progress?.Report(p.PercentComplete);
                    RenderProgressChanged?.Invoke(this, p);
                });

                await Scene.RenderAsync(_dpiX, _dpiY, renderProgress, cancellationToken);
                RenderProgressChanged?.Invoke(
                    this,
                    new SceneRenderProgress(100, RenderedBitmap ?? throw new InvalidOperationException()));
            }
            finally
            {
                IsRendering = false;
            }
        }

        private void ValidateNotRendering()
        {
            if (IsRendering)
            {
                throw new InvalidOperationException("Cannot set DPI while rendering.");
            }
        }
    }
}
