using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monogame.Processing
{
    public struct Style
    {
        public Color Fill;
        public Color Stroke;
        public Color Tint;

        public float StrokeWidth;
        public float TextSize;
        public ShapeMode RectMode;
        public ShapeMode EllipseMode;
        public BlendState BlendMode;

        public TextAlign TextAlign { get; internal set; }
    }
}
