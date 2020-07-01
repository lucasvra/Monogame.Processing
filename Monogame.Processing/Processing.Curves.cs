using Microsoft.Xna.Framework;

namespace Monogame.Processing
{
    partial class Processing
    {
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
        public void bezier(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4) => DrawBezier(new Vector2(x1, y1), new Vector2(x2, y2), new Vector2(x3, y3), new Vector2(x4, y4), _style.Stroke, _style.StrokeWidth);

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
        public void curve(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4) => DrawSpline(new Vector2(x1, y1), new Vector2(x2, y2), new Vector2(x3, y3), new Vector2(x4, y4), _style.Stroke, _style.StrokeWidth);

        #endregion
    }
}
