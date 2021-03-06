﻿using System;
using Microsoft.Xna.Framework.Input;
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
            var p = (x - (width / 2f))/ (width / 2f);


            surface.setTitle($"FPS: {frameRate:00.00}");

            var theta = vy < 0 ? TWO_PI - acos(vx / sqrt(sq(vx) + sq(vy))) : acos(vx / sqrt(sq(vx) + sq(vy)));
            var angle = radians(120);

            strokeWeight(1);
            stroke(0);
            fill(0, 100, 0, 200);
            arc(x, y, 50, 50, theta - angle, theta + angle, CHORD);

            x += vx;
            y += vy;

            if (x < 0 || x > width) vx *= -1;
            if (y < 0 || y > height) vy *= -1;

        }

        public override void MouseDragged()
        {
            vx = mouseX - pmouseX;
            vy = mouseY - pmouseY;
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
