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

- [x] draw()
- [x] exit()
- [x] loop()
- [x] noLoop()
- [x] pop()
- [x] popStyle()
- [x] push()
- [x] pushStyle()
- [x] redraw()
- [ ] setLocation()
- [ ] setResizable()
- [ ] setTitle()
- [x] setup()
- [ ] thread()


### Environment

- [ ] cursor()
- [ ] delay()
- [ ] displayDensity()
- [ ] focused
- [ ] frameCount
- [x] frameRate()
- [x] frameRate
- [x] fullScreen()
- [x] height
- [ ] noCursor()
- [x] noSmooth()
- [ ] pixelDensity()
- [ ] pixelHeight
- [ ] pixelWidth
- [ ] settings()
- [x] size()
- [x] smooth()
- [x] width

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

- [x] arc()
- [x] circle()
- [x] ellipse()
- [x] line()
- [x] point()
- [x] quad()
- [x] rect()
- [x] square()
- [x] triangle()

### Curves

- [x] bezier()
- [ ] bezierDetail()
- [ ] bezierPoint()
- [ ] bezierTangent()
- [x] curve()
- [ ] curveDetail()
- [ ] curvePoint()
- [ ] curveTangent()
- [ ] curveTightness()

### 3D Primitives

- [ ] box()
- [ ] sphere()
- [ ] sphereDetail()

### Attributes

- [x] ellipseMode()
- [x] rectMode()
- [ ] strokeCap()
- [ ] strokeJoin()
- [x] strokeWeight()

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

- [x] mouseButton
- [x] mouseClicked()
- [x] mouseDragged()
- [x] mouseMoved()
- [x] mousePressed()
- [x] mousePressed
- [x] mouseReleased()
- [x] mouseWheel()
- [x] mouseX
- [x] mouseY
- [x] pmouseX
- [x] pmouseY

### Keyboard

- [x] key
- [x] keyCode
- [x] keyPressed()
- [x] keyPressed
- [x] keyReleased()
- [x] keyTyped()

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

- [x] day()
- [x] hour()
- [x] millis()
- [x] minute()
- [x] month()
- [x] second()
- [x] year()

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
- [x] popMatrix()
- [ ] printMatrix()
- [x] pushMatrix()
- [x] resetMatrix()
- [x] rotate()
- [x] rotateX()
- [x] rotateY()
- [x] rotateZ()
- [x] scale()
- [x] shearX()
- [x] shearY()
- [x] translate()

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

- [x] background()
- [ ] clear()
- [ ] colorMode()
- [x] fill()
- [x] noFill()
- [x] noStroke()
- [x] stroke()

### Creating & Reading

- [x] alpha()
- [x] blue()
- [x] brightness()
- [x] color()
- [x] green()
- [x] hue()
- [x] lerpColor()
- [x] red()
- [x] saturation()

### Image

- [x] createImage()
- [x] PImage

### Loading & Displaying

- [x] image()
- [ ] imageMode()
- [x] loadImage()
- [ ] noTint()
- [ ] requestImage()
- [ ] tint()
- [ ] Textures
- [ ] texture()
- [ ] textureMode()
- [ ] textureWrap()

### Pixels

- [ ] blend()
- [x] copy()
- [ ] filter()
- [x] get()
- [x] loadPixels()
- [x] pixels[]
- [x] set()
- [x] updatePixels()

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

- [x] PVector

### Calculation

- [x] abs()
- [x] ceil()
- [x] constrain()
- [x] dist()
- [x] exp()
- [x] floor()
- [x] lerp()
- [x] log()
- [x] mag()
- [x] map()
- [x] max()
- [x] min()
- [x] norm()
- [x] pow()
- [x] round()
- [x] sq()
- [x] sqrt()

### Trigonometry

- [x] acos()
- [x] asin()
- [x] atan()
- [x] atan2()
- [x] cos()
- [x] degrees()
- [x] radians()
- [x] sin()
- [x] tan()

### Random

- [ ] noise()
- [ ] noiseDetail()
- [ ] noiseSeed()
- [x] random()
- [ ] randomGaussian()
- [x] randomSeed()

### Constants

- [x] HALF_PI
- [x] PI
- [x] QUARTER_PI
- [x] TAU
- [x] TWO_PI
