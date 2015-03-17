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

namespace WavePlaying
{
    public partial class MainWnd : Form
    {
        private WaveFileReader wfr = null;
        private DirectSoundOut dso = null;

        public MainWnd()
        {
            InitializeComponent();

            //ustawienia statycznego rozmmiaru ekranu
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
        }

        private void MainWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisposeProgram();
        }

        private void LoadFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Plik .wav (*.wav) | *.wav";

            DisposeProgram();

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                wfr = new WaveFileReader(ofd.FileName);
                dso = new DirectSoundOut();

                dso.Init(new WaveChannel32(wfr));
                dso.Play();

                PauseButton.Enabled = true;
            }
            else
            {
                return;
            }
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            if(dso != null)
            {
                if(dso.PlaybackState == PlaybackState.Playing)
                {
                    dso.Pause();
                }
                else if(dso.PlaybackState == PlaybackState.Paused || dso.PlaybackState == PlaybackState.Stopped)
                {
                    dso.Play();
                }
            }
        }

        private void DisposeProgram()
        {
            if(dso != null)
            {
                if(dso.PlaybackState == PlaybackState.Playing)
                {
                    dso.Stop();
                }
                dso.Dispose();
                dso = null;
            }

            if(wfr != null)
            {
                wfr.Dispose();
                wfr = null;
            }
        }
    }
}
