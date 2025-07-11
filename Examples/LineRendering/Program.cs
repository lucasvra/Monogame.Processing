using System;
using Monogame.Processing;

namespace LineRendering
{
    public class LineRendering : Processing
    {
        public override void Setup()
        {
            size(800, 600);
        }

        public override void Draw()
        {
            surface.setTitle($"FPS: {frameRate:00.00}");
            background(255);
            stroke(0);
            for (int i = 0; i < 5000; i++)
            {
                float x0 = random(width);
                float y0 = random(height);
                float x1 = random(width);
                float y1 = random(height);

                // purely 2D lines will trigger the GLU 
                // tessellator to add accurate line caps,
                // but performance will be substantially
                // lower.
                line(x0, y0, x1, y1);
            }
        }
    }
    public class Program
    {
        [STAThread]
        static void Main()
        {
            using var game = new LineRendering();
            game.Run();
        }
    }
}
