﻿// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Scene.cs" company="Justin Rockwood">
//   Copyright (c) Justin Rockwood. All Rights Reserved. Licensed under the Apache License, Version 2.0. See
//   LICENSE.txt in the project root for license information.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace RayTracerChallenge.App.Scenes
{
    using System.ComponentModel;
    using RayTracerChallenge.Library;

    public abstract class Scene
    {
        //// ===========================================================================================================
        //// Member Variables
        //// ===========================================================================================================

        private Canvas? _canvas;
        private BackgroundWorker? _worker;
        private DoWorkEventArgs? _workEventArgs;
        private int _highestPercentageReached;

        //// ===========================================================================================================
        //// Constructors
        //// ===========================================================================================================

        protected Scene(string title, string description)
        {
            Title = title;
            Description = description;
        }

        //// ===========================================================================================================
        //// Properties
        //// ===========================================================================================================

        public string Title { get; }
        public string Description { get; }

        public abstract int RequestedWidth { get; }
        public abstract int RequestedHeight { get; }

        protected bool ShouldCancel => _worker?.CancellationPending ?? false;

        //// ===========================================================================================================
        //// Methods
        //// ===========================================================================================================

        public void Render(Canvas canvas, BackgroundWorker worker, DoWorkEventArgs e)
        {
            _canvas = canvas;
            _worker = worker;
            _workEventArgs = e;
            _highestPercentageReached = 0;

            RenderToCanvas(canvas);
            worker.ReportProgress(100, canvas);

            _worker = null;
            _workEventArgs = null;
        }

        protected abstract void RenderToCanvas(Canvas canvas);

        protected void ReportProgress(int percentComplete)
        {
            if (percentComplete <= _highestPercentageReached)
            {
                return;
            }

            _worker?.ReportProgress(percentComplete, _canvas);
            _highestPercentageReached = percentComplete;
        }
    }
}
