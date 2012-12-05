using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IterativeMAPEstimation
{
   public class FeatureVector
    {
        PointF palmCenter;
        PointF locationThumb;
        PointF locationIndexFinger;
        PointF locationHeartFinger;
        PointF location4Finger;
        PointF locationLittleFinger;
        public float thumbAngle;
        public float indexAngle;
        public float heartAngle;
        public float fourthAngle;
        public float littleAngle;
        public float palmSize;



//CONSTRUCTOR

        public FeatureVector(List<PointF> fingertips,List<float> angles, PointF center, float size)
        {

            this.locationLittleFinger = fingertips[0];
            this.location4Finger = fingertips[1];
            this.locationHeartFinger = fingertips[2];
            this.locationIndexFinger = fingertips[3];
            this.locationThumb = fingertips[4];

            this.littleAngle = angles[0];
            this.fourthAngle = angles[1];
            this.heartAngle = angles[2];
            this.indexAngle = angles[3];
            this.thumbAngle = angles[4];

            this.palmCenter = center;
            this.palmSize = size;
        
        }

//GETTERS AND SETTERS   

        public PointF PalmCenter
        {
            get { return palmCenter; }
            set { palmCenter = value; }
        }


        public PointF LocationThumb
        {
            get { return locationThumb; }
            set { locationThumb = value; }
        }

        public PointF LocationIndexFinger
        {
            get { return locationIndexFinger; }
            set { locationIndexFinger = value; }
        }

        public PointF LocationHeartFinger
        {
            get { return locationHeartFinger; }
            set { locationHeartFinger = value; }
        }

        public PointF Location4Finger
        {
            get { return location4Finger; }
            set { location4Finger = value; }
        }

        public PointF LocationLittleFinger
        {
            get { return locationLittleFinger; }
            set { locationLittleFinger = value; }
        }

        public float PalmSize
        {
            get { return palmSize; }
            set { palmSize = value; }
        }



    }
}
