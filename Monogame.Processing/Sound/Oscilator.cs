using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Monogame.Processing.Sound
{
    public abstract class Oscilator:Generator
    {
        protected float frequency = 0.0f;

        /// <summary>
        /// Set multiple parameters at once
        /// </summary>
        /// <param name="freq"></param>
        /// <param name="amp"></param>
        /// <param name="add"></param>
        /// <param name="pos"></param>
        public void set(float freq, float amp, float add, float pos)
        {
            this.freq(freq);
            this.amp(amp);
            this.add(add);
            this.pan(pos);
        }

        /// <summary>
        /// Set the frequency of the oscillator in Hz.
        /// </summary>
        /// <param name="freq"></param>
        public void freq(float freq) => frequency = freq;

    }
}
