using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        public const int OPEN = (int)ArcMode.OPEN;
        public const int CHORD = (int)ArcMode.CHORD;
        public const int PIE = (int)ArcMode.PIE;
        public const int CENTER = (int)ShapeMode.CENTER;
        public const int RADIUS = (int)ShapeMode.RADIUS;
        public const int CORNER = (int)ShapeMode.CORNER;
        public const int CORNERS = (int)ShapeMode.CORNERS;

        #endregion

        #region Internal Variables
        private Random _rnd = new Random();
        private readonly Stopwatch _time = new Stopwatch();
        private float _maxFps;

        private readonly Stack<Style> _styleStack = new Stack<Style>();
        private Style _style;

        private readonly Stack<Matrix> _matrixStack = new Stack<Matrix>();
        private Matrix _matrix;

        private bool _draw = true;
        private bool _redraw = false;

        private int _lastFrameTime;
        private Primitives _primitives;
        private RenderTarget2D _lastFrame;
        readonly GraphicsDeviceManager _graphics;
        private MouseState pmouse;
        private KeyboardState pkeyboard;
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

        #endregion

        public abstract void Setup();

        public abstract void Draw();

        public virtual void Thread() { }

        public virtual void MousePressed() { }

        public virtual void MouseReleased() { }

        public virtual void MouseMoved() { }

        public virtual void MouseDragged() { }

        public virtual void MouseWheel(int count) { }

        public virtual void KeyPressed(Keys pkey) { }

        public virtual void KeyReleased(Keys pkey) { }

        public virtual void KeyTyped(Keys pkey) { }

        protected Processing()
        {
            surface = new Surface(Window);
            Window.ClientSizeChanged += Window_ClientSizeChanged;
            
            _style.EllipseMode = ShapeMode.CENTER;
            _style.RectMode = ShapeMode.CORNER;
            _style.Fill = Color.White;
            _style.Stroke = Color.Black;
            _style.StrokeWidth = 1;

            _maxFps = 60;

            _matrix = Matrix.Identity;

            _graphics = new GraphicsDeviceManager(this)
            {
                PreferMultiSampling = false,
                HardwareModeSwitch = false,
                PreferredBackBufferWidth = width,
                PreferredBackBufferHeight = height
            };

            _graphics.ApplyChanges();

            IsMouseVisible = true;
            IsFixedTimeStep = true;
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            size(Window.ClientBounds.Width, Window.ClientBounds.Width);
        }

        protected override void Update(GameTime gameTime)
        {
            Thread();
        }

        protected override void LoadContent()
        {
            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            
            _primitives = new Primitives(GraphicsDevice);
            PImage.graphicsDevice = GraphicsDevice;
            PImage.spriteBatch = _primitives.SpriteBatch;

            pmouse = Mouse.GetState();
            pkeyboard = Keyboard.GetState();

            _lastFrame = CreateRenderTarget();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            if (!_redraw && (!_draw || gameTime.TotalGameTime.TotalMilliseconds - _lastFrameTime < 1000 / _maxFps))
            {
                DrawToScreen(_lastFrame);
                base.Draw(gameTime);
                return;
            }

            if (frameCount == 0) _lastFrame = DrawToTexture(Setup, _lastFrame);

            frameRate = (float) Math.Round(1000.0 / (gameTime.TotalGameTime.TotalMilliseconds - _lastFrameTime), 1);
            _lastFrameTime = (int)gameTime.TotalGameTime.TotalMilliseconds;

            var target = DrawToTexture(() =>
            {
                UpdateMouse();
                UpdateKeyboard();
                Draw();
            }, _lastFrame);

            DrawToScreen(target);

            _lastFrame.Dispose();
            _lastFrame = target;

            base.Draw(gameTime);

            _redraw = false;
            frameCount++;
            Debug.WriteLine($"frameRate: {frameRate}");
        }

        private RenderTarget2D DrawToTexture(Action draw, Texture2D background)
        {
            var target = CreateRenderTarget();
            GraphicsDevice.SetRenderTarget(target);

            var sw = target.Width / (float) background.Width;
            var sh = target.Height / (float) background.Height;

            _primitives.SpriteBatch.Begin();
            _primitives.SpriteBatch.Draw(background, Vector2.Zero, null, Color.White, 0, Vector2.Zero,
                new Vector2(sw, sh), SpriteEffects.None, 0);
            _primitives.SpriteBatch.End();

            draw();

            GraphicsDevice.SetRenderTarget(null);
            CheckResize();
            return target;
        }
        private RenderTarget2D CreateRenderTarget()
        {
            var target = new RenderTarget2D(
                GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth,
                GraphicsDevice.PresentationParameters.BackBufferHeight, false,
                GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
            return target;
        }
        private void DrawToScreen(RenderTarget2D frame)
        {
            var sw = _graphics.PreferredBackBufferWidth / (float) frame.Width;
            var sh = _graphics.PreferredBackBufferHeight / (float) frame.Height;


            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.DepthStencilState = new DepthStencilState { DepthBufferEnable = true };

            _primitives.SpriteBatch.Begin();
            _primitives.SpriteBatch.Draw(frame, Vector2.Zero, null, Color.White, 0, Vector2.Zero, new Vector2(sw, sh), SpriteEffects.None, 0);
            _primitives.SpriteBatch.End();
        }

        private void CheckResize()
        {
            if (_graphics.PreferredBackBufferWidth == width && _graphics.PreferredBackBufferHeight == height) return;

            _graphics.PreferredBackBufferWidth = width; 
            _graphics.PreferredBackBufferHeight = height; 
            _graphics.ApplyChanges();

            _primitives.World = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0, 10);
        }

        private void UpdateKeyboard()
        {
            var keyboard = Keyboard.GetState();

            keyPressed = false;
            foreach (var pkey in keyboard.GetPressedKeys().Except(pkeyboard.GetPressedKeys()))
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

            foreach (var pkey in pkeyboard.GetPressedKeys().Except(keyboard.GetPressedKeys()))
            {
                KeyReleased(pkey);
                if (_letterKeys.ContainsKey(pkey)) KeyTyped(pkey);
            }

            pkeyboard = keyboard;
        }
        private void UpdateMouse()
        {
            var mouse = Mouse.GetState();

            pmouseX = mouseX;
            pmouseY = mouseY;

            mouseX = mouse.X;
            mouseY = mouse.Y;

            if (mouse.LeftButton == ButtonState.Pressed) mouseButton = LEFT;
            else if (mouse.RightButton == ButtonState.Pressed) mouseButton = RIGHT;
            else if (mouse.MiddleButton == ButtonState.Pressed) mouseButton = CENTER;
            else mouseButton = 0;

            mousePressed = mouseButton != 0;

            if (mousePressed) MousePressed();
            else MouseReleased();

            if (pmouseY != mouseY || pmouseX != mouseX)
            {
                MouseMoved();
                if (pmouse.LeftButton == ButtonState.Pressed && mouse.LeftButton == ButtonState.Pressed)
                    MouseDragged();
            }

            if (pmouse.HorizontalScrollWheelValue != mouse.HorizontalScrollWheelValue)
                MouseWheel(mouse.HorizontalScrollWheelValue - pmouse.HorizontalScrollWheelValue);

            pmouse = mouse;
        }
    }
}
