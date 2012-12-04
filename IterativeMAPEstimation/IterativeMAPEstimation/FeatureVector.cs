using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IterativeMAPEstimation
{
    class FeatureVector
    {
        PointF palmCenter;

        public PointF PalmCenter
        {
            get { return palmCenter; }
            set { palmCenter = value; }
        }

        PointF locationThumb;

        public PointF LocationThumb
        {
            get { return locationThumb; }
            set { locationThumb = value; }
        }
        PointF locationIndexFinger;

        public PointF LocationIndexFinger
        {
            get { return locationIndexFinger; }
            set { locationIndexFinger = value; }
        }
        PointF locationHeartFinger;

        public PointF LocationHeartFinger
        {
            get { return locationHeartFinger; }
            set { locationHeartFinger = value; }
        }
        PointF location4Finger;

        public PointF Location4Finger
        {
            get { return location4Finger; }
            set { location4Finger = value; }
        }
        PointF locationLittleFinger;

        public PointF LocationLittleFinger
        {
            get { return locationLittleFinger; }
            set { locationLittleFinger = value; }
        }

        float thumbIndexAngle;

        public float ThumbIndexAngle
        {
            get { return thumbIndexAngle; }
            set { thumbIndexAngle = value; }
        }
        float indexHeartAngle;

        public float IndexHeartAngle
        {
            get { return indexHeartAngle; }
            set { indexHeartAngle = value; }
        }
        float heartFourthAngle;

        public float HeartFourthAngle
        {
            get { return heartFourthAngle; }
            set { heartFourthAngle = value; }
        }
        float fourthLittleAngle;

        public float FourthLittleAngle
        {
            get { return fourthLittleAngle; }
            set { fourthLittleAngle = value; }
        }

        float palmSize;

        public float PalmSize
        {
            get { return palmSize; }
            set { palmSize = value; }
        }


        public FeatureVector()

        { }

    }
}
