// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Wpf
{
    using System.Windows;
    using RayTracerChallenge.App.Library.ViewModels;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            MainWindow = new MainWindow(new MainWindowViewModel());
            MainWindow.Show();
        }
    }
}
