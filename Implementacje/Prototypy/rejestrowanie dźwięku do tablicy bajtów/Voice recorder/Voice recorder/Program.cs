using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Voice_recorder
{
    class Program
    {
        static void Main(string[] args)
        {
            Microphone mic = Microphone.Default;
            if(mic == null)
            {
                Console.WriteLine("Brak domyślnego mikrofonu");
                Console.ReadKey();
                return;
            }
            else
            {
                    FrameworkDispatcher.Update();

                    Console.ReadKey();
            }
        }
    }
}
