using System;
using Monogame.Processing;

namespace SineWave
{
    public class SineWave : Processing
    {
        int xspacing = 16;   // How far apart should each horizontal location be spaced
        int w;              // Width of entire wave

        float theta = 0.0f;  // Start angle at 0
        float amplitude = 75.0f;  // Height of wave
        float period = 500.0f;  // How many pixels before the wave repeats
        float dx;  // Value for incrementing X, a function of period and xspacing
        float[] yvalues;  // Using an array to store height values for the wave
        
        public override void Setup()
        {
            size(640, 360);
            w = width + 16;
            dx = (TWO_PI / period) * xspacing;
            yvalues = new float[w / xspacing];
        }

        public override void Draw()
        {
            surface.setTitle($"FPS: {frameRate:00.00}");
            background(0f);
            calcWave();
            renderWave();
        }

        void calcWave()
        {
            // Increment theta (try different values for 'angular velocity' here
            theta += 0.02f;

            // For every x value, calculate a y value with sine function
            float x = theta;
            for (int i = 0; i < yvalues.Length; i++)
            {
                yvalues[i] = sin(x) * amplitude;
                x += dx;
            }
        }

        void renderWave()
        {
            noStroke();
            fill(255f);
            // A simple way to draw the wave with an ellipse at each location
            for (int x = 0; x < yvalues.Length; x++)
            {
                ellipse(x * xspacing, height / 2 + yvalues[x], 16, 16);
            }
        }
    }
    public class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new SineWave()) game.Run();
        }
    }
}
