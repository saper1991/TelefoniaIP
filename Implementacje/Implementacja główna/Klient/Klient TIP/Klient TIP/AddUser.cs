using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Commu;

namespace Klient_TIP
{
    public partial class AddUser : Form
    {
        public AddUser(string FileName)
        {
            InitializeComponent();

            if(!SocketOperations.ConnectStatus())
            {
                SocketOperations.ConnectSocket("192.168.1.2", 11326);
            }
        }


        private void OKButton_Click(object sender, EventArgs e)
        {
            string userNumber, userName;
            userNumber = UserNumberTbox.Text;
            userName = UserNameTbox.Text;

            if(int.Parse(userNumber) == null)
            {
                MessageBox.Show("Podany format liczbowy dla numeru jest nieprawidłowy!");
                return;
            }

            if(SocketOperations.addUser(userName, userNumber)) ////////
            {
                MessageBox.Show("Pomyślnie dodano nowego użytkownika!");
                saveConfig("Config.txt", userName, userNumber);
                this.Close();
            }
            else
            {
                MessageBox.Show("Nastąpił problem z dodaniem nowego użytkownika!");
                UserNumberTbox.Text = "";

            }
        }

        private void saveConfig(string fileName, string userName, string userNumber)
        {
            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(userName);
            sw.WriteLine(userNumber);
            sw.Close();
            fs.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if(SocketOperations.ConnectStatus())
            {
                SocketOperations.klient.Close();
            }
            Application.Exit();
        }
    }
}
