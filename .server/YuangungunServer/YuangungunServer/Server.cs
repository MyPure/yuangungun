using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Data;

namespace YuangungunServer
{
    class Serv
    {
        public Socket listenfd;
        public Conn[] conns;
        public int maxConn = 50;

        public int NewIndex()
        {
            if (conns == null)
            {
                return -1;
            }
            for (int i = 0; i < conns.Length; i++)
            {
                if (conns[i] == null)
                {
                    conns[i] = new Conn();
                    return i;
                }
                else if (conns[i].isUse == false)
                {
                    return i;
                }
            }
            return -1;
        }

        public void Start(string host, int port)
        {
            //数据库
            //string connStr = "Database=msgboard;Data Source=127.0.0.1;";
            //connStr += "User Id=root;Password=Aa000309;port=3306";
            //sqlConn = new MySqlConnection(connStr);
            //try
            //{
            //    sqlConn.Open();
            //}
            //catch(Exception e)
            //{
            //    Console.WriteLine("数据库连接失败: "+e.Message);
            //    return;
            //}

            conns = new Conn[maxConn];
            for (int i = 0; i < maxConn; i++)
            {
                conns[i] = new Conn();
            }
            //Socket
            listenfd = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //BInd
            IPAddress ipAdr = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEp = new IPEndPoint(ipAdr, 1234);
            listenfd.Bind(ipEp);
            //Listen
            listenfd.Listen(maxConn);
            //Accept
            listenfd.BeginAccept(AcceptCb, null);
            Console.WriteLine("[服务器]启动成功");
        }

        private void AcceptCb(IAsyncResult ar)
        {
            try
            {
                Socket socket = listenfd.EndAccept(ar);
                int index = NewIndex();

                if (index < 0)
                {
                    socket.Close();
                    Console.WriteLine("连接已满");
                }
                else
                {
                    Conn conn = conns[index];
                    conn.Init(socket);
                    string adr = conn.GetAdress();
                    Console.WriteLine("客户端 [" + adr + "] 连接 conn池ID: " + index);
                    conn.socket.BeginReceive(conn.readBuff, conn.buffCount, conn.BuffRemain(), SocketFlags.None, ReceiveCb, conn);
                    listenfd.BeginAccept(AcceptCb, null);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("AcceptCb失败:" + e.Message);
            }
        }

        private void ReceiveCb(IAsyncResult ar)
        {
            Conn conn = (Conn)ar.AsyncState;
            lock (conn)
            {
                try
                {
                    int count = conn.socket.EndReceive(ar);
                    Console.WriteLine(count);
                    if (count <= 0)
                    {
                        Console.WriteLine("收到 [" + conn.GetAdress() + "] 断开连接");
                        conn.Close();
                        return;
                    }
                    conn.buffCount += count;
                    ProcessData(conn);

                    conn.socket.BeginReceive(conn.readBuff, conn.buffCount, conn.BuffRemain(), SocketFlags.None, ReceiveCb, conn);
                }
                catch (Exception e)
                {
                    Console.WriteLine("[" + conn.GetAdress() + "] " + e.Message);
                    conn.Close();
                }
            }
        }

        private void ProcessData(Conn conn)
        {
            if(conn.buffCount < sizeof(int))
            {
                return;
            }
            Array.Copy(conn.readBuff, conn.lenBytes,sizeof(int));
            conn.msgLength = BitConverter.ToInt32(conn.lenBytes, 0);
            if(conn.buffCount < conn.msgLength + sizeof(int))
            {
                return;
            }
            string str = Encoding.UTF8.GetString(conn.readBuff, sizeof(int), conn.msgLength);
            Console.WriteLine("收到消息 [" + conn.GetAdress() + "] " + str);
            Send(conn, str);

            int count = conn.buffCount - conn.msgLength - sizeof(int);
            Array.Copy(conn.readBuff, sizeof(int) + conn.msgLength, conn.readBuff, 0, count);
            conn.buffCount = count;
            if(conn.buffCount > 0)
            {
                ProcessData(conn);
            }
        }

        public void Send(Conn conn, string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            byte[] length = BitConverter.GetBytes(bytes.Length);
            byte[] sendbuff = length.Concat(bytes).ToArray();
            try
            {
                for (int i = 0; i < conns.Length; i++)
                {
                    if (conns[i] == null || conns[i].isUse == false)
                    {
                        continue;
                    }
                    Console.WriteLine("转播消息给 " + conns[i].GetAdress());
                    conns[i].socket.Send(sendbuff, 0, sendbuff.Length, SocketFlags.None);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void HandleMsg(Conn conn, string str)
        {
            #region MySql
            //if(str == "_GET")
            //{
            //    string cmdStr = "select * from msg order by id desc limit 10;";
            //    MySqlCommand cmd = new MySqlCommand(cmdStr, sqlConn);
            //    try
            //    {
            //        MySqlDataReader dataReader = cmd.ExecuteReader();
            //        str = "";
            //        while (dataReader.Read())
            //        {
            //            str += dataReader["name"] + ":" + dataReader["msg"] + "\n\r";
            //        }
            //        dataReader.Close();
            //        byte[] bytes = Encoding.Default.GetBytes(str);
            //        conn.socket.Send(bytes);
            //    }
            //    catch(Exception e)
            //    {
            //        Console.WriteLine("数据库查询失败: " + e.Message);
            //    }
            //}
            //else
            //{
            //    string cmdStrFormat = "insert into msg set name = '{0}' ,msg = '{1}';";
            //    string cmdStr = string.Format(cmdStrFormat, conn.GetAdress(), str);
            //    MySqlCommand cmd = new MySqlCommand(cmdStr, sqlConn);
            //    try
            //    {
            //        cmd.ExecuteNonQuery();
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine("数据库插入失败: " + e.Message);
            //    }
            //}
            #endregion
            byte[] bytes = Encoding.Default.GetBytes(str);
            for (int i = 0; i < conns.Length; i++)
            {
                if (conns[i] == null || conns[i].isUse == false)
                {
                    continue;
                }
                Console.WriteLine("转播消息给 " + conns[i].GetAdress());
                conns[i].socket.Send(bytes);
            }
        }

        public void Close()
        {
            for(int i = 0;i < conns.Length; i++)
            {
                Conn conn = conns[i];
                if (conn == null) continue;
                if (!conn.isUse) continue;
                lock (conn)
                {
                    conn.Close();
                }
            }
        }
    }
}