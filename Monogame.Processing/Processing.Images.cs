using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Monogame.Processing
{
    partial class Processing
    {
        #region Images

        public void loadPixels()
        {
            pixels = new color[_nextFrame.Width * _nextFrame.Height];
            _nextFrame.GetData(pixels);
        }

        public void updatePixels()
        {
            if (pixels == null || pixels.Length == 0) return;
            _nextFrame.SetData(pixels);
        }

        public PImage loadImage(string filename) => new PImage(filename);
        public PImage requestImage(string filename) => PImage.Request(filename);

        public void imageMode(int mode) => _style.ImageMode = (ShapeMode)mode;
        public void imageMode(ShapeMode mode) => _style.ImageMode = mode;

        public void image(PImage img, float a, float b, float c, float d)
        {
            if (img == null || !img.IsLoaded) return;
            var (x, y, w, h) = _style.ImageMode switch
            {
                ShapeMode.CENTER => (a - c / 2, b - d / 2, c, d),
                ShapeMode.RADIUS => (a - c, b - d, c * 2, d * 2),
                ShapeMode.CORNERS => (a, b, c - a, d - b),
                _ => (a, b, c, d),
            };
            DrawImage(img.texture, x, y, w, h, _style.Tint);
        }

        public void image(PImage img, float a, float b) => image(img, a, b, img.width, img.height);
        public PImage createImage(int w, int h, int format) => new PImage(w, h);

        public color get(int x, int y)
        {
            var p = new color[1];
            _nextFrame.GetData(0, new Rectangle(x, y, 1, 1), p, 0, 1);
            return p[0];
        }

        public PImage get(int x, int y, int w, int h)
        {
            var p = new color[w * h];
            _nextFrame.GetData(0, new Rectangle(x, y, w, h), p, 0, p.Length);
            var img = new PImage(w, h);
            img.pixels = p.Select(c => (Color)c).ToArray();
            img.updatePixels();
            return img;
        }

        public void set(int x, int y, color c)
        {
            var p = new[] { c };
            _nextFrame.SetData(0, new Rectangle(x, y, 1, 1), p, 0, 1);
        }

        public void set(int x, int y, PImage img)
        {
            if (img == null || !img.IsLoaded) return;
            DrawImage(img.texture, x, y, img.width, img.height, Color.White);
        }

        public void textureMode(int mode) => _style.TextureMode = (TextureMode)mode;
        public void textureMode(TextureMode mode) => _style.TextureMode = mode;
        public void textureWrap(int wrap) => _style.TextureWrap = (TextureWrap)wrap;
        public void textureWrap(TextureWrap wrap) => _style.TextureWrap = wrap;
        public void texture(PImage image) => currentTexture = image;

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
