using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace prodolzhenie_leonova_shekh_polyakova320
{
    internal class Client
    {
        private async void SendMessage(string message)
        {
            using (TcpClient client = new TcpClient("80.80.80.140", 8888))
            {
                NetworkStream stream = client.GetStream();
                byte[] data = Encoding.UTF8.GetBytes(message);
                await stream.WriteAsync(data, 0, data.Length);
            }
        }

    }
}
