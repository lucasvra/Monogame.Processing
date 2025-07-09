

using Monogame.Processing;
using System;

using var game = new Test();
game.Run();


public class Test : Processing
{
    public override void Setup()
    {
        size(600, 600);
        background(255f);

        stroke(255, 0, 0);
        strokeWeight(5);
        fill(0,0,255);
    }

    public override void Draw()
    {        
        beginShape();
        // Exterior part of shape, clockwise winding
        vertex(300 -160, 300 -160);
        vertex(300 +160, 300 -160);
        vertex(300 +160, 300 +160);
        vertex(300 -160, 300 +160);
        // Interior part of shape, counter-clockwise winding
        beginContour();
        vertex(300-80, 300-80);
        vertex(300-80, 300+80);
        vertex(300+80, 300+80);
        vertex(300+80, 300-80);
        endContour();
        endShape();
    }
}