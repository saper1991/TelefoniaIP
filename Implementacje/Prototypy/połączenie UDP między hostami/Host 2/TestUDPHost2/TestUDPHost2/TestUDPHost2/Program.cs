using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TestUDPHost2
{
    class Program
    {
        static void Main(string[] args)
        {
            UdpClient host = new UdpClient(44444);
            try
            {
                Console.WriteLine("Rozpoczynam odbieranie danych...");
                IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 0);
                byte[] odebrane = host.Receive(ref ipep);

                for(int i = 0 ; i < odebrane.Length ; i++)
                {
                    Console.Write(odebrane[i] + " ");
                }
                Console.ReadKey();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}

//Odbieranie pakietów UDP
