<Query Kind="Program">
  <NuGetReference>MonoGame.Framework.WindowsDX</NuGetReference>
  <NuGetReference>Monogame.Processing</NuGetReference>
  <Namespace>Microsoft.Xna.Framework.Input</Namespace>
  <Namespace>Monogame.Processing</Namespace>
  <Namespace>static global::UserQuery</Namespace>
  <Namespace>System</Namespace>
</Query>

// Credits: https://twitter.com/VolfeganGeist

public class Star : Processing
{
	float i, c, t = 255, x, y, R, T;

	public override void Setup()
	{
		size(1080, 720); 
		stroke(t, t, 0);
}

	public override void Draw()
	{
		surface.setTitle($"FPS: {frameRate:00.00}");
    translate(560,360);
    background(0); 
		t -= .01f; 
		for (i = 2e3f; i > 0; i--)
		{
			c = t / 3 * (i % t < t / 8 ? 1 : 0); R = pow(i, .7f); T = pow(i, .5f);
			x = c * sin(R) + t * cos(T);
			y = c * cos(R) + t * sin(T) * cos((c == 0 ? 1 : 0) * i);
			rect(x, y, 8 * sin(i * 160 - 4 * t), 8 * sin(i * 160 - 4 * t));
		}

	}

}
public class Program
{
	[STAThread]
	static void Main()
	{
		using (var game = new Star()) game.Run();
	}
}