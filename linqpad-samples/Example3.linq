<Query Kind="Program">
  <NuGetReference Prerelease="true">MonoGame.Framework.DesktopGL</NuGetReference>
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

	void f(float x, float y, float r, float d)
	{
		if (d > 9)
		{
			line(x, y, x += cos(r) * d, y += sin(r) * d);
			f(x, y, r + d + t, d / 1.3f);
			f(x, y, r - d - t, d / 1.3f);
		}
	}
	
	public override void Setup()
	{
		size(720, 720); 
		stroke(-1, 72);
	}

	public override void Draw()
	{
		surface.setTitle($"FPS: {frameRate:00.00}");
		
		background(0); 
		t += .003f;
		for (r = 0; r < 7; r += PI / 3) f(360, 360, r, 160);
	}

}

public class Program
{
	static void Main()
	{
		using (var game = new Sketch()) game.Run();
	}
}