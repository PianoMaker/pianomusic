﻿using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using static Pianomusic.Models.Globals;

namespace Pianomusic.Models
{
    public class Duration : ICloneable
    {
        private DURATION duration;
        private DURMODIFIER modifier;
        private int tuplet;


        public Duration()
        { duration = DURATION.quater; modifier = DURMODIFIER.none; tuplet = 1; }

        public Duration(DURATION duration, DURMODIFIER modifier, int tuplet)
        {
            this.duration = duration;
            this.modifier = modifier;
            this.tuplet = tuplet;
        }

        private void Digit_to_duration(int digit)
        {
            int value = 1;
            if (digit == 0) throw new IncorrectNote("Incorrect duration: 0");
            while (digit % 2 == 0)
            {
                digit /= 2;
                value *= 2;
            }
            duration = (DURATION)value;
            tuplet = digit;
            modifier = DURMODIFIER.none;
        }
        public Duration(int digit)
        {
            Digit_to_duration(digit);
            modifier = DURMODIFIER.none;
        }
        public Duration(int digit, string? modifier)
        {
            Digit_to_duration(digit);
            switch (modifier)
            {
                case ".": this.modifier = DURMODIFIER.dotted; break;
                case "..": this.modifier = DURMODIFIER.doubledotted; break;
                case "...": this.modifier = DURMODIFIER.tripledotted; break;
                case "/": this.modifier = DURMODIFIER.tuplet; break;
                default: this.modifier = DURMODIFIER.none; break;
            }
        }

        public DURATION Dur
        { get { return duration; } set { duration = value; } }

        public double RelDuration()
        {
            double relduration = 4;
            relduration /= (int)duration;
            if (modifier == DURMODIFIER.none) relduration *= 1;
            else if (modifier == DURMODIFIER.dotted) relduration *= 1.5;
            else if (modifier == DURMODIFIER.doubledotted) relduration *= 1.75;
            else if (modifier == DURMODIFIER.tripledotted) relduration *= 1.875;
            if (tuplet > 0) relduration /= tuplet;
            return relduration;
        }

        public int MidiDuration(int PPQN)
        {
            return PPQN * (int)RelDuration();
            //Pulses Per Quarter Note - міряє тіки на чвертку
        }

        public int AbsDuration()
        { 
        if (PlaySpeedLocal > 0)
                return (int)(PlaySpeedLocal * RelDuration());
            else return (int)(playspeed * RelDuration()); 
        }

        public object Clone()
        {
            return new Duration(this.duration, this.modifier, this.tuplet);
        }

        public override string ToString()
        {
            var str = new StringBuilder();
            str.Append(duration.ToString());
            if (modifier != DURMODIFIER.none)
                str.Append(" " + modifier.ToString());
            if (tuplet != 1)
                str.Append(" tuplet: " + tuplet.ToString());
            return str.ToString();
        }
    }
}
