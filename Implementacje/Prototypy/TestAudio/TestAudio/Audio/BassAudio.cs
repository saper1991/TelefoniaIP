using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace TestAudio.Audio
{
    using Un4seen.Bass;

    class BassAudio
    {
        int rHandle;
        private BASSBuffer monBuffer = new BASSBuffer(2f, 44100, 2, 16);
        private int stream;
        private RECORDPROC _myrecordproc;
        private STREAMPROC _sproc;
        //Local Recording
        int backupStream;
        private byte[] _recbuffer;
        private int _byteswritten = 0;
        private DSPPROC _dupCallback;

        public BassAudio()
        {

        }

        public void Test()
        {
            BassNet.Registration("dako123d@gmail.com", "2X93918152222");
            Bass.BASS_RecordInit(-1);

            /*Funkcja, która znajduje numer urządzenia jakim jest mikrofon*/
            int mic = -1;
            BASSInputType flags;

            for (int n = 0; (flags = Bass.BASS_RecordGetInputType(n)) != BASSInputType.BASS_INPUT_TYPE_ERROR; n++)
            {
                if ((flags & BASSInputType.BASS_INPUT_TYPE_MASK) == BASSInputType.BASS_INPUT_TYPE_MIC)
                {
                    // found the mic!
                    mic = n;
                }
            }

            if (mic != -1)
                Console.WriteLine("Znaleziono mikrofon na kanale " + mic.ToString());
            else
                Console.WriteLine("Niestety nie znaleziono mikrofonu");


            /*Ustawienie mikrofonu na odpowiedni kanał. Wartość 0.2f oznacza, że mikrofon
            będzie nagrywał z głośnością 20%. Większych wartości nie ma co dawać bo będzie
            straszne sprzężenie zwrotne*/
            Bass.BASS_RecordSetInput(mic, BASSInput.BASS_INPUT_ON, 0.2f);
            _sproc = new STREAMPROC(MonitoringStream);
            //backupStream = Bass.BASS_StreamCreatePush(44100, 2, 0, IntPtr.Zero);

            _myrecordproc = new RECORDPROC(MyRecording);
            rHandle = Bass.BASS_RecordStart(44100, 2, BASSFlag.BASS_RECORD_PAUSE, _myrecordproc, IntPtr.Zero);
            Bass.BASS_ChannelPlay(rHandle, false);//rozpoczęcie przechwytywania dźwięku z mikrofonu

            Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
           
            stream = Bass.BASS_StreamCreate(44100, 2, 0, _sproc, IntPtr.Zero);
             
            Bass.BASS_ChannelPlay(stream, false);//rozpoczęcie odtwarzania strumienia na głośnikach
            Console.ReadKey();
            Console.WriteLine(System.Text.Encoding.UTF8.GetString(_recbuffer));
            Console.ReadKey();

            BASS_CHANNELINFO info = Bass.BASS_ChannelGetInfo(stream);
            Bass.BASS_SetDevice(2);
            int clone = Bass.BASS_StreamCreatePush(info.freq, info.chans, info.flags, IntPtr.Zero);
            // pause source stream to synchonise buffer contents
            Bass.BASS_ChannelPause(stream);
            int c = Bass.BASS_ChannelGetData(stream, IntPtr.Zero, (int)BASSData.BASS_DATA_AVAILABLE);
            byte[] buf = new byte[c];
            Bass.BASS_ChannelGetData(stream, buf, c);
            Bass.BASS_StreamPutData(clone, buf, c);
            // set DSP to copy new data from source stream
            _dupCallback = new DSPPROC(DupDSP);
            Bass.BASS_ChannelSetDSP(stream, _dupCallback, new IntPtr(clone), 0);
            //Bass.BASS_ChannelPlay(stream, false); // resume source
            Bass.BASS_ChannelPlay(clone, false); // play clone



            Bass.BASS_ChannelStop(rHandle);
            Bass.BASS_ChannelStop(stream);
            Console.ReadKey();
            Bass.BASS_ChannelPlay(backupStream, false);
            Console.ReadKey();
            Bass.BASS_RecordFree();
            Bass.BASS_Stop();
            Bass.BASS_Free();
            //timer1.Enabled = true;//w timerze jak poprzednio mamy rysowanie spektrum
        }

        private bool MyRecording(int handle, IntPtr buffer, int length, IntPtr user)
        {
            if (length > 0 && buffer != IntPtr.Zero)
            {
                // increase the rec buffer as needed 
                if (_recbuffer == null || _recbuffer.Length < length)
                    _recbuffer = new byte[length];
                // copy from managed to unmanaged memory
                Marshal.Copy(buffer, _recbuffer, 0, length);
                //Bass.BASS_StreamPutData(backupStream, buffer, length);
                _byteswritten += length; 
            }
            monBuffer.Write(buffer, length);
            return true;
        }

        private void DupDSP(int handle, int channel, IntPtr buffer, int length, IntPtr user)
        {
            Bass.BASS_StreamPutData(user.ToInt32(), buffer, length);
        }

        private int MonitoringStream(int handle, IntPtr buffer, int length, IntPtr user)
        {
            return monBuffer.Read(buffer, length, user.ToInt32());
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Un4seen.Bass.Misc.Visuals v = new Un4seen.Bass.Misc.Visuals();

            //this.picturebox1.image = v.createspectrum(rhandle,
            //                                  this.picturebox1.width,
            //                                  this.picturebox1.height,
            //                                  color.lime, color.red, color.black,
            //                                  false, false, false);
        }

        ~BassAudio()
        {

        }
    }
}
