using Microsoft.Xna.Framework;

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
            _nextFrame.SetData(pixels);
        }

        public PImage loadImage(string filename) => new PImage(filename);
        public void image(PImage img, float a, float b, float c, float d) => DrawImage(img.texture, a, b, c, d);
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
