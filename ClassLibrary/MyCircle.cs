using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ClassLibrary
{
    [Serializable]
    public class MyCircle
    {
        private int seq;

        private Point[] point = new Point[2];
        private Rectangle rectC;
        private int thick;
        private Color penColor;
        private Color brushColor;
        private bool isSolid;
        private bool isBrush;

        public MyCircle()
        {
            this.seq = 0;
            this.rectC = new Rectangle();
            this.penColor = Color.Black;
            this.brushColor = Color.Black;
            this.thick = 1;
            this.isSolid = true;
            this.isBrush = false;
        }

        public void setRectC(Point start, Point finish, int thick, bool isSolid, Color pen, Color brush, bool isBrush, int seq)
        {
            this.seq = seq;
            this.point[0] = start;
            this.point[1] = finish;

            rectC.X = Math.Min(start.X, finish.X);
            rectC.Y = Math.Min(start.Y, finish.Y);
            rectC.Width = Math.Abs(start.X - finish.X);
            rectC.Height = Math.Abs(start.Y - finish.Y);
            this.penColor = pen;
            this.brushColor = brush;
            this.thick = thick;
            this.isSolid = isSolid;
            this.isBrush = isBrush;
        }

        public Point getPoint1()
        {
            return this.point[0];
        }
        public Point getPoint2()
        {
            return this.point[1];
        }

        public Rectangle getRectC()
        {
            return this.rectC;
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

        public Color getBrushColor()
        {
            return this.brushColor;
        }

        public bool getBrush()
        {
            return this.isBrush;
        }

        public int getSequence()
        {
            return this.seq;
        }
    }
}
