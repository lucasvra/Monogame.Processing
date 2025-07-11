using System;
using Monogame.Processing;

namespace Text
{
    public class Text : Processing
    {
        readonly string[] letters = 
        {
            "Δ", "!", "\"", "#", "$", "%", "&", "'", "(", ")", "*", "+", ",", "-", ".", "/",
            "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", ":", ";", "<", "=", ">", "?",
            "@", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P",
            "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "[", "\\", "]", "^", "_", "`", "a",
            "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r",
            "s", "t", "u", "v", "w", "x", "y", "z", "{", "|", "}", "~", "Ä", "Ö", "Ü", "ä",
            "ö", "ü", "ß"
        };

        public override void Setup()
        {
            surface.setResizable(true);
            size(600, 600);
            background(255f);
            textSize(12);
            FrameRate(30);
            textAlign(TextAlign.CENTER);
        }

        public override void Draw()
        {
            surface.setTitle($"FPS: {frameRate:00.00}");
            background(255f);
            var n = 9;
            var (x0, y0) = (10f, 10f);
            var (dx, dy) = ((width - x0 - x0) / n, (height - y0 - y0) / n);

            var d = dist(mouseX, mouseY, width / 2f, height / 2f);
            var dmax = dist(0, 0, width / 2f, height / 2f);

            var b = map(d, 0, dmax, 0, 255);

            for (var i = 0; i < n; i++)
            {
                var x = x0 + i * dx + dx / 2;
                for (var j = 0; j < n; j++)
                {
                    
                    var y = y0 + j * dy + dy / 2;

                    fill(200f);
                    ellipse(x, y, 0.8f * dx, 0.8f * dy);
                    fill(map(x, 0, width, 0, 255), map(y, 0, height, 0, 255), b);
                    text(letters[j * n + i], x, y-10);
                }
            }

            fill(0, 255, 0);
            text("FPS: " + frameRate, 20, 0);

        }

 
    }
    public class Program
    {
        [STAThread]
        static void Main()
        {
            using var game = new Text();
            game.Run();
        }
    }
}
