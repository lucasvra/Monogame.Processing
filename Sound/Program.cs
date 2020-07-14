using System;
using Monogame.Processing;
using Monogame.Processing.Sound;

namespace Sound
{
	public class Sketch : Processing
    {
        const int numkeys = 18;
        readonly Oscilator[] notes = new Oscilator[numkeys];

        public override void Setup()
        {
            size(800, 300);
            strokeWeight(5);

            FrameRate(200);

            for (int note = 0; note < notes.Length; note++)
            {
                notes[note] = new SinOsc();
                notes[note].freq((float)(440.0 * Math.Pow(2f, (note - 9f) / 12.0f)));
            }
        }

        public override void Draw()
        {

            var w = width / numkeys;

            for (int note = 0; note < numkeys; note++)
            {
                var x = note * w;

                if (mousePressed && (mouseX >= x && mouseX <= (x + w)))
                {
                    fill(100f);
                    notes[note].play();
                }
                else
                {
                    fill(255f);
                    notes[note].stop();
                }

                rect(x, 5, w, height - 5);
            }
        }

    }

    public class Program
    {
        static void Main()
        {
            using (var game = new Sketch())
                game.Run();
        }
    }
}
