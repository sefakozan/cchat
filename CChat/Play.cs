using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CChat
{
	public static class Play
	{
        static CachedSound msg = new CachedSound("closed-hat-trimmed.wav");
        static CachedSound bombom = new CachedSound("Komik-Bom-Bom.mp3");
        static CachedSound tatil = new CachedSound("Komik-15-Tatil-Parodi.mp3");
        static CachedSound okul = new CachedSound("Komik-Uyan-Okullar-Acildi-Parodi.mp3");
        static CachedSound nereye = new CachedSound("Nereye Sıçacaklar ( Remix ) - Educatedear.mp3");
        static CachedSound beyin = new CachedSound("Beyin bedava.mp3");
        static CachedSound sezaiogul = new CachedSound("sezai-ogul.mp3");
        static CachedSound taumata = new CachedSound("Taumata.mp3");
        static CachedSound ivedikden = new CachedSound("receb ivedikden kulak cinlamasi.mp3");
        static CachedSound tata = new CachedSound("tatata.mp3");
        static CachedSound anektarlar = new CachedSound("anektar.mp3");

        static CachedSound[] CachedSounds = new CachedSound[] 
        {
            msg,
            bombom,
            tatil,
            okul,
            nereye,
            beyin,
            sezaiogul,
            taumata,
            ivedikden,
            tata,
            anektarlar,
        };

        public static void PlayAudio(string param, bool isSingle = false)
        {

            CachedSound selectedCachedSound;

            try
            {
                int index = Convert.ToInt32(param);
                selectedCachedSound = CachedSounds[index];

            }
            catch
            {
                selectedCachedSound = CachedSounds[0];
            }

            AudioPlaybackEngine.Instance.PlaySound(selectedCachedSound, isSingle);

        }

        public static Stream GetResourceStream(string filename)
		{
			Assembly asm = Assembly.GetExecutingAssembly();
			string resname = asm.GetName().Name + ".audio." + filename;
			//var names = asm.GetManifestResourceNames();
			return asm.GetManifestResourceStream(resname);
		}

	}


    class CachedSound
    {
        public float[] AudioData { get; private set; }
        public WaveFormat WaveFormat { get; private set; }
        public CachedSound(string audioFileName)
        {

            if (audioFileName.EndsWith(".mp3"))
            {

                using (Stream mp3File = Play.GetResourceStream(audioFileName))
                {
                    using (Mp3FileReader mp3FileReader = new Mp3FileReader(mp3File))
                    {
                        var sampleProvider = mp3FileReader.ToSampleProvider();

                        WaveFormat = sampleProvider.WaveFormat;
                        var wholeFile = new List<float>((int)(mp3FileReader.Length / 4));
                        var readBuffer = new float[sampleProvider.WaveFormat.SampleRate * sampleProvider.WaveFormat.Channels];
                        int samplesRead;
                        while ((samplesRead = sampleProvider.Read(readBuffer, 0, readBuffer.Length)) > 0)
                        {
                            wholeFile.AddRange(readBuffer.Take(samplesRead));
                        }
                        AudioData = wholeFile.ToArray();

                    }

                }

            }
            else 
            {

                using (Stream waveFile = Play.GetResourceStream(audioFileName))
                {
                    using (WaveFileReader waveFileReader = new WaveFileReader(waveFile))
                    {
                        var sampleProvider = waveFileReader.ToSampleProvider();


                        WaveFormat = sampleProvider.WaveFormat;
                        var wholeFile = new List<float>((int)(waveFileReader.Length / 4));
                        var readBuffer = new float[sampleProvider.WaveFormat.SampleRate * sampleProvider.WaveFormat.Channels];
                        int samplesRead;
                        while ((samplesRead = sampleProvider.Read(readBuffer, 0, readBuffer.Length)) > 0)
                        {
                            wholeFile.AddRange(readBuffer.Take(samplesRead));
                        }
                        AudioData = wholeFile.ToArray();

                    }

                }

            }

        }
    }


    class AutoDisposeFileReader : ISampleProvider
    {
        private readonly AudioFileReader reader;
        private bool isDisposed;
        public AutoDisposeFileReader(AudioFileReader reader)
        {
            this.reader = reader;
            this.WaveFormat = reader.WaveFormat;
        }


        public int Read(float[] buffer, int offset, int count)
        {
            if (isDisposed)
                return 0;

            

            int read = reader.Read(buffer, offset, count);
            if (read == 0)
            {
                reader.Dispose();
                isDisposed = true;
            }
            return read;
        }

        public WaveFormat WaveFormat { get; private set; }
    }


    class CachedSoundSampleProvider : ISampleProvider
    {
        private readonly CachedSound cachedSound;
        private long position;

        public CachedSoundSampleProvider(CachedSound cachedSound)
        {
            this.cachedSound = cachedSound;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            var availableSamples = cachedSound.AudioData.Length - position;
            var samplesToCopy = Math.Min(availableSamples, count);
            Array.Copy(cachedSound.AudioData, position, buffer, offset, samplesToCopy);
            position += samplesToCopy;
            return (int)samplesToCopy;
        }

        public WaveFormat WaveFormat { get { return cachedSound.WaveFormat; } }
    }


    class AudioPlaybackEngine : IDisposable
    {
        private readonly IWavePlayer outputDevice;
        private readonly MixingSampleProvider mixer;

        public AudioPlaybackEngine(int sampleRate = 44100, int channelCount = 2)
        {
            outputDevice = new WaveOutEvent();
            mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount));
            mixer.ReadFully = true;
            outputDevice.Init(mixer);
            outputDevice.Play();
        }

        //public void PlaySound(string fileName)
        //{
        //    var input = new AudioFileReader(fileName);
        //    AddMixerInput(new AutoDisposeFileReader(input));
        //}

        private ISampleProvider ConvertToRightChannelCount(ISampleProvider input)
        {
            if (input.WaveFormat.Channels == mixer.WaveFormat.Channels)
            {
                return input;
            }
            if (input.WaveFormat.Channels == 1 && mixer.WaveFormat.Channels == 2)
            {
                return new MonoToStereoSampleProvider(input);
            }
            throw new NotImplementedException("Not yet implemented this channel count conversion");
        }

        public void PlaySound(CachedSound sound, bool isSigle = false)
        {
            if (isSigle) 
            {
                mixer.RemoveAllMixerInputs();
            }
            
            AddMixerInput(new CachedSoundSampleProvider(sound));
        }

        private void AddMixerInput(ISampleProvider input)
        {
       
            mixer.AddMixerInput(ConvertToRightChannelCount(input));
            
        }

        public void Dispose()
        {
            outputDevice.Dispose();
        }

        public static readonly AudioPlaybackEngine Instance = new AudioPlaybackEngine(44100, 2);
    }



}
