// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="MainForm.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;
    using RayTracerChallenge.App.Scenes;
    using RayTracerChallenge.Library;

    public partial class MainForm : Form
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private readonly SceneList _sceneList = new SceneList();
        private readonly BackgroundWorker _backgroundWorker = new BackgroundWorker();

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        public MainForm()
        {
            InitializeComponent();
            InitializeScenes();
            InitializeBackgroundWorker();
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

        private void InitializeBackgroundWorker()
        {
            _backgroundWorker.WorkerReportsProgress = true;
            _backgroundWorker.WorkerSupportsCancellation = true;
            _backgroundWorker.DoWork += BackgroundWorkerOnDoWork;
            _backgroundWorker.ProgressChanged += BackgroundWorkerOnProgressChanged;
            _backgroundWorker.RunWorkerCompleted += BackgroundWorkerOnRunWorkerCompleted;
        }

        private void SetPictureBoxSizeMode()
        {
            _pictureBox.SizeMode =
                _naturalSizeRadioButton.Checked ? PictureBoxSizeMode.Normal : PictureBoxSizeMode.Zoom;
        }

        private void ReportPercentComplete(int progress, Canvas canvas)
        {
            _progressBar.Value = progress;
            _progressPercentLabel.Text = progress + @"%";

            var bitmap = canvas.ToBitmap();
            _pictureBox.Image = bitmap;
            Application.DoEvents();
        }

        //// ===========================================================================================================
        //// Event Handlers
        //// ===========================================================================================================

        private void BackgroundWorkerOnDoWork(object sender, DoWorkEventArgs e)
        {
            var context = e.Argument as BackgroundWorkerContext ?? throw new InvalidOperationException();

            var canvas = context.Canvas;
            context.Scene.Render(canvas, _backgroundWorker, e);
            e.Result = canvas;
        }

        private void BackgroundWorkerOnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var canvas = e.UserState as Canvas ?? throw new InvalidOperationException();
            ReportPercentComplete(e.ProgressPercentage, canvas);
        }

        private void BackgroundWorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var canvas = e.Result as Canvas ?? throw new InvalidOperationException();
            ReportPercentComplete(100, canvas);

            _renderButton.Enabled = true;
            _cancelButton.Visible = false;
            _progressBar.Visible = false;
            _progressPercentLabel.Visible = false;
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

            _renderButton.Enabled = false;

            var canvas = new Canvas(scene.RequestedWidth, scene.RequestedHeight);
            ReportPercentComplete(0, canvas);

            _progressPercentLabel.Visible = true;
            _progressBar.Visible = true;
            _cancelButton.Visible = true;
            _cancelButton.Enabled = true;

            _backgroundWorker.RunWorkerAsync(new BackgroundWorkerContext(scene, canvas));
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            _backgroundWorker.CancelAsync();
            _cancelButton.Enabled = false;
        }

        private void NaturalSizeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            SetPictureBoxSizeMode();
        }

        private void StretchImageRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            SetPictureBoxSizeMode();
        }

        //// ===========================================================================================================
        //// Classes
        //// ===========================================================================================================

        private sealed class BackgroundWorkerContext
        {
            public BackgroundWorkerContext(Scene scene, Canvas canvas)
            {
                Scene = scene;
                Canvas = canvas;
            }

            public Scene Scene { get; }
            public Canvas Canvas { get; }
        }
    }
}
