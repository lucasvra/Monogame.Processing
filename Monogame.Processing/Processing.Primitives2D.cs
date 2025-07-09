using System;
using Microsoft.Xna.Framework;

namespace Monogame.Processing
{
    partial class Processing
    {
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
                _ => throw new ArgumentOutOfRangeException()
            };

            var rx = w / 2;
            var ry = h / 2;
            var center = new Vector2(x + rx, y + ry);

            if (mode == ArcMode.OPEN)
            {
                if (_style.Fill.A != 0) FillArc(center, rx, ry, start, stop, _style.Fill, true);
                if (_style.Stroke.A != 0) DrawArc(center, rx, ry, start, stop, _style.Stroke, _style.StrokeWidth);
            }
            if (mode == ArcMode.CHORD)
            {
                if (_style.Fill.A != 0) FillArc(center, rx, ry, start, stop, _style.Fill, true);
                if (_style.Stroke.A != 0) DrawArc(center, rx, ry, start, stop, _style.Stroke, _style.StrokeWidth, 1);
            }
            if (mode == ArcMode.PIE)
            {
                if (_style.Fill.A != 0) FillArc(center, rx, ry, start, stop, _style.Fill);
                if (_style.Stroke.A != 0) DrawArc(center, rx, ry, start, stop, _style.Stroke, _style.StrokeWidth, 2);
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
                _ => throw new ArgumentOutOfRangeException()
            };

            var rx = w / 2;
            var ry = h / 2;
            var center = new Vector2(x + rx, y + ry);

            if (_style.Fill.A != 0) FillEllipse(center, rx, ry, _style.Fill);
            if (_style.Stroke.A != 0) DrawEllipse(center, rx, ry, _style.Stroke, _style.StrokeWidth);

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

        public void square(float x, float y, float extent) => rect(x, y, extent, extent);

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
                _ => throw new ArgumentOutOfRangeException()
            };

            if (_style.Fill.A != 0) FillRectangle(x, y, w, h, _style.Fill);
            if (_style.Stroke.A != 0) DrawRectangle(x, y, w, h, _style.Stroke, _style.StrokeWidth);
        }

        public void rect(float a, float b, float c, float d, float tl, float tr=-1, float br=-1, float bl=-1)
        {
            if (tr < 0) tr = tl;
            if (br < 0) br = tr;
            if (bl < 0) bl = br;

            var (x, y, w, h) = _style.RectMode switch
            {
                ShapeMode.CENTER => (a - c / 2, b - d / 2, c, d),
                ShapeMode.RADIUS => (a + c, b + d, c * 2, d * 2),
                ShapeMode.CORNER => (a, b, c, d),
                ShapeMode.CORNERS => (a, b, c - a, d - b),
                _ => throw new ArgumentOutOfRangeException()
            };

            if (_style.Fill.A != 0) FillRoundedRectangle(x, y, w, h, tl, tr, bl, br, _style.Fill);
            if (_style.Stroke.A != 0) DrawRoundedRectangle(x, y, w, h, tl, tr, bl, br, _style.Stroke, _style.StrokeWidth);
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
            if (_style.Stroke.A != 0) DrawLine(x1, y1, x2, y2, _style.Stroke, _style.StrokeWidth);
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
            if (_style.Fill.A != 0) FillQuad(new Vector2(x1, y1), new Vector2(x2, y2), new Vector2(x3, y3), new Vector2(x4, y4), _style.Fill);
            if (_style.Stroke.A != 0) DrawQuad(new Vector2(x1, y1), new Vector2(x2, y2), new Vector2(x3, y3), new Vector2(x4, y4), _style.Stroke, _style.StrokeWidth);
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
            if (_style.Fill.A != 0) FillTriangle(new Vector2(x1, y1), new Vector2(x2, y2), new Vector2(x3, y3), _style.Fill);
            if (_style.Stroke.A != 0) DrawTriangle(new Vector2(x1, y1), new Vector2(x2, y2), new Vector2(x3, y3), _style.Stroke, _style.StrokeWidth);
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
            if (_style.Stroke.A != 0) DrawPoint(new Vector2(x, y), _style.Stroke, _style.StrokeWidth);
        }

        #endregion
    }
}
