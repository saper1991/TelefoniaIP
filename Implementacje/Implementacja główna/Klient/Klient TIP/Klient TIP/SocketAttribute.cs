using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Commu;

namespace Klient_TIP
{
    public class SocketOperations
    {
        public static Socket klient = null;
        public static int port = 0;
        public static string ownIP;

        public static void ConnectSocket(string server, int port)
        {
            Socket s = null;
            IPHostEntry hostEntry = null;

            hostEntry = Dns.GetHostEntry(server);

            foreach (IPAddress address in hostEntry.AddressList)
            {
                if(address.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    continue;
                }

                IPEndPoint ipe = new IPEndPoint(address, port);
                Socket tempSocket =
                    new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    tempSocket.Connect(ipe); 
                }
                catch(SocketException ex)
                {
                    continue;
                }

                if (tempSocket.Connected)
                {
                    s = tempSocket;
                    break;
                }
                else
                {
                    continue;
                }
            }
            klient = s;
        }

        public static bool addUser(string userName, string userNumber)
        {
            ConnectSocket("localhost", 11326);
            string command = "ADD " + userName + " " + userNumber;
            SocketConverter.SendText(klient, command);
            string receiveText = SocketConverter.ReceiveText(klient);
            string[] recData = receiveText.Split(new char[] { ' ' });

            if (recData[0].Equals("OK"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ConnectStatus()
        {
            if(klient != null)
            {
                if(klient.Connected)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool ConnectWithCentral(string userName, int userNumber)
        {
            if (ConnectStatus())
            {
                ownIP = (klient.LocalEndPoint as IPEndPoint).Address.ToString();
                port = (klient.LocalEndPoint as IPEndPoint).Port;
                string commandToSend = "HELLO " + userName + " " + userNumber + " " + ownIP + " " + port;
                SocketConverter.SendText(klient, commandToSend);
                string receivedCommand = SocketConverter.ReceiveText(klient);
                string[] subcommands = receivedCommand.Split(new char[] { ' ' });

                if(subcommands[0].Equals("OK"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool releaseWithCentral(int userNumber)
        {
            string cmd = "EXIT " + userNumber;
            SocketConverter.SendText(klient, cmd);
            string result = SocketConverter.ReceiveText(klient);
            if(result.Equals("OK"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void socketDisconnect()
        {
            if(klient != null)
            {
                klient.Close();
            }
        }

        public static int sendInvite(string fromNumber, string toNumber)
        {
            if(ConnectStatus())
            {
                string cmd = "INVITE " + fromNumber + " " + toNumber + " " + ownIP + " " + port;
                SocketConverter.SendText(klient, cmd);
                string response = SocketConverter.ReceiveText(klient);

                if(response.Contains("NEX"))
                {
                    return 1;
                }
                else if(response.Contains("BUSY"))
                {
                    return 2;
                }
                else if(response.Contains("TRANSFER"))
                {
                    return 3;
                }
                else if(response.Contains("NAC"))
                {
                    return 4;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public static void sendBye(Socket currentSocket, string numberFrom, string numberTo)
        {
            string cmd = "BYE " + numberFrom + " " + numberTo;
            SocketConverter.SendText(currentSocket, cmd);
        }
    }
}
