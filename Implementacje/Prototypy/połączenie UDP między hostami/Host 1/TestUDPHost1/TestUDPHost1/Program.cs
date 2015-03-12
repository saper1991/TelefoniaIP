using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TestUDPHost1
{
    class Program
    {
        static void Main(string[] args)
        {
            UdpClient host = new UdpClient(55555);

            try
            {
                byte[] bufor = new byte[256];
                for(int i = 0 ; i < bufor.Length ; i++)
                {
                    bufor[i] = (byte)i;
                }
                host.Connect("127.0.0.1", 44444);
                Console.WriteLine("Naciśnij przycisk by rozpocząć transmisję...");
                Console.ReadKey();
                host.Send(bufor, bufor.Length);
                host.Close();
                Console.WriteLine("Dane zostały przesłane!");
                Console.ReadKey();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
//wysyłanie pakieów UDP