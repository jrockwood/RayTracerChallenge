namespace RayTracerChallenge.App
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label _mainTitleLabel;
            System.Windows.Forms.Label _descriptionLabel;
            System.Windows.Forms.Panel _sceneSelectionPanel;
            System.Windows.Forms.Panel _imagePanel;
            System.Windows.Forms.GroupBox _selectSceneGroupBox;
            System.Windows.Forms.Panel _renderPanel;
            this._pictureBox = new System.Windows.Forms.PictureBox();
            this._stretchImageRadioButton = new System.Windows.Forms.RadioButton();
            this._naturalSizeRadioButton = new System.Windows.Forms.RadioButton();
            this._sceneDescriptionLabel = new System.Windows.Forms.Label();
            this._sceneComboBox = new System.Windows.Forms.ComboBox();
            this._progressPercentLabel = new System.Windows.Forms.Label();
            this._cancelButton = new System.Windows.Forms.Button();
            this._progressBar = new System.Windows.Forms.ProgressBar();
            this._renderButton = new System.Windows.Forms.Button();
            _mainTitleLabel = new System.Windows.Forms.Label();
            _descriptionLabel = new System.Windows.Forms.Label();
            _sceneSelectionPanel = new System.Windows.Forms.Panel();
            _imagePanel = new System.Windows.Forms.Panel();
            _selectSceneGroupBox = new System.Windows.Forms.GroupBox();
            _renderPanel = new System.Windows.Forms.Panel();
            _sceneSelectionPanel.SuspendLayout();
            _imagePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox)).BeginInit();
            _selectSceneGroupBox.SuspendLayout();
            _renderPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _mainTitleLabel
            // 
            _mainTitleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            _mainTitleLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            _mainTitleLabel.Location = new System.Drawing.Point(13, 13);
            _mainTitleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            _mainTitleLabel.Name = "_mainTitleLabel";
            _mainTitleLabel.Size = new System.Drawing.Size(981, 46);
            _mainTitleLabel.TabIndex = 0;
            _mainTitleLabel.Text = "Ray Tracer Challenge";
            _mainTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            _mainTitleLabel.UseMnemonic = false;
            // 
            // _descriptionLabel
            // 
            _descriptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            _descriptionLabel.Location = new System.Drawing.Point(13, 59);
            _descriptionLabel.Name = "_descriptionLabel";
            _descriptionLabel.Size = new System.Drawing.Size(982, 62);
            _descriptionLabel.TabIndex = 1;
            _descriptionLabel.Text = "Displays scenes from the ray tracer built using the book, \"Ray Tracer Challenge\" " +
    "by Jamis Buck.\r\n";
            _descriptionLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // _sceneSelectionPanel
            // 
            _sceneSelectionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            _sceneSelectionPanel.Controls.Add(_imagePanel);
            _sceneSelectionPanel.Controls.Add(this._stretchImageRadioButton);
            _sceneSelectionPanel.Controls.Add(this._naturalSizeRadioButton);
            _sceneSelectionPanel.Location = new System.Drawing.Point(13, 297);
            _sceneSelectionPanel.Name = "_sceneSelectionPanel";
            _sceneSelectionPanel.Size = new System.Drawing.Size(982, 601);
            _sceneSelectionPanel.TabIndex = 2;
            // 
            // _imagePanel
            // 
            _imagePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            _imagePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            _imagePanel.Controls.Add(this._pictureBox);
            _imagePanel.Location = new System.Drawing.Point(0, 31);
            _imagePanel.Name = "_imagePanel";
            _imagePanel.Size = new System.Drawing.Size(981, 570);
            _imagePanel.TabIndex = 7;
            // 
            // _pictureBox
            // 
            this._pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pictureBox.Location = new System.Drawing.Point(0, 0);
            this._pictureBox.Name = "_pictureBox";
            this._pictureBox.Size = new System.Drawing.Size(977, 566);
            this._pictureBox.TabIndex = 3;
            this._pictureBox.TabStop = false;
            // 
            // _stretchImageRadioButton
            // 
            this._stretchImageRadioButton.AutoSize = true;
            this._stretchImageRadioButton.Location = new System.Drawing.Point(145, 0);
            this._stretchImageRadioButton.Name = "_stretchImageRadioButton";
            this._stretchImageRadioButton.Size = new System.Drawing.Size(123, 25);
            this._stretchImageRadioButton.TabIndex = 6;
            this._stretchImageRadioButton.TabStop = true;
            this._stretchImageRadioButton.Text = "Stretch image";
            this._stretchImageRadioButton.UseVisualStyleBackColor = true;
            this._stretchImageRadioButton.CheckedChanged += new System.EventHandler(this.StretchImageRadioButton_CheckedChanged);
            // 
            // _naturalSizeRadioButton
            // 
            this._naturalSizeRadioButton.AutoSize = true;
            this._naturalSizeRadioButton.Checked = true;
            this._naturalSizeRadioButton.Location = new System.Drawing.Point(2, 0);
            this._naturalSizeRadioButton.Name = "_naturalSizeRadioButton";
            this._naturalSizeRadioButton.Size = new System.Drawing.Size(137, 25);
            this._naturalSizeRadioButton.TabIndex = 5;
            this._naturalSizeRadioButton.TabStop = true;
            this._naturalSizeRadioButton.Text = "Use natural size";
            this._naturalSizeRadioButton.UseVisualStyleBackColor = true;
            this._naturalSizeRadioButton.CheckedChanged += new System.EventHandler(this.NaturalSizeRadioButton_CheckedChanged);
            // 
            // _selectSceneGroupBox
            // 
            _selectSceneGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            _selectSceneGroupBox.Controls.Add(this._sceneDescriptionLabel);
            _selectSceneGroupBox.Controls.Add(this._sceneComboBox);
            _selectSceneGroupBox.Location = new System.Drawing.Point(15, 127);
            _selectSceneGroupBox.Margin = new System.Windows.Forms.Padding(6);
            _selectSceneGroupBox.Name = "_selectSceneGroupBox";
            _selectSceneGroupBox.Padding = new System.Windows.Forms.Padding(6);
            _selectSceneGroupBox.Size = new System.Drawing.Size(980, 112);
            _selectSceneGroupBox.TabIndex = 3;
            _selectSceneGroupBox.TabStop = false;
            _selectSceneGroupBox.Text = "Select a scene";
            // 
            // _sceneDescriptionLabel
            // 
            this._sceneDescriptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._sceneDescriptionLabel.AutoEllipsis = true;
            this._sceneDescriptionLabel.Location = new System.Drawing.Point(318, 31);
            this._sceneDescriptionLabel.Name = "_sceneDescriptionLabel";
            this._sceneDescriptionLabel.Size = new System.Drawing.Size(653, 75);
            this._sceneDescriptionLabel.TabIndex = 2;
            this._sceneDescriptionLabel.Text = "Scene description";
            // 
            // _sceneComboBox
            // 
            this._sceneComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._sceneComboBox.FormattingEnabled = true;
            this._sceneComboBox.Location = new System.Drawing.Point(9, 31);
            this._sceneComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
            this._sceneComboBox.Name = "_sceneComboBox";
            this._sceneComboBox.Size = new System.Drawing.Size(300, 29);
            this._sceneComboBox.TabIndex = 1;
            this._sceneComboBox.SelectedIndexChanged += new System.EventHandler(this.SceneComboBox_SelectedIndexChanged);
            // 
            // _renderPanel
            // 
            _renderPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            _renderPanel.Controls.Add(this._progressPercentLabel);
            _renderPanel.Controls.Add(this._cancelButton);
            _renderPanel.Controls.Add(this._progressBar);
            _renderPanel.Controls.Add(this._renderButton);
            _renderPanel.Location = new System.Drawing.Point(15, 248);
            _renderPanel.Name = "_renderPanel";
            _renderPanel.Size = new System.Drawing.Size(980, 43);
            _renderPanel.TabIndex = 4;
            // 
            // _progressPercentLabel
            // 
            this._progressPercentLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._progressPercentLabel.AutoSize = true;
            this._progressPercentLabel.Location = new System.Drawing.Point(814, 6);
            this._progressPercentLabel.Name = "_progressPercentLabel";
            this._progressPercentLabel.Size = new System.Drawing.Size(50, 21);
            this._progressPercentLabel.TabIndex = 9;
            this._progressPercentLabel.Text = "100%";
            this._progressPercentLabel.Visible = false;
            // 
            // _cancelButton
            // 
            this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._cancelButton.Location = new System.Drawing.Point(872, 0);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(109, 32);
            this._cancelButton.TabIndex = 4;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            this._cancelButton.Visible = false;
            this._cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // _progressBar
            // 
            this._progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._progressBar.Location = new System.Drawing.Point(115, 0);
            this._progressBar.Name = "_progressBar";
            this._progressBar.Size = new System.Drawing.Size(693, 32);
            this._progressBar.TabIndex = 8;
            this._progressBar.Value = 50;
            this._progressBar.Visible = false;
            // 
            // _renderButton
            // 
            this._renderButton.Location = new System.Drawing.Point(0, 0);
            this._renderButton.Name = "_renderButton";
            this._renderButton.Size = new System.Drawing.Size(109, 32);
            this._renderButton.TabIndex = 4;
            this._renderButton.Text = "&Render";
            this._renderButton.UseVisualStyleBackColor = true;
            this._renderButton.Click += new System.EventHandler(this.RenderButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1007, 910);
            this.Controls.Add(_renderPanel);
            this.Controls.Add(_selectSceneGroupBox);
            this.Controls.Add(_sceneSelectionPanel);
            this.Controls.Add(_descriptionLabel);
            this.Controls.Add(_mainTitleLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(200, 200);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ray Tracer Challenge";
            _sceneSelectionPanel.ResumeLayout(false);
            _sceneSelectionPanel.PerformLayout();
            _imagePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox)).EndInit();
            _selectSceneGroupBox.ResumeLayout(false);
            _renderPanel.ResumeLayout(false);
            _renderPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox _pictureBox;
        private System.Windows.Forms.RadioButton _naturalSizeRadioButton;
        private System.Windows.Forms.RadioButton _stretchImageRadioButton;
        private System.Windows.Forms.ComboBox _sceneComboBox;
        private System.Windows.Forms.Label _sceneDescriptionLabel;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.ProgressBar _progressBar;
        private System.Windows.Forms.Button _renderButton;
        private System.Windows.Forms.Label _progressPercentLabel;
    }
}

