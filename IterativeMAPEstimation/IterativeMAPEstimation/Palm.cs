using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CCT.NUI.HandTracking
{
    public class Palm
    {
        private PointF location;
        private double distanceToContour;

        public Palm(PointF location, double distanceToContour)
        {
            this.location = location;
            this.distanceToContour = distanceToContour;
        }

        public PointF Location
        {
            get { return this.location; }
        }

        public double DistanceToContour
        {
            get { return this.distanceToContour; }
        }
    }
}
