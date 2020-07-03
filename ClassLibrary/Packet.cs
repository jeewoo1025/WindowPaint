using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ClassLibrary
{
    public enum PacketType
    {
        그림 = 0,
        채팅
    }

    public enum PacketSendERROR
    {
        정상 = 0,
        에러
    }

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

    [Serializable]
    public class Packet
    {
        public long Length;
        public int Type;

        public Packet()
        {
            this.Length = 0;
            this.Type = 0;
        }

        public static byte[] Serialize(Object o)
        {
            // 객체 --> Stream
            MemoryStream ms = new MemoryStream(1024 * 6);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, o);

            return ms.ToArray();
        }

        public static Object Deserialize(byte[] bt)
        {
            // Stream --> 객체
            MemoryStream ms = new MemoryStream(1024 * 6);
            foreach (byte b in bt)
                ms.WriteByte(b);

            ms.Position = 0;
            BinaryFormatter bf = new BinaryFormatter();
            Object obj = bf.Deserialize(ms);
            ms.Close();

            return obj;
        }
    }

    [Serializable]
    public class 그림Packet : Packet
    {
        public int mode;
        public MyLines line;
        public MyCircle circle;
        public MyRectangle rect;

        public 그림Packet()
        {
            this.Type = (int)PacketType.그림;

            this.mode = (int)DRAW_MODE.NONE;
            this.line = new MyLines();
            this.circle = new MyCircle();
            this.rect = new MyRectangle();
        }
    }

    [Serializable]
    public class 채팅Packet : Packet
    {
        public string id;
        public string data;

        public 채팅Packet()
        {
            this.Type = (int)PacketType.채팅;

            this.id = "";
            this.data = "";
        }
    }
}
