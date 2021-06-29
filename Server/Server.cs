using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace DNS_Simulation
{
    public partial class Server : Form
    {
        public Server()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            Connect();
        }

        private void Server_Load(object sender, EventArgs e)
        {
          //  Connect();
        }
        IPEndPoint iPEndPoint;
        Socket server;
        TcpListener tcpListener;
        Stream stream;
        List<Socket> clientsockets; // danh sách cách client đã kết nối
        void Connect()
        {

            clientsockets = new List<Socket>();
            iPEndPoint = new IPEndPoint(IPAddress.Any, 8080);
            tcpListener = new TcpListener(iPEndPoint);
            Thread thread = null;
            thread = new Thread(() =>
            {
                try
                {
                    while (true)
                    {

                        tcpListener.Start();
                        Socket client = tcpListener.AcceptSocket();
                        clientsockets.Add(client);
                        Thread rec = new Thread(Receive);
                        rec.IsBackground = true;
                        rec.Start(client);
                    }
                }
                catch
                {

                }
            });
            thread.IsBackground = true;
            thread.Start();
        }
        public bool Found(string dnsName)
        {

            XmlDocument root = new XmlDocument();
            root.Load("XMLFile1.xml");
            XmlNode node = root.SelectSingleNode("DNS/row/var[@name='" + dnsName + "']");               
            try
            {             
                if (node.Attributes["value"].Value != null)
                {
                    string value = node.Attributes["value"].Value; ///NOT-FOUND // CRASHED // NHO FIX NHA NGOC HUYEN
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public string findIP(string dnsName)
        {
            
            XmlDocument root = new XmlDocument();
            root.Load("XMLFile1.xml");
            XmlNode node = root.SelectSingleNode("DNS/row/var[@name='"+ dnsName +"']");
            string value = node.Attributes["value"].Value;
            return value;
        }

        void Send(Socket client, byte[] data)
        {         
            
            stream.Write(data, 0, data.Length);

        }
        
        void Receive(Object obj)
        {
            try
            {
                while (true)
                {
                    Socket client = obj as Socket;
                    byte[] rev = new byte[1024];
                    client.Receive(rev);
                    string s = Encoding.UTF8.GetString(rev);
                    s = s.Replace("\0", string.Empty);
                    if (Found(s))
                    {
                        rev = Encoding.UTF8.GetBytes(findIP(s));                        
                    }
                    else
                    {
                        string notFound = "Not Found!";
                        rev = Encoding.UTF8.GetBytes(notFound);
                    }
                    client.Send(rev);
                }
            }
            catch
            {
                MessageBox.Show("Ngắt kết nối", "Disconnected", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {

            Thread serverThread = new Thread(Connect);
            serverThread.IsBackground = true;
            serverThread.Start();
            
        }

        private void Server_FormClosed(object sender, FormClosedEventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Login loginForm = new Login();
            loginForm.Show();
        }
    }
}
