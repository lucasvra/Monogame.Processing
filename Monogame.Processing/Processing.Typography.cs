using System;
using Microsoft.Xna.Framework.Graphics;

namespace Monogame.Processing
{
    public abstract partial class Processing
    {
        /// <summary>
        /// Loads a SpriteFont asset and wraps it as a PFont.
        /// </summary>
        /// <param name="path">Content pipeline asset name or path without extension.</param>
        public PFont loadFont(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Font path cannot be empty.", nameof(path));

            var font = Content.Load<SpriteFont>(path);
            return new PFont(font, path);
        }

        /// <summary>
        /// Creates a PFont from a SpriteFont asset name using the supplied nominal size.
        /// </summary>
        /// <param name="name">Content pipeline asset name.</param>
        /// <param name="size">Nominal font size for text scaling.</param>
        public PFont createFont(string name, float size)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Font name cannot be empty.", nameof(name));
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size), "Font size must be greater than zero.");

            var font = Content.Load<SpriteFont>(name);
            return new PFont(font, name, size);
        }

        /// <summary>
        /// Sets the current font used by text rendering.
        /// </summary>
        public void textFont(PFont font)
        {
            _style.TextFont = font ?? throw new ArgumentNullException(nameof(font));
        }

        /// <summary>
        /// Sets the current font and text size used by text rendering.
        /// </summary>
        public void textFont(PFont font, float size)
        {
            textFont(font);
            textSize(size);
        }

        /// <summary>
        /// Sets the spacing between lines of text.
        /// </summary>
        public void textLeading(float leading) => _style.TextLeading = leading;

        /// <summary>
        /// Calculates the width of text using the current font and text size.
        /// </summary>
        public float textWidth(string text)
        {
            var lines = (text ?? string.Empty).Split('\n');
            var maxWidth = 0f;

            foreach (var line in lines)
            {
                var width = CurrentFont.MeasureString(line.TrimEnd('\r'), _style.TextSize).X;
                if (width > maxWidth) maxWidth = width;
            }

            return maxWidth;
        }

        /// <summary>
        /// Returns an approximation of the ascent for the current font and text size.
        /// </summary>
        public float textAscent() => CurrentFont.SpriteFont.LineSpacing * CurrentFont.GetScale(_style.TextSize) * 0.8f;

        /// <summary>
        /// Returns an approximation of the descent for the current font and text size.
        /// </summary>
        public float textDescent() => CurrentFont.SpriteFont.LineSpacing * CurrentFont.GetScale(_style.TextSize) * 0.2f;

        /// <summary>
        /// Sets the text rendering mode. Currently only MODEL is supported.
        /// </summary>
        public void textMode(TextMode mode)
        {
            if (mode != TextMode.MODEL) throw new NotSupportedException("Only TextMode.MODEL is currently supported.");
            _style.TextMode = mode;
        }

        /// <summary>
        /// Sets the text rendering mode. Currently only MODEL is supported.
        /// </summary>
        public void textMode(int mode) => textMode((TextMode)mode);

        private PFont CurrentFont => _style.TextFont ?? new PFont(_basicFont, "font");
    }
}
