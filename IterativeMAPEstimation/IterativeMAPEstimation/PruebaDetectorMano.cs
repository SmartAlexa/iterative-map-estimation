using Emgu.CV;
using Emgu.CV.Structure;
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
        AdaptiveSkinDetector detector;
        Image<Bgr, Byte> imagen;

        public Bitacora.Bitacora bitacora = new Bitacora.Bitacora();

        Seq<Point> hull;
        Seq<Point> filteredHull;
        Seq<MCvConvexityDefect> defects;
        MCvConvexityDefect[] defectArray;
        Rectangle handRect;
        MCvBox2D box;
        Ellipse ellip;

        public frmPruebaMano()
        {
            InitializeComponent();

            detector = new AdaptiveSkinDetector(1, AdaptiveSkinDetector.MorphingMethod.NONE);
      

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
            imagen = new Image<Bgr, byte>(archivo); 
            Image<Gray, Byte> skin = new Image<Gray, byte>(imagen.Width, imagen.Height);
            detector.Process(imagen,skin);
            ExtractContourAndHull(skin);

        }

        private void ExtractContourAndHull(Image<Gray, byte> skin)
        {
            using (MemStorage storage = new MemStorage())
            {

                Contour<Point> contours = skin.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST, storage);
                Contour<Point> biggestContour = null;

                Double Result1 = 0;
                Double Result2 = 0;
                while (contours != null)
                {
                    Result1 = contours.Area;
                    if (Result1 > Result2)
                    {
                        Result2 = Result1;
                        biggestContour = contours;
                    }
                    contours = contours.HNext;
                }

                if (biggestContour != null)
                {
                    Contour<Point> currentContour = biggestContour.ApproxPoly(biggestContour.Perimeter * 0.0025, storage);
                    imagen.Draw(currentContour, new Bgr(Color.LimeGreen), 2);
                    biggestContour = currentContour;


                    hull = biggestContour.GetConvexHull(Emgu.CV.CvEnum.ORIENTATION.CV_CLOCKWISE);
                    box = biggestContour.GetMinAreaRect();
                    PointF[] points = box.GetVertices();


                    Point[] ps = new Point[points.Length];
                    for (int i = 0; i < points.Length; i++)
                        ps[i] = new Point((int)points[i].X, (int)points[i].Y);

                    imagen.DrawPolyline(hull.ToArray(), true, new Bgr(200, 125, 75), 2);
                    imagen.Draw(new CircleF(new PointF(box.center.X, box.center.Y), 3), new Bgr(200, 125, 75), 2);

                    PointF center;
                    float radius;
                   
                    filteredHull = new Seq<Point>(storage);
                    for (int i = 0; i < hull.Total; i++)
                    {
                        if (Math.Sqrt(Math.Pow(hull[i].X - hull[i + 1].X, 2) + Math.Pow(hull[i].Y - hull[i + 1].Y, 2)) > box.size.Width / 10)
                        {
                            filteredHull.Push(hull[i]);
                        }
                    }

                    defects = biggestContour.GetConvexityDefacts(storage, Emgu.CV.CvEnum.ORIENTATION.CV_CLOCKWISE);

                    defectArray = defects.ToArray();
                }
            }
        }

        private void DetectarCentroPalma()
        { 
        
            
        
        }
    }
}
