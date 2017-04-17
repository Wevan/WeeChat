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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private UdpClient udpReceive;
        private UdpClient udpSend;
        private Thread thread;
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            udpReceive = new UdpClient(8888);
            thread = new Thread(ReceiveMessage);
            thread.IsBackground = true;
            thread.Start();

        }


        private void ReceiveMessage()
        {
            IPEndPoint remoteIEP = new IPEndPoint(IPAddress.Any,0);
            while (true)
            {
                if (udpReceive.Available == 0)
                {
                    continue;
                }
                byte[] buffer = udpReceive.Receive(ref remoteIEP);
                string words = Encoding.UTF8.GetString(buffer);
                showMsg(remoteIEP.ToString() + ":" + words);
            }
        }

        private void showMsg(string a)
        {

            txtAllMsg.AppendText(a + "\t\n");
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            showMsg("自己说："+txtSendMsg.Text);
            udpSend = new UdpClient();
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse(cmbIP.Text),8888);
            //把要传输的字符串转化成字节流，用容器盛放
            byte[] buffer = Encoding.UTF8.GetBytes(txtSendMsg.Text);
            int retry = 0;
            while (true)
            {
                try
                {
                    udpSend.Send(buffer, buffer.Length, iep);
                    break;
                }
                catch
                {
                    if (retry < 3)
                    {
                        continue;
                    }
                    else
                    {
                        showMsg(txtSendMsg.Text + "发送失败");
                        break;
                      
                    }
                }
            }
            txtSendMsg.Text = "";
            txtSendMsg.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            
            cmbIP.Items.Add(txtIP.Text);
            MessageBox.Show(txtIP.Text + "成功添加");
           
        }
    }
}
