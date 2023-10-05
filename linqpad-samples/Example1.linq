<Query Kind="Program">
  <NuGetReference Prerelease="true">MonoGame.Framework.WindowsDX</NuGetReference>
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
		if (d > 9)
		{
			circle(x += cos(r) * d, y += sin(r) * d, d / 4);
			f(x, y, r + d + t, d * .8f);
			f(x, y, r - d - t, d * .8f);
		}
	}

	public override void Setup()
	{
		size(720, 720);
		fill(0,0);
	}

	public override void Draw()
	{
		surface.setTitle($"FPS: {frameRate:00.00}");
		background(-1);
		t += .003f;
		for (r = 0; r < TAU; r += PI / 2) f(360, 360, r, 99);
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