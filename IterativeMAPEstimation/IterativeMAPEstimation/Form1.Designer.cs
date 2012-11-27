using System;
using System.Reflection;
namespace IterativeMAPEstimation
{
    partial class frmMain
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
            this.pctImage1 = new System.Windows.Forms.PictureBox();
            this.ofdCargar = new System.Windows.Forms.OpenFileDialog();
            this.btnCargarImagen = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pctImage1)).BeginInit();
            this.SuspendLayout();
            // 
            // pctImage1
            // 
            this.pctImage1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pctImage1.Location = new System.Drawing.Point(12, 12);
            this.pctImage1.Name = "pctImage1";
            this.pctImage1.Size = new System.Drawing.Size(264, 273);
            this.pctImage1.TabIndex = 0;
            this.pctImage1.TabStop = false;
            // 
            // ofdCargar
            // 
            this.ofdCargar.FileName = "openFileDialog1";
            // 
            // btnCargarImagen
            // 
            this.btnCargarImagen.Location = new System.Drawing.Point(12, 295);
            this.btnCargarImagen.Name = "btnCargarImagen";
            this.btnCargarImagen.Size = new System.Drawing.Size(75, 23);
            this.btnCargarImagen.TabIndex = 1;
            this.btnCargarImagen.Text = "Load";
            this.btnCargarImagen.UseVisualStyleBackColor = true;
            this.btnCargarImagen.Click += new System.EventHandler(this.btnCargarImagen_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 330);
            this.Controls.Add(this.btnCargarImagen);
            this.Controls.Add(this.pctImage1);
            this.Name = "frmMain";
            this.Text = "Palm detector prototype - 11.0.0.0";
            ((System.ComponentModel.ISupportInitialize)(this.pctImage1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pctImage1;
        private System.Windows.Forms.OpenFileDialog ofdCargar;
        private System.Windows.Forms.Button btnCargarImagen;
    }
}

