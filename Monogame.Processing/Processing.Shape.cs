using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Monogame.Processing
{
    public partial class Processing
    {
        private List<List<Vector2>> contours;
        private List<List<Vector2>> textureContours;
        private int currentContour = -1;
        private PImage currentTexture;
        private VertexPositionColorTexture[] _textureVertexBuffer = new VertexPositionColorTexture[1024];

        public void beginShape()
        {
            contours = new List<List<Vector2>> { new List<Vector2>() };
            textureContours = new List<List<Vector2>> { new List<Vector2>() };
            currentContour++;
            currentTexture = null;
        }

        public void beginContour()
        {
            currentContour++;
            contours.Add(new List<Vector2>());
            textureContours.Add(new List<Vector2>());
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
        public void vertex(int x, int y)
        {
            contours[currentContour].Add(new Vector2(x, y));
            textureContours[currentContour].Add(Vector2.Zero);
        }

        public void vertex(float x, float y) => vertex((int)x, (int)y);

        public void vertex(int x, int y, float u, float v)
        {
            contours[currentContour].Add(new Vector2(x, y));
            textureContours[currentContour].Add(TextureCoordinate(u, v));
        }

        public void vertex(float x, float y, float u, float v) => vertex((int)x, (int)y, u, v);

        public void endShape()
        {
            if (currentTexture?.IsLoaded == true) FillTexturedShape();
            else if (_style.Fill.A != 0)
            {
                contours[0].Reverse();
                FillPolygonWithHoles(Vector2.Zero, contours, _style.Fill);
            }

            if (_style.Stroke.A != 0) contours.ForEach(c => DrawPoints(Vector2.Zero, c.Concat(c.Take(1)).ToList(), _style.Stroke, _style.StrokeWidth));
            currentContour--;
            currentTexture = null;
        }

        private Vector2 TextureCoordinate(float u, float v)
        {
            if (_style.TextureMode == TextureMode.IMAGE && currentTexture?.IsLoaded == true)
                return new Vector2(u / currentTexture.width, v / currentTexture.height);
            return new Vector2(u, v);
        }

        private void FillTexturedShape()
        {
            var points = contours[0];
            var uv = textureContours[0];
            if (points.Count < 3) return;

            var triangleCount = points.Count - 2;
            var vertexCount = triangleCount * 3;
            if (vertexCount > _textureVertexBuffer.Length) System.Array.Resize(ref _textureVertexBuffer, vertexCount);

            var vertexIndex = 0;
            for (var i = 1; i < points.Count - 1; i++)
            {
                AddTexturedVertex(ref vertexIndex, points[0], uv[0]);
                AddTexturedVertex(ref vertexIndex, points[i], uv[i]);
                AddTexturedVertex(ref vertexIndex, points[i + 1], uv[i + 1]);
            }

            _basicEffect.TextureEnabled = true;
            _basicEffect.VertexColorEnabled = true;
            _basicEffect.Texture = currentTexture.texture;
            _basicEffect.World = _world;
            GraphicsDevice.BlendState = _style.BlendMode;
            GraphicsDevice.SamplerStates[0] = _style.TextureWrap == TextureWrap.REPEAT ? SamplerState.LinearWrap : SamplerState.LinearClamp;

            foreach (var pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, _textureVertexBuffer, 0, triangleCount);
            }

            _basicEffect.TextureEnabled = false;
            GraphicsDevice.SamplerStates[0] = _ssAnsiostropicClamp;
        }

        private void AddTexturedVertex(ref int vertexIndex, Vector2 point, Vector2 uv)
        {
            var transformed = Vector2.Transform(point, _matrix);
            _textureVertexBuffer[vertexIndex++] = new VertexPositionColorTexture(new Vector3(transformed.X, transformed.Y, 0), _style.Tint, uv);
        }
    }
}
