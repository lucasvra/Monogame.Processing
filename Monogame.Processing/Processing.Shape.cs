using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monogame.Processing
{
    public partial class Processing
    {
        private List<List<Vector2>> contours;
        private int currentContour = -1;
        
        public void beginShape()
        {
            contours = new List<List<Vector2>> { new List<Vector2>() };
            currentContour++;
        }

        public void beginContour()
        {
            currentContour++;
            contours.Add(new List<Vector2>());
        }

        public void endContour()
        {
            currentContour--;
        }

        /// <summary>
        /// All shapes are constructed by connecting a series of vertices. vertex() is used to specify the vertex coordinates for points, lines, triangles, quads, and polygons. It is used exclusively within the beginShape() and endShape() functions.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void vertex(int x, int y) => contours[currentContour].Add(new Vector2(x, y));

        public void endShape()
        {
            contours[0].Reverse();
            if (_style.Fill.A != 0) FillPolygonWithHoles(Vector2.Zero, contours, _style.Fill);
            if (_style.Stroke.A != 0) contours.ForEach(c => DrawPoints(Vector2.Zero, c.Concat(c.Take(1)).ToList(), _style.Stroke, _style.StrokeWidth)); ;
            currentContour--;
        }
    }
}
