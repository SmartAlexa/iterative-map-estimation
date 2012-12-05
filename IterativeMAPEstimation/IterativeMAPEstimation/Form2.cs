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
    public partial class frmPruebaDatos : Form
    {
        public frmPruebaDatos(FeatureVector vector)
        {
            InitializeComponent();

            lblThumb.Text += " " + vector.LocationThumb.ToString();
            lblIndex.Text += " " + vector.LocationIndexFinger.ToString();
            lblHeart.Text += " " + vector.LocationHeartFinger.ToString();
            lblFourth.Text += " " + vector.Location4Finger.ToString();
            lblLittle.Text += " " + vector.LocationLittleFinger.ToString();

            lblThumbAngle.Text += " " + vector.thumbAngle.ToString();
            lblIndexAngle.Text += " " + vector.indexAngle.ToString();
            lblHeartAngle.Text += " " + vector.heartAngle.ToString();
            lblFourthAngle.Text += " " + vector.fourthAngle.ToString();
            lblLittleAngle.Text += " " + vector.littleAngle.ToString();

            lblPalmCenter.Text += " " + vector.PalmCenter.ToString();
            lblPalmSize.Text += " " + vector.palmSize.ToString();
        }
    }
}
