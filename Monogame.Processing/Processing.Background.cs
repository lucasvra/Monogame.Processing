using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using static System.Math;

namespace Monogame.Processing
{
    partial class Processing
    {
        private readonly Dictionary<(double, double), List<Vector2>> _circleBuffer = new Dictionary<(double, double), List<Vector2>>();

        private int sides { get; set; }

        private void FillRoundedRectangle(float x, float y, float w, float h, float tl, float tr, float br, float bl, Color color)
        {
            FillQuad(new Vector2(x + tl, y), new Vector2(x + w - tr, y), new Vector2(x + w - tr, y + tr), new Vector2(x + tl, y + tl), color);
            FillQuad(new Vector2(x, y + tl), new Vector2(x + tl, y + tl), new Vector2(x + bl, y + h - bl), new Vector2(x, y + h - bl), color);
            FillQuad(new Vector2(x + bl, y + h - bl), new Vector2(x + w - br, y + h - br), new Vector2(x + w - br, y + h), new Vector2(x + bl, y + h), color);
            FillQuad(new Vector2(x + w - tr, y + tr), new Vector2(x + w, y + tr), new Vector2(x + w, y + h - br), new Vector2(x + w - br, y + h - br), color);
            FillQuad(new Vector2(x + tl, y + tl), new Vector2(x + w - tr, y + tr), new Vector2(x + w - br, y + h - br), new Vector2(x + bl, y + h - bl), color);

            FillArc(new Vector2(x + tl, y + tl), tl, tl, PI, PI+ HALF_PI, color);
            FillArc(new Vector2(x + w - tr, y + tr), tr, tr, PI + HALF_PI , TWO_PI, color);
            FillArc(new Vector2(x + bl, y + h - bl), bl, bl, HALF_PI, PI, color);
            FillArc(new Vector2(x + w - br, y + h - br), br, br, 0, HALF_PI, color);
        }

        private void DrawRoundedRectangle(float x, float y, float w, float h, float tl, float tr, float br, float bl, Color color, float thickness)
        {
            var points = new List<Vector2>();
            points.AddRange(CreateArcPoints(tl, tl, PI, PI + HALF_PI).Select(p => p + new Vector2(x + tl, y + tl)));
            points.AddRange(CreateArcPoints(tr, tr, PI + HALF_PI, TWO_PI).Select(p => p + new Vector2(x + w - tr, y + tr)));
            points.AddRange(CreateArcPoints(br, br, 0, HALF_PI).Select(p => p + new Vector2(x + w - br, y + h - br)));
            points.AddRange(CreateArcPoints(bl, bl, HALF_PI, PI).Select(p => p + new Vector2(x + bl, y + h - bl)));
            
            points.Add(points[0]);

            DrawPoints(Vector2.Zero, points, color, thickness);
        }

        private void DrawImage(Texture2D img, float x, float y, float w, float h, Color color)
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, _style.BlendMode);
            _spriteBatch.Draw(img, new Vector2(x, y), null, color, 0, Vector2.Zero, new Vector2(w / img.Width, h / img.Height), SpriteEffects.None, 0);
            _spriteBatch.End();
        }

        private void DrawText(Vector2 position, string text, SpriteFont font, Color color, float size)
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, _style.BlendMode, SamplerState.PointClamp, null, null, null, _matrix);
            _spriteBatch.DrawString(font, text, position, color, 0f, Vector2.Zero, size / 48f * Vector2.One,
                SpriteEffects.None, 0f);
            _spriteBatch.End();
        }

        private void DrawPoint(Vector2 position, Color color, float thickness)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, _style.BlendMode, null, null, null, null, _matrix);
            _spriteBatch.Draw(_pixel, position, null, color, 0, new Vector2(0.5f, 0.5f), new Vector2(thickness, thickness), SpriteEffects.None, 0);
            _spriteBatch.End();
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

        private void FillTriangle(IEnumerable<(Vector2 v1, Vector2 v2, Vector2 v3)> triangles, Color color)
        {
            var vertices = triangles.SelectMany(t =>
            {
                var (v1, v2, v3) = t;
                v1 = Vector2.Transform(v1, _matrix);
                v2 = Vector2.Transform(v2, _matrix);
                v3 = Vector2.Transform(v3, _matrix);

                return new[]
                {
                    new VertexPositionColor(new Vector3(v1.X, v1.Y, 0), color),
                    new VertexPositionColor(new Vector3(v2.X, v2.Y, 0), color),
                    new VertexPositionColor(new Vector3(v3.X, v3.Y, 0), color)
                };
            }).ToArray();

            _basicEffect.Alpha = color.A / 255.0f;
            _basicEffect.World = _world;
            _basicEffect.VertexColorEnabled = true;
            _basicEffect.LightingEnabled = false;

            GraphicsDevice.BlendState = _style.BlendMode;

            foreach (var pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length / 3);
            }
        }

        private void FillTriangle(Vector2 v1, Vector2 v2, Vector2 v3, Color color) =>
            FillTriangle(new[] { (v1, v2, v3) }, color);

        private List<Vector2> CreateEllipsePoints(double radiusx, double radiusy)
        {
            if (_circleBuffer.ContainsKey((radiusx, radiusy))) return _circleBuffer[(radiusx, radiusy)];

            const double max = 2.0 * PI;
            var step = max / sides;

            var vectors = Partitioner.Create(Enumerable.Range(0, sides))
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

            if (chord) triangles.Add((points[points.Count - 1], points[0], Vector2.Zero));

            return triangles;
        }

        private void DrawBezier(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Color color, float thickness)
        {
            var points = new List<Vector2>();
            for (var t = 0.0f; t <= 1.0f; t += 1.0f / sides)
            {
                points.Add((float)Pow(1 - t, 3) * p1 + 3.0f * (float)Pow(1 - t, 2) * t * p2 +
                           3.0f * (1 - t) * t * t * p3 + t * t * t * p4);
            }

            DrawPoints(Vector2.Zero, points, color, thickness);
        }

        private void DrawSpline(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, Color color, float thickness, float alpha = 0.5f)
        {
            float tj(float ti, Vector2 pi, Vector2 pj)
            {
                var (xi, yi) = pi;
                var (xj, yj) = pj;
                return (float)Pow((xj - xi) * (xj - xi) + (yj - yi) * (yj - yi), alpha / 2) + ti;
            }

            const float t0 = 0f;
            var t1 = tj(t0, p0, p1);
            var t2 = tj(t1, p1, p2);
            var t3 = tj(t2, p2, p3);



            var points = new List<Vector2>();
            for (var t = t1; t <= t2; t += (t2 - t1) / sides)
            {
                var a1 = (t1 - t) / (t1 - t0) * p0 + (t - t0) / (t1 - t0) * p1;
                var a2 = (t2 - t) / (t2 - t1) * p1 + (t - t1) / (t2 - t1) * p2;
                var a3 = (t3 - t) / (t3 - t2) * p2 + (t - t2) / (t3 - t2) * p3;

                var b1 = (t2 - t) / (t2 - t0) * a1 + (t - t0) / (t2 - t0) * a2;
                var b2 = (t3 - t) / (t3 - t1) * a2 + (t - t1) / (t3 - t1) * a3;

                var c = (t2 - t) / (t2 - t1) * b1 + (t - t1) / (t2 - t1) * b2;

                points.Add(c);
            }

            DrawPoints(Vector2.Zero, points, color, thickness);
        }

        private static Vector2 BezierPoint(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, float t)
        {
            return (float)Pow(1 - t, 3) * p1 + 3.0f * (float)Pow(1 - t, 2) * t * p2 +
                   3.0f * (1 - t) * t * t * p3 + t * t * t * p4;
        }

        private static float BezierPoint(float x1, float x2, float x3, float x4, float t)
        {
            return (float)Pow(1 - t, 3) * x1 + 3.0f * (float)Pow(1 - t, 2) * t * x2 +
                   3.0f * (1 - t) * t * t * x3 + t * t * t * x4;
        }

        private float SplinePoint(float x0, float x1, float x2, float x3, float t, float alpha = 0.5f)
        {
            float Tj(float ti, float xi, float xj) => (float)Pow((xj - xi) * (xj - xi), alpha / 2) + ti;

            const float t0 = 0f;
            var t1 = Tj(t0, x0, x1);
            var t2 = Tj(t1, x1, x2);
            var t3 = Tj(t2, x2, x3);

            var a1 = (t1 - t) / (t1 - t0) * x0 + (t - t0) / (t1 - t0) * x1;
            var a2 = (t2 - t) / (t2 - t1) * x1 + (t - t1) / (t2 - t1) * x2;
            var a3 = (t3 - t) / (t3 - t2) * x2 + (t - t2) / (t3 - t2) * x3;

            var b1 = (t2 - t) / (t2 - t0) * a1 + (t - t0) / (t2 - t0) * a2;
            var b2 = (t3 - t) / (t3 - t1) * a2 + (t - t1) / (t3 - t1) * a3;

            return (t2 - t) / (t2 - t1) * b1 + (t - t1) / (t2 - t1) * b2;
        }

        private List<Vector2> CreateCirclePoints(double radius) =>
            CreateEllipsePoints(radius, radius);

        // mode = 0 -> open
        // mode = 1 -> chord
        // mode = 2 -> pie
        private List<Vector2> CreateArcPoints(float radiusx, float radiusy, float startingAngle, float radians, int mode = 0)
        {
            var step = (radians - startingAngle) / sides;

            var points = Partitioner.Create(Enumerable.Range(0, sides))
                .AsParallel()
                .AsOrdered()
                .Select(i => startingAngle + i * step)
                .Select(theta => new Vector2((float)(radiusx * Cos(theta)), (float)(radiusy * Sin(theta))))
                .ToList();

            if ((sides - 1) * step + startingAngle < radians)
                points.Add(new Vector2((float) (radiusx * Cos(radians)), (float) (radiusy * Sin(radians))));

            if (mode == 1) points.Add(points[0]);
            if (mode == 2) points.AddRange(new[] { Vector2.Zero, points[0] });

            return points;
        }

        private void DrawTriangle(Vector2 v1, Vector2 v2, Vector2 v3, Color color, float thickness) =>
            DrawPoints(Vector2.Zero, new List<Vector2> { v1, v2, v3, v1 }, color, thickness);

        private void FillTriangles(Vector2 position, List<(Vector2 v1, Vector2 v2, Vector2 v3)> triangles, Color color)
        {
            triangles = triangles.Select(t => (t.v3 + position, t.v1 + position, t.v2 + position)).ToList();
            FillTriangle(triangles, color);
        }

        private void DrawRectangle(float x, float y, float w, float h, Color color, float thickness) =>
            DrawQuad(new Vector2(x, y), new Vector2(x + w, y), new Vector2(x + w, y + h), new Vector2(x, y + h), color, thickness);

        private void FillRectangle(float x, float y, float w, float h, Color color) =>
            FillQuad(new Vector2(x, y), new Vector2(x + w, y), new Vector2(x + w, y + h), new Vector2(x, y + h), color);

        private void DrawQuad(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Color color, float thickness) =>
            DrawPoints(Vector2.Zero, new List<Vector2> { p1, p2, p3, p4, p1 }, color, thickness);

        private void FillQuad(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Color color)
        {
            var triangles = new List<(Vector2 v1, Vector2 v2, Vector2 v3)> { (p1, p2, p3), (p3, p4, p1) };
            FillTriangles(Vector2.Zero, triangles, color);
        }

        private void DrawLine(IEnumerable<(float x1, float y1, float x2, float y2)> lines, Color color, float thickness)
        {
            var points = lines.Select(l => (p1: new Vector2(l.x1, l.y1), p2: new Vector2(l.x2, l.y2)))
                .Select(p => (p1: p.p1, distance: Vector2.Distance(p.p1, p.p2), angle: (float)Atan2(p.p2.Y - p.p1.Y, p.p2.X - p.p1.X)))
                .ToArray();

            _spriteBatch.Begin(SpriteSortMode.Deferred, _style.BlendMode, null, null, null, null, _matrix);
            foreach (var (p1, distance, angle) in points)
                _spriteBatch.Draw(_pixel, p1, null, color, angle, new Vector2(0, 0.5f), new Vector2(distance, thickness), SpriteEffects.None, 0);
            _spriteBatch.End();
        }

        private void DrawLine(float x1, float y1, float x2, float y2, Color color, float thickness) => DrawLine(new[] { (x1, y1, x2, y2) }, color, thickness);

        private void DrawEllipse(Vector2 center, float radiusx, float radiusy, Color color, float thickness) =>
            DrawPoints(center, CreateEllipsePoints(radiusx, radiusy), color, thickness);

        private void FillEllipse(Vector2 center, float radiusx, float radiusy, Color color) =>
            FillTriangles(center, CreateEllipseTriangles(radiusx, radiusy), color);

        private void DrawArc(Vector2 center, float radiusx, float radiusy, float startingAngle, float radians, Color color, float thickness, int mode = 0) =>
            DrawPoints(center, CreateArcPoints(radiusx, radiusy, startingAngle, radians, mode), color, thickness);

        private void FillArc(Vector2 center, float radiusx, float radiusy, float startingAngle, float radians, Color color, bool chord = false) =>
            FillTriangles(center, CreateArcTriangles(radiusx, radiusy, startingAngle, radians, chord), color);
    }
}
