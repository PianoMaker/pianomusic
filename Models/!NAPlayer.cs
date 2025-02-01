using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio;
using NAudio.CoreAudioApi;
using NAudio.Wave.SampleProviders;
using NAudio.Wave;
using static Pianomusic.Models.Globals;
using static Pianomusic.Models.Engine;
using NAudio.Lame;



namespace Pianomusic.Models
{

    public static class NAPlayer
    { // Відтворювач звуків через NAudio

        public static void Play(Note note)
        {
            Play(Pitch_to_hz(note.AbsPitch()), note.AbsDuration());
        }


        public static void Play(int hz, int ms)
        {
            try
            {
                var type = choosetimbre();
                //Console.WriteLine($"Trying to play {hz} hz, {type}");
                var Sgr = new SignalGenerator()
                {
                    Frequency = hz,
                    Type = type,
                    Gain = 0.5
                };
                //Console.WriteLine($"Sgr done, {Sgr.Frequency} hz, {Sgr.Gain} db");

                using (var Woe = new WaveOutEvent())
                {
                    if (Sgr == null)
                    {
                        throw new InvalidOperationException("SignalGenerator is null.");
                    }
                    try
                    {
                        //Console.WriteLine($"Trying Woe");
                        Woe.Init(Sgr);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error initializing WaveOutEvent: {ex.Message}");
                        return;
                    }
                    //Console.WriteLine($"Woe prepared");

                    Woe.Play();
                    Console.WriteLine($"Start playing {hz} hz");
                    Thread.Sleep(ms);
                    Woe.Stop();

                }            
            }
            catch
            {
               /* ThreadPool.QueueUserWorkItem((a) => MessageBox.Show($"impossible to produce {hz} hz"));*/
            }
        }

        public static SignalGeneratorType choosetimbre()
        {
            switch (timbre)
            {
                default: return SignalGeneratorType.Sin;
                case TIMBRE.sin: return SignalGeneratorType.Sin;
                case TIMBRE.tri: return SignalGeneratorType.Triangle;
                case TIMBRE.sawtooth: return SignalGeneratorType.SawTooth;
                case TIMBRE.square: return SignalGeneratorType.Square;
            }
        }


        public static void Play(Melody melody)
        {
            if (melody is not null)
                foreach (var note in melody.Notes)
                {
                    var Sgr = new SignalGenerator()
                    {
                        Frequency = Pitch_to_hz(note.AbsPitch()),
                        Type = choosetimbre(),
                    };
                    var Woe = new WaveOutEvent();
                    Woe.Init(Sgr);
                    Woe.Play();
                    Thread.Sleep(note.AbsDuration());
                    Woe.Stop();
                }
        }

        public static void Play(Chord chord)
        {
            if (chord is null || chord.Notes.Count < 1) return;

            var mixers = new List<ISampleProvider>();

            // Створення генераторів сигналів для кожної ноти та додавання їх до мікшера
            foreach (var note in chord.Notes)
            {
                var signalGenerator = new SignalGenerator()
                {
                    Frequency = Pitch_to_hz(note.AbsPitch()),
                    Type = choosetimbre(),
                    Gain = 0.5 // Задайте рівень гучності для кожної ноти (можна налаштувати)
                };
                mixers.Add(signalGenerator);
            }

            // Створення мікшера та додавання генераторів сигналів до нього
            var mixer = new MixingSampleProvider(mixers);

            using (var waveOut = new WaveOutEvent())
            {
                waveOut.Init(mixer);
                Thread.Sleep(20); // Невелика затримка перед відтворенням
                waveOut.Play();

                // Чекати, поки грає звук
                Thread.Sleep(chord.Notes[0].AbsDuration());
                waveOut.Stop();
            }
        }

        public static void PlayPitch(int pitch, int duration = playspeed)
        {
            pitch = Pitch_to_hz(pitch);
            if (pitch % 2 > 0) pitch++; // подолання глюку відтворення тонів непарним значенням частоти 
            Play(pitch, duration);
        }



        public static void Play<T>(LinkedList<T> progression) where T : Chord
        {

            foreach (Chord chord in progression)
            {
                Play(chord);
            }
        }

        public static void Play(List<ChordT> multichords)
        {
            foreach (Chord chord in multichords)
            {
                Play(chord);

            }
        }


        

    }




}

