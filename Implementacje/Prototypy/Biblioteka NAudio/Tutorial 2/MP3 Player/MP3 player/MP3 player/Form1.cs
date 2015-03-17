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

namespace MP3_player
{
    public partial class MainWnd : Form
    {
        private WaveFileReader wfr = null;
        private BlockAlignReductionStream bars = null;
        private DirectSoundOut dso = null;
        private WaveStream pcm = null;

        public MainWnd()
        {
            InitializeComponent();

            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
        }

        private void DisposeProgram()
        {
            if(wfr != null)
            {
                wfr.Dispose();
                wfr = null;
            }

            if (dso != null)
            {
                if (dso.PlaybackState == PlaybackState.Playing)
                {
                    dso.Stop();
                }
                dso.Dispose();
                dso = null;
            }

            if(bars != null)
            {
                bars.Dispose();
                bars = null;
            }
        }

        private void LoadFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Plik dźwiękowe | *.mp3; *.wav";

            DisposeProgram();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                dso = new DirectSoundOut();

                if (ofd.FileName.EndsWith(".mp3"))
                {
                    pcm = WaveFormatConversionStream.CreatePcmStream(new Mp3FileReader(ofd.FileName));
                    bars = new BlockAlignReductionStream(pcm);

                    
                }
                else if(ofd.FileName.EndsWith(".wav"))
                {
                    pcm = new WaveChannel32(new WaveFileReader(ofd.FileName));
                    bars = new BlockAlignReductionStream(pcm);
                }
                else
                {
                    throw new InvalidOperationException("Nie właściwy format pliku");
                }

                dso.Init(bars);
                dso.Play();
            }
            else
            {
                return;
            }
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            if (dso != null)
            {
                if (dso.PlaybackState == PlaybackState.Playing)
                {
                    dso.Pause();
                }
                else if (dso.PlaybackState == PlaybackState.Paused || dso.PlaybackState == PlaybackState.Stopped)
                {
                    dso.Play();
                }
            }
        }

        private void MainWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisposeProgram();
        }
    }
}
