namespace TwitNetBuilder
{
    partial class MainForm
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
            this.CodePadBox = new System.Windows.Forms.TextBox();
            this.CodePadLabel = new System.Windows.Forms.Label();
            this.BuildButton = new System.Windows.Forms.Button();
            this.EncryptKeyBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CodePadBox
            // 
            this.CodePadBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CodePadBox.Location = new System.Drawing.Point(94, 7);
            this.CodePadBox.Name = "CodePadBox";
            this.CodePadBox.Size = new System.Drawing.Size(162, 20);
            this.CodePadBox.TabIndex = 0;
            this.CodePadBox.Text = "http://jesusspork_.codepad.org/";
            this.CodePadBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // CodePadLabel
            // 
            this.CodePadLabel.AutoSize = true;
            this.CodePadLabel.Location = new System.Drawing.Point(12, 9);
            this.CodePadLabel.Name = "CodePadLabel";
            this.CodePadLabel.Size = new System.Drawing.Size(79, 13);
            this.CodePadLabel.TabIndex = 1;
            this.CodePadLabel.Text = "CodePad URL:";
            // 
            // BuildButton
            // 
            this.BuildButton.Location = new System.Drawing.Point(94, 59);
            this.BuildButton.Name = "BuildButton";
            this.BuildButton.Size = new System.Drawing.Size(162, 23);
            this.BuildButton.TabIndex = 2;
            this.BuildButton.Text = "Build";
            this.BuildButton.UseVisualStyleBackColor = true;
            this.BuildButton.Click += new System.EventHandler(this.BuildButtonClick);
            // 
            // EncryptKeyBox
            // 
            this.EncryptKeyBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.EncryptKeyBox.Location = new System.Drawing.Point(94, 33);
            this.EncryptKeyBox.Name = "EncryptKeyBox";
            this.EncryptKeyBox.Size = new System.Drawing.Size(162, 20);
            this.EncryptKeyBox.TabIndex = 3;
            this.EncryptKeyBox.Text = "default007";
            this.EncryptKeyBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Encryption Key:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(268, 89);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.EncryptKeyBox);
            this.Controls.Add(this.BuildButton);
            this.Controls.Add(this.CodePadLabel);
            this.Controls.Add(this.CodePadBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "TwitNet Builder 0.01";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox CodePadBox;
        private System.Windows.Forms.Label CodePadLabel;
        private System.Windows.Forms.Button BuildButton;
        private System.Windows.Forms.TextBox EncryptKeyBox;
        private System.Windows.Forms.Label label1;
    }
}

