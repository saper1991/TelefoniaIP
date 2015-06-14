using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Klient_TIP
{
    static class Program
    {

        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            if(!File.Exists("Config.txt"))
            {
                Application.Run(new AddUser("Config.txt"));
            }

            Application.Run(new MainWnd("Config.txt"));

        }

        static bool isEmpty(string fileName)
        {
            if(File.Open(fileName, FileMode.Open).Length == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
