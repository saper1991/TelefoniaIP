using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mp3_to_WaV_Converter
{
    public partial class MainWnd : Form
    {
        public MainWnd()
        {
            InitializeComponent();
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
        }

        private void ConvertButton_Click(object sender, EventArgs e)
        {
            string inFile = null, outFile = null;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Plik MP3 (*.mp3) | *.mp3";

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                inFile = ofd.FileName;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Plik WAV (.wav)| *.wav";

            if(sfd.ShowDialog() == DialogResult.OK)
            {
                outFile = sfd.FileName;
            }

            if(inFile != null && outFile != null)
            {
                using (Mp3FileReader mfr = new Mp3FileReader(inFile))
                {
                    using (WaveStream pcm = WaveFormatConversionStream.CreatePcmStream(mfr))
                    {
                        WaveFileWriter.CreateWaveFile(outFile, pcm);
                    }
                }
                MessageBox.Show("Plik został przekonwertowany!");
            }
            else
            {
                MessageBox.Show("Nie wybrano nazw dla plików");
            }
        }
    }
}
