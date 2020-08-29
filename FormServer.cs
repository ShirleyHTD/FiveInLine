using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiveInLineServer
{
    public partial class IP : Form
    {
        public IP()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void FormServer_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Port_TextChanged(object sender, EventArgs e)
        {

        }

        void ShowMsg(string msg)

        {

            ChatWindow.AppendText(msg + "\r\n");
            
        }

        //记录通信用的Socket
        Dictionary<string, Socket> dic = new Dictionary<string, Socket>();
        Dictionary<string, string> coupledic = new Dictionary<string, string>();

        // private Socket client;

        void AcceptInfo(object o)

        {

            Socket socket = o as Socket;

            while (true)

            {

                //通信用socket

                try

                {

                    //创建通信用的Socket

                    Socket tSocket = socket.Accept();

                    string point = tSocket.RemoteEndPoint.ToString();

                    //IPEndPoint endPoint = (IPEndPoint)client.RemoteEndPoint;

                    //string me = Dns.GetHostName();//得到本机名称

                    //MessageBox.Show(me);

                    ShowMsg(point + "连接成功！");

                    Users.Items.Add(point);

                    foreach(KeyValuePair<string, Socket> kp in dic)
                    {
                        
                        if (!coupledic.ContainsKey(kp.Key))
                        {
                            sendMessageToOther(kp.Key, "User/A/" + point);
                            sendMessageToItself(tSocket, "User/A/" + kp.Key);
                        }
                        

                    }

                    dic.Add(point, tSocket);

                    //接收消息

                    Thread th = new Thread(ReceiveMsg);

                    th.IsBackground = true;

                    th.Start(tSocket);

                }

                catch (Exception ex)

                {

                    ShowMsg(ex.Message);

                    break;
                }
            }
        }


        private void sendMessageToOther(string ipKey, string message)
        {
            try {
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                dic[ipKey].Send(buffer);
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message);
            }
        }

        private void sendMessageToItself(Socket socket, string message)
        {
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(message);

                socket.Send(buffer);
            }
            catch (Exception ex)
            {

                ShowMsg(ex.Message);

            }
        }

        void readMessage(string msg)
        {
            if (msg.Contains('/'))
            {
                string[] splitresult = msg.Split('/');
                if (splitresult != null && splitresult.Length >= 3)
                {
                    if (splitresult[1] == "ChatRoom")//come from server
                    {
                        foreach (KeyValuePair<string, Socket> kp in dic)
                        {
                            if (splitresult[0] != kp.Key)
                            {
                                sendMessageToOther(kp.Key, splitresult[0] + "/ChatRoom/" + splitresult[2]);
                            }
                        }
                    }else if (splitresult[1] == "Invite")
                    {
                        coupledic.Add(splitresult[0], splitresult[2]);
                        coupledic.Add(splitresult[2], splitresult[0]);
                        foreach (KeyValuePair<string, Socket> kp in dic)
                        {
                            if (splitresult[2] == kp.Key)
                            {
                                sendMessageToOther(kp.Key, msg);
                            }else if(splitresult[0] != kp.Key)
                            {
                                sendMessageToOther(kp.Key, "Remove/" + splitresult[0]);
                                sendMessageToOther(kp.Key, "Remove/" + splitresult[2]);
                            }                            
                        }
                    }else if(splitresult[1] == "ChessAdded")
                    {
                        foreach (KeyValuePair<string, Socket> kp in dic)
                        {
                            if (splitresult[2] == kp.Key)
                            {
                                sendMessageToOther(kp.Key, msg);
                            }
                        }
                    }
                }
            }
        }

        void ReceiveMsg(object o)

        {

            Socket client = o as Socket;

            while (true)

            {

                //接收客户端发送过来的数据

                try

                {

                    //定义byte数组存放从客户端接收过来的数据

                    byte[] buffer = new byte[1024 * 1024];

                    //将接收过来的数据放到buffer中，并返回实际接受数据的长度

                    int n = client.Receive(buffer);

                    //将字节转换成字符串

                    string words = Encoding.UTF8.GetString(buffer, 0, n);


                    readMessage(words);
                    ShowMsg(client.RemoteEndPoint.ToString() + ":" + words);

                }

                catch (Exception ex)

                {

                    ShowMsg(ex.Message);

                    break;

                }

            }

        }

        private void StartServer_Click_1(object sender, EventArgs e)
        {
            // ip地址
            IPAddress ip = IPAddress.Parse(TextBoxIPAddress.Text);


            // IPAddress ip = IPAddress.Any;

            //端口号

            System.Net.IPEndPoint point = new IPEndPoint(ip, int.Parse(Port.Text));

            //创建监听用的Socket

            /*
  
             * AddressFamily.InterNetWork：使用 IP4地址。
  
              SocketType.Stream：支持可靠、双向、基于连接的字节流，而不重复数据。此类型的 Socket 与单个对方主机进行通信，并且在通信开始之前需要远程主机连接。Stream 使用传输控制协议 (Tcp) ProtocolType 和 InterNetworkAddressFamily。
  
              ProtocolType.Tcp：使用传输控制协议。
  
             */

            //使用IPv4地址，流式socket方式，tcp协议传递数据

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //创建好socket后，必须告诉socket绑定的IP地址和端口号。

            //让socket监听point

            try
            {

                //socket监听哪个端口

                socket.Bind(point);

                //同一个时间点过来10个客户端，排队

                socket.Listen(10);

                ShowMsg("服务器开始监听");

                System.Threading.Thread thread = new Thread(AcceptInfo);

                thread.IsBackground = true;

                thread.Start(socket);

            }

            catch (Exception ex)
            {

                ShowMsg(ex.Message);

            }
        }

        private void Send_Click_1(object sender, EventArgs e)
        {
            try

            {

                ShowMsg(SendMessageBox.Text);

                string ip = Users.Text;

                sendMessageToOther(ip, SendMessageBox.Text);

                //byte[] buffer = Encoding.UTF8.GetBytes(SendMessageBox.Text);

                //dic[ip].Send(buffer);

                // client.Send(buffer);

            }

            catch (Exception ex)

            {

                ShowMsg(ex.Message);

            }
        }

        private void ChatWindow_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
