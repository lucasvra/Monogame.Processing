using static System.Math;

namespace Monogame.Processing
{
    partial class Processing
    {
        #region Trigonometry

        /// <summary>
        /// The inverse of cos(), returns the arc cosine of a value. 
        /// This function expects the values in the range of -1 to 1 and 
        /// values are returned in the range 0 to PI (3.1415927).
        /// </summary>
        /// <param name="value">float: the value whose arc cosine is to be returned</param>
        /// <returns>float</returns>
        public float acos(float value) => (float)Acos(value);

        /// <summary>
        /// The inverse of sin(), returns the arc sine of a value. 
        /// This function expects the values in the range of -1 to 1 and 
        /// values are returned in the range -PI/2 to PI/2.
        /// </summary>
        /// <param name="value">	float: the value whose arc sine is to be returned</param>
        /// <returns>float</returns>
        public float asin(float value) => (float)Asin(value);

        /// <summary>
        /// The inverse of tan(), returns the arc tangent of a value. 
        /// This function expects the values in the range of -Infinity to 
        /// Infinity (exclusive) and values are returned in the range -PI/2 to PI/2.
        /// </summary>
        /// <param name="value">float: -Infinity to Infinity (exclusive)</param>
        /// <returns>float</returns>
        public float atan(float value) => (float)Atan(value);

        /// <summary>
        /// Calculates the angle (in radians) from a specified point to the coordinate 
        /// origin as measured from the positive x-axis. Values are returned as a float 
        /// in the range from PI to -PI. The atan2() function is most often used for orienting 
        /// geometry to the position of the cursor. Note: The y-coordinate of the point is the 
        /// first parameter, and the x-coordinate is the second parameter, due the the 
        /// structure of calculating the tangent.
        /// </summary>
        /// <param name="y">float: y-coordinate of the point</param>
        /// <param name="x">float: x-coordinate of the point</param>
        /// <returns>float</returns>
        public float atan2(float y, float x) => (float)Atan2(y, x);

        /// <summary>
        /// Calculates the cosine of an angle. This function expects the values of the angle 
        /// parameter to be provided in radians (values from 0 to PI*2). Values are returned 
        /// in the range -1 to 1.
        /// </summary>
        /// <param name="angle">float: an angle in radians</param>
        /// <returns>float</returns>
        public float cos(float angle) => (float)Cos(angle);

        /// <summary>
        /// Calculates the sine of an angle. This function expects the values of the angle 
        /// parameter to be provided in radians (values from 0 to 6.28). Values are returned 
        /// in the range -1 to 1.
        /// </summary>
        /// <param name="angle">float: an angle in radians</param>
        /// <returns>float</returns>
        public float sin(float angle) => (float)Sin(angle);

        /// <summary>
        /// Calculates the ratio of the sine and cosine of an angle. This function expects 
        /// the values of the angle parameter to be provided in radians (values from 0 to PI*2). 
        /// Values are returned in the range infinity to -infinity.
        /// </summary>
        /// <param name="angle">float: an angle in radians</param>
        /// <returns>float</returns>
        public float tan(float angle) => (float)Tan(angle);

        /// <summary>
        /// Converts a radian measurement to its corresponding value in degrees. 
        /// Radians and degrees are two ways of measuring the same thing. There are 
        /// 360 degrees in a circle and 2*PI radians in a circle. For example, 
        /// 90° = PI/2 = 1.5707964. All trigonometric functions in Processing require 
        /// their parameters to be specified in radians.
        /// </summary>
        /// <param name="radians">float: radian value to convert to degrees</param>
        /// <returns>float</returns>
        public float degrees(float radians) => radians * 180 / PI;

        /// <summary>
        /// Converts a degree measurement to its corresponding value in radians. 
        /// Radians and degrees are two ways of measuring the same thing. There are 
        /// 360 degrees in a circle and 2*PI radians in a circle. For example, 
        /// 90° = PI/2 = 1.5707964. All trigonometric functions in Processing require 
        /// their parameters to be specified in radians.
        /// </summary>
        /// <param name="degrees">float: degree value to convert to radians</param>
        /// <returns>float</returns>
        public float radians(float degrees) => degrees * PI / 180;

        #endregion
    }
}
