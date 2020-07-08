using System;
using Monogame.Processing;
using Monogame.Processing.Sound;

namespace Ball
{
    public class Ball : Processing
    {
        private float x, y;
        private float vx = 1, vy = 0.5f;
        public override void Setup()
        {
            FrameRate(20);
            size(400, 400);
            background(200);
        }

        public override void Draw()
        {
            surface.setTitle($"FPS: {frameRate:00.00}");

            var theta = vy < 0 ? TWO_PI - acos(vx / sqrt(sq(vx) + sq(vy))) : acos(vx / sqrt(sq(vx) + sq(vy)));
            var angle = radians(120);

            strokeWeight(1);
            stroke(0);
            fill(0, 100, 0, 200);
            arc(x, y, 50, 50, theta - angle, theta + angle, (ArcMode)CHORD);

            x += vx;
            y += vy;

            if (x < 0 || x > width) vx *= -1;
            if (y < 0 || y > height) vy *= -1;
            
            printMatrix();
        }

        public override void MouseDragged()
        {
            vx = mouseX - pmouseX;
            vy = mouseY - pmouseY;
        }

        private SinOsc sine;
        public override void MouseClicked()
        {
            sine = new SinOsc();
            var f = (float) (440.0 * Math.Pow(2, (1 - 9) / 12.0f));
            sine.set(f,1,0,0);
            sine.play();
        }
    }
    public class Program
    {
        [STAThread]
        static void Main()
        {
            using var game = new Ball();
            game.Run();
        }
    }
}
