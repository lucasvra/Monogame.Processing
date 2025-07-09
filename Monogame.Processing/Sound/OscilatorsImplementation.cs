using System;
using Microsoft.Xna.Framework;

namespace Monogame.Processing.Sound
{
    public class SinOsc : Oscilator
    {
        protected override float OscilatorFunction() => (float) Math.Sin(frequency * time * 2 * Math.PI) + offset;
    }

    public class SawOsc : Oscilator
    {
        protected override float OscilatorFunction() => (float)(2 * (time * frequency - Math.Floor(time * frequency + 0.5))) + offset;
    }

    public class SqrOsc : Oscilator
    {
        protected override float OscilatorFunction() => (Math.Sin(frequency * time * 2 * Math.PI) >= 0 ? 1.0f : -1.0f) + offset;
    }

    public class TriOsc : Oscilator
    {
        protected override float OscilatorFunction() => (float)Math.Abs((2 * (time * frequency - Math.Floor(time * frequency + 0.5)))) * 2.0f - 1.0f + offset;
    }

    public class Pulse : Oscilator
    {
        private float w = 0f;

        public void width(float width) => w = MathHelper.Clamp(width, 0f, 1f);

        protected override float OscilatorFunction() => (Math.Sin(frequency * time * 2 * Math.PI) >= w ? 1.0f : -1.0f) + offset;
    }
}
