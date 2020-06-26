using System;
using System.Collections.Generic;
using Monogame.Processing;

namespace Espiral
{
    public class Espiral : Processing
    {
        public Processing process;
        public int liquidity = 1;
        public int r = 2;

        public List<Particle> all = new List<Particle>();
        public float[] h;

        public override void Setup()
        {
            process = this;
            size(700, 700);
            h = new float[width];
            stroke(10f);
            background(255f);
            fill(255, 200);
        }

        public override void Draw()
        {
            background(255f);

            if (mouseY < height - h[constrain(mouseX, 0, width - 1)] && mousePressed)
            {
                for (int i = 0; i < 30; i++) all.Add(new Particle(mouseX, mouseY, r, h, process));
            }

            foreach (var par in all) par.show();
            
            for (int i = 0; i < all.Count; i++)
            {
                if (!all[i].dead) continue;
                all.RemoveAt(i);
                i--;
            }
            
            for (int i = 0; i < width; i++)
            {
                if (h[i] > 0) line(i, height, i, height - h[i]);
            }
            
            for (int n = 0; n < liquidity; n++)
            {
                for (int i = 1; i < width; i++)
                {
                    if (!(abs(h[i] - h[i - 1]) > 3)) continue;

                    float avg = (h[i] + h[i - 1]) / 2;
                    h[i] = avg;
                    h[i - 1] = avg;
                }
            }

        }

        public class Particle
        {
            private readonly Processing p;
            public bool dead;
            float x, y, vx, vy;
            float px, py;
            private readonly float[] h;

            public Particle(int xp, int yp, int r, float[] h2, Processing process)
            {
                p = process;

                h = h2;
                x = xp;
                y = yp;
                px = x;
                py = y;
                var d = p.random(TWO_PI);
                var l = p.random(r / PI, r);
                vx = p.cos(d) * l;
                vy = p.sin(d) * l;
            }

            public void show()
            {
                p.line(x, y, px, py);
                px = x;
                py = y;
                x += vx;
                y += vy;
                vy += 0.1f;
                if (x != p.constrain(x, 0, p.width - 1)) dead = true;
                else
                {
                    if (!(y > p.height - h[(int) x])) return;
                    h[(int)x] += 1;
                    dead = true;
                }
            }
        }
    }
    public class Program
    {
        [STAThread]
        static void Main()
        {
            using var game = new Espiral();
            game.Run();
        }
    }
}
