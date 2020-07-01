using System;
using Microsoft.Xna.Framework;

namespace Monogame.Processing
{
    public readonly struct color
    {
        public byte R { get; }
        public byte G { get; }
        public byte B { get; }
        public byte A { get; }

        public color(byte r, byte g, byte b, byte a = 255)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public color(int argb)
        {
            var buffer = BitConverter.GetBytes(argb);
            A = buffer[3];
            R = buffer[2];
            G = buffer[1];
            B = buffer[0];

        }

        public static implicit operator int(color c) => BitConverter.ToInt32(new[]{c.A, c.R, c.G, c.B}, 0);

        public static implicit operator color(int argb) => new color(argb);

        public static implicit operator Color(color c) => Color.FromNonPremultiplied(c.R, c.G, c.B, c.A);

        public static implicit operator color(Color c) => new color(c.R, c.G, c.B, c.A);

        public (float h, float s, float l) HSL()
        {
            // Make r, g, and b fractions of 1
            var r = R / 255.0;
            var g = G / 255.0;
            var b = B / 255.0;

            // Find greatest and smallest channel values
            var cmin = Math.Min(Math.Min(r, g), b);
            var cmax = Math.Max(Math.Max(r, g), b);
            var delta = cmax - cmin;
            double h;

            // Calculate hue
            // No difference
            if (delta == 0) h = 0;
            // Red is max
            else if (cmax == r) h = ((g - b) / delta) % 6;
            // Green is max
            else if (cmax == g) h = (b - r) / delta + 2;
            // Blue is max
            else h = (r - g) / delta + 4;

            h = Math.Round(h * 60);

            // Make negative hues positive behind 360°
            if (h < 0) h += 360;

            // Calculate lightness
            var l = (cmax + cmin) / 2;

            // Calculate saturation
            var s = delta == 0 ? 0 : delta / (1 - Math.Abs(2 * l - 1));

            // Multiply l and s by 100
            s = Math.Round((s * 100),1);
            l = Math.Round((l * 100),1);

            return ((float) h, (float) s, (float) l);
        }
    }
}
