using System;

namespace Monogame.Processing.Sound
{
    public class WhiteNoise : Generator
    {
        readonly Random _rnd = new Random();

        protected override float OscilatorFunction()
        {
            var u1 = 1.0 - _rnd.NextDouble();
            var u2 = 1.0 - _rnd.NextDouble();
            return (float) (Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2));
        }
    }

    public class BrownNoise : Generator
    {
        readonly Random _rnd = new Random();
        float lastOutput = 0.0f;
        protected override float OscilatorFunction()
        {
            var u1 = 1.0 - _rnd.NextDouble();
            var u2 = 1.0 - _rnd.NextDouble();
            var white = (float)(Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2));
            var brown = (lastOutput + (0.02f * white)) / 1.02f;
            lastOutput = brown;
            return brown;
        }
    }

    public class pinkNoise : Generator
    {
        readonly Random _rnd = new Random();
        private float b0 = 0f, b1 = 0f, b2 = 0f, b3 = 0f, b4 = 0f, b5 = 0f, b6 = 0f;
        protected override float OscilatorFunction()
        {
            var u1 = 1.0 - _rnd.NextDouble();
            var u2 = 1.0 - _rnd.NextDouble();
            var white = (float)(Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2));
            var pink = white;

            b0 = 0.99886f * b0 + white * 0.0555179f;
            b1 = 0.99332f * b1 + white * 0.0750759f;
            b2 = 0.96900f * b2 + white * 0.1538520f;
            b3 = 0.86650f * b3 + white * 0.3104856f;
            b4 = 0.55000f * b4 + white * 0.5329522f;
            b5 = -0.7616f * b5 - white * 0.0168980f;

            pink = b0 + b1 + b2 + b3 + b4 + b5 + b6 + white * 0.5362f;
            pink *= 0.11f;
            b6 = white * 0.115926f;

            return pink;
        }
    }
}
