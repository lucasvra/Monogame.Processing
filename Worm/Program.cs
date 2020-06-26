using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Monogame.Processing;

namespace Worm
{
    public class Worm : Processing
    {
        float x2 = 400, y2;
        float tx = 600, ty, t;
        int up, up2;

        readonly List<(float x, float y)> bubbles = new List<(float x, float y)>();

        public override void Setup()
        {
            fullScreen();
            background(255);
        }

        public override void Draw()
        {
            ty = mouseY;
            tx = mouseX;

            background(250);
            strokeWeight(2);
            fill(Color.BlanchedAlmond);

            t++;

            if (keyCode == Keys.Escape) exit();
            if (keyCode == Keys.T) ellipse(tx, ty, 20, 20);
            if (t < 5) up = round(random(1, 0));
            if (t < 5) up2 = round(random(1, 0));
            if (up == 1) tx += t / 10;
            if (up == 0) tx -= t / 10;
            if (up2 == 1) ty += t / 10;
            if (up2 == 0) ty -= t / 10;
            if (t > 60) t = 0;
            if (tx > x2) x2 += (tx - x2) / 60;
            if (tx < x2) x2 -= (x2 - tx) / 60;
            if (ty > y2) y2 += (ty - y2) / 60;
            if (ty < y2) y2 -= (y2 - ty) / 60;

            bubbles.Add((x2, y2));

            if (bubbles.Count > 100) bubbles.RemoveAt(0);
            
            foreach (var t1 in bubbles) move(t1);
        }

        private void move((float x, float y) bubble)
        {
            var (x, y) = bubble;

            if (x > width) up = 0;
            if (y > height) up2 = 0;
            if (x < 0) up = 1;
            if (y < 0) up2 = 1;

            ellipse(x, y, 50, 50);
        }

	}
    public class Program
    {
        [STAThread]
        static void Main()
        {
            using var game = new Worm();
            game.Run();
        }
    }
}
