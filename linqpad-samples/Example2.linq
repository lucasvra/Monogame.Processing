<Query Kind="Program">
  <NuGetReference>MonoGame.Framework.WindowsDX</NuGetReference>
  <NuGetReference>Monogame.Processing</NuGetReference>
  <Namespace>Microsoft.Xna.Framework.Input</Namespace>
  <Namespace>Monogame.Processing</Namespace>
  <Namespace>static global::UserQuery</Namespace>
  <Namespace>System</Namespace>
</Query>

// Credits: https://twitter.com/Hau_kun

public class Sketch : Processing
{
	float r, t;
	void setup() {  }

	void f(float x, float y, float r, float d)
	{
		if (d > 3)
		{
			line(x, y, x += cos(r) * d, y += sin(r) * d);
			f(x, y, r + y / 99 - sin(r + t) + t, d * .7f);
			f(x, y, r - y / 99 + sin(r - t) - t, d * .7f);
		}
	}

	public override void Setup()
	{
		size(720,720);
		stroke(-1);
	}

	public override void Draw()
	{
		surface.setTitle($"FPS: {frameRate:00.00}");
		background(0f);
		t += .003f;
		for (r = 0; r < 6; r += PI / 3) f(360, 360, r, 110);
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