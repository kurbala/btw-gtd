﻿namespace Gtd.Client.Views.AddStuffToInbox
{
    partial class AddStuffToInboxForm
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this._cancel = new System.Windows.Forms.Button();
            this._ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(16, 15);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(503, 22);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // _cancel
            // 
            this._cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(420, 64);
            this._cancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(100, 28);
            this._cancel.TabIndex = 1;
            this._cancel.Text = "Cancel";
            this._cancel.UseVisualStyleBackColor = true;
            // 
            // _ok
            // 
            this._ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._ok.Enabled = false;
            this._ok.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._ok.Location = new System.Drawing.Point(312, 64);
            this._ok.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._ok.Name = "_ok";
            this._ok.Size = new System.Drawing.Size(100, 28);
            this._ok.TabIndex = 2;
            this._ok.Text = "Add Stuff";
            this._ok.UseVisualStyleBackColor = true;
            // 
            // AddStuffToInboxForm
            // 
            this.AcceptButton = this._ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancel;
            this.ClientSize = new System.Drawing.Size(536, 107);
            this.Controls.Add(this._ok);
            this.Controls.Add(this._cancel);
            this.Controls.Add(this.textBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "AddStuffToInboxForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Stuff to Inbox...";
            this.Load += new System.EventHandler(this.AddStuffToInboxForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button _cancel;
        private System.Windows.Forms.Button _ok;
    }
}