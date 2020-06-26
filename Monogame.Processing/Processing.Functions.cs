using System;
using System.Linq;
using Microsoft.Xna.Framework;
using static System.Math;

namespace Monogame.Processing
{
    public abstract partial class Processing
    {
        public void push()
        {
            pushStyle();
            pushMatrix();
        }

        public void pop()
        {
            popStyle();
            popMatrix();
        }
        public void pushMatrix() => _matrixStack.Push(_matrix);

        public void popMatrix()
        {
            if (_matrixStack.Count > 0)
            {
                _matrix = _matrixStack.Pop();
                _primitives.TransformMat = _matrix;
            }
        }

        public void resetMatrix()
        {
            _matrix = Matrix.Identity;
            _primitives.TransformMat = _matrix;
        }

        public void pushStyle() => _styleStack.Push(_style);

        public void popStyle()
        {
            if (_styleStack.Count > 0) _style = _styleStack.Pop();
        }

        public void exit() => Exit();

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
        public void FrameRate(float fps) => _maxFps = fps;

        /// <summary>
        /// Draws all geometry with smooth (anti-aliased) edges. This behavior is the default, 
        /// so smooth() only needs to be used when a program needs to set the smoothing in a 
        /// different way. The level parameter increases the level of smoothness. This is the 
        /// level of over sampling applied to the graphics buffer.
        /// </summary>
        /// <param name="level">int: either 2, 3, 4, or 8 depending on the renderer</param>
        public void smooth(int level = 0) => _graphics.PreferMultiSampling = true;

        /// <summary>
        /// Draws all geometry and fonts with jagged (aliased) edges and images when hard edges 
        /// between the pixels when enlarged rather than interpolating pixels. Note that smooth() 
        /// is active by default, so it is necessary to call noSmooth() to disable smoothing of 
        /// geometry, fonts, and images
        /// </summary>
        public void noSmooth() => _graphics.PreferMultiSampling = false;

        #region Color

        /// <summary>
        /// Defines the dimension of the display window width and height in units of pixels. In a 
        /// program that has the setup() function, the size() function must be the first line of
        /// code inside setup().
        /// </summary>
        /// <param name="width">int: width of the display window in units of pixels</param>
        /// <param name="height">int: height of the display window in units of pixels</param>
        /// <param name="renderer"></param>
        public void size(int width, int height, int renderer = 0)
        {
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// The background() function sets the color used for the background of the Processing window. 
        /// The default background is light gray. This function is typically used within draw() to clear 
        /// the display window at the beginning of each frame, but it can be used inside setup() to set 
        /// the background on the first frame of animation or if the backgound need only be set once. 
        /// </summary>
        /// <param name="rgb">int: any value of the color datatype</param>
        /// <param name="alpha">float: opacity of the background</param>
        public void background(int rgb, float alpha) =>
            GraphicsDevice.Clear(new color(((int)alpha << 24) + rgb));

        public void background(PImage img)
        {
            background(255);
            image(img, 0, 0, width, height);
        }

        /// <summary>
        /// The background() function sets the color used for the background of the Processing window. 
        /// The default background is light gray. This function is typically used within draw() to clear 
        /// the display window at the beginning of each frame, but it can be used inside setup() to set 
        /// the background on the first frame of animation or if the backgound need only be set once. 
        /// </summary>
        /// <param name="gray">float: specifies a value between white and black</param>
        public void background(float gray) =>
            GraphicsDevice.Clear(new color((byte)gray, (byte)gray, (byte)gray));

        /// <summary>
        /// The background() function sets the color used for the background of the Processing window. 
        /// The default background is light gray. This function is typically used within draw() to clear 
        /// the display window at the beginning of each frame, but it can be used inside setup() to set 
        /// the background on the first frame of animation or if the background need only be set once. 
        /// </summary>
        /// <param name="v1">float: red or hue value(depending on the current color mode) </param>
        /// <param name="v2">float: green or saturation value(depending on the current color mode) </param>
        /// <param name="v3">float: blue or brightness value (depending on the current color mode)</param>
        /// <param name="alpha">float: opacity of the background</param>
        public void background(float v1, float v2, float v3, float alpha = 255) =>
            GraphicsDevice.Clear(new color((byte)v1, (byte)v2, (byte)v3, (byte)alpha));

        /// <summary>
        /// Sets the color used to fill shapes. For example, if you run fill(204, 102, 0), all subsequent 
        /// shapes will be filled with orange. This color is either specified in terms of the RGB or HSB color
        ///  depending on the current colorMode(). The default color space is RGB, with each value in the range
        ///  from 0 to 255.  
        /// </summary>
        /// <param name="rgb">int: any value of the color datatype</param>
        /// <param name="alpha">float: opacity of the background</param>
        public void fill(int rgb, byte alpha = 255) =>
            _style.Fill = new color(((alpha << 24) + rgb));

        public void fill(color c) => _style.Fill = c;

        /// <summary>
        /// Sets the color used to fill shapes. For example, if you run fill(204, 102, 0), all subsequent 
        /// shapes will be filled with orange. This color is either specified in terms of the RGB or HSB color
        ///  depending on the current colorMode(). The default color space is RGB, with each value in the range
        ///  from 0 to 255.  
        /// </summary>
        /// <param name="gray">float: opacity of the background</param>
        public void fill(float gray) => _style.Fill = new color((byte) gray, (byte) gray, (byte) gray);

        /// <summary>
        /// Sets the color used to fill shapes. For example, if you run fill(204, 102, 0), all subsequent 
        /// shapes will be filled with orange. This color is either specified in terms of the RGB or HSB color
        ///  depending on the current colorMode(). The default color space is RGB, with each value in the range
        ///  from 0 to 255. 
        /// </summary>
        /// <param name="v1">float: red or hue value(depending on the current color mode) </param>
        /// <param name="v2">float: green or saturation value(depending on the current color mode) </param>
        /// <param name="v3">float: blue or brightness value (depending on the current color mode)</param>
        /// <param name="alpha">float: opacity of the background</param>
        public void fill(float v1, float v2, float v3, float alpha = 255) =>
            _style.Fill = new color((byte)v1, (byte)v2, (byte)v3, (byte)alpha);

        /// <summary>
        /// Disables filling geometry. If both noStroke() and noFill() are called, nothing will be drawn to the screen.
        /// </summary>
        public void nofill() => _style.Fill = Color.Transparent;

        /// <summary>
        /// Sets the color used to draw lines and borders around shapes. This color is either specified in 
        /// terms of the RGB or HSB color depending on the current colorMode(). The default color space is 
        /// RGB, with each value in the range from 0 to 255  
        /// </summary>
        /// <param name="rgb">int: any value of the color datatype</param>
        /// <param name="alpha">float: opacity of the background</param>
        public void stroke(int rgb, float alpha) => _style.Stroke = new color(((int)alpha << 24) + rgb);

        /// <summary>
        /// Sets the color used to draw lines and borders around shapes. This color is either specified in 
        /// terms of the RGB or HSB color depending on the current colorMode(). The default color space is 
        /// RGB, with each value in the range from 0 to 255  
        /// </summary>
        /// <param name="gray">float: opacity of the background</param>
        public void stroke(float gray) => _style.Stroke = new color((byte)gray, (byte)gray, (byte)gray);

        /// <summary>
        /// Sets the color used to draw lines and borders around shapes. This color is either specified in 
        /// terms of the RGB or HSB color depending on the current colorMode(). The default color space is 
        /// RGB, with each value in the range from 0 to 255
        /// </summary>
        /// <param name="v1">float: red or hue value(depending on the current color mode) </param>
        /// <param name="v2">float: green or saturation value(depending on the current color mode) </param>
        /// <param name="v3">float: blue or brightness value (depending on the current color mode)</param>
        /// <param name="alpha">float: opacity of the background</param>
        public void stroke(float v1, float v2, float v3, float alpha = 255) => 
            _style.Stroke = new color((byte)v1, (byte)v2, (byte)v3, (byte)alpha);

        /// <summary>
        /// Sets the width of the stroke used for lines, points, and the border around shapes. 
        /// All widths are set in units of pixels.
        /// </summary>
        /// <param name="weight">float: the weight (in pixels) of the stroke</param>
        public void strokeWeight(float weight) => _style.StrokeWidth = weight;

        public void noStroke() => _style.Stroke = Color.Transparent;

        #endregion

        #region Curves

        /// <summary>
        /// Draws a Bezier curve on the screen. These curves are defined by a series of anchor
        /// and control points. The first two parameters specify the first anchor point and the
        /// last two parameters specify the other anchor point. The middle parameters specify
        /// the control points which define the shape of the curve. Bezier curves were developed
        /// by French engineer Pierre Bezier. 
        /// </summary>
        /// <param name="x1">float: coordinates for the first anchor point</param>
        /// <param name="y1">float: coordinates for the first anchor point</param>
        /// <param name="x2">float: coordinates for the first control point</param>
        /// <param name="y2">float: coordinates for the first control point</param>
        /// <param name="x3">float: coordinates for the second control point</param>
        /// <param name="y3">float: coordinates for the second control point</param>
        /// <param name="x4">float: coordinates for the second anchor point</param>
        /// <param name="y4">float: coordinates for the second anchor point</param>
        public void bezier(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4) => _primitives.DrawBezier(new Vector2(x1, y1), new Vector2(x2, y2), new Vector2(x3, y3), new Vector2(x4, y4), _style.Stroke, _style.StrokeWidth);

        /// <summary>
        /// Draws a curved line on the screen. The first and second parameters specify the beginning
        /// control point and the last two parameters specify the ending control point. The middle
        /// parameters specify the start and stop of the curve. Longer curves can be created by putting
        /// a series of curve() functions together or using curveVertex(). An additional function called
        /// curveTightness() provides control for the visual quality of the curve. The curve() function
        /// is an implementation of Catmull-Rom splines.
        /// </summary>
        /// <param name="x1">float: coordinates for the beginning control point</param>
        /// <param name="y1">float: coordinates for the beginning control point</param>
        /// <param name="x2">float: coordinates for the first point</param>
        /// <param name="y2">float: coordinates for the first point</param>
        /// <param name="x3">float: coordinates for the second point</param>
        /// <param name="y3">float: coordinates for the second point</param>
        /// <param name="x4">float: coordinates for the ending control point</param>
        /// <param name="y4">float: coordinates for the ending control point</param>
        public void curve(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4) => _primitives.DrawSpline(new Vector2(x1, y1), new Vector2(x2, y2), new Vector2(x3, y3), new Vector2(x4, y4), _style.Stroke, _style.StrokeWidth);

        #endregion

        #region Time & Date

        public int second() => DateTime.Now.Second;

        public int minute() => DateTime.Now.Minute;

        public int hour() => DateTime.Now.Hour;

        public int day() => DateTime.Now.Day;

        public int month() => DateTime.Now.Month;

        public int year() => DateTime.Now.Year;

        public int millis() => (int)_time.ElapsedMilliseconds;

        #endregion

        #region 2D Primitives
        /// <summary>
        /// Draws an arc to the screen. Arcs are drawn along the outer edge of an ellipse defined
        /// by the a, b, c, and d parameters. The origin of the arc's ellipse may be changed with
        /// the ellipseMode() function. Use the start and stop parameters to specify the angles
        /// (in radians) at which to draw the arc. The start/stop values must be in clockwise order.
        /// 
        /// There are three ways to draw an arc; the rendering technique used is defined by the optional
        /// seventh parameter.The three options, depicted in the above examples, are PIE, OPEN, and
        /// CHORD. The default mode is the OPEN stroke with a PIE fill.
        /// 
        ///  In some cases, the arc() function isn't accurate enough for smooth drawing. For example, the
        /// shape may jitter on screen when rotating slowly. If you're having an issue with how arcs are
        /// rendered, you'll need to draw the arc yourself with beginShape()/endShape() or a PShape.
        /// </summary>
        /// <param name="a">float: x-coordinate of the arc's ellipse</param>
        /// <param name="b">float: y-coordinate of the arc's ellipse</param>
        /// <param name="c">float: width of the arc's ellipse by default</param>
        /// <param name="d">float: height of the arc's ellipse by default</param>
        /// <param name="start">float: angle to start the arc, specified in radians</param>
        /// <param name="stop">float: angle to stop the arc, specified in radians</param>
        /// <param name="mode">int: OPEN, CHORD or PIE</param>
        public void arc(float a, float b, float c, float d, float start, float stop, ArcMode mode = ArcMode.OPEN)
        {
            var (x, y, w, h) = _style.EllipseMode switch
            {
                ShapeMode.CENTER => (a - c / 2, b - d / 2, c, d),
                ShapeMode.RADIUS => (a + c, b + d, c * 2, d * 2),
                ShapeMode.CORNER => (a, b, c, d),
                ShapeMode.CORNERS => (a, b, c - a, d - b),
            };

            var rx = w / 2;
            var ry = h / 2;
            var center = new Vector2(x + rx, y + ry);

            if (mode == ArcMode.OPEN)
            {
                if (_style.Fill.A != 0) _primitives.FillArc(center, rx, ry, start, stop, _style.Fill, true);
                if (_style.Stroke.A != 0) _primitives.DrawArc(center, rx, ry, start, stop, _style.Stroke, _style.StrokeWidth);
            }
            if (mode == ArcMode.CHORD)
            {
                if (_style.Fill.A != 0) _primitives.FillArc(center, rx, ry, start, stop, _style.Fill, true);
                if (_style.Stroke.A != 0) _primitives.DrawArc(center, rx, ry, start, stop, _style.Stroke, _style.StrokeWidth, 1);
            }
            if (mode == ArcMode.PIE)
            {
                if (_style.Fill.A != 0) _primitives.FillArc(center, rx, ry, start, stop, _style.Fill);
                if (_style.Stroke.A != 0) _primitives.DrawArc(center, rx, ry, start, stop, _style.Stroke, _style.StrokeWidth, 2);
            }


        }

        /// <summary>
        /// Draws an ellipse(oval) to the screen. An ellipse with equal width and height is a circle.
        /// By default, the first two parameters set the location, and the third and fourth parameters 
        /// set the shape's width and height. The origin may be changed with the ellipseMode() function. 
        /// </summary>
        /// <param name="a">float: x-coordinate of the ellipse</param>
        /// <param name="b">float: y-coordinate of the ellipse</param>
        /// <param name="c">float: width of the ellipse by default</param>
        /// <param name="d">float: height of the ellipse by default</param>
        public void ellipse(float a, float b, float c, float d)
        {
            var (x, y, w, h) = _style.EllipseMode switch
            {
                ShapeMode.CENTER => (a - c / 2, b - d / 2, c, d),
                ShapeMode.RADIUS => (a + c, b + d, c * 2, d * 2),
                ShapeMode.CORNER => (a, b, c, d),
                ShapeMode.CORNERS => (a, b, c - a, d - b),
            };

            var rx = w / 2;
            var ry = h / 2;
            var center = new Vector2(x + rx, y + ry);

            if (_style.Fill.A != 0) _primitives.FillEllipse(center, rx, ry, _style.Fill);
            if (_style.Stroke.A != 0) _primitives.DrawEllipse(center + 0.5f * Vector2.One, rx, ry, _style.Stroke, _style.StrokeWidth);

        }

        /// <summary>
        /// Draws a circle to the screen. By default, the first two parameters set the location of the
        /// center, and the third sets the shape's width and height. The origin may be changed with the
        /// ellipseMode() function.
        /// </summary>
        /// <param name="x">float: x-coordinate of the ellipse</param>
        /// <param name="y">float: y-coordinate of the ellipse</param>
        /// <param name="extent">float: width and height of the ellipse by default</param>
        public void circle(float x, float y, float extent) => ellipse(x, y, extent, extent);


        /// <summary>
        /// Draws a rectangle to the screen. A rectangle is a four-sided shape with every angle at ninety degrees. 
        /// By default, the first two parameters set the location of the upper-left corner, the third sets 
        /// the width, and the fourth sets the height. The way these parameters are interpreted, however, may
        ///  be changed with the rectMode() function.
        /// </summary>
        /// <param name="a">float: x-coordinate of the rectangle by default</param>
        /// <param name="b">float: y-coordinate of the rectangle by default</param>
        /// <param name="c">float: width of the rectangle by default</param>
        /// <param name="d">float: height of the rectangle by default</param>
        public void rect(float a, float b, float c, float d)
        {
            var (x, y, w, h) = _style.RectMode switch
            {
                ShapeMode.CENTER => (a - c / 2, b - d / 2, c, d),
                ShapeMode.RADIUS => (a + c, b + d, c * 2, d * 2),
                ShapeMode.CORNER => (a, b, c, d),
                ShapeMode.CORNERS => (a, b, c - a, d - b),
            };

            if (_style.Fill.A != 0) _primitives.FillRectangle(x, y, w, h, _style.Fill);
            if (_style.Stroke.A != 0) _primitives.DrawRectangle(x - _style.StrokeWidth / 2, y - _style.StrokeWidth / 2, w + _style.StrokeWidth, h + _style.StrokeWidth, _style.Stroke, _style.StrokeWidth);
        }


        /// <summary>
        /// Draws a line (a direct path between two points) to the screen. The version of line() with 
        /// four parameters draws the line in 2D. To color a line, use the stroke() function. A line 
        /// cannot be filled, therefore the fill() function will not affect the color of a line. 2D lines 
        /// are drawn with a width of one pixel by default, but this can be changed with the strokeWeight() 
        /// function.
        /// </summary>
        /// <param name="x1">float: x-coordinate of the first point</param>
        /// <param name="y1">float: y-coordinate of the first point</param>
        /// <param name="x2">float: x-coordinate of the second point</param>
        /// <param name="y2">float: y-coordinate of the second point</param>
        public void line(float x1, float y1, float x2, float y2)
        {
            if (_style.Stroke.A != 0) _primitives.DrawLine(x1, y1, x2, y2, _style.Stroke, _style.StrokeWidth);
        }

        /// <summary>
        /// A quad is a quadrilateral, a four sided polygon. It is similar to a rectangle, but the angles 
        /// between its edges are not constrained to ninety degrees. The first pair of parameters (x1,y1) 
        /// sets the first vertex and the subsequent pairs should proceed clockwise or counter-clockwise 
        /// around the defined shape.
        /// </summary>
        /// <param name="x1">float: x-coordinate of the first corner</param>
        /// <param name="y1">float: y-coordinate of the first corner</param>
        /// <param name="x2">float: x-coordinate of the second corner</param>
        /// <param name="y2">float: y-coordinate of the second corner</param>
        /// <param name="x3">float: x-coordinate of the third corner</param>
        /// <param name="y3">float: y-coordinate of the third corner</param>
        /// <param name="x4">float: x-coordinate of the fourth corner</param>
        /// <param name="y4">float: y-coordinate of the fourth corner</param>
        public void quad(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            if (_style.Fill.A != 0) _primitives.FillQuad(new Vector2(x1, y1), new Vector2(x2, y2), new Vector2(x3, y3), new Vector2(x4, y4), _style.Fill);
            if (_style.Stroke.A != 0) _primitives.DrawQuad(new Vector2(x1, y1), new Vector2(x2, y2), new Vector2(x3, y3), new Vector2(x4, y4), _style.Stroke, _style.StrokeWidth);
        }

        /// <summary>
        /// A triangle is a plane created by connecting three points. The first 
        /// two arguments specify the first point, the middle two arguments 
        /// specify the second point, and the last two arguments specify the 
        /// third point
        /// </summary>
        /// <param name="x1">float: x-coordinate of the first point</param>
        /// <param name="y1">float: y-coordinate of the first point</param>
        /// <param name="x2">float: x-coordinate of the second point</param>
        /// <param name="y2">float: y-coordinate of the second point</param>
        /// <param name="x3">float: x-coordinate of the third point</param>
        /// <param name="y3">float: y-coordinate of the third point</param>
        public void triangle(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            if (_style.Fill.A != 0) _primitives.FillTriangle(new Vector2(x1, y1), new Vector2(x2, y2), new Vector2(x3, y3), _style.Fill);
            if (_style.Stroke.A != 0) _primitives.DrawTriangle(new Vector2(x1, y1), new Vector2(x2, y2), new Vector2(x3, y3), _style.Stroke, _style.StrokeWidth);
        }

        /// <summary>
        /// Draws a point, a coordinate in space at the dimension of one pixel. 
        /// The first parameter is the horizontal value for the point, the 
        /// second value is the vertical value for the point
        /// </summary>
        /// <param name="x">float: x-coordinate of the point</param>
        /// <param name="y">float: y-coordinate of the point</param>
        /// <param name="z">float: z-coordinate of the point</param>
        public void point(float x, float y, float z = 0)
        {
            if (_style.Stroke.A != 0) _primitives.DrawEllipse(new Vector2(x, y), 1, 1, _style.Stroke, _style.StrokeWidth);
        }

        #endregion

        #region Transform

        /// <summary>
        /// Rotates the amount specified by the angle parameter. Angles must be specified 
        /// in radians (values from 0 to TWO_PI), or they can be converted from degrees 
        /// to radians with the radians() function. 
        /// </summary>
        /// <param name="angle">float: angle of rotation specified in radians</param>
        public void rotate(float angle)
        {
            _matrix *= Matrix.CreateRotationZ(angle);
            _primitives.TransformMat = _matrix;
        }

        /// <summary>
        /// Increases or decreases the size of a shape by expanding and contracting vertices. 
        /// Objects always scale from their relative origin to the coordinate system. TransformMat 
        /// values are specified as decimal percentages. For example, the function call scale(2.0) 
        /// increases the dimension of a shape by 200%.
        /// </summary>
        /// <param name="s">float: percentage to scale the object</param>
        public void scale(float s)
        {
            _matrix *= Matrix.CreateScale(s);
            _primitives.TransformMat = _matrix;
        }

        /// <summary>
        /// Increases or decreases the size of a shape by expanding and contracting vertices. 
        /// Objects always scale from their relative origin to the coordinate system. TransformMat 
        /// values are specified as decimal percentages. For example, the function call scale(2.0) 
        /// increases the dimension of a shape by 200%. 
        /// </summary>
        /// <param name="x">float: percentage to scale the object in the x-axis</param>
        /// <param name="y">float: percentage to scale the object in the y-axis</param>
        public void scale(float x, float y)
        {
            _matrix *= Matrix.CreateScale(x, y, 1);
            _primitives.TransformMat = _matrix;
        }

        /// <summary>
        /// Specifies an amount to displace objects within the display window. The x parameter specifies
        /// left/right translation, the y parameter specifies up/down translation, and the z parameter
        /// specifies translations toward/away from the screen. Using this function with the z parameter
        /// requires using P3D as a parameter in combination with size as shown in the above example.
        ///
        /// Transformations are cumulative and apply to everything that happens after and subsequent calls
        /// to the function accumulates the effect.For example, calling translate(50, 0) and then translate(20, 0)
        /// is the same as translate(70, 0). If translate() is called within draw(), the transformation is reset when
        /// the loop begins again.This function can be further controlled by using pushMatrix() and popMatrix().
        /// </summary>
        /// <param name="x">float: left/right translation</param>
        /// <param name="y">float: up/down translation</param>
        public void translate(float x, float y)
        {
            _matrix *= Matrix.CreateTranslation(x, y, 0);
            _primitives.TransformMat = _matrix;
        }

        /// <summary>
        /// Shears a shape around the y-axis the amount specified by the angle parameter. Angles should be specified in
        /// radians (values from 0 to PI*2) or converted to radians with the radians() function. Objects are always sheared
        /// around their relative position to the origin and positive numbers shear objects in a clockwise direction.
        /// Transformations apply to everything that happens after and subsequent calls to the function accumulates the effect.
        /// For example, calling shearY(PI/2) and then shearY(PI/2) is the same as shearY(PI). If shearY() is called within the
        /// draw(), the transformation is reset when the loop begins again.
        ///
        /// Technically, shearY() multiplies the current transformation matrix by a rotation matrix.This function can be further
        /// controlled by the pushMatrix() and popMatrix() functions.
        /// </summary>
        /// <param name="angle">float: angle of shear specified in radians</param>
        public void shearY(float angle)
        {
            var aux = Matrix.Identity;
            aux.M12 = (float)Tan(angle);
            _matrix *= aux;
            _primitives.TransformMat = _matrix;
        }

        /// <summary>
        /// Shears a shape around the x-axis the amount specified by the angle parameter. Angles should be specified in
        /// radians (values from 0 to PI*2) or converted to radians with the radians() function. Objects are always sheared
        /// around their relative position to the origin and positive numbers shear objects in a clockwise direction.
        /// Transformations apply to everything that happens after and subsequent calls to the function accumulates the effect.
        /// For example, calling shearX(PI/2) and then shearX(PI/2) is the same as shearX(PI). If shearX() is called within the
        /// draw(), the transformation is reset when the loop begins again.
        ///
        /// Technically, shearX() multiplies the current transformation matrix by a rotation matrix.This function can be further
        /// controlled by the pushMatrix() and popMatrix() functions.
        /// </summary>
        /// <param name="angle"></param>
        public void shearX(float angle)
        {
            var aux = Matrix.Identity;
            aux.M21 = (float)Tan(angle);
            _matrix *= aux;
            _primitives.TransformMat = _matrix;
        }

        #endregion

        #region Calculation

        /// <summary>
        /// Calculates the absolute value (magnitude) of a number. The absolute 
        /// value of a number is always positive.
        /// </summary>
        /// <param name="n">int: number to compute</param>
        /// <returns>int</returns>
        public int abs(int n) => n < 0 ? -n : n;

        /// <summary>
        /// Calculates the absolute value (magnitude) of a number. The absolute 
        /// value of a number is always positive.
        /// </summary>
        /// <param name="n">float: number to compute</param>
        /// <returns>float</returns>
        public float abs(float n) => n < 0 ? -n : n;

        /// <summary>
        /// Calculates the closest int value that is greater than or equal to the 
        /// value of the parameter. For example, ceil(9.03) returns the value 10.
        /// </summary>
        /// <param name="n">float: number to round up</param>
        /// <returns>int</returns>
        public int ceil(float n) => (int)Ceiling(n);

        /// <summary>
        /// Constrains a value to not exceed a maximum and minimum value.
        /// </summary>
        /// <param name="amt">int: the value to constrain</param>
        /// <param name="low">int: minimum limit</param>
        /// <param name="high">	int: maximum limit</param>
        /// <returns>int</returns>
        public int constrain(int amt, int low, int high)
        {
            if (amt < low) return low;
            return amt > high ? high : amt;
        }

        /// <summary>
        /// Constrains a value to not exceed a maximum and minimum value.
        /// </summary>
        /// <param name="amt">float: the value to constrain</param>
        /// <param name="low">float: minimum limit</param>
        /// <param name="high">float: maximum limit</param>
        /// <returns>float</returns>
        public float constrain(float amt, float low, float high)
        {
            if (amt < low) return low;
            return amt > high ? high : amt;
        }

        /// <summary>
        /// Calculates the distance between two points.
        /// </summary>
        /// <param name="x1">float: x-coordinate of the first point</param>
        /// <param name="y1">float: y-coordinate of the first point</param>
        /// <param name="x2">float: x-coordinate of the second point</param>
        /// <param name="y2">float: y-coordinate of the second point</param>
        /// <returns>float</returns>
        public float dist(float x1, float y1, float x2, float y2) => (float)Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));

        /// <summary>
        /// Calculates the distance between two points.
        /// </summary>
        /// <param name="x1">float: x-coordinate of the first point</param>
        /// <param name="y1">float: y-coordinate of the first point</param>
        /// <param name="z1">float: z-coordinate of the first point</param>
        /// <param name="x2">float: x-coordinate of the second point</param>
        /// <param name="y2">float: y-coordinate of the second point</param>
        /// <param name="z2">float: z-coordinate of the second point</param>
        /// <returns>float</returns>
        public float dist(float x1, float y1, float z1, float x2, float y2, float z2) => (float)Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1) + (z2 - z1) * (z2 - z1));

        /// <summary>
        /// Returns Euler's number e (2.71828...) raised to the power of 
        /// the n parameter.
        /// </summary>
        /// <param name="n">float: exponent to raise</param>
        /// <returns>float</returns>
        public float exp(float n) => (float)Exp(n);

        /// <summary>
        /// Calculates the closest int value that is less than or equal 
        /// to the value of the parameter.
        /// </summary>
        /// <param name="n">float: number to round down</param>
        /// <returns>int</returns>
        public int floor(float n) => (int)Floor(n);

        /// <summary>
        /// Calculates a number between two numbers at a specific increment. 
        /// The amt parameter is the amount to interpolate between the two 
        /// values where 0.0 equal to the first point, 0.1 is very near the 
        /// first point, 0.5 is half-way in between, etc. The lerp function 
        /// is convenient for creating motion along a straight path and for 
        /// drawing dotted lines.
        /// </summary>
        /// <param name="start">float: first value</param>
        /// <param name="stop">float: second value</param>
        /// <param name="amp">float: float between 0.0 and 1.0</param>
        /// <returns>float</returns>
        public float lerp(float start, float stop, float amp) => start + (stop - start) * amp;

        /// <summary>
        /// Calculates the natural logarithm (the base-e logarithm) of a number. 
        /// This function expects the n parameter to be a value greater than 0.0.
        /// </summary>
        /// <param name="n">float: number greater than 0.0</param>
        /// <returns>float</returns>
        public float log(float n) => (float)Log(n);

        /// <summary>
        /// Calculates the magnitude (or length) of a vector. A vector is a direction 
        /// in space commonly used in computer graphics and linear algebra. Because it 
        /// has no "start" position, the magnitude of a vector can be thought of as the 
        /// distance from the coordinate 0,0 to its x,y value. Therefore, mag() is a 
        /// shortcut for writing dist(0, 0, x, y).
        /// </summary>
        /// <param name="a">float: first value</param>
        /// <param name="b">float: second value</param>
        /// <param name="c">float: third value</param>
        /// <returns>float</returns>
        public float mag(float a, float b, float c = 0) => dist(0, 0, 0, a, b, c);

        /// <summary>
        /// Re-maps a number from one range to another.
        /// </summary>
        /// <param name="value">float: the incoming value to be converted</param>
        /// <param name="start1">float: lower bound of the value's current range</param>
        /// <param name="stop1">float: upper bound of the value's current range</param>
        /// <param name="start2">float: lower bound of the value's target range</param>
        /// <param name="stop2">float: upper bound of the value's target range</param>
        /// <returns></returns>
        public float map(float value, float start1, float stop1, float start2, float stop2)
        {
            var amp = (value - start1) / (stop1 - start1);
            return start2 + (stop2 - start2) * amp;
        }

        /// <summary>
        /// Determines the largest value in a sequence of numbers, and then returns that value. 
        /// max() accepts either two or three float or int values as parameters, or an array 
        /// of any length.
        /// </summary>
        /// <param name="a">float: first number to compare</param>
        /// <param name="b">float: second number to compare</param>
        /// <param name="c">float: third number to compare</param>
        /// <returns>float</returns>
        public float max(float a, float b, float c = float.NegativeInfinity)
        {
            if (a > b && a > c) return a;
            if (b > c && b > a) return b;
            return c;
        }

        /// <summary>
        /// Determines the largest value in a sequence of numbers, and then returns that value. 
        /// max() accepts either two or three float or int values as parameters, or an array 
        /// of any length.
        /// </summary>
        /// <param name="a">int: first number to compare</param>
        /// <param name="b">int: second number to compare</param>
        /// <param name="c">int: third number to compare</param>
        /// <returns>int</returns>
        public int max(int a, int b, int c = int.MinValue)
        {
            if (a > b && a > c) return a;
            if (b > c && b > a) return b;
            return c;
        }

        /// <summary>
        /// Determines the largest value in a sequence of numbers, and then returns that value. 
        /// max() accepts either two or three float or int values as parameters, or an array 
        /// of any length.
        /// </summary>
        /// <param name="list">float[]: array of numbers to compare</param>
        /// <returns>float</returns>
        public float max(float[] list) => list.Max();

        /// <summary>
        /// Determines the largest value in a sequence of numbers, and then returns that value. 
        /// max() accepts either two or three float or int values as parameters, or an array 
        /// of any length.
        /// </summary>
        /// <param name="list">int[]: array of numbers to compare</param>
        /// <returns>int</returns>
        public int max(int[] list) => list.Max();

        /// <summary>
        /// Determines the smallest value in a sequence of numbers, and then returns that value. 
        /// min() accepts either two or three float or int values as parameters, or an array 
        /// of any length.
        /// </summary>
        /// <param name="a">float: first number to compare</param>
        /// <param name="b">float: second number to compare</param>
        /// <param name="c">float: third number to compare</param>
        /// <returns>float</returns>
        public float min(float a, float b, float c = float.PositiveInfinity)
        {
            if (a < b && a < c) return a;
            if (b < c && b < a) return b;
            return c;
        }

        /// <summary>
        /// Determines the smallest value in a sequence of numbers, and then returns that value. 
        /// min() accepts either two or three float or int values as parameters, or an array 
        /// of any length.
        /// </summary>
        /// <param name="a">int: first number to compare</param>
        /// <param name="b">int: second number to compare</param>
        /// <param name="c">int: third number to compare</param>
        /// <returns>int</returns>
        public int min(int a, int b, int c = int.MaxValue)
        {
            if (a < b && a < c) return a;
            if (b < c && b < a) return b;
            return c;
        }

        /// <summary>
        /// Determines the smallest value in a sequence of numbers, and then returns that value. 
        /// min() accepts either two or three float or int values as parameters, or an array 
        /// of any length.
        /// </summary>
        /// <param name="list">float[]: array of numbers to compare</param>
        /// <returns>float</returns>
        public float min(float[] list) => list.Min();

        /// <summary>
        /// Determines the smallest value in a sequence of numbers, and then returns that value. 
        /// min() accepts either two or three float or int values as parameters, or an array 
        /// of any length.
        /// </summary>
        /// <param name="list">int[]: array of numbers to compare</param>
        /// <returns>int</returns>
        public int min(int[] list) => list.Min();

        /// <summary>
        /// Normalizes a number from another range into a value between 0 and 1. Identical to 
        /// map(value, low, high, 0, 1). Numbers outside of the range are not clamped to 0 and 1, 
        /// because out-of-range values are often intentional and useful.
        /// </summary>
        /// <param name="value">float: the incoming value to be converted</param>
        /// <param name="start">float: lower bound of the value's current range</param>
        /// <param name="stop">	float: upper bound of the value's current range</param>
        /// <returns>float</returns>
        public float norm(float value, float start, float stop) => map(value, start, stop, 0, 1);

        /// <summary>
        /// Facilitates exponential expressions. The pow() function is an efficient way of 
        /// multiplying numbers by themselves (or their reciprocals) in large quantities. 
        /// For example, pow(3, 5) is equivalent to the expression 3*3*3*3*3 and pow(3, -5) 
        /// is equivalent to 1 / 3*3*3*3*3.
        /// </summary>
        /// <param name="n">float: base of the exponential expression</param>
        /// <param name="e">float: power by which to raise the base</param>
        /// <returns></returns>
        public float pow(float n, float e) => (float)Pow(n, e);

        /// <summary>
        /// Calculates the integer closest to the n parameter. 
        /// For example, round(133.8) returns the value 134.
        /// </summary>
        /// <param name="n">float: number to round</param>
        /// <returns>int</returns>
        public int round(float n) => (int)Round(n);

        /// <summary>
        /// Squares a number (multiplies a number by itself). 
        /// The result is always a positive number, as multiplying 
        /// two negative numbers always yields a positive result. 
        /// For example, -1 * -1 = 1.
        /// </summary>
        /// <param name="n">float: number to square</param>
        /// <returns>float</returns>
        public float sq(float n) => n * n;

        /// <summary>
        /// Calculates the square root of a number. The square root of a 
        /// number is always positive, even though there may be a valid 
        /// negative root. The square root s of number a is such that 
        /// s*s = a. It is the opposite of squaring.
        /// </summary>
        /// <param name="n">float: non-negative number</param>
        /// <returns>float</returns>
        public float sqrt(float n) => (float)Sqrt(n);

        #endregion

        #region Trigonometry

        /// <summary>
        /// The inverse of cos(), returns the arc cosine of a value. 
        /// This function expects the values in the range of -1 to 1 and 
        /// values are returned in the range 0 to PI (3.1415927).
        /// </summary>
        /// <param name="value">float: the value whose arc cosine is to be returned</param>
        /// <returns>float</returns>
        public float acos(float value) => (float)Acos(value);

        /// <summary>
        /// The inverse of sin(), returns the arc sine of a value. 
        /// This function expects the values in the range of -1 to 1 and 
        /// values are returned in the range -PI/2 to PI/2.
        /// </summary>
        /// <param name="value">	float: the value whose arc sine is to be returned</param>
        /// <returns>float</returns>
        public float asin(float value) => (float)Asin(value);

        /// <summary>
        /// The inverse of tan(), returns the arc tangent of a value. 
        /// This function expects the values in the range of -Infinity to 
        /// Infinity (exclusive) and values are returned in the range -PI/2 to PI/2.
        /// </summary>
        /// <param name="value">float: -Infinity to Infinity (exclusive)</param>
        /// <returns>float</returns>
        public float atan(float value) => (float)Atan(value);

        /// <summary>
        /// Calculates the angle (in radians) from a specified point to the coordinate 
        /// origin as measured from the positive x-axis. Values are returned as a float 
        /// in the range from PI to -PI. The atan2() function is most often used for orienting 
        /// geometry to the position of the cursor. Note: The y-coordinate of the point is the 
        /// first parameter, and the x-coordinate is the second parameter, due the the 
        /// structure of calculating the tangent.
        /// </summary>
        /// <param name="y">float: y-coordinate of the point</param>
        /// <param name="x">float: x-coordinate of the point</param>
        /// <returns>float</returns>
        public float atan2(float y, float x) => (float)Atan2(y, x);

        /// <summary>
        /// Calculates the cosine of an angle. This function expects the values of the angle 
        /// parameter to be provided in radians (values from 0 to PI*2). Values are returned 
        /// in the range -1 to 1.
        /// </summary>
        /// <param name="angle">float: an angle in radians</param>
        /// <returns>float</returns>
        public float cos(float angle) => (float)Cos(angle);

        /// <summary>
        /// Calculates the sine of an angle. This function expects the values of the angle 
        /// parameter to be provided in radians (values from 0 to 6.28). Values are returned 
        /// in the range -1 to 1.
        /// </summary>
        /// <param name="angle">float: an angle in radians</param>
        /// <returns>float</returns>
        public float sin(float angle) => (float)Sin(angle);

        /// <summary>
        /// Calculates the ratio of the sine and cosine of an angle. This function expects 
        /// the values of the angle parameter to be provided in radians (values from 0 to PI*2). 
        /// Values are returned in the range infinity to -infinity.
        /// </summary>
        /// <param name="angle">float: an angle in radians</param>
        /// <returns>float</returns>
        public float tan(float angle) => (float)Tan(angle);

        /// <summary>
        /// Converts a radian measurement to its corresponding value in degrees. 
        /// Radians and degrees are two ways of measuring the same thing. There are 
        /// 360 degrees in a circle and 2*PI radians in a circle. For example, 
        /// 90° = PI/2 = 1.5707964. All trigonometric functions in Processing require 
        /// their parameters to be specified in radians.
        /// </summary>
        /// <param name="radians">float: radian value to convert to degrees</param>
        /// <returns>float</returns>
        public float degrees(float radians) => radians * 180 / PI;

        /// <summary>
        /// Converts a degree measurement to its corresponding value in radians. 
        /// Radians and degrees are two ways of measuring the same thing. There are 
        /// 360 degrees in a circle and 2*PI radians in a circle. For example, 
        /// 90° = PI/2 = 1.5707964. All trigonometric functions in Processing require 
        /// their parameters to be specified in radians.
        /// </summary>
        /// <param name="degrees">float: degree value to convert to radians</param>
        /// <returns>float</returns>
        public float radians(float degrees) => degrees * PI / 180;

        #endregion

        #region Random

        /// <summary>
        /// Generates random numbers. Each time the random() function is called, 
        /// it returns an unexpected value within the specified range. If only 
        /// one parameter is passed to the function, it will return a float between 
        /// zero and the value of the high parameter. For example, random(5) returns 
        /// values between 0 and 5 (starting at zero, and up to, but not including, 5).
        /// </summary>
        /// <param name="low">float: lower limit</param>
        /// <param name="high">float: upper limit</param>
        /// <returns>float</returns>
        public float random(float low, float high) => map((float)_rnd.NextDouble(), 0, 1, low, high);

        /// <summary>
        /// Generates random numbers. Each time the random() function is called, 
        /// it returns an unexpected value within the specified range. If only 
        /// one parameter is passed to the function, it will return a float between 
        /// zero and the value of the high parameter. For example, random(5) returns 
        /// values between 0 and 5 (starting at zero, and up to, but not including, 5).
        /// </summary>
        /// <param name="high">float: upper limit</param>
        /// <returns>float</returns>
        public float random(float high) => random(0, high);

        /// <summary>
        /// Sets the seed value for random(). By default, random() produces different results 
        /// each time the program is run. Set the seed parameter to a constant to return the 
        /// same pseudo-random numbers each time the software is run.
        /// </summary>
        /// <param name="seed">int: seed value</param>
        public void randomSeed(int seed) => _rnd = new Random(seed);

        #endregion

        #region Attributes
        /// <summary>
        /// Modifies the location from which rectangles are drawn by changing the way in which 
        /// parameters given to rect() are interpreted. 
        /// The default mode is rectMode(CORNER), which interprets the first two parameters of 
        /// rect() as the upper-left corner of the shape, while the third and fourth parameters 
        /// are its width and height. 
        /// rectMode(CORNERS) interprets the first two parameters of 
        /// rect() as the location of one corner, and the third and fourth parameters as the 
        /// location of the opposite corner. 
        /// rectMode(CENTER) interprets the first two parameters 
        /// of rect() as the shape's center point, while the third and fourth parameters are its 
        /// width and height. 
        /// rectMode(RADIUS) also uses the first two parameters of rect() as 
        /// the shape's center point, but uses the third and fourth parameters to specify half 
        /// of the shapes's width and height.
        /// </summary>
        /// <param name="mode">int: either CORNER, CORNERS, CENTER, or RADIUS</param>
        public void rectMode(ShapeMode mode) => _style.RectMode = mode;

        /// <summary>
        /// Modifies the location from which rectangles are drawn by changing the way in which 
        /// parameters given to rect() are interpreted. 
        /// The default mode is rectMode(CORNER), which interprets the first two parameters of 
        /// rect() as the upper-left corner of the shape, while the third and fourth parameters 
        /// are its width and height. 
        /// rectMode(CORNERS) interprets the first two parameters of 
        /// rect() as the location of one corner, and the third and fourth parameters as the 
        /// location of the opposite corner. 
        /// rectMode(CENTER) interprets the first two parameters 
        /// of rect() as the shape's center point, while the third and fourth parameters are its 
        /// width and height. 
        /// rectMode(RADIUS) also uses the first two parameters of rect() as 
        /// the shape's center point, but uses the third and fourth parameters to specify half 
        /// of the shapes's width and height.
        /// </summary>
        /// <param name="mode">int: either CORNER, CORNERS, CENTER, or RADIUS</param>
        public void rectMode(int mode) => _style.RectMode = (ShapeMode)mode;

        /// <summary>
        /// Modifies the location from which ellipses are drawn by changing the way in which 
        /// parameters given to ellipse() are interpreted.The default mode is ellipseMode(CENTER), 
        /// which interprets the first two parameters of ellipse() as the shape's center point, 
        /// while the third and fourth parameters are its width and height. 
        /// ellipseMode(RADIUS) also uses the first two parameters of ellipse() as the shape's
        ///  center point, but uses the third and fourth parameters to specify half of the shapes's 
        /// width and height.
        /// ellipseMode(CORNER) interprets the first two parameters of ellipse() as the upper-left 
        /// corner of the shape, while the third and fourth parameters are its width and height.
        /// ellipseMode(CORNERS) interprets the first two parameters of ellipse() as the location
        ///  of one corner of the ellipse's bounding box, and the third and fourth parameters as the 
        /// location of the opposite corner.
        /// </summary>
        /// <param name="mode">int: either CENTER, RADIUS, CORNER, or CORNERS</param>
        public void ellipseMode(int mode) => _style.EllipseMode = (ShapeMode)mode;

        /// <summary>
        /// Modifies the location from which ellipses are drawn by changing the way in which 
        /// parameters given to ellipse() are interpreted.The default mode is ellipseMode(CENTER), 
        /// which interprets the first two parameters of ellipse() as the shape's center point, 
        /// while the third and fourth parameters are its width and height. 
        /// ellipseMode(RADIUS) also uses the first two parameters of ellipse() as the shape's
        ///  center point, but uses the third and fourth parameters to specify half of the shapes's 
        /// width and height.
        /// ellipseMode(CORNER) interprets the first two parameters of ellipse() as the upper-left 
        /// corner of the shape, while the third and fourth parameters are its width and height.
        /// ellipseMode(CORNERS) interprets the first two parameters of ellipse() as the location
        ///  of one corner of the ellipse's bounding box, and the third and fourth parameters as the 
        /// location of the opposite corner.
        /// </summary>
        /// <param name="mode">int: either CENTER, RADIUS, CORNER, or CORNERS</param>
        public void ellipseMode(ShapeMode mode) => _style.EllipseMode = mode;

        #endregion

        #region Images
        public PImage loadImage(string filename) => new PImage(filename);
        public void image(PImage img, float a, float b, float c, float d) => _primitives.DrawImage(img.Texture, a, b, c, d);
        public void image(PImage img, float a, float b) => image(img, a, b, img.width, img.height);
        public PImage createImage(int w, int h, int format) => new PImage(w, h);

        public float blue(color c) => c.B;
        public float red(color c) => c.R;
        public float green(color c) => c.G;
        public float alpha(color c) => c.A;
        public float hue(color c) => c.HSL().h;
        public float brightness(color c) => c.HSL().l;
        public float saturation(color c) => c.HSL().s;
        public color lerpColor(color c1, color c2, float amt) => Color.Lerp(c1, c2, amt);

        #endregion
    }
}
