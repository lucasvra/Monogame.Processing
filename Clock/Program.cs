using System;
using Monogame.Processing;

namespace Clock
{
    public class Clock : Processing
    {
        int cx, cy;
        float secondsRadius;
        float minutesRadius;
        float hoursRadius;
        float clockDiameter;

        public override void Setup()
        {
            surface.setTitle("Clock");
            surface.setResizable(true);

            size(640, 360);
            stroke(255f);
            smooth();

            int radius = min(width, height) / 2;
            secondsRadius = radius * 0.72f;
            minutesRadius = radius * 0.60f;
            hoursRadius = radius * 0.50f;
            clockDiameter = radius * 1.8f;

            cx = width / 2;
            cy = height / 2;
        }

        public override void Draw()
        {
            background(0f);

            // Draw the clock background
            fill(80f);
            noStroke();
            ellipse(cx, cy, clockDiameter, clockDiameter);

            // Angles for sin() and cos() start at 3 o'clock;
            // subtract HALF_PI to make them start at the top
            var s = map(second(), 0, 60, 0, TWO_PI) - HALF_PI;
            var m = map(minute() + norm(second(), 0, 60), 0, 60, 0, TWO_PI) - HALF_PI;
            var h = map(hour() + norm(minute(), 0, 60), 0, 24, 0, TWO_PI * 2) - HALF_PI;

            // Draw the hands of the clock
            stroke(255f);
            strokeWeight(1);
            line(cx, cy, cx + cos(s) * secondsRadius, cy + sin(s) * secondsRadius);
            strokeWeight(2);
            line(cx, cy, cx + cos(m) * minutesRadius, cy + sin(m) * minutesRadius);
            strokeWeight(4);
            line(cx, cy, cx + cos(h) * hoursRadius, cy + sin(h) * hoursRadius);

            // Draw the minute ticks
            strokeWeight(2);
            for (int a = 0; a < 360; a += 6)
            {
                float angle = radians(a);
                float x = cx + cos(angle) * secondsRadius;
                float y = cy + sin(angle) * secondsRadius;
                point(x, y);
            }
        }
    }
    public class Program
    {
        [STAThread]
        static void Main()
        {
            using var game = new Clock();
            game.Run();
        }
    }
}
