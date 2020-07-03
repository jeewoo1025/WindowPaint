using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary
{
    [Serializable]
    public class MyRectangle
    {
        private int seq;

        private Point[] point = new Point[2];
        private Rectangle rect;
        private Color penColor;
        private Color brushColor;
        private int thick;
        private bool isSolid;
        private bool isBrush;

        public MyRectangle()
        {
            this.seq = 0;
            this.rect = new Rectangle();
            this.penColor = Color.Black;
            this.brushColor = Color.Black;
            this.thick = 1;
            this.isSolid = true;
            this.isBrush = false;
        }

        public void setRect(Point start, Point finish, int thick, bool isSolid, Color pen, Color brush, bool isBrush, int seq)
        {
            this.seq = seq;
            this.point[0] = start;
            this.point[1] = finish;

            rect.X = Math.Min(start.X, finish.X);
            rect.Y = Math.Min(start.Y, finish.Y);
            rect.Width = Math.Abs(finish.X - start.X);
            rect.Height = Math.Abs(finish.Y - start.Y);
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

        public Rectangle getRect()
        {
            return this.rect;
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
