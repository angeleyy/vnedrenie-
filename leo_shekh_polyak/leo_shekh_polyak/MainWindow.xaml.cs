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
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace leo_shekh_polyak
{
    public partial class MainWindow : Window
    {
        private TcpListener server;
        private Thread listenThread;

        private Client client;

        public MainWindow()
        {
            InitializeComponent();

            sendButton.Click += SendButton_Click;
            messageTextBox.KeyDown += MessageTextBox_KeyDown;

            StartListening();

            client = new Client();
            client.MessageReceived += Client_MessageReceived;
        }

        private void Client_MessageReceived(string message)
        {
            Dispatcher.Invoke(() =>
            {
                chatHistoryListBox.Items.Add("Server: " + message);
            });
        }

        private void StartListening()
        {
            server = new TcpListener(IPAddress.Any, 8888);
            server.Start();

            listenThread = new Thread(new ThreadStart(ListenForClients));
            listenThread.Start();
        }

        private void ListenForClients()
        {
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientCommunication));
                clientThread.Start(client);
            }
        }

        private void HandleClientCommunication(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            StreamWriter writer = new StreamWriter(tcpClient.GetStream())
            { AutoFlush = true };
            StreamReader reader = new StreamReader(tcpClient.GetStream());

            string clientMessage;
            while ((clientMessage = reader.ReadLine()) != null)
            {
                Dispatcher.Invoke(() =>
                {
                    chatHistoryListBox.Items.Add("Client: " + clientMessage);
                });
            }


            tcpClient.Close();
        }
        private void SendMessage(string message)
        {
            chatHistoryListBox.Items.Add("Client: " + message);
            client.SendMessage(message);
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string message = messageTextBox.Text;
            SendMessage(message);
            messageTextBox.Clear();
        }
        private void MessageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendButton_Click(sender, e);
            }
        }

        private void messageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void sendButton_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void chatHistoryListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
