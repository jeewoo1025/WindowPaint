using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary;
using System.Net.Sockets;
using System.Threading;

namespace 세계그림판Client
{
    enum DRAW_MODE : int
    {
        NONE = 0,
        HANDMODE,               // 손
        PENMODE,                // 펜
        LINEMODE,               // 선
        CIRCLEMODE,             // 원
        RECTMODE,               // 사각형
        EDITMODE                // 기타
    }

    public partial class Form1 : Form
    {
        // 소켓 통신
        private string IP;
        private int port;
        private string userID;

        private bool m_bConnect = false;
        private Thread m_Thread;
        private NetworkStream m_Stream;
        private TcpClient m_Client;

        private byte[] sendBuffer = new byte[BUF_SIZE];
        private byte[] readBuffer = new byte[BUF_SIZE];

        // 도형
        private DRAW_MODE mode;
        private bool isBrush;
        private bool isSolid;                                       // 선의 종류 (실선, 점선)
        private Point prevPoint;                                    // 전 점
        private Color penColor, brushColor;                         // 선 색깔, 면 색깔
        private Point start, finish, currPoint;                     // 선
        private Pen pen;                                            // 펜
        private SolidBrush brush;                                   // 브러쉬
        private int totalNum, nPen, nLine, nRect, nCircle;          // 개수
        private int thick;                                          // 두께
        private MyLines[] myPens;                                   // 펜
        private MyLines[] myLines;                                  // 선
        private MyCircle[] myCircles;                               // 원
        private MyRectangle[] myRects;                              // 사각형
        private float ratio;
        private bool isMouseMove;

        // 기타
        public const int MAX_SIZE = 200;
        public const int MAX_POINT = 2000;
        public const int BUF_SIZE = 1024 * 6;
        
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        
        public Form1(string ip, int port, string id)
        {
            InitializeComponent();
            InitShapes();
            panel1.MouseWheel += new MouseEventHandler(mousewheel);

            // 소켓 통신
            this.IP = ip;
            this.port = port;
            this.userID = id;

            try
            {
                this.m_Client = new TcpClient();
                this.m_Client.Connect(this.IP, this.port);
                this.m_bConnect = true;
                this.m_Stream = this.m_Client.GetStream();

                this.m_Thread = new Thread(new ThreadStart(Receive));
                this.m_Thread.Start();
            }
            catch(Exception e)
            {
                return;
            }
        }

        //////////////////////////////////////////////////////////////////////////

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Send채팅Packet("Exit");

            this.m_bConnect = false;

            if(this.m_Client != null)
                this.m_Client.Close();

            if(this.m_Stream != null)
                this.m_Stream.Close();

            if(this.m_Thread != null)
                this.m_Thread.Abort();
        }

        public void Send()
        {
            this.m_Stream.Write(this.sendBuffer, 0, this.sendBuffer.Length);
            this.m_Stream.Flush();

            Array.Clear(sendBuffer, 0, sendBuffer.Length);
        }

        public void Send그림Packet(int mode, Object obj)
        {
            if (!this.m_bConnect)
                return;

            그림Packet packet = new 그림Packet();
            packet.mode = mode;

            if (mode == (int)DRAW_MODE.LINEMODE || mode == (int)DRAW_MODE.PENMODE)
                packet.line = (MyLines)obj;
            else if (mode == (int)DRAW_MODE.CIRCLEMODE)
                packet.circle = (MyCircle)obj;
            else if (mode == (int)DRAW_MODE.RECTMODE)
                packet.rect = (MyRectangle)obj;

            Packet.Serialize((packet)).CopyTo(this.sendBuffer, 0);
            this.Send();
        }

        public void Send채팅Packet(string data)
        {
            if (!this.m_bConnect)
                return;

            채팅Packet packet = new 채팅Packet();
            packet.id = this.userID;
            packet.data = data;

            Packet.Serialize((packet)).CopyTo(this.sendBuffer, 0);
            this.Send();

        }

        private void Receive()
        {
            int nRead = 0;

            try
            {
                while(this.m_Client.Connected)
                {
                    Array.Clear(readBuffer, 0, readBuffer.Length);
                    nRead = this.m_Stream.Read(readBuffer, 0, BUF_SIZE);
                    if (nRead <= 0)
                        continue;

                    Packet packet = (Packet)Packet.Deserialize(readBuffer);

                    switch ((int)packet.Type)
                    {
                        case (int)PacketType.그림:
                            {
                                그림Packet tmpPacket = (그림Packet)Packet.Deserialize(readBuffer);
                                if (tmpPacket.mode == (int)DRAW_MODE.PENMODE)
                                {
                                    MyLines pen = tmpPacket.line;
                                    myPens[nPen++].setPoint(pen.getPoint1(), pen.getPoint2(), pen.getThick(), pen.getSolid(), pen.getPenColor(), this.totalNum++);
                                }
                                else if (tmpPacket.mode == (int)DRAW_MODE.LINEMODE)
                                {
                                    MyLines line = tmpPacket.line;
                                    myLines[nLine++].setPoint(line.getPoint1(), line.getPoint2(), line.getThick(), line.getSolid(), line.getPenColor(), this.totalNum++);
                                }
                                else if (tmpPacket.mode == (int)DRAW_MODE.CIRCLEMODE)
                                {
                                    MyCircle circle = tmpPacket.circle;
                                    myCircles[nCircle++].setRectC(circle.getPoint1(), circle.getPoint2(), circle.getThick(), circle.getSolid(), circle.getPenColor(), circle.getBrushColor(), circle.getBrush(), this.totalNum++);
                                }
                                else if (tmpPacket.mode == (int)DRAW_MODE.RECTMODE)
                                {
                                    MyRectangle rect = tmpPacket.rect;
                                    myRects[nRect++].setRect(rect.getPoint1(), rect.getPoint2(), rect.getThick(), rect.getSolid(), rect.getPenColor(), rect.getBrushColor(), rect.getBrush(), this.totalNum++);
                                }

                                panel1.Invalidate();
                                panel1.Update();
                                break;
                            }
                        case (int)PacketType.채팅:
                            {
                                채팅Packet tmpPacket = (채팅Packet)Packet.Deserialize(readBuffer);
                                this.Invoke(new MethodInvoker(delegate ()
                                {
                                    String msg = String.Format("[{0}] : {1}", tmpPacket.id, tmpPacket.data);
                                    txtChat.AppendText(msg + "\r\n");
                                }));
                                break;
                            }
                    }
                }
            }
            catch
            {
            }
        }

        //////////////////////////////////////////////////////////////////////////

        private void changeShape(object sender, EventArgs e)
        {
            // 그림 메뉴
            var btnOption = sender as ToolStripMenuItem;

            if (btnOption == mnuHand)
            {
                // 손
                this.mode = DRAW_MODE.HANDMODE;

                this.mnuHand.Checked = true;
                this.mnuPencil.Checked = false;
                this.mnuLine.Checked = false;
                this.mnuCircle.Checked = false;
                this.mnuRect.Checked = false;
            }
            else if(btnOption == mnuPencil)
            {
                // 펜슬
                this.mode = DRAW_MODE.PENMODE;

                this.mnuHand.Checked = false;
                this.mnuPencil.Checked = true;
                this.mnuLine.Checked = false;
                this.mnuCircle.Checked = false;
                this.mnuRect.Checked = false;
            }
            else if(btnOption == mnuLine)
            {
                // 선
                this.mode = DRAW_MODE.LINEMODE;

                this.mnuHand.Checked = false;
                this.mnuPencil.Checked = false;
                this.mnuLine.Checked = true;
                this.mnuCircle.Checked = false;
                this.mnuRect.Checked = false;
            }
            else if(btnOption == mnuCircle)
            {
                // 원
                this.mode = DRAW_MODE.CIRCLEMODE;

                this.mnuHand.Checked = false;
                this.mnuPencil.Checked = false;
                this.mnuLine.Checked = false;
                this.mnuCircle.Checked = true;
                this.mnuRect.Checked = false;
            }
            else if(btnOption == mnuRect)
            {
                // 사각형
                this.mode = DRAW_MODE.RECTMODE;

                this.mnuHand.Checked = false;
                this.mnuPencil.Checked = false;
                this.mnuLine.Checked = false;
                this.mnuCircle.Checked = false;
                this.mnuRect.Checked = true;
            }
        }

        private void changeLineThick(object sender, EventArgs e)
        {
            // 선 굵기 메뉴
            var btnOption = sender as ToolStripMenuItem;

            if(btnOption == mnuLine1)
            {
                this.thick = 1;
                this.isSolid = true;

                this.mnuLine1.Checked = true;
                this.mnuLine2.Checked = false;
                this.mnuLine3.Checked = false;
                this.mnuLine4.Checked = false;
                this.mnuLine5.Checked = false;
            }
            else if(btnOption == mnuLine2)
            {
                this.thick = 2;
                this.isSolid = true;

                this.mnuLine1.Checked = false;
                this.mnuLine2.Checked = true;
                this.mnuLine3.Checked = false;
                this.mnuLine4.Checked = false;
                this.mnuLine5.Checked = false;
            }
            else if(btnOption == mnuLine3)
            {
                this.thick = 3;
                this.isSolid = true;

                this.mnuLine1.Checked = false;
                this.mnuLine2.Checked = false;
                this.mnuLine3.Checked = true;
                this.mnuLine4.Checked = false;
                this.mnuLine5.Checked = false;
            }
            else if (btnOption == mnuLine4)
            {
                this.thick = 4;
                this.isSolid = true;

                this.mnuLine1.Checked = false;
                this.mnuLine2.Checked = false;
                this.mnuLine3.Checked = false;
                this.mnuLine4.Checked = true;
                this.mnuLine5.Checked = false;
            }
            else if (btnOption == mnuLine5)
            {
                this.thick = 5;
                this.isSolid = true;

                this.mnuLine1.Checked = false;
                this.mnuLine2.Checked = false;
                this.mnuLine3.Checked = false;
                this.mnuLine4.Checked = false;
                this.mnuLine5.Checked = true;
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            // 마우스 눌렸을때
            if(this.mode == DRAW_MODE.HANDMODE)
                this.isMouseMove = true;

            prevPoint.X = (int)(e.X / ratio);
            prevPoint.Y = (int)(e.Y / ratio);

            start.X = (int)(e.X / ratio);
            start.Y = (int)(e.Y / ratio);
        }

        private void moveAllThings(int x, int y)
        {
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            // 마우스 움직일 때
            if ((start.X == 0) && (start.Y == 0))
                return;

            finish.X = (int)(e.X / ratio);
            finish.Y = (int)(e.Y / ratio);

            if(mode == DRAW_MODE.HANDMODE && this.isMouseMove)
            {
                moveAllThings((int)(e.X/ratio), (int)(e.Y/ratio));
            }
            else if (mode == DRAW_MODE.PENMODE)
            {
                myPens[nPen].setPoint(prevPoint, finish, thick, isSolid, penColor, this.totalNum);
                Send그림Packet((int)mode, myPens[nPen]);

                this.nPen++;
                this.totalNum++;

                prevPoint.X = finish.X;
                prevPoint.Y = finish.Y;
            }
            else if (mode == DRAW_MODE.LINEMODE)
                myLines[nLine].setPoint(start, finish, thick, isSolid, penColor, this.totalNum);
            else if (mode == DRAW_MODE.CIRCLEMODE)
                myCircles[nCircle].setRectC(start, finish, thick, isSolid, penColor, brushColor, isBrush, this.totalNum);
            else if (mode == DRAW_MODE.RECTMODE)
                myRects[nRect].setRect(start, finish, thick, isSolid, penColor, brushColor, isBrush, this.totalNum);

            panel1.Invalidate();
            panel1.Update();
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            // 마우스 버튼이 띠어졌을때
            if (mode == DRAW_MODE.HANDMODE)
            {
                this.isMouseMove = false;
            }
            else if (mode == DRAW_MODE.PENMODE)
            {
                finish.X = (int)(e.X / ratio);
                finish.Y = (int)(e.Y / ratio);

                myPens[nPen].setPoint(prevPoint, finish, thick, isSolid, penColor, this.totalNum);
                Send그림Packet((int)mode, myPens[nPen]);

                nPen++;
                this.totalNum++;
            }
            else if (mode == DRAW_MODE.LINEMODE)
            {
                Send그림Packet((int)mode, myLines[nLine]);

                nLine++;
                this.totalNum++;
            }
            else if (mode == DRAW_MODE.CIRCLEMODE)
            {
                Send그림Packet((int)mode, myCircles[nCircle]);

                nCircle++;
                this.totalNum++;
            }
            else if (mode == DRAW_MODE.RECTMODE)
            {
                Send그림Packet((int)mode, myRects[nRect]);

                nRect++;
                this.totalNum++;
            }

            start.X = 0;
            start.Y = 0;
            finish.X = 0;
            finish.Y = 0;
        }

        private void txtSend_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                Send채팅Packet(txtSend.Text);
                txtSend.Text = "";
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Send채팅Packet(txtSend.Text);
            txtSend.Text = "";
        }

        private void txtFill_Click(object sender, EventArgs e)
        {
            if (!isBrush)
            {
                // 채우기 
                isBrush = true;
                txtFill.ForeColor = Color.Red;
            }
            else
            {
                // 채우기 해제
                isBrush = false;
                txtFill.ForeColor = Color.Black;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            int i = 0, j = 0;

            e.Graphics.ScaleTransform(ratio, ratio);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            for (j = 0; j <= totalNum; j++)
            {
                // 점그리기
                for (i = 0; i <= nPen; i++)
                {
                    if (myPens[i].getSequence() == j)
                    {
                        pen.Width = myPens[i].getThick() * ratio;
                        pen.DashStyle = DashStyle.Solid;
                        pen.Color = myPens[i].getPenColor();

                        if (i > 0)
                            e.Graphics.DrawLine(pen, myPens[i].getPoint1(), myPens[i].getPoint2());
                    }
                }

                // 선 그리기
                for (i = 0; i <= nLine; i++)
                {
                    if (myLines[i].getSequence() == j)
                    {
                        if (!myLines[i].getSolid())
                        {
                            pen.Width = 1;
                            pen.DashStyle = DashStyle.Dot;
                        }
                        else
                        {
                            pen.Width = myLines[i].getThick() * ratio;
                            pen.DashStyle = DashStyle.Solid;
                        }
                        pen.Color = myLines[i].getPenColor();

                        e.Graphics.DrawLine(pen, myLines[i].getPoint1(), myLines[i].getPoint2());
                    }
                }

                // 원 그리기
                for (i = 0; i <= nCircle; i++)
                {
                    if (myCircles[i].getSequence() == j)
                    {
                        if (!myCircles[i].getSolid())
                        {
                            pen.Width = 1;
                            pen.DashStyle = DashStyle.Dot;
                        }
                        else
                        {
                            pen.Width = myCircles[i].getThick() * ratio;
                            pen.DashStyle = DashStyle.Solid;
                        }
                        pen.Color = myCircles[i].getPenColor();

                        e.Graphics.DrawEllipse(pen, myCircles[i].getRectC());

                        if (myCircles[i].getBrush())
                        {
                            // 채우기
                            brush.Color = myCircles[i].getBrushColor();
                            e.Graphics.FillEllipse(brush, myCircles[i].getRectC());
                        }
                    }
                }

                // 사각형 그리기
                for (i = 0; i <= nRect; i++)
                {
                    if (myRects[nRect].getSequence() == j)
                    {
                        if (!myRects[i].getSolid())
                        {
                            pen.Width = 1;
                            pen.DashStyle = DashStyle.Dot;
                        }
                        else
                        {
                            pen.Width = myRects[i].getThick() * ratio;
                            pen.DashStyle = DashStyle.Solid;
                        }
                        pen.Color = myRects[i].getPenColor();

                        e.Graphics.DrawRectangle(pen, myRects[i].getRect());

                        if (myRects[i].getBrush())
                        {
                            // 채우기
                            brush.Color = myRects[i].getBrushColor();
                            e.Graphics.FillRectangle(brush, myRects[i].getRect());
                        }
                    }
                }

                pen.Width = thick;
                pen.DashStyle = DashStyle.Solid;
            }
        }

        private void InitShapes()
        {
            this.thick = 1;
            mnuLine1.Checked = true;

            // 모드
            this.isSolid = true;
            this.mode = DRAW_MODE.NONE;
            this.isBrush = false;
            this.ratio = 1.0f;
            this.isMouseMove = false;

            // 색깔
            this.penColor = Color.Black;
            this.brushColor = Color.Black;

            // 시작점, 끝점, 펜, 브러쉬
            this.prevPoint = new Point(0, 0);
            this.start = new Point(0, 0);
            this.finish = new Point(0, 0);
            this.currPoint = new Point(0, 0);

            this.pen = new Pen(btnLineCld.BackColor);
            this.brush = new SolidBrush(btnShapeCld.BackColor);
            this.totalNum = 0;

            // 점, 선, 원, 사각형
            int i = 0;
            this.myPens = new MyLines[MAX_POINT];
            this.nPen = 0;
            for (i = 0; i < MAX_POINT; i++)
                myPens[i] = new MyLines();

            this.myLines = new MyLines[MAX_SIZE];
            this.nLine = 0;
            for (i = 0; i < MAX_SIZE; i++)
                myLines[i] = new MyLines();

            this.myCircles = new MyCircle[MAX_SIZE];
            this.nCircle = 0;
            for (i = 0; i < MAX_SIZE; i++)
                myCircles[i] = new MyCircle();

            this.myRects = new MyRectangle[MAX_SIZE];
            this.nRect = 0;
            for (i = 0; i < MAX_SIZE; i++)
                myRects[i] = new MyRectangle();
        }

        private void btnLineCld_Click(object sender, EventArgs e)
        {
            cld.Color = btnLineCld.BackColor;
            cld.ShowDialog();
            btnLineCld.BackColor = cld.Color;
            penColor = cld.Color;
        }

        private void btnShapeCld_Click(object sender, EventArgs e)
        {
            cld.Color = btnShapeCld.BackColor;
            cld.ShowDialog();
            btnShapeCld.BackColor = cld.Color;
            brushColor = cld.Color;
        }

        private void mousewheel(object sender, MouseEventArgs e)
        {
            int lines = e.Delta * SystemInformation.MouseWheelScrollLines;

            if (lines > 0)
            {
                ratio *= 1.1f;
                if (ratio > 10.0) 
                    ratio = 10.0f;
            }
            else if (lines < 0)
            {
                ratio *= 0.9f;
                if (ratio < 1) 
                    ratio = 1.0f;
            }

            panel1.Height = (int)(panel1.Height * ratio);
            panel1.Width = (int)(panel1.Width * ratio);

            int i, j;
            for (j = 0; j < totalNum; j++)
            {
                for(i = 0; i < nPen; i++)
                {
                    if (myPens[i].getSequence() == j)
                        myPens[i].setPoint(myPens[i].getPoint1(), myPens[i].getPoint2(), myPens[i].getThick(), myPens[i].getSolid(), myPens[i].getPenColor(), myPens[i].getSequence());
                }

                for(i = 0; i < nLine; i++)
                {
                    if (myLines[i].getSequence() == j)
                        myLines[i].setPoint(myLines[i].getPoint1(), myLines[i].getPoint2(), myLines[i].getThick(), myLines[i].getSolid(), myLines[i].getPenColor(), myLines[i].getSequence());
                }

                for(i = 0; i < nCircle; i++)
                {
                    if (myCircles[i].getSequence() == j)
                        myCircles[i].setRectC(myCircles[i].getPoint1(), myCircles[i].getPoint2(), myCircles[i].getThick(), myCircles[i].getSolid(), myCircles[i].getPenColor(), myCircles[i].getBrushColor(), myCircles[i].getBrush(), myCircles[i].getSequence());
                }

                for(i = 0; i < nRect; i++)
                {
                    if (myRects[i].getSequence() == j)
                        myRects[i].setRect(myRects[i].getPoint1(), myRects[i].getPoint2(), myRects[i].getThick(), myRects[i].getSolid(), myRects[i].getPenColor(), myRects[i].getBrushColor(), myRects[i].getBrush(), myRects[i].getSequence());
                }
            }

            panel1.Invalidate();
            panel1.Update();
        }
    }

    public class DoubleBufferPanel : Panel
    {
        public DoubleBufferPanel()
        {
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
        }
    }
}
