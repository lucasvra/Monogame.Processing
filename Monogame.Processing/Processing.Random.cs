using System;
using static System.Math;

namespace Monogame.Processing
{
    partial class Processing
    {
        #region Random

        /// <summary>
        /// Generates random numbers. Each time the random() function is called, 
        /// it returns an unexpected value within the specified range. If only 
        /// one parameter is passed to the function, it will return a float between 
        /// zero and the value of the high parameter. For example, random(5) returns 
        /// values between 0 and 5 (starting at zero, and up to, but not including, 5).
        /// </summary>
        /// <param name="low">float: lower limit</param>
        /// <param name="high">float: upper limit</param>
        /// <returns>float</returns>
        public float random(float low, float high) => map((float)_rnd.NextDouble(), 0, 1, low, high);

        /// <summary>
        /// Generates random numbers. Each time the random() function is called, 
        /// it returns an unexpected value within the specified range. If only 
        /// one parameter is passed to the function, it will return a float between 
        /// zero and the value of the high parameter. For example, random(5) returns 
        /// values between 0 and 5 (starting at zero, and up to, but not including, 5).
        /// </summary>
        /// <param name="high">float: upper limit</param>
        /// <returns>float</returns>
        public float random(float high) => random(0, high);

        /// <summary>
        /// Sets the seed value for random(). By default, random() produces different results 
        /// each time the program is run. Set the seed parameter to a constant to return the 
        /// same pseudo-random numbers each time the software is run.
        /// </summary>
        /// <param name="seed">int: seed value</param>
        public void randomSeed(int seed) => _rnd = new Random(seed);

        /// <summary>
        /// Returns a float from a random series of numbers having a mean of 0 and standard
        /// deviation of 1. Each time the randomGaussian() function is called, it returns a
        /// number fitting a Gaussian, or normal, distribution. There is theoretically no minimum
        /// or maximum value that randomGaussian() might return. Rather, there is just a very
        /// low probability that values far from the mean will be returned; and a higher
        /// probability that numbers near the mean will be returned.
        /// </summary>
        /// <returns>float</returns>
        public float randomGaussian()
        {
            var u1 = 1.0 - _rnd.NextDouble();
            var u2 = 1.0 - _rnd.NextDouble();
            return (float)(Sqrt(-2.0 * Log(u1)) * Sin(2.0 * PI * u2));
        }

        /// <summary>
        /// 	Returns the Perlin noise value at specified coordinates. Perlin noise is a random
        /// sequence generator producing a more natural, harmonic succession of numbers than that of
        /// the standard random() function. It was developed by Ken Perlin in the 1980s and has been
        /// used in graphical applications to generate procedural textures, shapes, terrains, and other
        /// seemingly organic forms.
        /// </summary>
        /// <param name="x">float: x-coordinate in noise space</param>
        /// <param name="y">float: y-coordinate in noise space</param>
        /// <param name="z">float: z-coordinate in noise space</param>
        /// <returns>float</returns>
        public float noise(float x, float y = 0, float z = 0) => (float)_perlin.OctavePerlin(x, y, z);

        /// <summary>
        /// Adjusts the character and level of detail produced by the Perlin noise function. Similar
        /// to harmonics in physics, noise is computed over several octaves. Lower octaves contribute
        /// more to the output signal and as such define the overall intensity of the noise, whereas
        /// higher octaves create finer-grained details in the noise sequence.
        ///
        /// By default, noise is computed over 4 octaves with each octave contributing exactly half
        /// than its predecessor, starting at 50% strength for the first octave.This falloff amount
        /// can be changed by adding an additional function parameter.For example, a falloff factor
        /// of 0.75 means each octave will now have 75% impact (25% less) of the previous lower octave.
        /// While any number between 0.0 and 1.0 is valid, note that values greater than 0.5 may result
        /// in noise() returning values greater than 1.0.
        ///
        /// By changing these parameters, the signal created by the noise() function can be adapted to
        /// fit very specific needs and characteristics.
        /// </summary>
        /// <param name="lod">int: number of octaves to be used by the noise</param>
        /// <param name="falloff">float: falloff factor for each octave</param>
        public void noiseDetail(int lod, float falloff)
        {
            PerlinNoise.Octaves = lod;
            PerlinNoise.Persistence = falloff;
        }

        /// <summary>
        /// Adjusts the character and level of detail produced by the Perlin noise function. Similar
        /// to harmonics in physics, noise is computed over several octaves. Lower octaves contribute
        /// more to the output signal and as such define the overall intensity of the noise, whereas
        /// higher octaves create finer-grained details in the noise sequence.
        ///
        /// By default, noise is computed over 4 octaves with each octave contributing exactly half
        /// than its predecessor, starting at 50% strength for the first octave.This falloff amount
        /// can be changed by adding an additional function parameter.For example, a falloff factor
        /// of 0.75 means each octave will now have 75% impact (25% less) of the previous lower octave.
        /// While any number between 0.0 and 1.0 is valid, note that values greater than 0.5 may result
        /// in noise() returning values greater than 1.0.
        ///
        /// By changing these parameters, the signal created by the noise() function can be adapted to
        /// fit very specific needs and characteristics.
        /// </summary>
        /// <param name="lod">int: number of octaves to be used by the noise</param>
        public void noiseDetail(int lod) => PerlinNoise.Octaves = lod;

        /// <summary>
        /// Sets the seed value for noise(). By default, noise() produces different results each
        /// time the program is run. Set the seed parameter to a constant to return the same
        /// pseudo-random numbers each time the software is run.
        /// </summary>
        /// <param name="seed">int: seed value</param>
        public void noiseSeed(int seed) => _perlin = new PerlinNoise(seed, -1);

        #endregion
    }
}
