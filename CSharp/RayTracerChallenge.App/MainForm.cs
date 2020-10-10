// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="MainForm.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App
{
    using System;
    using System.Windows.Forms;
    using RayTracerChallenge.App.Scenes;
    using RayTracerChallenge.Library;

    public partial class MainForm : Form
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private readonly SceneList _sceneList = new SceneList();

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public MainForm()
        {
            InitializeComponent();
            InitializeScenes();
            SetPictureBoxSizeMode();
        }

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        private void InitializeScenes()
        {
            foreach (var scene in _sceneList.Scenes)
            {
                _sceneComboBox.Items.Add(scene);
            }

            _sceneComboBox.SelectedIndex = 0;
            _sceneComboBox.DisplayMember = "Title";
        }

        private void SetPictureBoxSizeMode()
        {
            _pictureBox.SizeMode =
                _naturalSizeRadioButton.Checked ? PictureBoxSizeMode.Normal : PictureBoxSizeMode.Zoom;
        }

        private void SceneComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _sceneDescriptionLabel.Text = (_sceneComboBox.SelectedItem as Scene)?.Description ?? string.Empty;
        }

        private void RenderButton_Click(object sender, EventArgs e)
        {
            if (!(_sceneComboBox.SelectedItem is Scene scene))
            {
                return;
            }

            var canvas = new Canvas(scene.RequestedWidth, scene.RequestedHeight);
            scene.Render(canvas);

            var bitmap = canvas.ToBitmap();
            _pictureBox.Image = bitmap;
        }

        private void NaturalSizeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            SetPictureBoxSizeMode();
        }

        private void StretchImageRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            SetPictureBoxSizeMode();
        }
    }
}
