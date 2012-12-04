using Emgu.CV.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IterativeMAPEstimation
{
    public partial class frmPruebaMano : Form
    {

        private string archivo;

        public frmPruebaMano()
        {
            InitializeComponent();

            ImageBox imgCaja = new ImageBox();
            imgCaja.Location = new System.Drawing.Point(50, 50);
            
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            if (opnCargar.ShowDialog() == DialogResult.OK)
            {

                archivo = opnCargar.FileName;
                procesarImagen();
  
            }
        }


        private void procesarImagen()
        { 
        
        }
    }
}
