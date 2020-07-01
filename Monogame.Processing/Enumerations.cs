namespace Monogame.Processing
{
    public enum ShapeMode
    {
        CENTER = 0,
        RADIUS = 1,
        CORNER = 2,
        CORNERS = 3,
    }

    public enum ArcMode
    {
        OPEN = 0,
        CHORD = 1,
        PIE = 2,
    }

    public enum MouseButton
    {
        LEFT = 2,
        RIGHT = 1,
        CENTER = 0,
    }

    public enum BlendMode
    {
        BLEND, //linear interpolation of colours: C = A * factor + B
        ADD, //additive blending with white clip: C = min(A * factor + B, 255)
        SUBTRACT, //subtractive blending with black clip: C = max(B, //A * factor, 0)
        DARKEST, //only the darkest colour succeeds: C = min(A * factor, B)
        LIGHTEST, //only the lightest colour succeeds: C = max(A * factor, B)
        DIFFERENCE, //subtract colors from underlying image.
        EXCLUSION, //similar to DIFFERENCE, but less extreme.
        MULTIPLY, //Multiply the colors, result will always be darker.
        SCREEN, //Opposite multiply, uses inverse values of the colors.
        OVERLAY, //A mix of MULTIPLY and SCREEN. Multiplies dark values, and screens light values.
        HARD_LIGHT, //SCREEN when greater than 50% gray, MULTIPLY when lower.
        SOFT_LIGHT, //Mix of DARKEST and LIGHTEST. Works like OVERLAY, but not as harsh.
        DODGE, //Lightens light tones and increases contrast, ignores darks. Called "Color Dodge" in Illustrator and Photoshop.
        BURN, //Darker areas are applied, increasing contrast, ignores lights. Called "Color Burn" in Illustrator and Photoshop.
    }

    public enum Filter
    {
        /// <summary>
        /// Converts the image to black and white pixels depending if they are above or below the threshold defined by the level parameter. The parameter must be between 0.0 (black) and 1.0 (white). If no level is specified, 0.5 is used.
        /// </summary>
        THRESHOLD,
        /// <summary>
        /// Converts any colors in the image to grayscale equivalents. No parameter is used
        /// </summary>
        GRAY,
        /// <summary>
        /// Sets the alpha channel to entirely opaque. No parameter is used.
        /// </summary>
        OPAQUE,
        /// <summary>
        /// Sets each pixel to its inverse value. No parameter is used.
        /// </summary>
        INVERT,
        /// <summary>
        /// Limits each channel of the image to the number of colors specified as the parameter. The parameter can be set to values between 2 and 255, but results are most noticeable in the lower ranges.
        /// </summary>
        POSTERIZE,
        /// <summary>
        /// Executes a Gaussian blur with the level parameter specifying the extent of the blurring. If no parameter is used, the blur is equivalent to Gaussian blur of radius 1. Larger values increase the blur.
        /// </summary>
        BLUR,
        /// <summary>
        /// Reduces the light areas. No parameter is used.
        /// </summary>
        ERODE,
        /// <summary>
        /// Increases the light areas. No parameter is used.
        /// </summary>
        DILATE
    }


}
