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

## Implementation

### Structure

- [*] draw()
- [*] exit()
- [*] loop()
- [*] noLoop()
- [*] pop()
- [*] popStyle()
- [*] push()
- [*] pushStyle()
- [*] redraw()
- [ ] setLocation()
- [ ] setResizable()
- [ ] setTitle()
- [*] setup()
- [ ] thread()


### Environment

- [ ] cursor()
- [ ] delay()
- [ ] displayDensity()
- [ ] focused
- [ ] frameCount
- [*] frameRate()
- [*] frameRate
- [*] fullScreen()
- [*] height
- [ ] noCursor()
- [*] noSmooth()
- [ ] pixelDensity()
- [ ] pixelHeight
- [ ] pixelWidth
- [ ] settings()
- [*] size()
- [*] smooth()
- [*] width

### Conversion

- [ ] binary()
- [ ] boolean()
- [ ] byte()
- [ ] char()
- [ ] float()
- [ ] hex()
- [ ] int()
- [ ] str()
- [ ] unbinary()
- [ ] unhex()

### String Functions

- [ ] join()
- [ ] match()
- [ ] matchAll()
- [ ] nf()
- [ ] nfc()
- [ ] nfp()
- [ ] nfs()
- [ ] split()
- [ ] splitTokens()
- [ ] trim()

### Array Functions

- [ ] append()
- [ ] arrayCopy()
- [ ] concat()
- [ ] expand()
- [ ] reverse()
- [ ] shorten()
- [ ] sort()
- [ ] splice()
- [ ] subset()

### Shape

- [ ] createShape()
- [ ] loadShape()
- [ ] PShape

### 2D Primitives

- [*] arc()
- [*] circle()
- [*] ellipse()
- [*] line()
- [*] point()
- [*] quad()
- [*] rect()
- [*] square()
- [*] triangle()

### Curves

- [*] bezier()
- [ ] bezierDetail()
- [ ] bezierPoint()
- [ ] bezierTangent()
- [*] curve()
- [ ] curveDetail()
- [ ] curvePoint()
- [ ] curveTangent()
- [ ] curveTightness()

### 3D Primitives

- [ ] box()
- [ ] sphere()
- [ ] sphereDetail()

### Attributes

- [*] ellipseMode()
- [*] rectMode()
- [ ] strokeCap()
- [ ] strokeJoin()
- [*] strokeWeight()

### Vertex

- [ ] beginContour()
- [ ] beginShape()
- [ ] bezierVertex()
- [ ] curveVertex()
- [ ] endContour()
- [ ] endShape()
- [ ] quadraticVertex()
- [ ] vertex()

### Loading & Displaying

- [ ] shape()
- [ ] shapeMode()

### Mouse

- [*] mouseButton
- [*] mouseClicked()
- [*] mouseDragged()
- [*] mouseMoved()
- [*] mousePressed()
- [*] mousePressed
- [*] mouseReleased()
- [*] mouseWheel()
- [*] mouseX
- [*] mouseY
- [*] pmouseX
- [*] pmouseY

### Keyboard

- [*] key
- [*] keyCode
- [*] keyPressed()
- [*] keyPressed
- [*] keyReleased()
- [*] keyTyped()

### Files

- [ ] BufferedReader
- [ ] createInput()
- [ ] createReader()
- [ ] launch()
- [ ] loadBytes()
- [ ] loadJSONArray()
- [ ] loadJSONObject()
- [ ] loadStrings()
- [ ] loadTable()
- [ ] loadXML()
- [ ] parseJSONArray()
- [ ] parseJSONObject()
- [ ] parseXML()
- [ ] selectFolder()
- [ ] selectInput()

### Time & Date

- [*] day()
- [*] hour()
- [*] millis()
- [*] minute()
- [*] month()
- [*] second()
- [*] year()

### Text Area

- [ ] print()
- [ ] printArray()
- [ ] println()

### Image

- [ ] save()
- [ ] saveFrame()

### Files

- [ ] beginRaw()
- [ ] beginRecord()
- [ ] createOutput()
- [ ] createWriter()
- [ ] endRaw()
- [ ] endRecord()
- [ ] PrintWriter
- [ ] saveBytes()
- [ ] saveJSONArray()
- [ ] saveJSONObject()
- [ ] saveStream()
- [ ] saveStrings()
- [ ] saveTable()
- [ ] saveXML()
- [ ] selectOutput()

### Transform

- [ ] applyMatrix()
- [*] popMatrix()
- [ ] printMatrix()
- [*] pushMatrix()
- [*] resetMatrix()
- [*] rotate()
- [*] rotateX()
- [*] rotateY()
- [*] rotateZ()
- [*] scale()
- [*] shearX()
- [*] shearY()
- [*] translate()

### Lights, Camera

- [ ] ambientLight()
- [ ] directionalLight()
- [ ] lightFalloff()
- [ ] lights()
- [ ] lightSpecular()
- [ ] noLights()
- [ ] normal()
- [ ] pointLight()
- [ ] spotLight()

### Camera

- [ ] beginCamera()
- [ ] camera()
- [ ] endCamera()
- [ ] frustum()
- [ ] ortho()
- [ ] perspective()
- [ ] printCamera()
- [ ] printProjection()

### Coordinates

- [ ] modelX()
- [ ] modelY()
- [ ] modelZ()
- [ ] screenX()
- [ ] screenY()
- [ ] screenZ()

### Material Properties

- [ ] ambient()
- [ ] emissive()
- [ ] shininess()
- [ ] specular()

### Color Setting

- [*] background()
- [ ] clear()
- [ ] colorMode()
- [*] fill()
- [*] noFill()
- [*] noStroke()
- [*] stroke()

### Creating & Reading

- [*] alpha()
- [*] blue()
- [*] brightness()
- [*] color()
- [*] green()
- [*] hue()
- [*] lerpColor()
- [*] red()
- [*] saturation()

### Image

- [*] createImage()
- [*] PImage

### Loading & Displaying

- [*] image()
- [ ] imageMode()
- [*] loadImage()
- [ ] noTint()
- [ ] requestImage()
- [ ] tint()
- [ ] Textures
- [ ] texture()
- [ ] textureMode()
- [ ] textureWrap()

### Pixels

- [ ] blend()
- [*] copy()
- [ ] filter()
- [*] get()
- [*] loadPixels()
- [*] pixels[]
- [*] set()
- [*] updatePixels()

### Rendering

- [ ] blendMode()
- [ ] clip()
- [ ] createGraphics()
- [ ] hint()
- [ ] noClip()
- [ ] PGraphics
- [ ] Shaders
- [ ] loadShader()
- [ ] PShader
- [ ] resetShader()
- [ ] shader()
- [ ] Typography
- [ ] PFont

### Loading & Displaying

- [ ] createFont()
- [ ] loadFont()
- [ ] text()
- [ ] textFont()

### Attributes

- [ ] textAlign()
- [ ] textLeading()
- [ ] textMode()
- [ ] textSize()
- [ ] textWidth()

### Metrics

- [ ] textAscent()
- [ ] textDescent()

### Math

- [*] PVector

### Calculation

- [*] abs()
- [*] ceil()
- [*] constrain()
- [*] dist()
- [*] exp()
- [*] floor()
- [*] lerp()
- [*] log()
- [*] mag()
- [*] map()
- [*] max()
- [*] min()
- [*] norm()
- [*] pow()
- [*] round()
- [*] sq()
- [*] sqrt()

### Trigonometry

- [*] acos()
- [*] asin()
- [*] atan()
- [*] atan2()
- [*] cos()
- [*] degrees()
- [*] radians()
- [*] sin()
- [*] tan()

### Random

- [ ] noise()
- [ ] noiseDetail()
- [ ] noiseSeed()
- [*] random()
- [ ] randomGaussian()
- [*] randomSeed()

### Constants

- [*] HALF_PI
- [*] PI
- [*] QUARTER_PI
- [*] TAU
- [*] TWO_PI
