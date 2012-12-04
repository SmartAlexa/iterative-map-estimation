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
        Image<Bgr, Byte> imagen;
        ImageBox imgCaja;

        Ycc YCrCb_min;
        Ycc YCrCb_max;

        public Bitacora.Bitacora bitacora = new Bitacora.Bitacora();
        IColorSkinDetector skinDetector;

        Seq<Point> hull;
        Seq<Point> filteredHull;
        Seq<MCvConvexityDefect> defects;
        MCvConvexityDefect[] defectArray;
        Rectangle handRect;
        MCvBox2D box;
        Ellipse ellip;

        private float contourReduction;
        private int searchRadius;

        private Palm result;

        public frmPruebaMano()
        {
            InitializeComponent();

            detector = new AdaptiveSkinDetector(1, AdaptiveSkinDetector.MorphingMethod.NONE);

            YCrCb_min = new Ycc(0, 131, 80);
            YCrCb_max = new Ycc(255, 185, 135);

            imgCaja = new ImageBox();
            imgCaja.Location = new System.Drawing.Point(20, 50);
            this.Controls.Add(imgCaja);
            imgCaja.Show();
            
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

            imgCaja.Height = imagen.Height;
            imgCaja.Width = imagen.Width;

            this.Height = imgCaja.Height + 100;
            this.Width = imgCaja.Width + 50;

            imgCaja.Image = imagen;

            //Image<Gray, Byte> skin = new Image<Gray, byte>(imagen.Width, imagen.Height);
           // detector.Process(imagen,skin);

            skinDetector = new YCrCbSkinDetector();

            Image<Gray, Byte> skin = skinDetector.DetectSkin(imagen, YCrCb_min, YCrCb_max);

            ExtractFeatures(skin);

            //imgCaja.Image = skin;

            

        }

        private void ExtractFeatures(Image<Gray, byte> skin)
        {

            Contour<Point> currentContour = null;
            Contour<Point> biggestContour = null;

            using (MemStorage storage = new MemStorage())
            {

            #region extractContourAndHull
                Contour<Point> contours = skin.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST, storage);


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
                    currentContour = biggestContour.ApproxPoly(biggestContour.Perimeter * 0.0025, storage);
                    imagen.Draw(currentContour, new Bgr(Color.LimeGreen), 2);
                    biggestContour = currentContour;


                    hull = biggestContour.GetConvexHull(Emgu.CV.CvEnum.ORIENTATION.CV_CLOCKWISE);
                    box = biggestContour.GetMinAreaRect();
                    PointF[] points = box.GetVertices();


                    Point[] ps = new Point[points.Length];
                    for (int i = 0; i < points.Length; i++)
                        ps[i] = new Point((int)points[i].X, (int)points[i].Y);

                    imagen.DrawPolyline(hull.ToArray(), true, new Bgr(200, 125, 75), 2);
                   // imagen.Draw(new CircleF(new PointF(box.center.X, box.center.Y), 3), new Bgr(200, 125, 75), 2);

                  //  PointF center;
                   // float radius;
                   
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

            #endregion



            #region find palm center

            this.searchRadius = 6;
            this.contourReduction = 3;

            //this.result = null;

            DetectarCentroPalma(biggestContour.ToList<Point>(), obtenerListaCandidatos(box));


            CircleF centerCircle = new CircleF(result.Location, 5f);
            imagen.Draw(centerCircle, new Bgr(Color.Brown), 56);


            #endregion

            #region defects drawing

            int fingerNum = 0;

            for (int i = 0; i < defects.Total; i++)
            {
                PointF startPoint = new PointF((float)defectArray[i].StartPoint.X,
                                                (float)defectArray[i].StartPoint.Y);

                PointF depthPoint = new PointF((float)defectArray[i].DepthPoint.X,
                                                (float)defectArray[i].DepthPoint.Y);

                PointF endPoint = new PointF((float)defectArray[i].EndPoint.X,
                                                (float)defectArray[i].EndPoint.Y);

                LineSegment2D startDepthLine = new LineSegment2D(defectArray[i].StartPoint, defectArray[i].DepthPoint);

                LineSegment2D depthEndLine = new LineSegment2D(defectArray[i].DepthPoint, defectArray[i].EndPoint);

                CircleF startCircle = new CircleF(startPoint, 5f);

                CircleF depthCircle = new CircleF(depthPoint, 5f);

                CircleF endCircle = new CircleF(endPoint, 5f);

                //Custom heuristic based on some experiment, double check it before use
                if ((startCircle.Center.Y < box.center.Y || depthCircle.Center.Y < box.center.Y) && (startCircle.Center.Y < depthCircle.Center.Y) && (Math.Sqrt(Math.Pow(startCircle.Center.X - depthCircle.Center.X, 2) + Math.Pow(startCircle.Center.Y - depthCircle.Center.Y, 2)) > box.size.Height / 6.5))
                {
                    fingerNum++;
                    imagen.Draw(startDepthLine, new Bgr(Color.Green), 2);
                    //currentFrame.Draw(depthEndLine, new Bgr(Color.Magenta), 2);
                }


               imagen.Draw(startCircle, new Bgr(Color.Red), 2);
                imagen.Draw(depthCircle, new Bgr(Color.Yellow), 5);
                //currentFrame.Draw(endCircle, new Bgr(Color.DarkBlue), 4);
            }
            #endregion

            }

        private void DetectarCentroPalma(IList<Point> contour, IList<Point> candidates)
        {

            double[] distances = new double[candidates.Count];

            Parallel.For(0, candidates.Count, (index) =>
            {
                distances[index] = FindMaxDistance(contour, candidates[index]);
            });

            double maxDistance = this.result == null ? 0 : this.result.DistanceToContour;
            int maxIndex = -1;
            for (int index = 0; index < distances.Length; index++)
            {
                if (distances[index] > maxDistance)
                {
                    maxDistance = distances[index];
                    maxIndex = index;
                }
            }
            if (maxIndex >= 0)
            {
                this.result = new Palm(candidates[maxIndex], maxDistance);
            }
        
        }
        private double FindMaxDistance(IList<Point> contourPoints, Point candidate)
        {
            double result = double.MaxValue;
            foreach (var point in contourPoints)
            {
                result = Math.Min(PointFunctions.Distance(point.X, point.Y, candidate.X, candidate.Y), result);
            }
            return result;
        }

        private List<Point> obtenerListaCandidatos(MCvBox2D box)
        {
            PointF[] points = box.GetVertices();


           int p1 = int.Parse(Math.Round(points[0].X).ToString());
           int p2 = int.Parse(Math.Round(points[0].Y).ToString());
           int p3 = int.Parse(Math.Round(points[1].X).ToString());
           int p4 = int.Parse(Math.Round(points[1].Y).ToString());
           int p5 = int.Parse(Math.Round(points[2].X).ToString());
           int p6 = int.Parse(Math.Round(points[2].Y).ToString());
           int p7 = int.Parse(Math.Round(points[3].X).ToString());
           int p8 = int.Parse(Math.Round(points[3].Y).ToString());

           int[] equises = new int[]
           {
           p1,p3,p5,p7
           
           };

           int[] yes = new int[]
           {
           p2,p4,p6,p8
           
           };


           int minimo = p1;
            int maximo = p1;
           foreach (int i in equises)
           {
               if (i < minimo)
               {
                   minimo = i;
               }

               if (i > maximo)
               {
                   maximo = i;
               }
           
           }

           int minimoY = p2;
           int maximoY = p2;

           foreach (int i in yes)
           {
               if (i < minimoY)
               {
                   minimoY = i;
               }

               if (i > maximoY)
               {
                   maximoY = i;
               }

           }

            Point vertice1 = new Point(minimo, minimoY);
            Point vertice2 = new Point(maximo, maximoY);

            List<Point> listaPuntos = new List<Point>();

            for (int i = vertice1.X + 1; i < vertice2.X ; i= i + 4)
            {
                for (int j = vertice1.Y - 1; j < vertice2.Y; j = j + 4)

                {

                    Point punto = new Point(i, j);

                    listaPuntos.Add(punto);
                
                }
            }

        return listaPuntos;
        }
    }
}
