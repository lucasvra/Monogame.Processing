using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static System.Math;
using TriangleNet.Geometry;
using TriangleNet.Topology;
using TriangleNet.Meshing;
using Point = TriangleNet.Geometry.Point;
using SimdVector = System.Numerics.Vector; // Alias para o Vector do System.Numerics
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Monogame.Processing
{
    partial class Processing
    {
        // Cache expandido para incluir arcos
        private readonly Dictionary<(double, double), List<Vector2>> _circleBuffer = new Dictionary<(double, double), List<Vector2>>();
        private readonly Dictionary<(float, float, float, float), List<Vector2>> _arcBuffer = new Dictionary<(float, float, float, float), List<Vector2>>();
        
        // Arrays reutilizáveis para evitar alocações
        private VertexPositionColor[] _vertexBuffer = new VertexPositionColor[1024];
        private Vector2[] _pointBuffer = new Vector2[1024];
        private (Vector2 v1, Vector2 v2, Vector2 v3)[] _triangleBuffer = new (Vector2, Vector2, Vector2)[512];

        private int sides { get; set; }

        private void FillRoundedRectangle(float x, float y, float w, float h, float tl, float tr, float br, float bl, Color color)
        {
            // Batch all triangles together
            var triangleCount = 0;
            
            // Add quad triangles
            AddQuadTriangles(ref triangleCount, new Vector2(x + tl, y), new Vector2(x + w - tr, y), new Vector2(x + w - tr, y + tr), new Vector2(x + tl, y + tl));
            AddQuadTriangles(ref triangleCount, new Vector2(x, y + tl), new Vector2(x + tl, y + tl), new Vector2(x + bl, y + h - bl), new Vector2(x, y + h - bl));
            AddQuadTriangles(ref triangleCount, new Vector2(x + bl, y + h - bl), new Vector2(x + w - br, y + h - br), new Vector2(x + w - br, y + h), new Vector2(x + bl, y + h));
            AddQuadTriangles(ref triangleCount, new Vector2(x + w - tr, y + tr), new Vector2(x + w, y + tr), new Vector2(x + w, y + h - br), new Vector2(x + w - br, y + h - br));
            AddQuadTriangles(ref triangleCount, new Vector2(x + tl, y + tl), new Vector2(x + w - tr, y + tr), new Vector2(x + w - br, y + h - br), new Vector2(x + bl, y + h - bl));

            // Add arc triangles
            AddArcTrianglesToBuffer(ref triangleCount, new Vector2(x + tl, y + tl), tl, tl, (float)PI, (float)(PI + HALF_PI));
            AddArcTrianglesToBuffer(ref triangleCount, new Vector2(x + w - tr, y + tr), tr, tr, (float)(PI + HALF_PI), (float)TWO_PI);
            AddArcTrianglesToBuffer(ref triangleCount, new Vector2(x + bl, y + h - bl), bl, bl, (float)HALF_PI, (float)PI);
            AddArcTrianglesToBuffer(ref triangleCount, new Vector2(x + w - br, y + h - br), br, br, 0, (float)HALF_PI);

            // Draw all triangles at once
            FillTriangleBuffer(triangleCount, color);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddQuadTriangles(ref int triangleCount, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
        {
            if (triangleCount + 2 > _triangleBuffer.Length)
                Array.Resize(ref _triangleBuffer, _triangleBuffer.Length * 2);
                
            _triangleBuffer[triangleCount++] = (p1, p2, p3);
            _triangleBuffer[triangleCount++] = (p3, p4, p1);
        }

        private void AddArcTrianglesToBuffer(ref int triangleCount, Vector2 center, float radiusx, float radiusy, float startingAngle, float radians)
        {
            var points = CreateArcPointsOptimized(radiusx, radiusy, startingAngle, radians);
            
            if (triangleCount + points.Count - 1 > _triangleBuffer.Length)
                Array.Resize(ref _triangleBuffer, _triangleBuffer.Length * 2);
                
            for (var i = 1; i < points.Count; i++)
                _triangleBuffer[triangleCount++] = (points[i - 1] + center, points[i] + center, center);
        }

        private void DrawRoundedRectangle(float x, float y, float w, float h, float tl, float tr, float br, float bl, Color color, float thickness)
        {
            var pointCount = 0;
            
            // Use pre-allocated buffer
            AddArcPointsToBuffer(ref pointCount, tl, tl, (float)PI, (float)(PI + HALF_PI), new Vector2(x + tl, y + tl));
            AddArcPointsToBuffer(ref pointCount, tr, tr, (float)(PI + HALF_PI), (float)TWO_PI, new Vector2(x + w - tr, y + tr));
            AddArcPointsToBuffer(ref pointCount, br, br, 0, (float)HALF_PI, new Vector2(x + w - br, y + h - br));
            AddArcPointsToBuffer(ref pointCount, bl, bl, (float)HALF_PI, (float)PI, new Vector2(x + bl, y + h - bl));
            
            if (pointCount < _pointBuffer.Length)
                _pointBuffer[pointCount] = _pointBuffer[0];
            pointCount++;

            DrawPointsOptimized(Vector2.Zero, pointCount, color, thickness);
        }

        private void AddArcPointsToBuffer(ref int pointCount, float radiusx, float radiusy, float start, float end, Vector2 offset)
        {
            var points = CreateArcPointsOptimized(radiusx, radiusy, start, end);
            
            if (pointCount + points.Count > _pointBuffer.Length)
                Array.Resize(ref _pointBuffer, _pointBuffer.Length * 2);
                
            for (int i = 0; i < points.Count; i++)
                _pointBuffer[pointCount++] = points[i] + offset;
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
            _spriteBatch.DrawString(font, text, position, color, 0f, Vector2.Zero, size / 48f * Vector2.One, SpriteEffects.None, 0f);
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
            
            _spriteBatch.Begin(SpriteSortMode.Deferred, _style.BlendMode, null, null, null, null, _matrix);
            
            for (int i = 1; i < points.Count; i++)
            {
                var p1 = points[i - 1] + position;
                var p2 = points[i] + position;
                var distance = Vector2.Distance(p1, p2);
                var angle = (float)Atan2(p2.Y - p1.Y, p2.X - p1.X);
                
                _spriteBatch.Draw(_pixel, p1, null, color, angle, new Vector2(0, 0.5f), new Vector2(distance, thickness), SpriteEffects.None, 0);
            }
            
            _spriteBatch.End();
        }

        private void DrawPointsOptimized(Vector2 position, int pointCount, Color color, float thickness)
        {
            if (pointCount < 2) return;
            
            _spriteBatch.Begin(SpriteSortMode.Deferred, _style.BlendMode, null, null, null, null, _matrix);
            
            for (int i = 1; i < pointCount; i++)
            {
                var p1 = _pointBuffer[i - 1] + position;
                var p2 = _pointBuffer[i] + position;
                var distance = Vector2.Distance(p1, p2);
                var angle = (float)Atan2(p2.Y - p1.Y, p2.X - p1.X);
                
                _spriteBatch.Draw(_pixel, p1, null, color, angle, new Vector2(0, 0.5f), new Vector2(distance, thickness), SpriteEffects.None, 0);
            }
            
            _spriteBatch.End();
        }

        private void FillTriangleBuffer(int triangleCount, Color color)
        {
            var vertexCount = triangleCount * 3;
            if (vertexCount > _vertexBuffer.Length)
                Array.Resize(ref _vertexBuffer, vertexCount);
                
            // Use SIMD when possible for matrix transformations
            if (SimdVector.IsHardwareAccelerated && triangleCount > 8)       
                TransformTrianglesSIMD(triangleCount, color);
            else TransformTrianglesNormal(triangleCount, color);
            

            _basicEffect.Alpha = color.A / 255.0f;
            _basicEffect.World = _world;
            _basicEffect.VertexColorEnabled = true;
            _basicEffect.LightingEnabled = false;

            GraphicsDevice.BlendState = _style.BlendMode;

            foreach (var pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, _vertexBuffer, 0, triangleCount);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void TransformTrianglesNormal(int triangleCount, Color color)
        {
            var vertexIndex = 0;
            for (int i = 0; i < triangleCount; i++)
            {
                var (v1, v2, v3) = _triangleBuffer[i];
                v1 = Vector2.Transform(v1, _matrix);
                v2 = Vector2.Transform(v2, _matrix);
                v3 = Vector2.Transform(v3, _matrix);
                
                _vertexBuffer[vertexIndex++] = new VertexPositionColor(new Vector3(v1.X, v1.Y, 0), color);
                _vertexBuffer[vertexIndex++] = new VertexPositionColor(new Vector3(v2.X, v2.Y, 0), color);
                _vertexBuffer[vertexIndex++] = new VertexPositionColor(new Vector3(v3.X, v3.Y, 0), color);
            }
        }

        private void TransformTrianglesSIMD(int triangleCount, Color color)
        {
            // Extract matrix components for SIMD
            var m11 = _matrix.M11;
            var m12 = _matrix.M12;
            var m21 = _matrix.M21;
            var m22 = _matrix.M22;
            var m41 = _matrix.M41;
            var m42 = _matrix.M42;
            
            var vertexIndex = 0;
            for (int i = 0; i < triangleCount; i++)
            {
                var (v1, v2, v3) = _triangleBuffer[i];
                
                // Manual matrix multiplication is faster for 2D
                float x1 = v1.X * m11 + v1.Y * m21 + m41;
                float y1 = v1.X * m12 + v1.Y * m22 + m42;
                float x2 = v2.X * m11 + v2.Y * m21 + m41;
                float y2 = v2.X * m12 + v2.Y * m22 + m42;
                float x3 = v3.X * m11 + v3.Y * m21 + m41;
                float y3 = v3.X * m12 + v3.Y * m22 + m42;
                
                _vertexBuffer[vertexIndex++] = new VertexPositionColor(new Vector3(x1, y1, 0), color);
                _vertexBuffer[vertexIndex++] = new VertexPositionColor(new Vector3(x2, y2, 0), color);
                _vertexBuffer[vertexIndex++] = new VertexPositionColor(new Vector3(x3, y3, 0), color);
            }
        }

        private void FillTriangle(IEnumerable<(Vector2 v1, Vector2 v2, Vector2 v3)> triangles, Color color)
        {
            var triangleList = triangles as List<(Vector2, Vector2, Vector2)> ?? triangles.ToList();
            var triangleCount = triangleList.Count;
            
            if (triangleCount > _triangleBuffer.Length)
                Array.Resize(ref _triangleBuffer, triangleCount);
                
            for (int i = 0; i < triangleCount; i++)
                _triangleBuffer[i] = triangleList[i];
                
            FillTriangleBuffer(triangleCount, color);
        }

        private void FillTriangle(IEnumerable<(Vector3 v1, Vector3 v2, Vector3 v3)> triangles, Color color)
        {
            var vertices = triangles.SelectMany(t =>
            {
                var (v1, v2, v3) = t;
                v1 = Vector3.Transform(v1, _matrix);
                v2 = Vector3.Transform(v2, _matrix);
                v3 = Vector3.Transform(v3, _matrix);

                return new[]
                {
                    new VertexPositionColor(v1, color),
                    new VertexPositionColor(v2, color),
                    new VertexPositionColor(v3, color)
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

        private void FillTriangle(Vector2 v1, Vector2 v2, Vector2 v3, Color color)
        {
            _triangleBuffer[0] = (v1, v2, v3);
            FillTriangleBuffer(1, color);
        }

        private List<Vector2> CreateEllipsePoints(double radiusx, double radiusy)
        {
            var key = (radiusx, radiusy);
            if (_circleBuffer.TryGetValue(key, out var cached))
                return cached;

            var vectors = new List<Vector2>(sides + 1);
            var step = TWO_PI / sides;
            
            // Unroll loop partially for better performance
            for (int i = 0; i < sides; i++)
            {
                var theta = i * step;
                vectors.Add(new Vector2((float)(radiusx * Cos(theta)), (float)(radiusy * Sin(theta))));
            }
            
            vectors.Add(vectors[0]); // Close the circle

            _circleBuffer[key] = vectors;
            return vectors;
        }

        private List<(Vector2 v1, Vector2 v2, Vector2 v3)> CreateEllipseTriangles(double radiusx, double radiusy)
        {
            var points = CreateEllipsePoints(radiusx, radiusy);
            var triangles = new List<(Vector2 v1, Vector2 v2, Vector2 v3)>(points.Count - 1);

            for (var i = 1; i < points.Count; i++)
                triangles.Add((points[i - 1], points[i], Vector2.Zero));

            return triangles;
        }

        private List<(Vector2 v1, Vector2 v2, Vector2 v3)> CreateArcTriangles(float radiusx, float radiusy, float startingAngle, float radians, bool chord = false)
        {
            var points = CreateArcPointsOptimized(radiusx, radiusy, startingAngle, radians);
            var triangles = new List<(Vector2 v1, Vector2 v2, Vector2 v3)>(points.Count);

            for (int i = 1; i < points.Count; i++)
                triangles.Add((points[i - 1], points[i], Vector2.Zero));

            if (chord)
                triangles.Add((points[points.Count - 1], points[0], Vector2.Zero));

            return triangles;
        }

        private void DrawBezier(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Color color, float thickness)
        {
            var pointCount = 0;
            var step = 1.0f / sides;
            
            if (sides + 1 > _pointBuffer.Length)
                Array.Resize(ref _pointBuffer, sides + 1);
            
            for (var t = 0.0f; t <= 1.0f; t += step)
            {
                var oneMinusT = 1 - t;
                var oneMinusTSquared = oneMinusT * oneMinusT;
                var oneMinusTCubed = oneMinusTSquared * oneMinusT;
                var tSquared = t * t;
                var tCubed = tSquared * t;
                
                _pointBuffer[pointCount++] = oneMinusTCubed * p1 + 3.0f * oneMinusTSquared * t * p2 +
                                              3.0f * oneMinusT * tSquared * p3 + tCubed * p4;
            }

            DrawPointsOptimized(Vector2.Zero, pointCount, color, thickness);
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

            var pointCount = 0;
            var step = (t2 - t1) / sides;
            
            if (sides + 1 > _pointBuffer.Length)
                Array.Resize(ref _pointBuffer, sides + 1);
            
            for (var t = t1; t <= t2; t += step)
            {
                var a1 = (t1 - t) / (t1 - t0) * p0 + (t - t0) / (t1 - t0) * p1;
                var a2 = (t2 - t) / (t2 - t1) * p1 + (t - t1) / (t2 - t1) * p2;
                var a3 = (t3 - t) / (t3 - t2) * p2 + (t - t2) / (t3 - t2) * p3;

                var b1 = (t2 - t) / (t2 - t0) * a1 + (t - t0) / (t2 - t0) * a2;
                var b2 = (t3 - t) / (t3 - t1) * a2 + (t - t1) / (t3 - t1) * a3;

                _pointBuffer[pointCount++] = (t2 - t) / (t2 - t1) * b1 + (t - t1) / (t2 - t1) * b2;
            }

            DrawPointsOptimized(Vector2.Zero, pointCount, color, thickness);
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

        private List<Vector2> CreateArcPoints(float radiusx, float radiusy, float startingAngle, float radians, int mode = 0)
        {
            return CreateArcPointsOptimized(radiusx, radiusy, startingAngle, radians, mode);
        }

        private List<Vector2> CreateArcPointsOptimized(float radiusx, float radiusy, float startingAngle, float radians, int mode = 0)
        {
            var key = (radiusx, radiusy, startingAngle, radians);
            if (mode == 0 && _arcBuffer.TryGetValue(key, out var cached))
                return cached;

            var step = (radians - startingAngle) / sides;
            var points = new List<Vector2>(sides + 3);

            for (int i = 0; i <= sides; i++)
            {
                var theta = startingAngle + i * step;
                if (theta > radians) theta = radians;
                points.Add(new Vector2((float)(radiusx * Cos(theta)), (float)(radiusy * Sin(theta))));
                if (theta >= radians) break;
            }

            if (mode == 1) points.Add(points[0]);
            if (mode == 2)
            {
                points.Add(Vector2.Zero);
                points.Add(points[0]);
            }

            if (mode == 0)
                _arcBuffer[key] = points;

            return points;
        }

        private void DrawTriangle(Vector2 v1, Vector2 v2, Vector2 v3, Color color, float thickness)
        {
            _pointBuffer[0] = v1;
            _pointBuffer[1] = v2;
            _pointBuffer[2] = v3;
            _pointBuffer[3] = v1;
            DrawPointsOptimized(Vector2.Zero, 4, color, thickness);
        }

        public void FillPolygonWithHoles(Vector2 position, List<List<Vector2>> contours, Color color)
        {
            var polygon = new Polygon();

            var mainContour = contours[0];
            var points = new Vertex[mainContour.Count];
            for (int i = 0; i < mainContour.Count; i++)
            {
                points[i] = new Vertex(mainContour[i].X, mainContour[i].Y);
            }
            polygon.Add(new Contour(points));

            for (int i = 1; i < contours.Count; i++)
            {
                List<Vector2> hole = contours[i];
                points = new Vertex[hole.Count];
                for (int j = 0; j < hole.Count; j++)
                {
                    points[j] = new Vertex(hole[j].X, hole[j].Y);
                }
                polygon.Add(new Contour(points), true);
            }

            ConstraintOptions options = new ConstraintOptions() { ConformingDelaunay = true };
            var mesh = polygon.Triangulate(options);

            var triangleCount = 0;
            foreach (Triangle triangle in mesh.Triangles)
            {
                if (triangleCount >= _triangleBuffer.Length)
                    Array.Resize(ref _triangleBuffer, _triangleBuffer.Length * 2);
                    
                Vector2 p1 = new Vector2((float)triangle.GetVertex(0).X, (float)triangle.GetVertex(0).Y) + position;
                Vector2 p2 = new Vector2((float)triangle.GetVertex(1).X, (float)triangle.GetVertex(1).Y) + position;
                Vector2 p3 = new Vector2((float)triangle.GetVertex(2).X, (float)triangle.GetVertex(2).Y) + position;
                _triangleBuffer[triangleCount++] = (p1, p2, p3);
            }

            FillTriangleBuffer(triangleCount, color);
        }

        private void FillTriangles(Vector2 position, List<(Vector2 v1, Vector2 v2, Vector2 v3)> triangles, Color color)
        {
            var count = triangles.Count;
            if (count > _triangleBuffer.Length)
                Array.Resize(ref _triangleBuffer, count);
                
            for (int i = 0; i < count; i++)
            {
                var t = triangles[i];
                _triangleBuffer[i] = (t.v3 + position, t.v1 + position, t.v2 + position);
            }
            
            FillTriangleBuffer(count, color);
        }

        private void DrawRectangle(float x, float y, float w, float h, Color color, float thickness)
        {
            _pointBuffer[0] = new Vector2(x, y);
            _pointBuffer[1] = new Vector2(x + w, y);
            _pointBuffer[2] = new Vector2(x + w, y + h);
            _pointBuffer[3] = new Vector2(x, y + h);
            _pointBuffer[4] = _pointBuffer[0];
            DrawPointsOptimized(Vector2.Zero, 5, color, thickness);
        }

        private void FillRectangle(float x, float y, float w, float h, Color color)
        {
            _triangleBuffer[0] = (new Vector2(x, y), new Vector2(x + w, y), new Vector2(x + w, y + h));
            _triangleBuffer[1] = (new Vector2(x + w, y + h), new Vector2(x, y + h), new Vector2(x, y));
            FillTriangleBuffer(2, color);
        }

        private void DrawQuad(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Color color, float thickness)
        {
            _pointBuffer[0] = p1;
            _pointBuffer[1] = p2;
            _pointBuffer[2] = p3;
            _pointBuffer[3] = p4;
            _pointBuffer[4] = p1;
            DrawPointsOptimized(Vector2.Zero, 5, color, thickness);
        }

        private void FillQuad(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Color color)
        {
            _triangleBuffer[0] = (p1, p2, p3);
            _triangleBuffer[1] = (p3, p4, p1);
            FillTriangleBuffer(2, color);
        }

        private void DrawLine(IEnumerable<(float x1, float y1, float x2, float y2)> lines, Color color, float thickness)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, _style.BlendMode, null, null, null, null, _matrix);
            
            foreach (var (x1, y1, x2, y2) in lines)
            {
                var p1 = new Vector2(x1, y1);
                var p2 = new Vector2(x2, y2);
                var distance = Vector2.Distance(p1, p2);
                var angle = (float)Atan2(y2 - y1, x2 - x1);
                
                _spriteBatch.Draw(_pixel, p1, null, color, angle, new Vector2(0, 0.5f), new Vector2(distance, thickness), SpriteEffects.None, 0);
            }
            
            _spriteBatch.End();
        }

        private void DrawLine(float x1, float y1, float x2, float y2, Color color, float thickness)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, _style.BlendMode, null, null, null, null, _matrix);
            
            var p1 = new Vector2(x1, y1);
            var p2 = new Vector2(x2, y2);
            var distance = Vector2.Distance(p1, p2);
            var angle = (float)Atan2(y2 - y1, x2 - x1);
            
            _spriteBatch.Draw(_pixel, p1, null, color, angle, new Vector2(0, 0.5f), new Vector2(distance, thickness), SpriteEffects.None, 0);
            
            _spriteBatch.End();
        }

        private void DrawEllipse(Vector2 center, float radiusx, float radiusy, Color color, float thickness) =>
            DrawPoints(center, CreateEllipsePoints(radiusx, radiusy), color, thickness);

        private void FillEllipse(Vector2 center, float radiusx, float radiusy, Color color) =>
            FillTriangles(center, CreateEllipseTriangles(radiusx, radiusy), color);

        private void DrawArc(Vector2 center, float radiusx, float radiusy, float startingAngle, float radians, Color color, float thickness, int mode = 0) =>
            DrawPoints(center, CreateArcPointsOptimized(radiusx, radiusy, startingAngle, radians, mode), color, thickness);

        private void FillArc(Vector2 center, float radiusx, float radiusy, float startingAngle, float radians, Color color, bool chord = false) =>
            FillTriangles(center, CreateArcTriangles(radiusx, radiusy, startingAngle, radians, chord), color);
    }
}