using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Runtime.InteropServices;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace 세계그림판
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

        private TcpListener m_listener;
        private NetworkStream m_Stream;
        private bool m_bClientOn = false;
        private Thread m_Thread;
        private int nChild = 0;
        private List<TcpClient> clientList;

        private byte[] sendBuffer = new byte[BUF_SIZE];
        private byte[] readBuffer = new byte[BUF_SIZE];

        // 도형
        private Point prevPoint;                                // 전 점
        private Color penColor, brushColor;                     // 선 색깔, 면 색깔
        private Point start, finish;                            // 선
        private Pen pen;                                        // 펜
        private SolidBrush brush;                               // 브러쉬
        private int totalNum, nPen, nLine, nRect, nCircle;      // 개수
        private int thick;                                      // 두께
        private MyLines[] myPens;                               // 펜
        private MyLines[] myLines;                              // 선
        private MyCircle[] myCircles;                           // 원
        private MyRectangle[] myRects;                          // 사각형

        // 백업 파일
        private string fileName = "server_shapes";
        private List<그림Packet> fileInfo = new List<그림Packet>();

        // 기타
        public const int MAX_SIZE = 200;
        public const int MAX_POINT = 2000;
        public const int MAX_CLIENTS = 10;
        public const int BUF_SIZE = 1024 * 6;

        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////

        public Form1(string ip, int port)
        {
            InitializeComponent();
            InitShapes();

            // 소켓 통신
            this.IP = ip;
            this.port = port;

            this.clientList = new List<TcpClient>();

            this.m_Thread = new Thread(new ThreadStart(ServerStart));
            this.m_Thread.Start();

            // 백업 파일 생성
            using (FileStream fs = File.Open(fileName, FileMode.OpenOrCreate))
            {
                if (fs.Length > 0)
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    fileInfo = (List<그림Packet>)bf.Deserialize(fs);

                    foreach(var item in fileInfo)
                    {
                        if(item.mode == (int)DRAW_MODE.PENMODE)
                        {
                            MyLines pen = item.line;
                            myPens[nPen++].setPoint(pen.getPoint1(), pen.getPoint2(), pen.getThick(), pen.getSolid(), pen.getPenColor(), this.totalNum++);
                        }
                        else if(item.mode == (int)DRAW_MODE.LINEMODE)
                        {
                            MyLines line = item.line;
                            myLines[nLine++].setPoint(line.getPoint1(), line.getPoint2(), line.getThick(), line.getSolid(), line.getPenColor(), this.totalNum++);
                        }
                        else if(item.mode == (int)DRAW_MODE.CIRCLEMODE)
                        {
                            MyCircle circle = item.circle;
                            myCircles[nCircle++].setRectC(circle.getPoint1(), circle.getPoint2(), circle.getThick(), circle.getSolid(), circle.getPenColor(), circle.getBrushColor(), circle.getBrush(), this.totalNum++);
                        }
                        else if(item.mode == (int)DRAW_MODE.RECTMODE)
                        {
                            MyRectangle rect = item.rect;
                            myRects[nRect++].setRect(rect.getPoint1(), rect.getPoint2(), rect.getThick(), rect.getSolid(), rect.getPenColor(), rect.getBrushColor(), rect.getBrush(), this.totalNum++);
                        }
                    }
                }    
            }
        }

        //////////////////////////////////////////////////////////////////////////

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 백업 파일 생성 or 덮어쓰기
            if (this.totalNum > 0)
            {
                using (FileStream fs = File.Open(fileName, FileMode.Create, FileAccess.Write))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, fileInfo);
                }
            }

            this.m_listener.Stop();

            if(this.m_Stream != null)
                this.m_Stream.Close();

            if(this.m_Thread != null)
                this.m_Thread.Abort();
        }

        public void Send()
        {
            if (this.m_Stream == null)
                return;

            this.m_Stream.Write(this.sendBuffer, 0, this.sendBuffer.Length);
            this.m_Stream.Flush();

            Array.Clear(sendBuffer, 0, sendBuffer.Length);
        }

        public void addList<T>(int cnt, T[] src, List<T> dest)
        {
            if (cnt <= 0)
                return;

            for(int i = 0; i <= cnt; i++)
            {
                dest.Add(src[i]);
            }
        }

        public void Send초기화Packet(TcpClient client)
        {
            if ((!this.m_bClientOn))
                return;

            if (this.totalNum <= 0)
                return;

            this.m_Stream = client.GetStream();

            int i, j;
            for(j = 0; j <= totalNum; j++)
            {
                for(i = 0; i <= nPen; i++)
                {
                    if(myPens[i].getSequence() == j)
                        Send그림Packet((int)DRAW_MODE.PENMODE, myPens[i], client);
                }

                for(i = 0; i <= nLine; i++)
                {
                    if(myLines[i].getSequence() == j)
                        Send그림Packet((int)DRAW_MODE.LINEMODE, myLines[i], client);
                }

                for(i = 0; i <= nCircle; i++)
                {
                    if(myCircles[i].getSequence() == j)
                        Send그림Packet((int)DRAW_MODE.CIRCLEMODE, myCircles[i], client);
                }

                for(i = 0; i <= nRect; i++)
                {
                    if(myRects[i].getSequence() == j)
                        Send그림Packet((int)DRAW_MODE.RECTMODE, myRects[i], client);
                }

            }
        }

        public void Send그림Packet(int mode, Object obj, TcpClient client)
        {
            if (!this.m_bClientOn)
                return;

            this.m_Stream = client.GetStream();

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

        public void Send채팅Packet(string id, string data)
        {
            if (!this.m_bClientOn)
                return;

            foreach (TcpClient item in clientList)
            {
                this.m_Stream = item.GetStream();

                채팅Packet packet = new 채팅Packet();
                packet.id = id;
                packet.data = data;

                Packet.Serialize((packet)).CopyTo(this.sendBuffer, 0);
                this.Send();
            }
        }

        private void Receive(Object obj)
        {
            TcpClient currClient = (TcpClient)obj;
            NetworkStream r_Stream = (NetworkStream)currClient.GetStream();
            int nRead = 0;

            try
            {
                while (currClient.Connected)
                {
                    Array.Clear(readBuffer, 0, readBuffer.Length);
                    nRead = r_Stream.Read(readBuffer, 0, BUF_SIZE);
                    if (nRead <= 0)
                        continue;

                    Packet packet = (Packet)Packet.Deserialize(readBuffer);

                    switch ((int)packet.Type)
                    {
                        case (int)PacketType.그림:
                            {
                                그림Packet tmpPacket = (그림Packet)Packet.Deserialize(readBuffer);
                                this.fileInfo.Add(tmpPacket);
                                Object sendItem = null;

                                if(tmpPacket.mode == (int)DRAW_MODE.PENMODE)
                                {
                                    MyLines pen = tmpPacket.line;
                                    sendItem = tmpPacket.line;

                                    myPens[nPen++].setPoint(pen.getPoint1(), pen.getPoint2(), pen.getThick(), pen.getSolid(), pen.getPenColor(), this.totalNum++);
                                }
                                else if (tmpPacket.mode == (int)DRAW_MODE.LINEMODE)
                                {
                                    MyLines line = tmpPacket.line;
                                    sendItem = tmpPacket.line;

                                    myLines[nLine++].setPoint(line.getPoint1(), line.getPoint2(), line.getThick(), line.getSolid(), line.getPenColor(), this.totalNum++);
                                }
                                else if(tmpPacket.mode == (int)DRAW_MODE.CIRCLEMODE)
                                {
                                    MyCircle circle = tmpPacket.circle;
                                    sendItem = tmpPacket.circle;

                                    myCircles[nCircle++].setRectC(circle.getPoint1(), circle.getPoint2(), circle.getThick(), circle.getSolid(), circle.getPenColor(), circle.getBrushColor(), circle.getBrush(), this.totalNum++);
                                }
                                else if(tmpPacket.mode == (int)DRAW_MODE.RECTMODE)
                                {
                                    MyRectangle rect = tmpPacket.rect;
                                    sendItem = tmpPacket.rect;

                                    myRects[nRect++].setRect(rect.getPoint1(), rect.getPoint2(), rect.getThick(), rect.getSolid(), rect.getPenColor(), rect.getBrushColor(), rect.getBrush(), this.totalNum++);
                                }

                                foreach(var itr in clientList)
                                {
                                    Send그림Packet(tmpPacket.mode, sendItem, itr);
                                }

                                panel1.Invalidate();
                                panel1.Update();
                                break;
                            }
                        case (int)PacketType.채팅:
                            {
                                채팅Packet tmpPacket = (채팅Packet)Packet.Deserialize(readBuffer);

                                if(tmpPacket.data.Equals("Exit"))
                                {
                                    if (nChild > 0)
                                    {
                                        nChild--;
                                        clientList.Remove(currClient);
                                    }

                                    currClient.Close();
                                    if (r_Stream != null)
                                        r_Stream.Close();

                                    return;
                                }

                                String msg = String.Format("[{0}] : {1}", tmpPacket.id, tmpPacket.data);
                                this.Invoke(new MethodInvoker(delegate ()
                                {
                                    txtChat.AppendText(msg + "\r\n");
                                }));

                                Send채팅Packet(tmpPacket.id, tmpPacket.data);
                                break;
                            }
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void ServerStart()
        {
            try
            {
                this.m_listener = new TcpListener(IPAddress.Parse(this.IP), this.port);
                this.m_listener.Start();

                while(true)
                {
                    TcpClient client = this.m_listener.AcceptTcpClient();

                    if(nChild >= 10)
                    {
                        string errMsg = "연결된 Client의 수가 " + nChild.ToString() + "입니다. 연결이 더이상 불가능합니다";
                        MessageBox.Show(errMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }

                    if(client.Connected)
                    {
                        this.m_bClientOn = true;
                        this.m_Stream = client.GetStream();

                        // client 추가
                        nChild++;
                        clientList.Add(client);

                        // 쓰레드에게 인자 전달
                        Thread m_ThReader = new Thread(new ParameterizedThreadStart(Receive));
                        m_ThReader.Start(client);

                        // 초기화Packet 전송
                        Send초기화Packet(client);
                    }
                }
            }
            catch
            {
            }
        }

        //////////////////////////////////////////////////////////////////////////

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            int i = 0, j = 0;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            for (j = 0; j <= totalNum; j++)
            {
                // 점그리기
                for (i = 0; i <= nPen; i++)
                {
                    if (myPens[i].getSequence() == j)
                    {
                        pen.Width = myPens[i].getThick();
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
                            pen.Width = myLines[i].getThick();
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
                            pen.Width = myCircles[i].getThick();
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
                    if (myRects[i].getSequence() == j)
                    {
                        if (!myRects[i].getSolid())
                        {
                            pen.Width = 1;
                            pen.DashStyle = DashStyle.Dot;
                        }
                        else
                        {
                            pen.Width = myRects[i].getThick();
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

            // 모드
            this.penColor = Color.Black;
            this.brushColor = Color.Black;

            // 시작점, 끝점, 펜, 브러쉬
            this.prevPoint = new Point(0, 0);
            this.start = new Point(0, 0);
            this.finish = new Point(0, 0);
            this.pen = new Pen(Color.Black);
            this.brush = new SolidBrush(Color.Black);

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
