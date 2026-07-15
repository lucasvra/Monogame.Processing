using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monogame.Processing
{
    /// <summary>
    /// Off-screen Processing drawing surface backed by a MonoGame RenderTarget2D.
    /// Use beginDraw()/endDraw() around drawing commands, then render it with image().
    /// </summary>
    public class PGraphics : PImage, IDisposable
    {
        private readonly Processing _processing;
        private bool _isDrawing;

        internal RenderTarget2D RenderTarget => (RenderTarget2D)texture;

        internal PGraphics(Processing processing, int w, int h)
            : base(new RenderTarget2D(
                PImage.graphicsDevice,
                w,
                h,
                false,
                PImage.graphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24))
        {
            _processing = processing;
        }

        public void beginDraw()
        {
            if (_isDrawing) return;
            _processing.BeginPGraphics(this);
            _isDrawing = true;
        }

        public void endDraw()
        {
            if (!_isDrawing) return;
            _processing.EndPGraphics(this);
            _isDrawing = false;
        }

        public void Dispose()
        {
            texture?.Dispose();
            GC.SuppressFinalize(this);
        }

        public void background(float gray) => WithDrawing(() => _processing.background(gray));
        public void background(float v1, float v2, float v3, float alpha = 255) => WithDrawing(() => _processing.background(v1, v2, v3, alpha));
        public void background(color rgb, float alpha) => WithDrawing(() => _processing.background(rgb, alpha));
        public void background(PImage img) => WithDrawing(() => _processing.background(img));
        public void clear() => WithDrawing(() => _processing.clear());

        public void fill(int rgb, byte alpha = 255) => _processing.fill(rgb, alpha);
        public void fill(color c) => _processing.fill(c);
        public void fill(float gray) => _processing.fill(gray);
        public void fill(float v1, float v2, float v3, float alpha = 255) => _processing.fill(v1, v2, v3, alpha);
        public void noFill() => _processing.noFill();
        public void stroke(int rgb, float alpha) => _processing.stroke(rgb, alpha);
        public void stroke(float gray) => _processing.stroke(gray);
        public void stroke(float v1, float v2, float v3, float alpha = 255) => _processing.stroke(v1, v2, v3, alpha);
        public void strokeWeight(float weight) => _processing.strokeWeight(weight);
        public void noStroke() => _processing.noStroke();
        public void tint(int rgb, byte alpha = 255) => _processing.tint(rgb, alpha);
        public void tint(color c) => _processing.tint(c);
        public void tint(float gray) => _processing.tint(gray);
        public void tint(float v1, float v2, float v3, float alpha = 255) => _processing.tint(v1, v2, v3, alpha);
        public void noTint() => _processing.noTint();

        public void rect(float a, float b, float c, float d) => WithDrawing(() => _processing.rect(a, b, c, d));
        public void rect(float a, float b, float c, float d, float tl, float tr = -1, float br = -1, float bl = -1) => WithDrawing(() => _processing.rect(a, b, c, d, tl, tr, br, bl));
        public void ellipse(float a, float b, float c, float d) => WithDrawing(() => _processing.ellipse(a, b, c, d));
        public void circle(float x, float y, float extent) => WithDrawing(() => _processing.circle(x, y, extent));
        public void square(float x, float y, float extent) => WithDrawing(() => _processing.square(x, y, extent));
        public void line(float x1, float y1, float x2, float y2) => WithDrawing(() => _processing.line(x1, y1, x2, y2));
        public void point(float x, float y, float z = 0) => WithDrawing(() => _processing.point(x, y, z));
        public void triangle(float x1, float y1, float x2, float y2, float x3, float y3) => WithDrawing(() => _processing.triangle(x1, y1, x2, y2, x3, y3));
        public void quad(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4) => WithDrawing(() => _processing.quad(x1, y1, x2, y2, x3, y3, x4, y4));
        public void arc(float a, float b, float c, float d, float start, float stop, ArcMode mode = ArcMode.OPEN) => WithDrawing(() => _processing.arc(a, b, c, d, start, stop, mode));
        public void image(PImage img, float a, float b, float c, float d) => WithDrawing(() => _processing.image(img, a, b, c, d));
        public void image(PImage img, float a, float b) => WithDrawing(() => _processing.image(img, a, b));

        public void clip(float x, float y, float w, float h) => WithDrawing(() => _processing.clip(x, y, w, h));
        public void noClip() => WithDrawing(() => _processing.noClip());
        public void hint(params int[] options) => _processing.hint(options);

        private void WithDrawing(Action action)
        {
            if (_isDrawing)
            {
                action();
                return;
            }

            beginDraw();
            action();
            endDraw();
        }
    }
}
