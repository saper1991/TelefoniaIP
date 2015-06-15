using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Commu;
using System.Threading;

namespace Klient_TIP
{
    public partial class MainWnd : Form
    {
        private Socket klient = null;                   //"wtyczka" klienta do serwera
        private string user, number;                    //lokalny użytkownik, lokalny numer
        private string chosedNumber = "";               //wybrany numer z klawiatury
        private bool ConnectWithCentralStatus = false;  //czy klient jest podłączony z centralą
        Thread serverThread = null;                     //wątek służący do nasłuchiwania połączeń przychodzących
        string[] requests = null;                       //przechowywanie składowych odebranych komunikatów
        string numberFrom, numberTo;                    //numer osoby nawiązyjącej połączenie przychodzące 

        bool isArrival = false;                         //flaga połączenia przychodzącego

        string remoteIP, remotePort;                    //dane dotyczące klienta z którym chcemu

        #region skladowe_klasy

        public MainWnd(string FileName)
        {
            initFromConfigFile();
            InitializeComponent();
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
        }
        private void MainWnd_Load(object sender, EventArgs e)
        {
            if (!SocketOperations.ConnectStatus())
            {
                SocketOperations.ConnectSocket("192.168.1.2", 11326);
            }

            display("Łączenie... ", Color.Green);
            if (SocketOperations.ConnectWithCentral(user, int.Parse(number)))
            {
                display("Połączono!\n", Color.Green);
                ConnectWithCentralStatus = true;

                serverThread = new Thread(callListen);
                serverThread.Start();
            }
            else
            {
                display("Błąd!\n", Color.Red);
            }
        }

        private void CloseApplicationButton_Click(object sender, EventArgs e)
        {
            if (SocketOperations.ConnectStatus())
            {
                display("\nRozłączanie...", Color.Green);
                if (ConnectWithCentralStatus)
                {
                    if (SocketOperations.releaseWithCentral(int.Parse(number)))
                    {
                        ConnectWithCentralStatus = false;
                        display("Gotowe", Color.Green);
                    }   
                }
                Thread.Sleep(1000);
            }

            SocketOperations.socketDisconnect();
            Application.Exit();
        }

        #endregion

        #region klawisze_cyfer
        private void oneKeyButton_Click(object sender, EventArgs e)
        {
            chosedNumber += "1";
            display("1", Color.Blue);
        }

        private void twoKeyButton_Click(object sender, EventArgs e)
        {
            chosedNumber += "2";
            display("2", Color.Blue);
        }

        private void threeKeyButton_Click(object sender, EventArgs e)
        {
            chosedNumber += "3";
            display("3", Color.Blue);
        }

        private void fourKeyButton_Click(object sender, EventArgs e)
        {
            chosedNumber += "4";
            display("4", Color.Blue);
        }

        private void fiveKeyButton_Click(object sender, EventArgs e)
        {
            chosedNumber += "5";
            display("5", Color.Blue);
        }

        private void sixKeyButton_Click(object sender, EventArgs e)
        {
            chosedNumber += "6";
            display("6", Color.Blue);
        }

        private void sevenKeyButton_Click(object sender, EventArgs e)
        {
            chosedNumber += "7";
            display("7", Color.Blue);
        }

        private void eightKeyButton_Click(object sender, EventArgs e)
        {
            chosedNumber += "8";
            display("8", Color.Blue);
        }

        private void nineKeyButton_Click(object sender, EventArgs e)
        {
            chosedNumber += "9";
            display("9", Color.Blue);
        }

        private void zeroKeyButton_Click(object sender, EventArgs e)
        {
            chosedNumber += "0";
            display("0", Color.Blue);
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            if(chosedNumber.Length != 0)
            {
                chosedNumber = chosedNumber.Remove(chosedNumber.Length - 1);
                InsertNumberTBox.Text = InsertNumberTBox.Text.Remove(InsertNumberTBox.Text.Length - 1);
            }
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            if(ConnectButton.Text.Equals("Połącz"))
            {
                if (chosedNumber != number)
                {
                    display("\nŁączenie z " + chosedNumber + "... \n", Color.Green);
                }
                else
                {
                    display("\nNie można wykonać połączenia do siebie!", Color.Red);
                    chosedNumber = "";
                    return;
                }

                int resultCode = SocketOperations.sendInvite(number, chosedNumber);

                if (resultCode == 1) //NEX
                {
                    display("Nie ma takiego numeru!\n", Color.Red);
                }
                else if(resultCode == 2) // BUSY
                {
                    display("Zajęty!\n", Color.Red);
                }
                else if(resultCode == 3) //TRANSFER
                {
                    display("Czekaj na odbiór połączenia...\n", Color.Green);
                    string receiveMsg = SocketConverter.ReceiveText(SocketOperations.klient);

                    if(receiveMsg.Contains("OK"))
                    {
                        ConnectButton.Text = "Rozłącz";
                        SecureConnectButton.Text = "Połącz Bezpiecznie";

                        String[] requests = receiveMsg.Split(new char[] { ' ' });
                        remoteIP = requests[3];
                        remotePort = requests[4];

                        // kod dla polaczenia glosowego!

                        display("\nPołączono!\n", Color.Green);
                    }
                    else if(receiveMsg.Contains("BUSY"))
                    {
                        display("\nOdbiorca odrzucił połączenie!\n", Color.Red);
                    }

                }
                else if (resultCode == 4) //NAC
                {
                    display("Niedostępny!\n", Color.Red);
                }

                
                //SocketConverter.SendText(Handler, "OK");
            }
            else if(ConnectButton.Text.Equals("Odbierz"))
            {
                requests[0] = "OK";
                requests[3] = SocketOperations.ownIP;
                requests[4] = SocketOperations.port.ToString();
                
                SocketConverter.SendText(Handler, String.Join(" ", requests));
            }
            else if(ConnectButton.Text.Equals("Rozłącz"))
            {
                if(isArrival)
                {
                    SocketOperations.sendBye(Handler, number, numberFrom);
                }
                else
                {
                    SocketOperations.sendBye(klient, number, numberFrom);
                }

                display("\nRozłączono z " + numberFrom + "!\n", Color.Green);
            }

            chosedNumber = "";
        }
        #endregion

        #region klasy_pomocnicze
        private void initFromConfigFile()
        {
            if (!File.Exists("Config.txt")) { return; }

            StreamReader sr = new StreamReader("Config.txt");
            user = sr.ReadLine();
            number = sr.ReadLine();
            sr.Close();


            this.Text = user + ":" + number;
        }

        private void display(string text, Color color)
        {
            InsertNumberTBox.ForeColor = color;
            InsertNumberTBox.Text += (text);
        }
        #endregion

        #region procedury_serwera

        private Socket Server = null;
        private Socket Handler = null;
        private int serverPort = 0;

        private void callListen()
        {
            byte[] address = { 127, 0, 0, 1 };
            IPAddress serverAddress = new IPAddress(address);
            TcpListener listener = new TcpListener(serverAddress, SocketOperations.port + 1);

            try
            {
                listener.Start();
                while(true)
                {
                    Handler = listener.Server.Accept();

                    try
                    {
                        serviceRequest();
                    }
                    catch(Exception ex)
                    {
                        Handler.Close();
                        continue;
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }

        private void serviceRequest()
        {
            string recText;
            

            while(true)
            {
                recText = SocketConverter.ReceiveText(Handler);
                requests = recText.Split(new char[] {' '});

                numberFrom = requests[1];
                numberTo = requests[2];
                string IPFrom = requests[3];
                string port = requests[4];

                if(requests[0].Equals("INVITE"))
                {
                    isArrival = true;

                    if(ConnectButton.InvokeRequired)
                    {
                        ConnectButton.BeginInvoke(new Action(() =>
                            {
                                ConnectButton.Text = "Odbierz";
                            }));
                    }

                    if(SecureConnectButton.InvokeRequired)
                    {
                        SecureConnectButton.BeginInvoke(new Action(() =>
                        {
                            SecureConnectButton.Text = "Odrzuć";
                        }));
                    }

                    if(InsertNumberTBox.InvokeRequired)
                    {
                        InsertNumberTBox.BeginInvoke(new Action(() =>
                            {
                                display("Połączenie przychodzące: " + numberFrom, Color.Green);
                            }));
                    }
                }
            }
        }
        #endregion

        private void InsertNumberTBox_DoubleClick(object sender, EventArgs e)
        {
            InsertNumberTBox.Clear();
            chosedNumber = "";
        }
    }
}
