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
        Image<Bgr, Byte> imagen2copia;

        Ycc YCrCb_min;
        Ycc YCrCb_max;

        FeatureVector observedImageVector;
        FeatureVector hypothesisImageVector;

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
                imagen2copia = new Image<Bgr, byte>(archivo);

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

            observedImageVector =  ObservedImageFunctions.ExtractFeatures(skin,imagen);
           hypothesisImageVector = HypothesisImageFunctions.createFirstHypothesis(imagen2);



            //imgCaja.Image = skin;

            imgCaja.Refresh();
            imgCaja2.Refresh();

        }


        private void btnExtract_Click(object sender, EventArgs e)
        {
            procesarImagen();
        }

        private void btnMAP_Click(object sender, EventArgs e)
        {

            while (MAPestimationOperations.definiteL > 35)
            {

                FeatureVector newVector = MAPestimationOperations.core(observedImageVector, hypothesisImageVector);
                hypothesisImageVector = newVector;

                // imagen2 = imagen2copia;
                //imgCaja2.Image = imagen2;
                // imgCaja2.Refresh();

                label1.Text = "L = " + MAPestimationOperations.definiteL.ToString();

                label1.Refresh();

                imagen2 = new Image<Bgr, byte>(archivo);
                imgCaja2.Image = imagen2;

                PointF puntoC = hypothesisImageVector.PalmCenter; ;
                Point punt = new Point(int.Parse(puntoC.X.ToString()), int.Parse(puntoC.Y.ToString()));
                CircleF centerCircle = new CircleF(puntoC, 5f);
                imagen2.Draw(centerCircle, new Bgr(Color.Brown), 3);

                Point p1 = new Point(int.Parse((hypothesisImageVector.PalmCenter.X - 90).ToString()), int.Parse((hypothesisImageVector.PalmCenter.Y - 90).ToString()));
                Point p2 = new Point(int.Parse((hypothesisImageVector.PalmCenter.X - 90).ToString()), int.Parse((hypothesisImageVector.PalmCenter.Y + 90).ToString()));
                Point p3 = new Point(int.Parse((hypothesisImageVector.PalmCenter.X + 90).ToString()), int.Parse((hypothesisImageVector.PalmCenter.Y - 90).ToString()));
                Point p4 = new Point(int.Parse((hypothesisImageVector.PalmCenter.X + 90).ToString()), int.Parse((hypothesisImageVector.PalmCenter.Y + 90).ToString()));

                LineSegment2D line = new LineSegment2D(p1,p2);
                LineSegment2D line1 = new LineSegment2D(p1,p3);
                LineSegment2D line2 = new LineSegment2D(p3, p4);
                LineSegment2D line3 = new LineSegment2D(p2, p4);

                imagen2.Draw(line, new Bgr(Color.Brown), 3);
                imagen2.Draw(line1, new Bgr(Color.Brown), 3);
                imagen2.Draw(line2, new Bgr(Color.Brown), 3);
                imagen2.Draw(line3, new Bgr(Color.Brown), 3);

                List<PointF> fingertips = new List<PointF>();

                fingertips.Add(hypothesisImageVector.LocationThumb);
                fingertips.Add(hypothesisImageVector.LocationIndexFinger);
                fingertips.Add(hypothesisImageVector.LocationHeartFinger);
                fingertips.Add(hypothesisImageVector.Location4Finger);
                fingertips.Add(hypothesisImageVector.LocationLittleFinger);

                foreach (PointF p in fingertips)
                {

                    CircleF circle = new CircleF(p, 5f);
                    

                    imagen2.Draw(circle, new Bgr(Color.Red), 3);


                    Point pun = new Point(int.Parse(p.X.ToString()), int.Parse(p.Y.ToString()));
                    LineSegment2D lineaDedoCentro = new LineSegment2D(pun, punt);
                    imagen2.Draw(lineaDedoCentro, new Bgr(Color.Green), 2);

                    imgCaja2.Refresh();
                }

                
            }


            MessageBox.Show("Done!");
        }
    }
}
