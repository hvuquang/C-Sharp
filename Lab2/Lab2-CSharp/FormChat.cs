using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace Lab2_CSharp
{
    public partial class FormChat : Form
    {
        public FormChat()
        { 
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;
            //ServicePointManager.DefaultConnectionLimit = 25;
            Connect();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Send();
            if (txbMessage.Text != String.Empty)
            {
                AddMessage(txbMessage.Text, 2);
                txbMessage.Clear();
            }
            else
            {
                MessageBox.Show("Điền thông tin trước khi gửi", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        IPEndPoint IP;
        Socket client;

        void Connect() {
            // ip: địa chỉ server
            IP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4000);
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            //client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 5000); // set the receive timeout to 5 seconds
            //client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 5000); // set the send timeout to 5 seconds
            //client.ReceiveTimeout = 10000;


            //kêt nối với server
            try
            {
                client.Connect(IP);
            }
            catch
            {
                MessageBox.Show("Không thể kết nối server!", "Lỗi", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

            Thread listen = new Thread(Receive);
            listen.IsBackground = true;
            listen.Start();
        }
        void Close() { 
            client.Close();
        }
        void Send() {
            if (txbMessage.Text != string.Empty) client.Send(Serialize(txbMessage.Text));
        }
        void Receive() {
            //nhận tin liên tục -> while
            try
            {
                while (true)
                {
                    //biến nhận data được sent
                    byte[] data = new byte[1024 * 5000];
                    client.Receive(data);

                    string message = (string)Deserialize(data);

                    AddMessage(message, 1);
                } 
            }
            catch(Exception e) 
            {
                MessageBox.Show(e.Message, "Lỗi từ client");
                Close();
            }

        }

        // thêm message vào khung chat

        void AddMessage(string s, int id)
        {
            DateTime currentDateTime = DateTime.Now;
            if (id == 1)
            {
                lsvMessage.Items.Add(new ListViewItem() { Text = "[ " + currentDateTime + " ]" + " Server: " + s });
            }
            else if (id == 2)
            {
                lsvMessage.Items.Add(new ListViewItem() { Text = "[ " + currentDateTime + " ]" + " Client: " + s });
            }
        }

        byte[] Serialize(object obj) {  
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();

            //phân mãnh obj theo kiểu binary ghi vô stream
            formatter.Serialize(stream, obj);

            //trả 1 dãy byte
           return stream.ToArray(); 
        }
        object Deserialize(byte[] data) {
            MemoryStream stream = new MemoryStream(data);
            BinaryFormatter formatter = new BinaryFormatter();

            //gom mãnh obj theo kiểu binary ghi vô stream
            return formatter.Deserialize(stream);
        }

        //đóng kết nối khi đóng form
        private void FormChat_FormClosed(object sender, FormClosedEventArgs e)
        {
            Close();
        }
    }
}
