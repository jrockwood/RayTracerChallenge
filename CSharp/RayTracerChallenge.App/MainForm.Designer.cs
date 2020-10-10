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
            System.Windows.Forms.Label _selectSceneLabel;
            this._pictureBox = new System.Windows.Forms.PictureBox();
            this._stretchImageRadioButton = new System.Windows.Forms.RadioButton();
            this._naturalSizeRadioButton = new System.Windows.Forms.RadioButton();
            this._renderButton = new System.Windows.Forms.Button();
            this._sceneDescriptionLabel = new System.Windows.Forms.Label();
            this._sceneComboBox = new System.Windows.Forms.ComboBox();
            _mainTitleLabel = new System.Windows.Forms.Label();
            _descriptionLabel = new System.Windows.Forms.Label();
            _sceneSelectionPanel = new System.Windows.Forms.Panel();
            _imagePanel = new System.Windows.Forms.Panel();
            _selectSceneLabel = new System.Windows.Forms.Label();
            _sceneSelectionPanel.SuspendLayout();
            _imagePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox)).BeginInit();
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
            _sceneSelectionPanel.Controls.Add(this._renderButton);
            _sceneSelectionPanel.Controls.Add(this._sceneDescriptionLabel);
            _sceneSelectionPanel.Controls.Add(this._sceneComboBox);
            _sceneSelectionPanel.Controls.Add(_selectSceneLabel);
            _sceneSelectionPanel.Location = new System.Drawing.Point(13, 124);
            _sceneSelectionPanel.Name = "_sceneSelectionPanel";
            _sceneSelectionPanel.Size = new System.Drawing.Size(982, 774);
            _sceneSelectionPanel.TabIndex = 2;
            // 
            // _imagePanel
            // 
            _imagePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            _imagePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            _imagePanel.Controls.Add(this._pictureBox);
            _imagePanel.Location = new System.Drawing.Point(0, 145);
            _imagePanel.Name = "_imagePanel";
            _imagePanel.Size = new System.Drawing.Size(981, 629);
            _imagePanel.TabIndex = 7;
            // 
            // _pictureBox
            // 
            this._pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pictureBox.Location = new System.Drawing.Point(0, 0);
            this._pictureBox.Name = "_pictureBox";
            this._pictureBox.Size = new System.Drawing.Size(977, 625);
            this._pictureBox.TabIndex = 3;
            this._pictureBox.TabStop = false;
            // 
            // _stretchImageRadioButton
            // 
            this._stretchImageRadioButton.AutoSize = true;
            this._stretchImageRadioButton.Location = new System.Drawing.Point(143, 114);
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
            this._naturalSizeRadioButton.Location = new System.Drawing.Point(0, 114);
            this._naturalSizeRadioButton.Name = "_naturalSizeRadioButton";
            this._naturalSizeRadioButton.Size = new System.Drawing.Size(137, 25);
            this._naturalSizeRadioButton.TabIndex = 5;
            this._naturalSizeRadioButton.TabStop = true;
            this._naturalSizeRadioButton.Text = "Use natural size";
            this._naturalSizeRadioButton.UseVisualStyleBackColor = true;
            this._naturalSizeRadioButton.CheckedChanged += new System.EventHandler(this.NaturalSizeRadioButton_CheckedChanged);
            // 
            // _renderButton
            // 
            this._renderButton.Location = new System.Drawing.Point(0, 59);
            this._renderButton.Name = "_renderButton";
            this._renderButton.Size = new System.Drawing.Size(109, 32);
            this._renderButton.TabIndex = 4;
            this._renderButton.Text = "&Render";
            this._renderButton.UseVisualStyleBackColor = true;
            this._renderButton.Click += new System.EventHandler(this.RenderButton_Click);
            // 
            // _sceneDescriptionLabel
            // 
            this._sceneDescriptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._sceneDescriptionLabel.AutoEllipsis = true;
            this._sceneDescriptionLabel.Location = new System.Drawing.Point(306, 24);
            this._sceneDescriptionLabel.Name = "_sceneDescriptionLabel";
            this._sceneDescriptionLabel.Size = new System.Drawing.Size(675, 172);
            this._sceneDescriptionLabel.TabIndex = 2;
            this._sceneDescriptionLabel.Text = "Scene description";
            // 
            // _sceneComboBox
            // 
            this._sceneComboBox.FormattingEnabled = true;
            this._sceneComboBox.Location = new System.Drawing.Point(0, 24);
            this._sceneComboBox.Name = "_sceneComboBox";
            this._sceneComboBox.Size = new System.Drawing.Size(300, 29);
            this._sceneComboBox.TabIndex = 1;
            this._sceneComboBox.SelectedIndexChanged += new System.EventHandler(this.SceneComboBox_SelectedIndexChanged);
            // 
            // _selectSceneLabel
            // 
            _selectSceneLabel.AutoSize = true;
            _selectSceneLabel.Location = new System.Drawing.Point(0, 0);
            _selectSceneLabel.Name = "_selectSceneLabel";
            _selectSceneLabel.Size = new System.Drawing.Size(109, 21);
            _selectSceneLabel.TabIndex = 0;
            _selectSceneLabel.Text = "&Select a scene:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1007, 910);
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
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox _pictureBox;
        private System.Windows.Forms.Label _sceneDescriptionLabel;
        private System.Windows.Forms.ComboBox _sceneComboBox;
        private System.Windows.Forms.Button _renderButton;
        private System.Windows.Forms.RadioButton _naturalSizeRadioButton;
        private System.Windows.Forms.RadioButton _stretchImageRadioButton;
    }
}

