using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestAudio.Audio;

namespace TestAudio
{
    class Program
    {
        static void Main(string[] args)
        {
            BassAudio bass = new BassAudio();
            while (true)
            {
                Console.WriteLine("Tryby pracy(1-3):");
                switch (Console.ReadLine())
                {
                    case "1":
                        {
                            bass.Test();
                            break;
                        }
                    case "2":
                        break;
                    case "3":
                        break;
                    default:
                        break;
                }
            }
            
        }
    }
}
