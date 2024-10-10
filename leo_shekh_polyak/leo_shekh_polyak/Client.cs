using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows;

namespace leo_shekh_polyak
{
    public class Client
    {
        private TcpClient client;
        private Thread receiveThread;

        public event Action<string> MessageReceived;

        public void Start(string ipAddress, int port)
        {
            client = new TcpClient();
            client.Connect(ipAddress, port);

            receiveThread = new Thread(new ThreadStart(ReceiveMessages));
            receiveThread.Start();
        }

        private void ReceiveMessages()
        {
            StreamReader reader = new StreamReader(client.GetStream());

            string serverMessage;
            while ((serverMessage = reader.ReadLine()) != null)
            {
                MessageReceived?.Invoke(serverMessage);
            }

            client.Close();
        }

        public void SendMessage(string message)
        {
            StreamWriter writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
            writer.WriteLine(message);
        }
    }
}
