using Emgu.CV;
using Emgu.CV.Structure;
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
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnCargarImagen_Click(object sender, EventArgs e)
        {
            if (ofdCargar.ShowDialog() == DialogResult.OK)
            {


                try
                {
                    Image<Bgr, Byte> image = new Image<Bgr, byte>(ofdCargar.FileName);
                    pctImage1.Image = image.ToBitmap();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);

                }
            }
        }
    }
}
