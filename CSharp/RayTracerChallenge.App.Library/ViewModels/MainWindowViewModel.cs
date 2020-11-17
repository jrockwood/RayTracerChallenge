// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Library.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Windows.Media.Imaging;
    using RayTracerChallenge.App.Library.Commands;
    using RayTracerChallenge.App.Library.Scenes;

    public class MainWindowViewModel : Observable
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private readonly RenderSceneCommand _startCommand;

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public MainWindowViewModel()
        {
            Scenes = new SceneList();
            Scenes.SelectedSceneChanged += OnSelectedSceneChanged;

            _startCommand = new RenderSceneCommand();
            _startCommand.RenderProgressChanged += OnStartCommandRenderProgressChanged;
            _startCommand.PropertyChanged += OnStartCommandPropertyChanged;

            RenderProcessViewModel = new LongRunningProcessProgressViewModel(_startCommand)
            {
                StartButtonText = "Render"
            };

            // Set the selected index last so that all of the event handlers will run.
            Scenes.SelectedIndex = Scenes.Count > 0 ? Scenes.Count - 1 : -1;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public double RenderDpiX
        {
            get => _startCommand.DpiX;
            set => _startCommand.DpiX = value;
        }

        public double RenderDpiY
        {
            get => _startCommand.DpiY;
            set => _startCommand.DpiY = value;
        }

        public bool UseSingleThreadForRender
        {
            get => _startCommand.UseSingleThreadForRender;
            set => _startCommand.UseSingleThreadForRender = value;
        }

        public bool IsRendering => _startCommand.IsRendering;

        public SceneList Scenes { get; }

        public LongRunningProcessProgressViewModel RenderProcessViewModel { get; }

        public BitmapSource? RenderedBitmap => _startCommand.RenderedBitmap;

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        private void OnSelectedSceneChanged(object? sender, EventArgs e)
        {
            var scene = Scenes.SelectedScene;
            _startCommand.Scene = scene;
            RenderProcessViewModel.Reset();
        }

        private void OnStartCommandRenderProgressChanged(object? sender, SceneRenderProgress e)
        {
            OnPropertyChanged(nameof(RenderedBitmap));
        }

        private void OnStartCommandPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(RenderSceneCommand.DpiX):
                    OnPropertyChanged(nameof(RenderDpiX));
                    break;

                case nameof(RenderSceneCommand.DpiY):
                    OnPropertyChanged(nameof(RenderDpiY));
                    break;

                case nameof(RenderSceneCommand.RenderedBitmap):
                    OnPropertyChanged(nameof(RenderedBitmap));
                    break;

                case nameof(RenderSceneCommand.UseSingleThreadForRender):
                    OnPropertyChanged(nameof(UseSingleThreadForRender));
                    break;

                case nameof(RenderSceneCommand.IsRendering):
                    OnPropertyChanged(nameof(IsRendering));
                    break;
            }
        }
    }
}
