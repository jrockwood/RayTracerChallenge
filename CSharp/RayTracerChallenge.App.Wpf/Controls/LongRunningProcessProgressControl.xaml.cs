// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="LongRunningProcessProgressControl.xaml.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Wpf.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using RayTracerChallenge.App.Library.ViewModels;

    public partial class LongRunningProcessProgressControl : UserControl
    {
        public LongRunningProcessProgressControl()
        {
            InitializeComponent();
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel",
            typeof(LongRunningProcessProgressViewModel),
            typeof(LongRunningProcessProgressControl),
            new PropertyMetadata(default(LongRunningProcessProgressViewModel?), OnViewModelPropertyChanged));

        private static void OnViewModelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (LongRunningProcessProgressControl)d;
            control.DataContext = e.NewValue;
        }

        public LongRunningProcessProgressViewModel? ViewModel
        {
            get => (LongRunningProcessProgressViewModel?)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }
    }
}
