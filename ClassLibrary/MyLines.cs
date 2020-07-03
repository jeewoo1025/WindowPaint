using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ClassLibrary
{
    [Serializable]
    public class MyLines
    {
        private int seq;

        private Point[] point = new Point[2];
        private int thick;
        private Color penColor;
        private bool isSolid;

        public MyLines()
        {
            this.seq = 0;
            point[0] = new Point();
            point[1] = new Point();
            this.penColor = Color.Black;
            this.thick = 1;
            this.isSolid = true;
        }

        public void setPoint(Point start, Point finish, int thick, bool isSolid, Color color, int seq)
        {
            this.seq = seq;

            point[0] = start;
            point[1] = finish;
            this.thick = thick;
            this.penColor = color;
            this.isSolid = isSolid;
        }

        public Point getPoint1()
        {
            return this.point[0];
        }
        public Point getPoint2()
        {
            return this.point[1];
        }

        public int getThick()
        {
            return this.thick;
        }

        public bool getSolid()
        {
            return this.isSolid;
        }

        public Color getPenColor()
        {
            return this.penColor;
        }

        public int getSequence()
        {
            return this.seq;
        }
    }
}
