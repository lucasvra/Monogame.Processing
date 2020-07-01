using Microsoft.Xna.Framework;

namespace Monogame.Processing
{
    partial class Processing
    {
        #region Color

        public void noTint() => _style.Tint = Color.White;

        /// <summary>
        /// </summary>
        /// <param name="rgb">int: any value of the color datatype</param>
        /// <param name="alpha">float: opacity of the background</param>
        public void tint(int rgb, byte alpha = 255) =>
            _style.Tint = new color(((alpha << 24) + rgb));

        public void tint(color c) => _style.Tint = c;

        /// <summary>
        /// </summary>
        /// <param name="gray">float: opacity of the background</param>
        public void tint(float gray) => _style.Tint = new color((byte)gray, (byte)gray, (byte)gray);

        /// <summary>
        /// </summary>
        /// <param name="v1">float: red or hue value(depending on the current color mode) </param>
        /// <param name="v2">float: green or saturation value(depending on the current color mode) </param>
        /// <param name="v3">float: blue or brightness value (depending on the current color mode)</param>
        /// <param name="alpha">float: opacity of the background</param>
        public void tint(float v1, float v2, float v3, float alpha = 255) =>
            _style.Tint = new color((byte)v1, (byte)v2, (byte)v3, (byte)alpha);



        /// <summary>
        /// Draws all geometry with smooth (anti-aliased) edges. This behavior is the default, 
        /// so smooth() only needs to be used when a program needs to set the smoothing in a 
        /// different way. The level parameter increases the level of smoothness. This is the 
        /// level of over sampling applied to the graphics buffer.
        /// </summary>
        /// <param name="level">int: either 2, 3, 4, or 8 depending on the renderer</param>
        public void smooth(int level = 0)
        {
            _graphics.PreferMultiSampling = true;
            GraphicsDevice.PresentationParameters.MultiSampleCount = level;
            _graphics.ApplyChanges();
        }

        /// <summary>
        /// Draws all geometry and fonts with jagged (aliased) edges and images when hard edges 
        /// between the pixels when enlarged rather than interpolating pixels. Note that smooth() 
        /// is active by default, so it is necessary to call noSmooth() to disable smoothing of 
        /// geometry, fonts, and images
        /// </summary>
        public void noSmooth()
        {
            _graphics.PreferMultiSampling = false;
            _graphics.ApplyChanges();
        }

        /// <summary>
        /// Defines the dimension of the display window width and height in units of pixels. In a 
        /// program that has the setup() function, the size() function must be the first line of
        /// code inside setup().
        /// </summary>
        /// <param name="width">int: width of the display window in units of pixels</param>
        /// <param name="height">int: height of the display window in units of pixels</param>
        /// <param name="renderer"></param>
        public void size(int width, int height, int renderer = 0)
        {
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// The background() function sets the color used for the background of the Processing window. 
        /// The default background is light gray. This function is typically used within draw() to clear 
        /// the display window at the beginning of each frame, but it can be used inside setup() to set 
        /// the background on the first frame of animation or if the background need only be set once. 
        /// </summary>
        /// <param name="rgb">int: any value of the color datatype</param>
        /// <param name="alpha">float: opacity of the background</param>
        public void background(color rgb, float alpha)
        {
            background(new color(((int) alpha << 24) + rgb));
        }

        public void clear() => background(0);

        public void background(PImage img)
        {
            background(255);
            image(img, 0, 0, width, height);
        }

        /// <summary>
        /// The background() function sets the color used for the background of the Processing window. 
        /// The default background is light gray. This function is typically used within draw() to clear 
        /// the display window at the beginning of each frame, but it can be used inside setup() to set 
        /// the background on the first frame of animation or if the background need only be set once. 
        /// </summary>
        /// <param name="gray">float: specifies a value between white and black</param>
        public void background(float gray) =>
            FillRectangle(0, 0, width, height, new color((byte)gray, (byte)gray, (byte)gray));

        /// <summary>
        /// The background() function sets the color used for the background of the Processing window. 
        /// The default background is light gray. This function is typically used within draw() to clear 
        /// the display window at the beginning of each frame, but it can be used inside setup() to set 
        /// the background on the first frame of animation or if the background need only be set once. 
        /// </summary>
        /// <param name="v1">float: red or hue value(depending on the current color mode) </param>
        /// <param name="v2">float: green or saturation value(depending on the current color mode) </param>
        /// <param name="v3">float: blue or brightness value (depending on the current color mode)</param>
        /// <param name="alpha">float: opacity of the background</param>
        public void background(float v1, float v2, float v3, float alpha = 255)
        {
            var c = new color((byte) v1, (byte) v2, (byte) v3, (byte) alpha);
            FillRectangle(0, 0, width, height, c);
        }
        
        /// <summary>
        /// Sets the color used to fill shapes. For example, if you run fill(204, 102, 0), all subsequent 
        /// shapes will be filled with orange. This color is either specified in terms of the RGB or HSB color
        ///  depending on the current colorMode(). The default color space is RGB, with each value in the range
        ///  from 0 to 255.  
        /// </summary>
        /// <param name="rgb">int: any value of the color datatype</param>
        /// <param name="alpha">float: opacity of the background</param>
        public void fill(int rgb, byte alpha = 255) =>
            _style.Fill = new color(((alpha << 24) + rgb));

        public void fill(color c) => _style.Fill = c;

        /// <summary>
        /// Sets the color used to fill shapes. For example, if you run fill(204, 102, 0), all subsequent 
        /// shapes will be filled with orange. This color is either specified in terms of the RGB or HSB color
        ///  depending on the current colorMode(). The default color space is RGB, with each value in the range
        ///  from 0 to 255.
        /// </summary>
        /// <param name="gray">float: opacity of the background</param>
        public void fill(float gray) => _style.Fill = new color((byte)gray, (byte)gray, (byte)gray);

        /// <summary>
        /// Sets the color used to fill shapes. For example, if you run fill(204, 102, 0), all subsequent 
        /// shapes will be filled with orange. This color is either specified in terms of the RGB or HSB color
        ///  depending on the current colorMode(). The default color space is RGB, with each value in the range
        ///  from 0 to 255. 
        /// </summary>
        /// <param name="v1">float: red or hue value(depending on the current color mode) </param>
        /// <param name="v2">float: green or saturation value(depending on the current color mode) </param>
        /// <param name="v3">float: blue or brightness value (depending on the current color mode)</param>
        /// <param name="alpha">float: opacity of the background</param>
        public void fill(float v1, float v2, float v3, float alpha = 255) =>
            _style.Fill = new color((byte)v1, (byte)v2, (byte)v3, (byte)alpha);

        /// <summary>
        /// Disables filling geometry. If both noStroke() and noFill() are called, nothing will be drawn to the screen.
        /// </summary>
        public void noFill() => _style.Fill = Color.Transparent;

        /// <summary>
        /// Sets the color used to draw lines and borders around shapes. This color is either specified in 
        /// terms of the RGB or HSB color depending on the current colorMode(). The default color space is 
        /// RGB, with each value in the range from 0 to 255  
        /// </summary>
        /// <param name="rgb">int: any value of the color datatype</param>
        /// <param name="alpha">float: opacity of the background</param>
        public void stroke(int rgb, float alpha) => _style.Stroke = new color(((int)alpha << 24) + rgb);

        /// <summary>
        /// Sets the color used to draw lines and borders around shapes. This color is either specified in 
        /// terms of the RGB or HSB color depending on the current colorMode(). The default color space is 
        /// RGB, with each value in the range from 0 to 255  
        /// </summary>
        /// <param name="gray">float: opacity of the background</param>
        public void stroke(float gray) => _style.Stroke = new color((byte)gray, (byte)gray, (byte)gray);

        /// <summary>
        /// Sets the color used to draw lines and borders around shapes. This color is either specified in 
        /// terms of the RGB or HSB color depending on the current colorMode(). The default color space is 
        /// RGB, with each value in the range from 0 to 255
        /// </summary>
        /// <param name="v1">float: red or hue value(depending on the current color mode) </param>
        /// <param name="v2">float: green or saturation value(depending on the current color mode) </param>
        /// <param name="v3">float: blue or brightness value (depending on the current color mode)</param>
        /// <param name="alpha">float: opacity of the background</param>
        public void stroke(float v1, float v2, float v3, float alpha = 255) =>
            _style.Stroke = new color((byte)v1, (byte)v2, (byte)v3, (byte)alpha);

        /// <summary>
        /// Sets the width of the stroke used for lines, points, and the border around shapes. 
        /// All widths are set in units of pixels.
        /// </summary>
        /// <param name="weight">float: the weight (in pixels) of the stroke</param>
        public void strokeWeight(float weight) => _style.StrokeWidth = weight;

        public void noStroke() => _style.Stroke = Color.Transparent;

        #endregion
    }
}
