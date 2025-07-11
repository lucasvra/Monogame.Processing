using System;
using Monogame.Processing;

namespace LineRendering
{
    public class CircleRendering : Processing
    {
        public override void Setup()
        {
            size(800, 600);
        }

        public override void Draw()
        {
            surface.setTitle($"FPS: {frameRate:00.00}");
            background(255);
            fill(0, 200);
            for (int i = 0; i < 5000; i++)
            {
                float x = random(width);
                float y = random(height);
                float r = random(10);

                circle(x, y, r);
            }
        }
    }
    public class Program
    {
        [STAThread]
        static void Main()
        {
            using var game = new CircleRendering();
            game.Run();
        }
    }
}