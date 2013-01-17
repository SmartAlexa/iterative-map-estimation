using CCT.NUI.Core;
using CCT.NUI.HandTracking;
using Emgu.CV;
using Emgu.CV.Structure;
using HandGestureRecognition.SkinDetector;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IterativeMAPEstimation
{
    public static class ObservedImageFunctions
    {
       static Seq<Point> hull;
       static Seq<Point> filteredHull;
       static Seq<MCvConvexityDefect> defects;
       static MCvConvexityDefect[] defectArray;
       static Rectangle handRect;
       static MCvBox2D box;
       static Ellipse ellip;

       private static float contourReduction;
        private static int searchRadius;

        private static Palm result;

        public static FeatureVector ExtractFeatures(Image<Gray, byte> skin, Image<Bgr, Byte> imagen)
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

            #region find palm center(needs change)

           searchRadius = 6;
           contourReduction = 3;

            //this.result = null;

            DetectarCentroPalma(biggestContour.ToList<Point>(), obtenerListaCandidatos(box));



            PointF punto = new PointF(405, 380);
            Point punt = new Point(405, 380);
            CircleF centerCircle = new CircleF(punto, 5f);
            //CircleF centerCircle = new CircleF(result.Location, 5f);
            imagen.Draw(centerCircle, new Bgr(Color.Brown), 3);

            /* 
             for (int i = 0; i < defects.Total; i++)
             {
                 LineSegment2D lineaDedoCentro = new LineSegment2D(defectArray[i].StartPoint, punt);
                 imagen.Draw(lineaDedoCentro, new Bgr(Color.Green), 2);
            
             }
             * */


            #endregion

            List<PointF> fingertips = defectsDrawing(imagen, ref punt);

            #region create feature vector
            List<PointF> newFingertips = ordenarFingertips(fingertips);
            List<float> angles = calculateFingerAngles(fingertips, punto);

            FeatureVector vector = new FeatureVector(newFingertips, angles, punto, 5);
            //MessageBox.Show("Done");

           // frmPruebaDatos datos = new frmPruebaDatos(vector);
           // datos.Show();

            #endregion

            return vector;
        }

        private static List<PointF> defectsDrawing(Image<Bgr, Byte> imagen, ref Point punt)
        {
            int fingerNum = 0;
            List<PointF> fingertips = new List<PointF>();
            for (int i = 0; i < defects.Total; i++)
            {
                PointF startPoint = new PointF((float)defectArray[i].StartPoint.X,
                                                (float)defectArray[i].StartPoint.Y);

                PointF depthPoint = new PointF((float)defectArray[i].DepthPoint.X,
                                                (float)defectArray[i].DepthPoint.Y);

                PointF endPoint = new PointF((float)defectArray[i].EndPoint.X,
                                                (float)defectArray[i].EndPoint.Y);

                LineSegment2D lineaDedoCentro = new LineSegment2D(defectArray[i].StartPoint, punt);

                //LineSegment2D startDepthLine = new LineSegment2D(defectArray[i].StartPoint, defectArray[i].DepthPoint);

                //LineSegment2D depthEndLine = new LineSegment2D(defectArray[i].DepthPoint, defectArray[i].EndPoint);

                CircleF startCircle = new CircleF(startPoint, 5f);

                CircleF depthCircle = new CircleF(depthPoint, 5f);

                CircleF endCircle = new CircleF(endPoint, 5f);



                //Custom heuristic based on some experiment, double check it before use
                if ((startCircle.Center.Y < box.center.Y || depthCircle.Center.Y < box.center.Y) && (startCircle.Center.Y < depthCircle.Center.Y) && (Math.Sqrt(Math.Pow(startCircle.Center.X - depthCircle.Center.X, 2) + Math.Pow(startCircle.Center.Y - depthCircle.Center.Y, 2)) > box.size.Height / 6.5))
                {
                    fingerNum++;
                    //imagen.Draw(startDepthLine, new Bgr(Color.Green), 2);
                    //imagen.Draw(depthEndLine, new Bgr(Color.Magenta), 2);

                    imagen.Draw(lineaDedoCentro, new Bgr(Color.Green), 2);
                    fingertips.Add(startPoint);
                }

                imagen.Draw(startCircle, new Bgr(Color.Red), 2);
                //imagen.Draw(depthCircle, new Bgr(Color.Yellow), 5);
                //imagen.Draw(endCircle, new Bgr(Color.DarkBlue), 4);
            }
            return fingertips;
        }
        public static List<PointF> ordenarFingertips(List<PointF> fingertips)
        {
            /*
            List<PointF> listaNueva = new List<PointF>();
            
            for (int i = 0; i < fingertips.Count; i++)
            {
                float punto = fingertips.ElementAt(i).X;
                listaNueva.Add(punto);

            }
            */

            for (int pasadas = 0; pasadas < fingertips.Count - 1; pasadas++)
            {
                for (int i = 0; i < fingertips.Count - 1; i++)
                {
                    if (fingertips[i].X > fingertips[i + 1].X)
                    {

                        PointF temp;      // variable temporal para el intercambio
                        temp = fingertips[i];
                        fingertips[i] = fingertips[i + 1];
                        fingertips[i + 1] = temp;

                    }

                }
            }

            //MessageBox.Show(listaNueva.ToString());
            return fingertips;
        }
        public static  void DetectarCentroPalma(IList<Point> contour, IList<PointF> candidates)
        {

            double[] distances = new double[candidates.Count];

            Parallel.For(0, candidates.Count, (index) =>
            {
                distances[index] = FindMaxDistance(contour, candidates[index]);
            });

            double maxDistance = result == null ? 0 : result.DistanceToContour;
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
                result = new Palm(candidates[maxIndex], maxDistance);
            }

        }
        public static double FindMaxDistance(IList<Point> contourPoints, PointF candidate)
        {
            double result = double.MaxValue;
            foreach (var point in contourPoints)
            {
                result = Math.Min(PointFunctions.Distance(point.X, point.Y, candidate.X, candidate.Y), result);
            }
            return result;
        }
        public static List<PointF> obtenerListaCandidatos(MCvBox2D box)
        {
            PointF[] points = box.GetVertices();


            float p1 = points[0].X;
            float p2 = points[0].Y;
            float p3 = points[1].X;
            float p4 = points[1].Y;
            float p5 = points[2].X;
            float p6 = points[2].Y;
            float p7 = points[3].X;
            float p8 = points[3].Y;

            float[] equises = new float[]
           {
           p1,p3,p5,p7
           
           };

            float[] yes = new float[]
           {
           p2,p4,p6,p8
           
           };


            float minimo = p1;
            float maximo = p1;
            foreach (float i in equises)
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

            float minimoY = p2;
            float maximoY = p2;

            foreach (float i in yes)
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

            PointF vertice1 = new PointF(minimo, minimoY);
            PointF vertice2 = new PointF(maximo, maximoY);

            List<PointF> listaPuntos = new List<PointF>();

            for (float i = vertice1.X + 1; i < vertice2.X; i = i + 4)
            {
                for (float j = vertice1.Y - 1; j < vertice2.Y; j = j + 4)
                {

                    PointF punto = new PointF(i, j);

                    listaPuntos.Add(punto);

                }
            }

            return listaPuntos;
        }
        public static List<float> calculateFingerAngles(List<PointF> fingertips, PointF center)
        {
            List<float> listaAngulos = new List<float>();

            foreach (PointF p in fingertips)
            {

                float c1 = center.X - p.X;
                float c2 = center.Y - p.Y;

                float h = float.Parse(Math.Sqrt((c1 * c1) + (c2 * c2)).ToString());

                float sinAlpha = c2 / h;

                Double angulo = Math.Asin(sinAlpha);
                Double anguloGrad = (angulo * 360) / (2 * 3.14158);
                float alpha = float.Parse(anguloGrad.ToString());

                if (p.X > center.X)
                {

                    alpha = 180 - alpha;

                }

                listaAngulos.Add(alpha);

            }


            return listaAngulos;
        }
    }
}
