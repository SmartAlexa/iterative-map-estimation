using CCT.NUI.Core;
using CCT.NUI.HandTracking;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using HandGestureRecognition.SkinDetector;
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
        AdaptiveSkinDetector detector;

        ImageBox imgCaja;
        ImageBox imgCaja2;

        Image<Bgr, Byte> imagen;
        Image<Bgr, Byte> imagen2;

        Ycc YCrCb_min;
        Ycc YCrCb_max;


        IColorSkinDetector skinDetector;

        public Bitacora.Bitacora bitacora = new Bitacora.Bitacora();


        public frmPruebaMano()
        {
            InitializeComponent();

            detector = new AdaptiveSkinDetector(1, AdaptiveSkinDetector.MorphingMethod.NONE);

            YCrCb_min = new Ycc(0, 131, 80);
            YCrCb_max = new Ycc(255, 185, 135);

            imgCaja = new ImageBox();
            imgCaja.Height = 306;
            imgCaja.Width = 430;
            imgCaja.Location = new System.Drawing.Point(12, 42);
            imgCaja.SizeMode = PictureBoxSizeMode.StretchImage;

            imgCaja2 = new ImageBox();
            imgCaja2.Height = 306;
            imgCaja2.Width = 430;
            imgCaja2.Location = new System.Drawing.Point(478, 42);
            imgCaja2.SizeMode = PictureBoxSizeMode.StretchImage;

            imgCaja.BorderStyle = BorderStyle.FixedSingle;
            imgCaja2.BorderStyle = BorderStyle.FixedSingle;

            this.Controls.Add(imgCaja);
            this.Controls.Add(imgCaja2);

            imgCaja.Show();
            
        }
            
        

        private void btnCargar_Click(object sender, EventArgs e)
        {
            if (opnCargar.ShowDialog() == DialogResult.OK)
            {
                archivo = "";
                archivo = opnCargar.FileName;

                imagen = new Image<Bgr, byte>(archivo);
                imagen2 = new Image<Bgr, byte>(archivo);

                imgCaja.Image = imagen;
                imgCaja2.Image = imagen2;

                imgCaja.Refresh();
                imgCaja2.Refresh();
  
            }
        }


        private void procesarImagen()
        {


            //Image<Gray, Byte> skin = new Image<Gray, byte>(imagen.Width, imagen.Height);
           // detector.Process(imagen,skin);

            skinDetector = new YCrCbSkinDetector();

            Image<Gray, Byte> skin = skinDetector.DetectSkin(imagen, YCrCb_min, YCrCb_max);

            ExtractFeatures(skin);

            //imgCaja.Image = skin;

            imgCaja.Refresh();

        }



        private void btnExtract_Click(object sender, EventArgs e)
        {
            procesarImagen();
        }
    }
}
