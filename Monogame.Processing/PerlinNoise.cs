// ref: http://flafla2.github.io/2014/08/09/perlinnoise.html

using System;
using System.Collections.Generic;

namespace Monogame.Processing
{
    public class PerlinNoise
    {

        public static int Octaves { get; set; } = 4;
        public static double Persistence { get; set; } = 0.5;

        private readonly Dictionary<(float, float, float), float> _mem = new Dictionary<(float, float, float), float>();

        public int seed { get; }
        public int Repeat;

        public PerlinNoise(int seed, int repeat = -1)
        {
            this.seed = seed;
            Repeat = repeat;
        }

        public PerlinNoise(int repeat = -1)
        {
            seed = DateTime.Now.Millisecond;
            Repeat = repeat;
        }

        public float OctavePerlin(float x, float y, float z)
        {
            var n = (float) OctavePerlin(x, y, z, Octaves, Persistence);
            return n;
        }

        private double OctavePerlin(double x, double y, double z, int octaves, double persistence)
        {
            x += seed;
            y += seed;
            z += seed;

            double total = 0;
            double frequency = 1;
            double amplitude = 1;
            double maxValue = 0; // Used for normalizing result to 0.0 - 1.0

            for (var i = 0; i < octaves; i++)
            {
                total += Perlin(x * frequency, y * frequency, z * frequency) * amplitude;

                maxValue += amplitude;

                amplitude *= persistence;
                frequency *= 2;
            }

            return total / maxValue;
        }

        private static readonly int[] Permutation =
        {
            // Hash lookup table as defined by Ken Perlin.  This is a randomly
            // arranged array of all numbers from 0-255 inclusive.
            151, 160, 137, 91, 90, 15,
            131, 13, 201, 95, 96, 53, 194, 233, 7, 225, 140, 36, 103, 30, 69, 142, 8, 99, 37, 240, 21, 10,
            23, 190, 6, 148, 247, 120, 234, 75, 0, 26, 197, 62, 94, 252, 219, 203, 117, 35, 11, 32, 57, 177, 33,
            88, 237, 149, 56, 87, 174, 20, 125, 136, 171, 168, 68, 175, 74, 165, 71, 134, 139, 48, 27, 166,
            77, 146, 158, 231, 83, 111, 229, 122, 60, 211, 133, 230, 220, 105, 92, 41, 55, 46, 245, 40, 244,
            102, 143, 54, 65, 25, 63, 161, 1, 216, 80, 73, 209, 76, 132, 187, 208, 89, 18, 169, 200, 196,
            135, 130, 116, 188, 159, 86, 164, 100, 109, 198, 173, 186, 3, 64, 52, 217, 226, 250, 124, 123,
            5, 202, 38, 147, 118, 126, 255, 82, 85, 212, 207, 206, 59, 227, 47, 16, 58, 17, 182, 189, 28, 42,
            223, 183, 170, 213, 119, 248, 152, 2, 44, 154, 163, 70, 221, 153, 101, 155, 167, 43, 172, 9,
            129, 22, 39, 253, 19, 98, 108, 110, 79, 113, 224, 232, 178, 185, 112, 104, 218, 246, 97, 228,
            251, 34, 242, 193, 238, 210, 144, 12, 191, 179, 162, 241, 81, 51, 145, 235, 249, 14, 239, 107,
            49, 192, 214, 31, 181, 199, 106, 157, 184, 84, 204, 176, 115, 121, 50, 45, 127, 4, 150, 254,
            138, 236, 205, 93, 222, 114, 67, 29, 24, 72, 243, 141, 128, 195, 78, 66, 215, 61, 156, 180
        };

        private static readonly int[] P; // Doubled permutation to avoid overflow

        static PerlinNoise()
        {
            P = new int[512];
            for (var x = 0; x < 512; x++) P[x] = Permutation[x % 256];
        }

        private double Perlin(double x, double y, double z)
        {
            if (Repeat > 0)
            {
                x %= Repeat;
                y %= Repeat;
                z %= Repeat;
            }

            // Calculate the "unit cube" that the point asked will be located in
            // The left bound is ( |_x_|,|_y_|,|_z_| ) and the right bound is that
            // plus 1.  Next we calculate the location (from 0.0 to 1.0) in that cube.
            // We also fade the location to smooth the result.
            var xi = (int) x & 255; 
            var yi = (int) y & 255; 
            var zi = (int) z & 255; 
            var xf = x - (int) x;   
            var yf = y - (int) y;
            var zf = z - (int) z;
            var u = fade(xf);
            var v = fade(yf);
            var w = fade(zf);

            var aaa = P[P[P[xi] + yi] + zi];
            var aba = P[P[P[xi] + inc(yi)] + zi];
            var aab = P[P[P[xi] + yi] + inc(zi)];
            var abb = P[P[P[xi] + inc(yi)] + inc(zi)];
            var baa = P[P[P[inc(xi)] + yi] + zi];
            var bba = P[P[P[inc(xi)] + inc(yi)] + zi];
            var bab = P[P[P[inc(xi)] + yi] + inc(zi)];
            var bbb = P[P[P[inc(xi)] + inc(yi)] + inc(zi)];

            // The gradient function calculates the dot product between a pseudorandom
            // gradient vector and the vector from the input coordinate to the 8
            var x1 = lerp(grad(aaa, xf, yf, zf), grad(baa, xf - 1, yf, zf), u);

            // This is all then lerped together as a sort of weighted average based on the faded (u,v,w)
            // values we made earlier.
            var x2 = lerp(grad(aba, xf, yf - 1, zf), grad(bba, xf - 1, yf - 1, zf), u);
            var y1 = lerp(x1, x2, v);

            x1 = lerp(grad(aab, xf, yf, zf - 1), grad(bab, xf - 1, yf, zf - 1), u);
            x2 = lerp(grad(abb, xf, yf - 1, zf - 1), grad(bbb, xf - 1, yf - 1, zf - 1), u);

            var y2 = lerp(x1, x2, v);

            // For convenience we bound it to 0 - 1 (theoretical min/max before is -1 - 1)
            var n = (lerp(y1, y2, w) + 1) / 2;

            return n;
        }

        public int inc(int num)
        {
            num++;
            if (Repeat > 0) num %= Repeat;

            return num;
        }

        public static double grad(int hash, double x, double y, double z)
        {
            var h = hash & 15;       // Take the hashed value and take the first 4 bits of it (15 == 0b1111)
            var u = h < 8 ? x : y; // If the most significant bit (MSB) of the hash is 0 then set u = x.  Otherwise y.


            double v;

            if (h < 4) v = y;                   // If the first and second significant bits are 0 set v = y
            else if (h == 12 || h == 14) v = x; // If the first and second significant bits are 1 set v = x
            else v = z;                         // If the first and second significant bits are not equal (0/1, 1/0) set v = z

            // Use the last 2 bits to decide if u and v are positive or negative.  Then return their addition.
            return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v); 
        }
        
        // Fade function as defined by Ken Perlin.  This eases coordinate values
        // so that they will "ease" towards integral values.  This ends up smoothing
        // the final output.
        public static double fade(double t) => t * t * t * (t * (t * 6 - 15) + 10);

        public static double lerp(double a, double b, double x) => a + x * (b - a);
    }
}
