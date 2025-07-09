using System;
using Microsoft.Xna.Framework.Input;
using Monogame.Processing;

namespace P3
{
    public class P3 : Processing
    {
        int posX, posY;
        int pas1, pas2;
        int tam;
        int G, B;

        public override void Setup()
        {
            fullScreen();
            background(103, 128, 159);

            (pas1, pas2) = (5, 7);
            (posX, posY) = (350, 300);
            (G, B) = (100, 200);
        }

        public override void Draw()
        {
            surface.setTitle($"FPS: {frameRate:00.00}");
            posX += pas1;
            if (posX >= width - 100)
            {
                
                pas1 *= -1;
                G = (int)random(100, 225);
                B = (int)random(200, 255);
            }

            if (posX <= 100)
            {
                pas1 *= -1;
                G = (int)random(100, 225);
                B = (int)random(200, 255);
            }

            posY += pas2;
            if (posY >= height - 100)
            {
                pas2 *= -1;
                G = (int)random(100, 225);
                B = (int)random(200, 255);
            }

            if (posY <= 100)
            {
                pas2 *= -1;
                G = (int)random(100, 225);
                B = (int)random(200, 255);
            }

            stroke(0, G, B);
            strokeWeight(150);
            rect(0, 0, width, height);
            noStroke();
            fill(0, G, B);
            ellipse(posX, posY, 40, 40);
            fill(0, G, B, 30);
            ellipse(posX, posY, tam, tam);
            fill(255, 255, 255, 98);
            ellipse(posX, posY, 20, 20);
        }

        public override void MousePressed()
        {
            pas1 = (int)random(-10, 10);
            pas2 = (int)random(-10, 10);
            tam = (int)random(5, 10);
            posX = mouseX;
            posY = mouseY;
            tam = (int)random(40, 50);
        }

        public override void KeyPressed(Keys pkey)
        {
            if(pkey == Keys.Escape) exit();
        }
    }
    public class Program
    {
        [STAThread]
        static void Main()
        {
            using var game = new P3();
            game.Run();
        }
    }
}
