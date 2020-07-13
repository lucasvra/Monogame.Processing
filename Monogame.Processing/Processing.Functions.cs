using System;
using System.IO;
using Microsoft.Xna.Framework;

namespace Monogame.Processing
{
    /// <summary>
    /// 
    /// </summary>
    public abstract partial class Processing
    {
        public void saveFrame(string path = "")
        {
            if (path == string.Empty) path = $"screen-{frameCount:0000}.png";

            switch (Path.GetExtension(path))
            {
                case ".jpg":
                    using (var stream = File.Create(path))
                        _nextFrame.SaveAsJpeg(stream, _nextFrame.Width, _nextFrame.Height);
                    break;
                case ".png":
                    using (var stream = File.Create(path))
                        _nextFrame.SaveAsPng(stream, _nextFrame.Width, _nextFrame.Height);
                    break;
                default:
                    throw new Exception("Only jpg and png are supported");
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void cursor() => IsMouseVisible = true;

        /// <summary>
        /// 
        /// </summary>
        public void noCursor() => IsMouseVisible = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        public void textSize(float size) => _style.TextSize = size;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void text(string text, float x, float y) =>
            DrawText(new Vector2(x, y), text, _basicFont, _style.Fill, _style.TextSize);
            //_basicFont.DrawTextToTexture(text, _style.Fill, _style.TextSize, x, y);

        /// <summary>
        /// The delay() function halts for a specified time. Delay times are specified in thousandths of a second.
        /// For example, running delay(3000) will stop the program for three seconds and delay(500) will stop the
        /// program for a half-second.
        /// </summary>
        /// <param name="napTime">int: milliseconds to pause before running draw() again</param>
        public void delay(int napTime) => System.Threading.Thread.Sleep(napTime);

        /// <summary>
        /// 
        /// </summary>
        public void push()
        {
            pushStyle();
            pushMatrix();
        }

        /// <summary>
        /// 
        /// </summary>
        public void pop()
        {
            popStyle();
            popMatrix();
        }

        /// <summary>
        /// 
        /// </summary>
        public void pushMatrix() => _matrixStack.Push(_matrix);

        /// <summary>
        /// 
        /// </summary>
        public void popMatrix()
        {
            if (_matrixStack.Count <= 0) return;
            _matrix = _matrixStack.Pop();
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void resetMatrix()
        {
            _matrix = Matrix.Identity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n00"></param>
        /// <param name="n01"></param>
        /// <param name="n02"></param>
        /// <param name="n03"></param>
        /// <param name="n10"></param>
        /// <param name="n11"></param>
        /// <param name="n12"></param>
        /// <param name="n13"></param>
        /// <param name="n20"></param>
        /// <param name="n21"></param>
        /// <param name="n22"></param>
        /// <param name="n23"></param>
        /// <param name="n30"></param>
        /// <param name="n31"></param>
        /// <param name="n32"></param>
        /// <param name="n33"></param>
        public void applyMatrix(float n00, float n01, float n02, float n03, float n10, float n11, float n12, float n13, float n20, float n21, float n22, float n23, float n30, float n31, float n32, float n33)
        {
            var aux = new Matrix(n00, n01, n02, n03, n10, n11, n12, n13, n20, n21, n22, n23, n30, n31, n32, n33);
            _matrix *= aux;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n00"></param>
        /// <param name="n01"></param>
        /// <param name="n02"></param>
        /// <param name="n10"></param>
        /// <param name="n11"></param>
        /// <param name="n12"></param>
        public void applyMatrix(float n00, float n01, float n02, float n10, float n11, float n12)
        {
            applyMatrix(n00, n01, n02, 0.0f, n10, n11, n12, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
        }

        /// <summary>
        /// 
        /// </summary>
        public void printMatrix()
        {
            Console.WriteLine($"{_matrix.M11: 0.0000;-0.0000}  {_matrix.M12: 0.0000;-0.0000}  {_matrix.M13: 0.0000;-0.0000}  {_matrix.M14: 0.0000;-0.0000}");
            Console.WriteLine($"{_matrix.M21: 0.0000;-0.0000}  {_matrix.M22: 0.0000;-0.0000}  {_matrix.M23: 0.0000;-0.0000}  {_matrix.M24: 0.0000;-0.0000}");
            Console.WriteLine($"{_matrix.M31: 0.0000;-0.0000}  {_matrix.M32: 0.0000;-0.0000}  {_matrix.M33: 0.0000;-0.0000}  {_matrix.M34: 0.0000;-0.0000}");
            Console.WriteLine($"{_matrix.M41: 0.0000;-0.0000}  {_matrix.M42: 0.0000;-0.0000}  {_matrix.M43: 0.0000;-0.0000}  {_matrix.M44: 0.0000;-0.0000}");
        }

        /// <summary>
        /// The print() function writes to the console area, the black rectangle at the bottom of the Processing
        /// environment. This function is often helpful for looking at the data a program is producing.
        /// The companion function println() works like print(), but creates a new line of text for each call to
        /// the function. More than one parameter can be passed into the function by separating them with commas.
        /// Alternatively, individual elements can be separated with quotes ("") and joined with the addition
        /// operator (+).
        /// </summary>
        /// <param name="val"></param>
        public void print(params object[] val)
        {
            if (val.Length == 0) return;
            Console.Write(val[0]);
            for (var i = 1; i < val.Length; i++) Console.Write($" {val[i]}");
        }

        /// <summary>
        /// The println() function writes to the console area, the black rectangle at the bottom of the Processing environment.
        /// This function is often helpful for looking at the data a program is producing. Each call to this function creates
        /// a new line of output. More than one parameter can be passed into the function by separating them with commas.
        /// Alternatively, individual elements can be separated with quotes ("") and joined with the addition operator (+).
        /// </summary>
        /// <param name="val"></param>
        public void printLn(params object[] val)
        {
            print(val);
            Console.WriteLine();
        }

        /// <summary>
        /// The printArray() function writes array data to the text area of the Processing environment's
        /// console. A new line is put between each element of the array. This function can only print one
        /// dimensional arrays.
        /// </summary>
        /// <param name="array"></param>
        public void printArray(object[] array)
        {
            for (int i = 0; i < array.Length; i++) Console.WriteLine($"[{i}] {array[i]}");
        }

        /// <summary>
        /// 
        /// </summary>
        public void pushStyle() => _styleStack.Push(_style);

        /// <summary>
        /// 
        /// </summary>
        public void popStyle()
        {
            if (_styleStack.Count > 0) _style = _styleStack.Pop();
        }

        /// <summary>
        /// 
        /// </summary>
        public void exit() => Exit();

        /// <summary>
        /// 
        /// </summary>
        public void fullScreen()
        {
            size(GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height);
            _graphics.ToggleFullScreen();
        } 

        /// <summary>
        /// Executes the code within draw() one time. This functions allows the program to update the
        /// display window only when necessary, for example when an event registered by mousePressed()
        /// or keyPressed() occurs.
        ///
        /// In structuring a program, it only makes sense to call redraw() within events such as
        /// mousePressed(). This is because redraw() does not run draw() immediately(it only sets a flag
        /// that indicates an update is needed).
        ///
        /// The redraw() function does not work properly when called inside draw(). To enable/disable
        /// animations, use loop() and noLoop().
        /// </summary>
        public void redraw() => _redraw = true;

        /// <summary>
        /// Stops Processing from continuously executing the code within draw(). If loop() is called,
        /// the code in draw() begins to run continuously again. If using noLoop() in setup(), it should
        /// be the last line inside the block.
        ///
        /// When noLoop() is used, it's not possible to manipulate or access the screen inside event
        /// handling functions such as mousePressed() or keyPressed(). Instead, use those functions to
        /// call redraw() or loop(), which will run draw(), which can update the screen properly. This
        /// means that when noLoop() has been called, no drawing can happen, and functions like saveFrame()
        /// or loadPixels() may not be used.
        ///
        /// Note that if the sketch is resized, redraw() will be called to update the sketch, even after
        /// noLoop() has been specified.Otherwise, the sketch would enter an odd state until loop() was
        /// called.
        /// </summary>
        public void noLoop() => _draw = false;

        /// <summary>
        /// By default, Processing loops through draw() continuously, executing the code within it.
        /// However, the draw() loop may be stopped by calling noLoop(). In that case, the draw()
        /// loop can be resumed with loop().
        /// </summary>
        public void loop() => _draw = true;

        /// <summary>
        /// Specifies the number of frames to be displayed every second. For example, the function call
        /// frameRate(30) will attempt to refresh 30 times a second. If the processor is not fast enough
        /// to maintain the specified rate, the frame rate will not be achieved. Setting the frame rate
        /// within setup() is recommended. The default rate is 60 frames per second.
        /// </summary>
        /// <param name="fps">float: number of desired frames per second</param>
        public void FrameRate(float fps)
        {
            _maxFps = fps;
            TargetElapsedTime = TimeSpan.FromSeconds(1d / _maxFps);
        }


    }
}
