using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Sockets;
using System.Net;

namespace prodolzhenie_leonova_shekh_polyakova320
{
    public partial class MainWindow : Window
    {
        private TcpListener _server;
        public MainWindow()
        {
            InitializeComponent();
            StartServer();
        }
        private async void StartServer()
        {
            _server = new TcpListener(IPAddress.Any, 8888);
            _server.Start();
            while (true)
            {
                var client = await _server.AcceptTcpClientAsync();
                HandleClient(client);
            }
        }

        private async void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Received: " + message);

            client.Close();
        }
    }
}