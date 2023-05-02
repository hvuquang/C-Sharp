using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;

namespace Server
{
    public partial class Server : Form
    {

        IPEndPoint IP;
        Socket server;
        //List<Socket> clientList
        List<Socket> clientList;

        public Server()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;
            //ServicePointManager.DefaultConnectionLimit = 25;

            Connect();
        }

        private void Server_FormClosed(object sender, FormClosedEventArgs e)
        {
            Close();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            foreach (Socket s in clientList)
            {
                Send(s);
            }
            if (txbMessage.Text != String.Empty)
            {
                AddMessage(txbMessage.Text, 1);
                txbMessage.Clear();
            }
            else
            {
                MessageBox.Show("Điền thông tin trước khi gửi", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        void Connect()
        {
            clientList = new List<Socket>();
            // ip: địa chỉ server
            IP = new IPEndPoint(IPAddress.Any, 4000);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            //server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 5000); // set the receive timeout to 5 seconds
            //server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 5000); // set the send timeout to 5 seconds
            //server.ReceiveTimeout = 10000;


            //tạo chat, đợi IP
            server.Bind(IP);
            //tạo thread
            Thread Listen = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        server.Listen(100);
                        Socket client = server.Accept();
                        clientList.Add(client);

                        Thread receive = new Thread(Receive);
                        receive.IsBackground = true;
                        receive.Start(client);
                    }
                }
                catch
                {
                    IP = new IPEndPoint(IPAddress.Any, 9999);
                    server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                }
            });
            Listen.IsBackground = true;
            Listen.Start();
        }
        void Close()
        {
            server.Close();
        }
        void Send(Socket client)
        {
            if (txbMessage.Text != string.Empty)
            {
                client.Send(Serialize(txbMessage.Text));
            }
        }

        //đưa vào dạng object
        void Receive(object obj)
        {
            Socket client = obj as Socket;
            //nhận tin liên tục -> while
            try
            {
                while (true)
                {
                    //biến nhận data được sent
                    byte[] data = new byte[1024 * 5000];
                    client.Receive(data);

                    string message = (string)Deserialize(data);

                    AddMessage(message, 2);
                }
            }
            catch
            {
                clientList.Remove(client);
                client.Close();
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

        byte[] Serialize(object obj)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();

            //phân mãnh obj theo kiểu binary ghi vô stream
            formatter.Serialize(stream, obj);

            //trả 1 dãy byte
            return stream.ToArray();
        }
        object Deserialize(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            BinaryFormatter formatter = new BinaryFormatter();

            //gom mãnh obj theo kiểu binary ghi vô stream
            return formatter.Deserialize(stream);
        }

        //đóng kết nối khi đóng form
    }
}