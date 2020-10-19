// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Wpf
{
    using System.Windows;
    using RayTracerChallenge.App.Library.ViewModels;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;
            DataContext = viewModel;
            RenderProgressControl.ViewModel = viewModel.RenderProcessViewModel;

            RenderImage.DpiChanged += OnRenderImageDpiChanged;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public MainWindowViewModel ViewModel { get; }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        private void OnRenderImageDpiChanged(object sender, DpiChangedEventArgs e)
        {
            ViewModel.RenderDpiX = e.NewDpi.PixelsPerInchX;
            ViewModel.RenderDpiY = e.NewDpi.PixelsPerInchY;
        }
    }
}
