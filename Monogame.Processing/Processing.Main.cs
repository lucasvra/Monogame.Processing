using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Monogame.Processing
{
    public abstract partial class Processing : Game
    {
        public class Surface
        {
            private readonly GameWindow _window;

            public Surface(GameWindow window) => _window = window;

            public void setLocation(int x, int y) => _window.Position = new Point(x, y);
            public void setResizable(bool val) => _window.AllowUserResizing = val;
            public void setTitle(string title) => _window.Title = title;
        }

        #region Constants

        public const float HALF_PI = MathHelper.PiOver2;
        public const float PI = MathHelper.Pi;
        public const float QUARTER_PI = MathHelper.PiOver4;
        public const float TWO_PI = MathHelper.TwoPi;
        public const float TAU = 6.28318530717958647693f;

        public const int LEFT = (int)MouseButton.LEFT;
        public const int RIGHT = (int)MouseButton.RIGHT;
        public const ArcMode OPEN = ArcMode.OPEN;
        public const ArcMode CHORD = ArcMode.CHORD;
        public const ArcMode PIE = ArcMode.PIE;
        public const int CENTER = (int)ShapeMode.CENTER;
        public const int RADIUS = (int)ShapeMode.RADIUS;
        public const int CORNER = (int)ShapeMode.CORNER;
        public const int CORNERS = (int)ShapeMode.CORNERS;

        public const BlendMode BLEND = BlendMode.BLEND;
        public const BlendMode ADD = BlendMode.ADD;
        public const BlendMode SUBTRACT = BlendMode.SUBTRACT;
        public const BlendMode DARKEST = BlendMode.DARKEST;
        public const BlendMode LIGHTEST = BlendMode.LIGHTEST;
        public const BlendMode DIFFERENCE = BlendMode.DIFFERENCE;
        public const BlendMode EXCLUSION = BlendMode.EXCLUSION;
        public const BlendMode MULTIPLY = BlendMode.MULTIPLY;
        public const BlendMode SCREEN = BlendMode.SCREEN;
        public const BlendMode OVERLAY = BlendMode.OVERLAY;
        public const BlendMode HARD_LIGHT = BlendMode.HARD_LIGHT;
        public const BlendMode SOFT_LIGHT = BlendMode.SOFT_LIGHT;
        public const BlendMode DODGE = BlendMode.DODGE;
        public const BlendMode BURN = BlendMode.BURN;

        #endregion

        #region Internal Variables
        private Random _rnd = new Random();
        private PerlinNoise _perlin = new PerlinNoise();
        private readonly Stopwatch _time = new Stopwatch();
        private float _maxFps;

        private readonly Stack<Style> _styleStack = new Stack<Style>();
        private Style _style;

        private readonly Stack<Matrix> _matrixStack = new Stack<Matrix>();
        private Matrix _matrix;

        private bool _draw = true;
        private bool _redraw;

        private int _lastFrameTime;
        private SpriteFont _basicFont;
        private RenderTarget2D _lastFrame;
        private RenderTarget2D _nextFrame;
        readonly GraphicsDeviceManager _graphics;
        private MouseState _pmouse;
        private KeyboardState _pkeyboard;

        private Matrix _world;
        private Texture2D _pixel;
        private SpriteBatch _spriteBatch;
        private BasicEffect _basicEffect;

        readonly SamplerState _ssAnsiostropicClamp = new SamplerState()
        {
            Filter = TextureFilter.Anisotropic,
            AddressU = TextureAddressMode.Clamp,
            AddressV = TextureAddressMode.Clamp,
            AddressW = TextureAddressMode.Clamp,
        };
        
        private readonly Dictionary<Keys, string> _letterKeys = new Dictionary<Keys, string>
        {
            {Keys.A,"A"}, {Keys.B, "B"}, {Keys.C, "C"}, {Keys.D, "D"}, {Keys.E, "E"}, {Keys.F, "F"},
            {Keys.G,"G"}, {Keys.H, "H"}, {Keys.I, "I"}, {Keys.J, "J"}, {Keys.K, "K"}, {Keys.L, "L"},
            {Keys.M,"M"}, {Keys.N, "N"}, {Keys.O, "O"}, {Keys.P, "P"}, {Keys.Q, "Q"}, {Keys.R, "R"},
            {Keys.S,"S"}, {Keys.T, "T"}, {Keys.U, "U"}, {Keys.V, "V"}, {Keys.X, "X"}, {Keys.Z, "Z"},
            {Keys.W,"W"}, {Keys.Y, "Y"},
        };
        #endregion

        #region External Variables

        public int mouseX { get; private set; } = 0;
        public int mouseY { get; private set; } = 0;
        public int pmouseX { get; private set; } = 0;
        public int pmouseY { get; private set; } = 0;
        public int mouseButton { get; private set; } = 0;
        public int width { get; private set; } = 300;
        public int height { get; private set; } = 300;
        public int frameCount { get; private set; } = 0;
        public float frameRate { get; private set; } = 0;
        public bool mousePressed { get; private set; } = false;
        public bool keyPressed { get; private set; } = false;
        public Keys keyCode { get; private set; } = 0;
        public char key { get; private set; }

        public readonly Surface surface;
        public color[] pixels { get; private set; }

        public TouchLocation[] touches { get; private set; } = new TouchLocation[0];

        #endregion

        #region Processing Functions and Events

        /// <summary>
        /// The setup() function is run once, when the program starts. It's used to define initial environment properties
        /// such as screen size and to load media such as images and fonts as the program starts. There can only be one
        /// setup() function for each program and it shouldn't be called again after its initial execution.
        ///
        /// If the sketch is a different dimension than the default, the size() function or fullScreen() function must be
        /// the first line in setup().
        ///
        /// Note: Variables declared within setup() are not accessible within other functions, including draw().
        /// </summary>
        public abstract void Setup();

        /// <summary>
        /// Called directly after setup(), the draw() function continuously executes the lines of code contained inside
        /// its block until the program is stopped or noLoop() is called. draw() is called automatically and should never
        /// be called explicitly. All Processing programs update the screen at the end of draw(), never earlier.
        /// 
        /// To stop the code inside of draw() from running continuously, use noLoop(), redraw() and loop(). If noLoop()
        /// is used to stop the code in draw() from running, then redraw() will cause the code inside draw() to run a
        /// single time, and loop() will cause the code inside draw() to resume running continuously.
        /// 
        /// The number of times draw() executes in each second may be controlled with the frameRate() function.
        /// 
        /// It is common to call background() near the beginning of the draw() loop to clear the contents of the window,
        /// as shown in the first example above. Since pixels drawn to the window are cumulative, omitting background()
        /// may result in unintended results.
        /// 
        /// There can only be one draw() function for each sketch, and draw() must exist if you want the code to run
        /// continuously, or to process events such as mousePressed(). Sometimes, you might have an empty call to draw()
        /// in your program, as shown in the second example above.
        /// </summary>
        public abstract void Draw();

        /// <summary>
        /// Processing sketches follow a specific sequence of steps: setup() first, followed by draw() over and over and
        /// over again in a loop. A thread is also a series of steps with a beginning, a middle, and an end. A Processing
        /// sketch is a single thread, often referred to as the "Animation" thread. Other threads' sequences, however, can
        /// run independently of the main animation loop. In fact, you can launch any number of threads at one time, and they
        /// will all run concurrently.
        /// 
        /// You cannot draw to the screen from a function called by thread(). Because it runs independently, the code will
        /// not be synchronized to the animation thread, causing strange or at least inconsistent results.Use thread() to
        /// load files or do other tasks that take time.When the task is finished, set a variable that indicates the task
        /// is complete, and check that from inside your draw() method.
        /// 
        /// Processing uses threads quite often, such as with library functions like captureEvent() and movieEvent(). These
        /// functions are triggered by a different thread running behind the scenes, and they alert Processing whenever they
        /// have something to report.This is useful when you need to perform a task that takes too long and would slow down
        /// the main animation's frame rate, such as grabbing data from the network. If a separate thread gets stuck or has
        /// an error, the entire program won't grind to a halt, since the error only stops that individual thread.
        ///
        /// Writing your own thread can be a complex endeavor that involves extending the Java Thread class. However, the
        /// thread() method is a quick and dirty way to implement a simple thread in Processing.By passing in a String that
        /// matches the name of a function declared elsewhere in the sketch, Processing will execute that function in a separate
        /// thread.
        /// </summary>
        public virtual void Thread() { }

        /// <summary>
        /// The mousePressed() function is called once after every time a mouse button is pressed. The mouseButton variable
        /// (see the related reference entry) can be used to determine which button has been pressed.
        ///
        /// Mouse and keyboard events only work when a program has draw(). Without draw(), the code is only run once and then
        /// stops listening for events.
        /// </summary>
        public virtual void MousePressed() { }

        /// <summary>
        /// The mouseReleased() function is called every time a mouse button is released.
        ///
        /// Mouse and keyboard events only work when a program has draw(). Without draw(), the code is only run once and then
        /// stops listening for events.
        /// </summary>
        public virtual void MouseReleased() { }

        /// <summary>
        /// The mouseMoved() function is called every time the mouse moves and a mouse button is not pressed. (If a button
        /// is being pressed, mouseDragged() is called instead.)
        ///
        /// Mouse and keyboard events only work when a program has draw(). Without draw(), the code is only run once and then
        /// stops listening for events.
        /// </summary>
        public virtual void MouseMoved() { }

        /// <summary>
        /// The mouseDragged() function is called once every time the mouse moves while a mouse button is pressed. (If a button
        /// is not being pressed, mouseMoved() is called instead.)
        /// 
        /// Mouse and keyboard events only work when a program has draw(). Without draw(), the code is only run once and then
        /// stops listening for events.
        /// </summary>
        public virtual void MouseDragged() { }

        /// <summary>
        /// The code within the mouseWheel() event function is run when the mouse wheel is moved. (Some mice don't have wheels
        /// and this function is only applicable with mice that have a wheel.) The getCount() function used within mouseWheel()
        /// returns positive values when the mouse wheel is rotated down (toward the user), and negative values for the other
        /// direction (up or away from the user). On OS X with "natural" scrolling enabled, the values are opposite.
        ///
        /// Mouse and keyboard events only work when a program has draw(). Without draw(), the code is only run once and then
        /// stops listening for events.
        /// </summary>
        /// <param name="count"></param>
        public virtual void MouseWheel(int count) { }

        /// <summary>
        /// The mouseClicked() function is called after a mouse button has been pressed and then released.
        ///
        /// Mouse and keyboard events only work when a program has draw(). Without draw(), the code is only run once and then stops
        /// listening for events.
        /// </summary>
        public virtual void MouseClicked() { }

        /// <summary>
        /// The keyPressed() function is called once every time a key is pressed. The key that was pressed is stored in the key variable.
        /// </summary>
        /// <param name="pkey"></param>
        public virtual void KeyPressed(Keys pkey) { }

        /// <summary>
        /// The keyReleased() function is called once every time a key is released. The key that was released will be stored in the key
        /// variable. See key and keyCode for more information.
        /// </summary>
        /// <param name="pkey"></param>
        public virtual void KeyReleased(Keys pkey) { }

        /// <summary>
        /// The keyTyped() function is called once every time a key is pressed, but action keys such as Ctrl, Shift, and Alt are ignored.
        /// </summary>
        /// <param name="pkey"></param>
        public virtual void KeyTyped(Keys pkey) { }

        protected virtual void TouchMoved(TouchLocation touch) { }

        protected virtual void TouchEnded(TouchLocation touch) { }

        protected virtual void TouchStarted(TouchLocation touch) { }

        #endregion

        protected Processing()
        {
            pixels = new color[0];
            surface = new Surface(Window);
            Window.ClientSizeChanged += Window_ClientSizeChanged;
            
            _style.EllipseMode = ShapeMode.CENTER;
            _style.RectMode = ShapeMode.CORNER;
            _style.Fill = Color.White;
            _style.Tint = Color.White;
            _style.Stroke = Color.Black;
            _style.StrokeWidth = 1;
            _style.TextSize = 12;
            _style.BlendMode = BlendState.NonPremultiplied;

            _maxFps = 60;

            _matrix = Matrix.Identity;

            _graphics = new GraphicsDeviceManager(this)
            {
                PreferMultiSampling = true,
                HardwareModeSwitch = false,
                SynchronizeWithVerticalRetrace = false,
                PreferredBackBufferWidth = width,
                PreferredBackBufferHeight = height,
                GraphicsProfile = GraphicsProfile.HiDef
            };

            sides = 30;
            _matrix = Matrix.Identity;

            IsMouseVisible = true;
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1d / _maxFps);
            _time.Start();
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            if(Window.AllowUserResizing) size(Window.ClientBounds.Width, Window.ClientBounds.Height);
        }

        protected override void Update(GameTime gameTime) => Thread();

        protected override void LoadContent()
        {
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            GraphicsDevice.SamplerStates[0] = _ssAnsiostropicClamp;
            GraphicsDevice.PresentationParameters.MultiSampleCount = 2;
            _world = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0, 10);

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _basicEffect = new BasicEffect(GraphicsDevice)
            {
                Alpha = 1.0f,
                VertexColorEnabled = true,
                LightingEnabled = false,
                World = _world
            };

            _pixel = new Texture2D(_spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            _pixel.SetData(new[] { Color.White });

            PImage.graphicsDevice = GraphicsDevice;
            PImage.spriteBatch = _spriteBatch;

            //_basicFont = new BasicFontTexture(GraphicsDevice, _spriteBatch);
            _basicFont = (new EmbeddedResourceContentManager(GraphicsDevice)).Load<SpriteFont>("font");

            _pmouse = Mouse.GetState();
            _pkeyboard = Keyboard.GetState();

            _lastFrame = CreateRenderTarget(null);
            _nextFrame = CreateRenderTarget(null);
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Black);

            if (!_redraw && !_draw )
            {
                DrawToTarget(_lastFrame);
                base.Draw(gameTime);
                return;
            }

            // reset transform
            _matrix = Matrix.Identity;

            frameRate = (float) 1000.0 / (_time.ElapsedMilliseconds - _lastFrameTime);
            _lastFrameTime = (int) _time.ElapsedMilliseconds;

            // draw to frame
            _nextFrame = CreateRenderTarget(_nextFrame);
            GraphicsDevice.SetRenderTarget(_nextFrame);
            GraphicsDevice.Clear(Color.Black);

            var sw = _nextFrame.Width / (float)_lastFrame.Width;
            var sh = _nextFrame.Height / (float)_lastFrame.Height;

            _spriteBatch.Begin();
            _spriteBatch.Draw(_lastFrame, Vector2.Zero, null, Color.White, 0, Vector2.Zero, new Vector2(sw, sh), SpriteEffects.None, 0);
            _spriteBatch.End();

            PImage.currentTarget = _nextFrame;

            if (frameCount == 0) Setup();
            else
            {
                UpdateMouse();
                UpdateKeyboard();
                UpdateTouch();
                Draw();
            }
            
            GraphicsDevice.SetRenderTarget(null);
            CheckResize();

            DrawToTarget(_nextFrame);


            // reuse frame
            var aux = _nextFrame;
            _nextFrame = _lastFrame;
            _lastFrame = aux;

            base.Draw(gameTime);

            _redraw = false;
            frameCount++;
        }

        private RenderTarget2D CreateRenderTarget(RenderTarget2D frame)
        {
            if (frame != null && (frame.Width == GraphicsDevice.PresentationParameters.BackBufferWidth &&
                 frame.Height == GraphicsDevice.PresentationParameters.BackBufferHeight)) return frame;

            if (frame != null && !frame.IsDisposed) frame.Dispose();

            return new RenderTarget2D(
                GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth,
                GraphicsDevice.PresentationParameters.BackBufferHeight, false,
                GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
        }
        private void DrawToTarget(Texture2D frame, RenderTarget2D target = null)
        {
            var sw = _graphics.PreferredBackBufferWidth  / (float) frame.Width;
            var sh = _graphics.PreferredBackBufferHeight / (float) frame.Height;

            GraphicsDevice.SetRenderTarget(target);
            GraphicsDevice.DepthStencilState = new DepthStencilState
            {
                DepthBufferEnable = true
            };

            _spriteBatch.Begin();
            _spriteBatch.Draw(frame, Vector2.Zero, null, Color.White, 0, Vector2.Zero, new Vector2(sw, sh), SpriteEffects.None, 0);
            _spriteBatch.End();
        }
        private void CheckResize()
        {
            if (_graphics.PreferredBackBufferWidth == width && _graphics.PreferredBackBufferHeight == height) return;

            _graphics.PreferredBackBufferWidth = width; 
            _graphics.PreferredBackBufferHeight = height; 
            _graphics.ApplyChanges();

            _world = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0, 10);
        }
        private void UpdateKeyboard()
        {
            var keyboard = Keyboard.GetState();

            keyPressed = false;
            foreach (var pkey in keyboard.GetPressedKeys().Except(_pkeyboard.GetPressedKeys()))
            {
                KeyPressed(pkey);
                keyPressed = true;
                keyCode = pkey;
                if (!_letterKeys.ContainsKey(pkey)) continue;

                if (keyboard.CapsLock || keyboard[Keys.RightShift] == KeyState.Down || keyboard[Keys.LeftShift] == KeyState.Down)
                    key = _letterKeys[pkey][0];
                else
                    key = _letterKeys[pkey].ToLower()[0];
            }

            foreach (var pkey in _pkeyboard.GetPressedKeys().Except(keyboard.GetPressedKeys()))
            {
                KeyReleased(pkey);
                if (_letterKeys.ContainsKey(pkey)) KeyTyped(pkey);
            }

            _pkeyboard = keyboard;
        }
        private void UpdateMouse()
        {
            var mouse = Mouse.GetState();

            pmouseX = mouseX;
            pmouseY = mouseY;

            mouseX = mouse.X;
            mouseY = mouse.Y;

            mouseButton = 0;
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                mouseButton = LEFT;
                MousePressed();
            }

            if (mouse.RightButton == ButtonState.Pressed)
            {
                mouseButton = RIGHT;
                MousePressed();
            }

            if (mouse.MiddleButton == ButtonState.Pressed)
            {
                mouseButton = CENTER;
                MousePressed();
            }
            
            mousePressed = mouseButton != 0;
            if (!mousePressed) MouseReleased();

            // clicked
            if (mouse.LeftButton == ButtonState.Released && _pmouse.LeftButton == ButtonState.Pressed) MouseClicked();
            if (mouse.RightButton == ButtonState.Released && _pmouse.RightButton == ButtonState.Pressed) MouseClicked();
            if (mouse.MiddleButton == ButtonState.Released && _pmouse.MiddleButton == ButtonState.Pressed) MouseClicked();

            if (pmouseY != mouseY || pmouseX != mouseX)
            {
                if (_pmouse.LeftButton == ButtonState.Pressed && mouse.LeftButton == ButtonState.Pressed)
                    MouseDragged();
                else MouseMoved();
            }

            if (_pmouse.HorizontalScrollWheelValue != mouse.HorizontalScrollWheelValue)
                MouseWheel(mouse.HorizontalScrollWheelValue - _pmouse.HorizontalScrollWheelValue);

            _pmouse = mouse;
        }

        private void UpdateTouch()
        {
            touches = TouchPanel.GetState().Select(t => t).ToArray();

            foreach (var touch in touches.Where(t => t.State == TouchLocationState.Pressed))
                TouchStarted(touch);

            foreach (var touch in touches.Where(t => t.State == TouchLocationState.Released))
                TouchEnded(touch);

            foreach (var touch in touches.Where(t => t.State == TouchLocationState.Moved))
                TouchMoved(touch);
        }
    }
}
