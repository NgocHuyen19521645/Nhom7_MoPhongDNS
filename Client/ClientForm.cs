using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
    public partial class ClientForm : Form
    {
        public ClientForm()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            Connect();
        }
        IPEndPoint iPEndPoint;
        TcpClient tcpClient;
        Stream stream;

        //Khởi động kết nối giữa Client và Server
        void Connect()
        {
            iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
            tcpClient = new TcpClient();
            tcpClient.Connect(iPEndPoint);
            stream = tcpClient.GetStream();
        }

        // Hàm gửi dữ liệu từ Client
        void Send()
        {
            if (tbDNS.Text != string.Empty)
            {
                byte[] data = Encoding.UTF8.GetBytes(tbDNS.Text);
                stream.Write(data, 0, data.Length);

            }
            else
                MessageBox.Show("Nhập lại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        void Addmessage(string mess)
        {
            this.tbResult.Text = mess;
            this.tbResult.Enabled = false;

        }
        void Receive()
        {
            try
            {
                while (true)
                {
                    byte[] rev = new byte[1024];
                    stream.Read(rev, 0, rev.Length);
                    string s = Encoding.UTF8.GetString(rev);
                    if(s == "Not Found!")
                    {
                        MessageBox.Show("Not Found!", "NOTIFICATION", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        Addmessage(s); //nhận dữ liệu từ server đưa lên tb
                    }
                    
                }
            }
            catch
            {
                Close();
            }

        }

        void Close()
        {
            stream.Close();
        }


        private void button1_Click(object sender, EventArgs e)
        {

            Send();
            Thread rev = new Thread(Receive);
            rev.IsBackground = true;
            rev.Start();
        }

        private void ClientForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Close();
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {

        }


    }
}
