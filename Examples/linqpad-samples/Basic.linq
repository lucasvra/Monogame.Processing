<Query Kind="Program">
  <NuGetReference Prerelease="true">MonoGame.Framework.DesktopGL</NuGetReference>
  <NuGetReference>Monogame.Processing</NuGetReference>
  <Namespace>Microsoft.Xna.Framework.Input</Namespace>
  <Namespace>Monogame.Processing</Namespace>
  <Namespace>static global::UserQuery</Namespace>
  <Namespace>System</Namespace>
</Query>

public class Sketch : Processing
{

	public override void Setup()
	{

	}

	public override void Draw()
	{
		
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