using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static System.Math;

namespace Monogame.Processing
{
    public class Primitives
    {
        private readonly GraphicsDevice _device;
        public Matrix World;
        public readonly Texture2D Pixel;
        public readonly SpriteBatch SpriteBatch;
        public readonly BasicEffect basicEffect;

        private readonly Dictionary<(double, double), List<Vector2>> _circleBuffer = new Dictionary<(double, double), List<Vector2>>();

        public int Sides { get; set; }
        public Matrix TransformMat { get; set; }

        public Primitives(GraphicsDevice device)
        {
            Sides = 30;
            TransformMat = Matrix.Identity;

            _device = device;
            World = Matrix.CreateOrthographicOffCenter(0, _device.Viewport.Width, _device.Viewport.Height, 0, 0, 10);

            SpriteBatch = new SpriteBatch(device);
            basicEffect = new BasicEffect(_device) {Alpha = 1.0f, VertexColorEnabled = true, LightingEnabled = false, World = World};

            Pixel = new Texture2D(SpriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Pixel.SetData(new[] { Color.White });
        }

        public void DrawImage(Texture2D img, float x, float y, float w, float h)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(img, new Vector2(x, y), null, Color.White, 0, Vector2.Zero, new Vector2(w/img.Width, h/img.Height), SpriteEffects.None, 0);
            SpriteBatch.End();
        }

        public void DrawText(Vector2 position, string text, SpriteFont font, Color color)
        {
            SpriteBatch.Begin();
            SpriteBatch.DrawString(font, text, position, color);
            SpriteBatch.End();
        }

        private void DrawPoints(Vector2 position, IReadOnlyList<Vector2> points, Color color, float thickness)
        {
            if (points.Count < 2) return;

            var lines = Enumerable.Range(1, points.Count - 1).Select(i =>
            {
                var (x1, y1) = points[i - 1] + position;
                var (x2, y2) = points[i] + position;
                return (x1, y1, x2, y2);
            });

            DrawLine(lines, color, thickness);
        }

        public void FillTriangle(IEnumerable<(Vector2 v1, Vector2 v2, Vector2 v3)> triangles, Color color)
        {
            var vertices = triangles.SelectMany(t =>
            {
                var (v1, v2, v3) = t;
                v1 = Vector2.Transform(v1, TransformMat);
                v2 = Vector2.Transform(v2, TransformMat);
                v3 = Vector2.Transform(v3, TransformMat);

                return new[]
                {
                    new VertexPositionColor(new Vector3(v1.X, v1.Y, 0), color),
                    new VertexPositionColor(new Vector3(v2.X, v2.Y, 0), color),
                    new VertexPositionColor(new Vector3(v3.X, v3.Y, 0), color)
                };
            }).ToArray();

            basicEffect.Alpha = color.A / 255.0f;
            basicEffect.World = World;
            basicEffect.VertexColorEnabled = true;
            basicEffect.LightingEnabled = false;


            foreach (var pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length / 3);
            }
        }

        public void FillTriangle(Vector2 v1, Vector2 v2, Vector2 v3, Color color) =>
            FillTriangle(new[] {(v1, v2, v3)}, color);

        private List<Vector2> CreateEllipsePoints(double radiusx, double radiusy)
        {
            if (_circleBuffer.ContainsKey((radiusx, radiusy))) return _circleBuffer[(radiusx, radiusy)];
            
            const double max = 2.0 * PI;
            var step = max / Sides;

            var vectors = Partitioner.Create(Enumerable.Range(0, Sides))
                .AsParallel()
                .AsOrdered()
                .Select(i => i * step)
                .Select(theta => new Vector2((float)(radiusx * Cos(theta)), (float)(radiusy * Sin(theta))))
                .ToList();

            vectors.Add(new Vector2((float)(radiusx * Cos(0)), (float)(radiusy * Sin(0))));

            _circleBuffer.Add((radiusx, radiusy), vectors);

            return _circleBuffer[(radiusx, radiusy)];
        }

        private List<(Vector2 v1, Vector2 v2, Vector2 v3)> CreateEllipseTriangles(double radiusx, double radiusy)
        {
            var points = CreateEllipsePoints(radiusx, radiusy);
            var triangles = new List<(Vector2 v1, Vector2 v2, Vector2 v3)>();

            for (var i = 1; i < points.Count; i++) triangles.Add((points[i - 1], points[i], Vector2.Zero));

            return triangles;
        }

        private List<(Vector2 v1, Vector2 v2, Vector2 v3)> CreateArcTriangles(float radiusx, float radiusy, float startingAngle, float radians, bool chord = false)
        {
            var points = CreateArcPoints(radiusx, radiusy, startingAngle, radians);

            var triangles = Partitioner.Create(Enumerable.Range(1, points.Count - 1)).AsParallel().Select(i => (points[i - 1], points[i], Vector2.Zero)).ToList();

            if (chord) triangles.Add((points[points.Count-1], points[0], Vector2.Zero));

            return triangles;
        }

        public void DrawBezier(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Color color, float thickness)
        {
            var points = new List<Vector2>();
            for (var t = 0.0f; t <= 1.0f; t += 1.0f / Sides)
            {
                points.Add((float)Pow(1 - t, 3) * p1 + 3.0f * (float)Pow(1 - t, 2) * t * p2 +
                           3.0f * (1 - t) * t * t * p3 + t * t * t * p4);
            }

            DrawPoints(Vector2.Zero, points, color, thickness);
        }

        public void DrawSpline(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, Color color, float thickness, float alpha = 0.5f)
        {
            float tj(float ti, Vector2 pi, Vector2 pj)
            {
                var (xi, yi) = pi;
                var (xj, yj) = pj;
                return (float)Pow((xj - xi) * (xj - xi) + (yj - yi) * (yj - yi), alpha / 2) + ti;
            }

            var t0 = 0f;
            var t1 = tj(t0, p0, p1);
            var t2 = tj(t1, p1, p2);
            var t3 = tj(t2, p2, p3);



            var points = new List<Vector2>();
            for (var t = t1; t <= t2; t += (t2 - t1) / Sides)
            {
                var A1 = (t1 - t) / (t1 - t0) * p0 + (t - t0) / (t1 - t0) * p1;
                var A2 = (t2 - t) / (t2 - t1) * p1 + (t - t1) / (t2 - t1) * p2;
                var A3 = (t3 - t) / (t3 - t2) * p2 + (t - t2) / (t3 - t2) * p3;

                var B1 = (t2 - t) / (t2 - t0) * A1 + (t - t0) / (t2 - t0) * A2;
                var B2 = (t3 - t) / (t3 - t1) * A2 + (t - t1) / (t3 - t1) * A3;

                var C = (t2 - t) / (t2 - t1) * B1 + (t - t1) / (t2 - t1) * B2;

                points.Add(C);
            }

            DrawPoints(Vector2.Zero, points, color, thickness);
        }

        private List<Vector2> CreateCirclePoints(double radius) =>
            CreateEllipsePoints(radius, radius);

        // mode = 0 -> open
        // mode = 1 -> chord
        // mode = 2 -> pie
        private List<Vector2> CreateArcPoints(float radiusx, float radiusy, float startingAngle, float radians, int mode = 0)
        {
            var step = (radians - startingAngle) / Sides;

            var points = Partitioner.Create(Enumerable.Range(0, Sides))
                .AsParallel()
                .AsOrdered()
                .Select(i => startingAngle + i * step)
                .Select(theta => new Vector2((float) (radiusx * Cos(theta)), (float) (radiusy * Sin(theta))))
                .ToList();
            
            
            if (mode == 1) points.Add(points[0]);
            if (mode == 2) points.AddRange(new[] { Vector2.Zero, points[0] });

            return points;
        }

        public void DrawTriangle(Vector2 v1, Vector2 v2, Vector2 v3, Color color, float thickness) =>
            DrawPoints(Vector2.Zero, new List<Vector2> { v1, v2, v3, v1 }, color, thickness);

        private void FillTriangles(Vector2 position, List<(Vector2 v1, Vector2 v2, Vector2 v3)> triangles, Color color)
        {
            triangles = triangles.Select(t => (t.v3 + position, t.v1 + position, t.v2 + position)).ToList();
            FillTriangle(triangles, color);
        }

        public void DrawRectangle(float x, float y, float w, float h, Color color, float thickness) =>
            DrawQuad(new Vector2(x, y), new Vector2(x + w, y), new Vector2(x + w, y + h), new Vector2(x, y + h), color, thickness);

        public void FillRectangle(float x, float y, float w, float h, Color color) =>
            FillQuad(new Vector2(x, y), new Vector2(x + w, y), new Vector2(x + w, y + h), new Vector2(x, y + h), color);

        public void DrawQuad(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Color color, float thickness) =>
            DrawPoints(Vector2.Zero, new List<Vector2> { p1, p2, p3, p4, p1 }, color, thickness);

        public void FillQuad(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Color color)
        {
            var triangles = new List<(Vector2 v1, Vector2 v2, Vector2 v3)> { (p1, p2, p3), (p3, p4, p1) };
            FillTriangles(Vector2.Zero, triangles, color);
        }

        public void DrawLine(IEnumerable<(float x1, float y1, float x2, float y2)> lines, Color color, float thickness)
        {
            var points =  lines.Select(l => (p1: new Vector2(l.x1, l.y1), p2: new Vector2(l.x2, l.y2)))
                .Select(p => (p1: p.p1, distance: Vector2.Distance(p.p1, p.p2), angle: (float) Atan2(p.p2.Y - p.p1.Y, p.p2.X - p.p1.X)))
                .ToArray();

            SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, TransformMat);
            foreach (var l in points) 
                SpriteBatch.Draw(Pixel, l.p1, null, color, l.angle, new Vector2(0, 0.5f), new Vector2(l.distance, thickness), SpriteEffects.None, 0);
            SpriteBatch.End();
        }

        public void DrawLine(float x1, float y1, float x2, float y2, Color color, float thickness) => DrawLine(new[] {(x1, y1, x2, y2)}, color, thickness);

        public void DrawEllipse(Vector2 center, float radiusx, float radiusy, Color color, float thickness) =>
            DrawPoints(center, CreateEllipsePoints(radiusx, radiusy), color, thickness);

        public void FillEllipse(Vector2 center, float radiusx, float radiusy, Color color) =>
            FillTriangles(center, CreateEllipseTriangles(radiusx, radiusy), color);

        public void DrawArc(Vector2 center, float radiusx, float radiusy, float startingAngle, float radians, Color color, float thickness, int mode = 0) =>
            DrawPoints(center, CreateArcPoints(radiusx, radiusy, startingAngle, radians, mode), color, thickness);

        public void FillArc(Vector2 center, float radiusx, float radiusy, float startingAngle, float radians, Color color, bool chord = false) =>
            FillTriangles(center, CreateArcTriangles(radiusx, radiusy, startingAngle, radians, chord), color);
    }
}
