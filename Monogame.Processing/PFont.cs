using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monogame.Processing
{
    /// <summary>
    /// Wraps a MonoGame SpriteFont asset for use with Processing typography APIs.
    /// </summary>
    public class PFont
    {
        internal const float DefaultSize = 48f;

        internal PFont(SpriteFont spriteFont, string name, float size = DefaultSize)
        {
            SpriteFont = spriteFont;
            Name = name;
            Size = size;
        }

        internal SpriteFont SpriteFont { get; }

        /// <summary>
        /// The asset name or path used to load this font.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The nominal font size used as the scaling baseline.
        /// </summary>
        public float Size { get; }

        internal Vector2 MeasureString(string text, float size)
        {
            return SpriteFont.MeasureString(text ?? string.Empty) * GetScale(size);
        }

        internal float GetScale(float size) => size / Size;
    }
}
