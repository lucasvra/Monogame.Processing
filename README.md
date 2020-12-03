# Monogame.Processing
A library that implements Processing Language functions for 2D graphics on Monogame


To create a new sketch, it is necessary to create a class that inherits *Processing*
```csharp
public class Sketch : Processing
{
  public override void Setup()
  {
    // setup() code goes here
  }
  
  public override void Draw()
  {
    // draw() code goes here
  }
}
```

To run the sketch, it is necessary to use the *Run* method and the entry point of the program must have the attirbute STAThread
```csharp
[STAThread]
static void Main()
{
  using (var game = new Sketch())  game.Run();
}
```

## Implementation Notes

- Most 2D Primitives are implemented, but the drawing functions need optimization for better performance. 
- Some types as *color*, *PImage* and *PVector* are partially implemented.
- Many implemented functions were not tested, so bugs could occur.

For details, see the [Wiki](https://github.com/lucasvra/Monogame.Processing/wiki/Implementation) 

## Contributing

Feel encouraged to contribute to Monogame.Processing. Create your own fork, make the desired changes, commit, and make a pull request.

## License

MonoGame.Processing is released under the The MIT License (MIT).
