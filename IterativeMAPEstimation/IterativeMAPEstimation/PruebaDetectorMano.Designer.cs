namespace IterativeMAPEstimation
{
    partial class frmPruebaMano
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
            this.btnCargar = new System.Windows.Forms.Button();
            this.opnCargar = new System.Windows.Forms.OpenFileDialog();
            this.btnExtract = new System.Windows.Forms.Button();
            this.btnMAP = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCargar
            // 
            this.btnCargar.Location = new System.Drawing.Point(13, 13);
            this.btnCargar.Name = "btnCargar";
            this.btnCargar.Size = new System.Drawing.Size(113, 23);
            this.btnCargar.TabIndex = 0;
            this.btnCargar.Text = "Cargar Imagen";
            this.btnCargar.UseVisualStyleBackColor = true;
            this.btnCargar.Click += new System.EventHandler(this.btnCargar_Click);
            // 
            // btnExtract
            // 
            this.btnExtract.Location = new System.Drawing.Point(13, 381);
            this.btnExtract.Name = "btnExtract";
            this.btnExtract.Size = new System.Drawing.Size(113, 23);
            this.btnExtract.TabIndex = 1;
            this.btnExtract.Text = "Extract Features";
            this.btnExtract.UseVisualStyleBackColor = true;
            this.btnExtract.Click += new System.EventHandler(this.btnExtract_Click);
            // 
            // btnMAP
            // 
            this.btnMAP.Location = new System.Drawing.Point(133, 381);
            this.btnMAP.Name = "btnMAP";
            this.btnMAP.Size = new System.Drawing.Size(75, 23);
            this.btnMAP.TabIndex = 2;
            this.btnMAP.Text = "MAP";
            this.btnMAP.UseVisualStyleBackColor = true;
            this.btnMAP.Click += new System.EventHandler(this.btnMAP_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(225, 390);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "L =";
            // 
            // frmPruebaMano
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 416);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnMAP);
            this.Controls.Add(this.btnExtract);
            this.Controls.Add(this.btnCargar);
            this.Name = "frmPruebaMano";
            this.Text = "PruebaDetectorMano";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCargar;
        private System.Windows.Forms.OpenFileDialog opnCargar;
        private System.Windows.Forms.Button btnExtract;
        private System.Windows.Forms.Button btnMAP;
        private System.Windows.Forms.Label label1;
    }
}