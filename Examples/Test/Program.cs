

using Monogame.Processing;
using System;

using var game = new Test();
game.Run();


public class Test : Processing
{
    float x = 300, y = 300, angulo = 0;
    public override void Setup()
    {
        size(600, 600);
    }

    public override void Draw()
    {
        background(255f);
        translate(x, y);
        rotate(angulo);
       
        
        rect(-15, -15, 30, 30);

        if(keyPressed)
        {
            if (key == 'q') angulo -= 0.1f;
            if (key == 'e') angulo += 0.1f;
            if (key == 'w') y -= 1.0f;
            if (key == 's') y += 1.0f;
            if (key == 'a') x -= 1.0f;
            if (key == 'd') x += 1.0f;
        }
    }
}