using static System.Console;
using static Pianomusic.Models.Globals;
using static Pianomusic.Models.Engine;
using Microsoft.Win32.SafeHandles;
using NAudio.Wave.SampleProviders;
using NAudio.Wave;
#pragma warning disable CA1416


namespace Pianomusic.Models
{
    public class Beeper
    {//Відтворювач звуків за допомогою консольного плеєра
        private int hz;
        private int ms;
        List<int> ?melody;
        List<string> ?sounds;

        public Beeper()
        {
            ms = playspeed;
        }
        public Beeper(int hz)
        {
            this.hz = hz;
            ms = playspeed;
        }

        public Beeper(int hz, int ms)
        { this.hz = hz; this.ms = ms; }

        public Beeper(List<int> melody)
        {
            this.melody = melody;
            ms = playspeed;
        }

        public Beeper(List<string> sounds)
        {
            foreach (var sound in sounds)
            {
                melody.Add(key_to_pitch(sound));
            }
            ms = playspeed;
        }
        public int Hz
        {
            get { return hz; }
            set { hz = value; }
        }
        public int Ms
        {
            get { return ms; }
            set { ms = value; }
        }
        public List<int> Melody
        {
            get { return melody; }
            set { melody = value; }
        }


        public static void Play(Note note)
        {
            var hz = Pitch_to_hz(note.AbsPitch());
            if (hz < 37) throw new Exception($"Thissound is too low: {hz} hz");
            Beep(hz, note.AbsDuration());
        }

        public static void Play(int hz, int ms)
        {
            try
            {
                Beep(hz, ms);
            }
            catch { /*ThreadPool.QueueUserWorkItem((a) => MessageBox.Show($"impossible to produce {hz} hz"));*/ }
        }

        public static void PlayPitch(int pitch, int duration = playspeed)
        {
            pitch = Pitch_to_hz(pitch);
            if (pitch % 2 > 0) pitch++; // подолання глюку відтворення тонів непарним значенням частоти 
            Beep(pitch, duration);
        }

        public static void Play(List<int> pitches, int duration = playspeed)
        {
            foreach (int i in pitches)
            {
                Beep(Pitch_to_hz(pitches[i]), duration);
            }
        }

        public static void Play(List<string> sounds, int duration = playspeed)
        {
            foreach (string note in sounds)
            {
                Beep(Pitch_to_hz(key_to_pitch(note)), duration);
            }
        }


        public static void Play<T>(T scale) where T : Scale
        {

            foreach (Note note in scale.Notes)
            {
                Play(Pitch_to_hz(note.AbsPitch()), note.AbsDuration());
            }
        }

        public static void Play<T>(LinkedList<T> progression) where T : Chord
        {

            foreach (Chord chord in progression)
            {
                Play(chord);
            }
        }

        internal static void PlayHz(int hz, int ms)
        {
            Beep(hz, ms);
        }
    }


}

