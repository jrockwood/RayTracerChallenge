// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Library.ViewModels
{
    using System;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using RayTracerChallenge.App.Library.Commands;
    using RayTracerChallenge.App.Library.Extensions;
    using RayTracerChallenge.App.Library.Scenes;

    public class MainWindowViewModel : Observable
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private readonly RenderSceneCommand _startCommand;
        private WriteableBitmap? _canvasBitmap;
        private double _renderDpiX;
        private double _renderDpiY;

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public MainWindowViewModel()
        {
            Scenes = new SceneList();
            Scenes.SelectedSceneChanged += OnSelectedSceneChanged;

            _startCommand = new RenderSceneCommand();
            _startCommand.RenderProgressChanged += OnRenderProgressChanged;

            RenderProcessViewModel = new LongRunningProcessProgressViewModel(_startCommand)
            {
                StartButtonText = "Render"
            };

            _renderDpiX = _renderDpiY = 96;

            // Set the selected index last so that all of the event handlers will run.
            Scenes.SelectedIndex = Scenes.Count > 0 ? Scenes.Count - 1 : -1;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public double RenderDpiX
        {
            get => _renderDpiX;
            set
            {
                if (SetProperty(ref _renderDpiX, value))
                {
                    ResetCanvasBitmap();
                }
            }
        }

        public double RenderDpiY
        {
            get => _renderDpiY;
            set
            {
                if (SetProperty(ref _renderDpiY, value))
                {
                    ResetCanvasBitmap();
                }
            }
        }

        public SceneList Scenes { get; }

        public LongRunningProcessProgressViewModel RenderProcessViewModel { get; }

        public WriteableBitmap? CanvasBitmap
        {
            get => _canvasBitmap;
            set => SetProperty(ref _canvasBitmap, value);
        }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        private void ResetCanvasBitmap()
        {
            RenderProcessViewModel.Reset();
            CanvasBitmap = null;
        }

        private void OnSelectedSceneChanged(object? sender, EventArgs e)
        {
            var scene = Scenes.SelectedScene;
            _startCommand.Scene = scene;
            ResetCanvasBitmap();
        }

        private void OnRenderProgressChanged(object? sender, SceneRenderProgress e)
        {
            if (CanvasBitmap == null)
            {
                int width = e.Canvas.Width;
                int height = e.Canvas.Height;

                CanvasBitmap = new WriteableBitmap(width, height, RenderDpiX, RenderDpiY, PixelFormats.Bgr24, null);
            }

            e.Canvas.RenderToWriteableBitmap(CanvasBitmap);
        }
    }
}
