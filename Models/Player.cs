using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pianomusic.Models.Globals;
using static Pianomusic.Models.Beeper;
using static Pianomusic.Models.NAPlayer;
using static Pianomusic.Models.Engine;
using Pianomusic.Models;

namespace Pianomusic.Models
{
    public static class Player
{
 // Відтворює звуки.        
        public static void Play(int hz, int ms)
        {
            if (player is null) Menues.ChoosePlayer();
            if (player == PLAYER.beeper)
                Beeper.PlayHz(hz, ms);
            else if (player == PLAYER.naudio)
                NAPlayer.Play(hz, ms);
            else Menues.ChoosePlayer();
        }

        public static void Play(Note note)
        {
            if (player is null) Menues.ChoosePlayer();
            if (player == PLAYER.beeper)
                Beeper.Play(note);
            else if (player == PLAYER.naudio)
                NAPlayer.Play(note);
            else if (player == PLAYER.midiplayer)
                MidiFile0.Play(note);
            else Menues.ChoosePlayer();
        }

        public static void Play(Chord chord)
        {
            if (player is null) Menues.ChoosePlayer();
            if (player == PLAYER.beeper)
                Beeper.Play(chord);
            else if (player == PLAYER.naudio)
                NAPlayer.Play(chord);
            else if (player == PLAYER.midiplayer)
                MidiFile0.Play(chord);
            else Menues.ChoosePlayer();
        }

        public static void Play(Melody melody)
        {
            if (player is null) Menues.ChoosePlayer();
            if (player == PLAYER.beeper)
                Beeper.Play(melody);
            else if (player == PLAYER.naudio)
                NAPlayer.Play(melody);
            else if (player == PLAYER.midiplayer)
                MidiFile0.Play(melody);
            else Menues.ChoosePlayer();
        }

        public static void Play<T>(LinkedList<T> progression) where T : Chord
        {
            if (player is null) Menues.ChoosePlayer();
            if (player == PLAYER.beeper)
                Beeper.Play(progression);
            else if (player == PLAYER.naudio)
                NAPlayer.Play(progression);
            else Menues.ChoosePlayer();
        }

        public static void Play(List<string> sounds)
        {
            if (player is null) Menues.ChoosePlayer();
            if (player == PLAYER.beeper)
                foreach (string note in sounds)
                {
                    Beeper.Play(new Note(note));
                }
            else if (player == PLAYER.naudio)
                foreach (string note in sounds)
                {
                    NAPlayer.Play(new Note(note));
                }
            else if (player == PLAYER.midiplayer)
                foreach (string note in sounds)
                {
                    MidiFile0.Play(new Note(note));
                }

        }

   

        public static void Play<T>(List<T> chords) where T : Chord
        {
            if (player == null) Menues.ChoosePlayer();
            if (player == PLAYER.beeper)
                foreach (T chord in chords)
                {
                    Beeper.Play(chord);
                }
            else if (player == PLAYER.naudio)
                foreach (T chord in chords)
                {
                    NAPlayer.Play(chord);
                }
            else if (player == PLAYER.midiplayer)
                foreach (T chord in chords)
                {
                    MidiFile0.Play(chord);
                }
            else Menues.ChoosePlayer();
        }


    }
}
