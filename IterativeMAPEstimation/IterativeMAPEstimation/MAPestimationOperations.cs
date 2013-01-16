using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IterativeMAPEstimation
{
    public static class MAPestimationOperations
    {
        public static FeatureVector core(FeatureVector original, FeatureVector hypothesis)
        {
            List<PointF> thumbPoints = possiblePositions(hypothesis.LocationThumb);
            List<PointF> indexPoints = possiblePositions(hypothesis.LocationIndexFinger);
            List<PointF> heartPoints = possiblePositions(hypothesis.LocationHeartFinger);
            List<PointF> fourthPoints = possiblePositions(hypothesis.Location4Finger);
            List<PointF> littlePoints = possiblePositions(hypothesis.LocationLittleFinger);
            List<PointF> centerPoints = possiblePositions(hypothesis.PalmCenter);

            Double mismatch = 0;
            int cost = 0;
            Double L=0;
            FeatureVector finalHypothesis = new FeatureVector();

            foreach (PointF pThumb in thumbPoints)
            {
                foreach (PointF pIndex in indexPoints)
                {
                    foreach (PointF pHeart in heartPoints)
                    {
                        foreach (PointF pFourth in fourthPoints)
                        {
                            foreach (PointF pLittle in littlePoints)
                            {
                                foreach (PointF pCent in centerPoints)
                                {

                                    Double misCent = mismatchCalc(original.PalmCenter, pCent);
                                    Double misThumb = mismatchCalc(original.LocationThumb, pThumb);
                                    Double misIndex = mismatchCalc(original.LocationIndexFinger, pIndex);
                                    Double misHeart = mismatchCalc(original.LocationHeartFinger, pHeart);
                                    Double misFourth = mismatchCalc(original.Location4Finger, pFourth);
                                    Double misLittle = mismatchCalc(original.LocationLittleFinger, pLittle);

                                    int costCent = relativeCost(hypothesis.PalmCenter, pCent);
                                    int costThumb = relativeCost(hypothesis.LocationThumb, pThumb);
                                    int costIndex = relativeCost(hypothesis.LocationIndexFinger, pIndex);
                                    int costHeart = relativeCost(hypothesis.LocationHeartFinger, pHeart);
                                    int costFourth = relativeCost(hypothesis.Location4Finger, pFourth);
                                    int costLittle = relativeCost(hypothesis.LocationLittleFinger, pLittle);


                                    mismatch = misCent + misThumb + misIndex + misHeart + misFourth + misLittle;
                                    cost = costCent+ costThumb + costIndex + costHeart + costFourth + costLittle;

                                    Double newL = mismatch + cost;

                                    List<PointF> fingertips = new List<PointF>();
                                    fingertips.Add(pThumb);
                                    fingertips.Add(pIndex);
                                    fingertips.Add(pHeart);
                                    fingertips.Add(pFourth);
                                    fingertips.Add(pLittle);

                                    List<float> angulos = new List<float>();
                                    angulos = calculateFingerAngles(fingertips,pCent);

                                    FeatureVector newHypothesis = new FeatureVector(fingertips,angulos,pCent,5);
     
                                    if (L >= newL)
                                    {
                                        L = newL;
                                        finalHypothesis = newHypothesis;
                                    }
                                }
                            }

                        }
                    }
                }
            }

            return finalHypothesis;
            
        }
        public static Double mismatchCalc(PointF p, PointF p2)
        {

            Double mis = Math.Sqrt((Math.Abs(p.X - p2.X) * Math.Abs(p.X - p2.X)) + (Math.Abs(p.Y - p2.Y) * Math.Abs(p.Y - p2.Y)));

            return mis;
        }

        public static List<PointF> possiblePositions(PointF p)
        {

            List<PointF> list = new List<PointF>();

            PointF p1 = new PointF(p.X - 1,p.Y);
            PointF p2 = new PointF(p.X - 1, p.Y - 1);
            PointF p3 = new PointF(p.X - 1, p.Y + 1);
            PointF p4 = new PointF(p.X + 1, p.Y);
            PointF p5 = new PointF(p.X + 1, p.Y + 1);
            PointF p6 = new PointF(p.X + 1, p.Y - 1);
            PointF p7 = new PointF(p.X, p.Y + 1);
            PointF p8 = new PointF(p.X, p.Y - 1);

            list.Add(p);
            list.Add(p1);
            list.Add(p2);
            list.Add(p3);
            list.Add(p4);
            list.Add(p5);
            list.Add(p6);
            list.Add(p7);
            list.Add(p8);

            return list;
        }

        public static int relativeCost(PointF p, PointF p1)
        {
            int cost = 0;

            if (p.X == p1.X && p.Y == p1.Y)
            {

                cost = 0;
            
            }
            else if ((p.X != p1.X && p.Y == p1.Y) || (p.X == p1.X && p.Y != p1.Y))
            {

                cost = 1;

            }
            else
            {
                cost = 2;
            }


            return cost;
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
