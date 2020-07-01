using Microsoft.Xna.Framework.Graphics;

namespace Monogame.Processing
{
    partial class Processing
    {
        #region Attributes

        public void blendMode(BlendMode mode)
        {
            _style.BlendMode = mode switch
            {
                BlendMode.ADD => new BlendState
                {
                    ColorBlendFunction = BlendFunction.Add
                },
                BlendMode.DODGE => new BlendState
                {
                    ColorSourceBlend = Blend.One,
                    ColorDestinationBlend = Blend.One,
                    ColorBlendFunction = BlendFunction.Add
                },
                BlendMode.MULTIPLY => new BlendState
                {
                    ColorSourceBlend = Blend.DestinationColor,
                    ColorDestinationBlend = Blend.Zero,
                    ColorBlendFunction = BlendFunction.Add
                },
                BlendMode.LIGHTEST => new BlendState
                {
                    ColorSourceBlend = Blend.One,
                    ColorDestinationBlend = Blend.One,
                    ColorBlendFunction = BlendFunction.Max
                },
                BlendMode.DARKEST => new BlendState
                {
                    ColorSourceBlend = Blend.One,
                    ColorDestinationBlend = Blend.One,
                    ColorBlendFunction = BlendFunction.Min
                },
                BlendMode.SUBTRACT => new BlendState
                {
                    ColorBlendFunction = BlendFunction.Subtract
                },
                _ => BlendState.AlphaBlend,
            };
        }

        /// <summary>
        /// Modifies the location from which rectangles are drawn by changing the way in which 
        /// parameters given to rect() are interpreted. 
        /// The default mode is rectMode(CORNER), which interprets the first two parameters of 
        /// rect() as the upper-left corner of the shape, while the third and fourth parameters 
        /// are its width and height. 
        /// rectMode(CORNERS) interprets the first two parameters of 
        /// rect() as the location of one corner, and the third and fourth parameters as the 
        /// location of the opposite corner. 
        /// rectMode(CENTER) interprets the first two parameters 
        /// of rect() as the shape's center point, while the third and fourth parameters are its 
        /// width and height. 
        /// rectMode(RADIUS) also uses the first two parameters of rect() as 
        /// the shape's center point, but uses the third and fourth parameters to specify half 
        /// of the shapes's width and height.
        /// </summary>
        /// <param name="mode">int: either CORNER, CORNERS, CENTER, or RADIUS</param>
        public void rectMode(ShapeMode mode) => _style.RectMode = mode;

        /// <summary>
        /// Modifies the location from which rectangles are drawn by changing the way in which 
        /// parameters given to rect() are interpreted. 
        /// The default mode is rectMode(CORNER), which interprets the first two parameters of 
        /// rect() as the upper-left corner of the shape, while the third and fourth parameters 
        /// are its width and height. 
        /// rectMode(CORNERS) interprets the first two parameters of 
        /// rect() as the location of one corner, and the third and fourth parameters as the 
        /// location of the opposite corner. 
        /// rectMode(CENTER) interprets the first two parameters 
        /// of rect() as the shape's center point, while the third and fourth parameters are its 
        /// width and height. 
        /// rectMode(RADIUS) also uses the first two parameters of rect() as 
        /// the shape's center point, but uses the third and fourth parameters to specify half 
        /// of the shapes's width and height.
        /// </summary>
        /// <param name="mode">int: either CORNER, CORNERS, CENTER, or RADIUS</param>
        public void rectMode(int mode) => _style.RectMode = (ShapeMode)mode;

        /// <summary>
        /// Modifies the location from which ellipses are drawn by changing the way in which 
        /// parameters given to ellipse() are interpreted.The default mode is ellipseMode(CENTER), 
        /// which interprets the first two parameters of ellipse() as the shape's center point, 
        /// while the third and fourth parameters are its width and height. 
        /// ellipseMode(RADIUS) also uses the first two parameters of ellipse() as the shape's
        ///  center point, but uses the third and fourth parameters to specify half of the shapes's 
        /// width and height.
        /// ellipseMode(CORNER) interprets the first two parameters of ellipse() as the upper-left 
        /// corner of the shape, while the third and fourth parameters are its width and height.
        /// ellipseMode(CORNERS) interprets the first two parameters of ellipse() as the location
        ///  of one corner of the ellipse's bounding box, and the third and fourth parameters as the 
        /// location of the opposite corner.
        /// </summary>
        /// <param name="mode">int: either CENTER, RADIUS, CORNER, or CORNERS</param>
        public void ellipseMode(int mode) => _style.EllipseMode = (ShapeMode)mode;

        /// <summary>
        /// Modifies the location from which ellipses are drawn by changing the way in which 
        /// parameters given to ellipse() are interpreted.The default mode is ellipseMode(CENTER), 
        /// which interprets the first two parameters of ellipse() as the shape's center point, 
        /// while the third and fourth parameters are its width and height. 
        /// ellipseMode(RADIUS) also uses the first two parameters of ellipse() as the shape's
        ///  center point, but uses the third and fourth parameters to specify half of the shapes's 
        /// width and height.
        /// ellipseMode(CORNER) interprets the first two parameters of ellipse() as the upper-left 
        /// corner of the shape, while the third and fourth parameters are its width and height.
        /// ellipseMode(CORNERS) interprets the first two parameters of ellipse() as the location
        ///  of one corner of the ellipse's bounding box, and the third and fourth parameters as the 
        /// location of the opposite corner.
        /// </summary>
        /// <param name="mode">int: either CENTER, RADIUS, CORNER, or CORNERS</param>
        public void ellipseMode(ShapeMode mode) => _style.EllipseMode = mode;

        #endregion
    }
}
