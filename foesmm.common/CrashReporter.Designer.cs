namespace foesmm.common
{
    partial class CrashReporter
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.crashdumpContents = new System.Windows.Forms.TextBox();
            this.detailsButton = new System.Windows.Forms.LinkLabel();
            this.closeButton = new System.Windows.Forms.Button();
            this.crashdumpButton = new System.Windows.Forms.LinkLabel();
            this.errorImage = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.errorReason = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorImage)).BeginInit();
            this.SuspendLayout();
            // 
            // crashdumpContents
            // 
            this.crashdumpContents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.crashdumpContents.Location = new System.Drawing.Point(13, 214);
            this.crashdumpContents.MinimumSize = new System.Drawing.Size(799, 343);
            this.crashdumpContents.Multiline = true;
            this.crashdumpContents.Name = "crashdumpContents";
            this.crashdumpContents.ReadOnly = true;
            this.crashdumpContents.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.crashdumpContents.Size = new System.Drawing.Size(799, 343);
            this.crashdumpContents.TabIndex = 0;
            this.crashdumpContents.Visible = false;
            // 
            // detailsButton
            // 
            this.detailsButton.AutoSize = true;
            this.detailsButton.Location = new System.Drawing.Point(12, 161);
            this.detailsButton.Name = "detailsButton";
            this.detailsButton.Size = new System.Drawing.Size(137, 25);
            this.detailsButton.TabIndex = 1;
            this.detailsButton.TabStop = true;
            this.detailsButton.Text = "Show Details";
            this.detailsButton.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.detailsButton_LinkClicked);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(684, 151);
            this.closeButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 16);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(128, 44);
            this.closeButton.TabIndex = 2;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // crashdumpButton
            // 
            this.crashdumpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.crashdumpButton.AutoSize = true;
            this.crashdumpButton.Location = new System.Drawing.Point(484, 161);
            this.crashdumpButton.Margin = new System.Windows.Forms.Padding(3, 0, 16, 0);
            this.crashdumpButton.Name = "crashdumpButton";
            this.crashdumpButton.Size = new System.Drawing.Size(181, 25);
            this.crashdumpButton.TabIndex = 3;
            this.crashdumpButton.TabStop = true;
            this.crashdumpButton.Text = "Show Crashdump";
            this.crashdumpButton.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.crashdumpButton_LinkClicked);
            // 
            // errorImage
            // 
            this.errorImage.Location = new System.Drawing.Point(25, 25);
            this.errorImage.Margin = new System.Windows.Forms.Padding(16);
            this.errorImage.Name = "errorImage";
            this.errorImage.Size = new System.Drawing.Size(64, 64);
            this.errorImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.errorImage.TabIndex = 4;
            this.errorImage.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(113, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(226, 25);
            this.label1.TabIndex = 5;
            this.label1.Text = "Fatal Error Occured:";
            // 
            // errorReason
            // 
            this.errorReason.Location = new System.Drawing.Point(113, 66);
            this.errorReason.Margin = new System.Windows.Forms.Padding(3, 16, 16, 16);
            this.errorReason.Name = "errorReason";
            this.errorReason.Size = new System.Drawing.Size(686, 66);
            this.errorReason.TabIndex = 6;
            this.errorReason.Text = "label2";
            // 
            // CrashReporter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(824, 569);
            this.Controls.Add(this.errorReason);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.errorImage);
            this.Controls.Add(this.crashdumpButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.detailsButton);
            this.Controls.Add(this.crashdumpContents);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CrashReporter";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Something bad happened";
            ((System.ComponentModel.ISupportInitialize)(this.errorImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox crashdumpContents;
        private System.Windows.Forms.LinkLabel detailsButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.LinkLabel crashdumpButton;
        private System.Windows.Forms.PictureBox errorImage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label errorReason;
    }
}