using System.Linq;
using static System.Math;

namespace Monogame.Processing
{
    partial class Processing
    {
        #region Calculation

        /// <summary>
        /// Calculates the absolute value (magnitude) of a number. The absolute 
        /// value of a number is always positive.
        /// </summary>
        /// <param name="n">int: number to compute</param>
        /// <returns>int</returns>
        public int abs(int n) => n < 0 ? -n : n;

        /// <summary>
        /// Calculates the absolute value (magnitude) of a number. The absolute 
        /// value of a number is always positive.
        /// </summary>
        /// <param name="n">float: number to compute</param>
        /// <returns>float</returns>
        public float abs(float n) => n < 0 ? -n : n;

        /// <summary>
        /// Calculates the closest int value that is greater than or equal to the 
        /// value of the parameter. For example, ceil(9.03) returns the value 10.
        /// </summary>
        /// <param name="n">float: number to round up</param>
        /// <returns>int</returns>
        public int ceil(float n) => (int)Ceiling(n);

        /// <summary>
        /// Constrains a value to not exceed a maximum and minimum value.
        /// </summary>
        /// <param name="amt">int: the value to constrain</param>
        /// <param name="low">int: minimum limit</param>
        /// <param name="high">	int: maximum limit</param>
        /// <returns>int</returns>
        public int constrain(int amt, int low, int high)
        {
            if (amt < low) return low;
            return amt > high ? high : amt;
        }

        /// <summary>
        /// Constrains a value to not exceed a maximum and minimum value.
        /// </summary>
        /// <param name="amt">float: the value to constrain</param>
        /// <param name="low">float: minimum limit</param>
        /// <param name="high">float: maximum limit</param>
        /// <returns>float</returns>
        public float constrain(float amt, float low, float high)
        {
            if (amt < low) return low;
            return amt > high ? high : amt;
        }

        /// <summary>
        /// Calculates the distance between two points.
        /// </summary>
        /// <param name="x1">float: x-coordinate of the first point</param>
        /// <param name="y1">float: y-coordinate of the first point</param>
        /// <param name="x2">float: x-coordinate of the second point</param>
        /// <param name="y2">float: y-coordinate of the second point</param>
        /// <returns>float</returns>
        public float dist(float x1, float y1, float x2, float y2) => (float)Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));

        /// <summary>
        /// Calculates the distance between two points.
        /// </summary>
        /// <param name="x1">float: x-coordinate of the first point</param>
        /// <param name="y1">float: y-coordinate of the first point</param>
        /// <param name="z1">float: z-coordinate of the first point</param>
        /// <param name="x2">float: x-coordinate of the second point</param>
        /// <param name="y2">float: y-coordinate of the second point</param>
        /// <param name="z2">float: z-coordinate of the second point</param>
        /// <returns>float</returns>
        public float dist(float x1, float y1, float z1, float x2, float y2, float z2) => (float)Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1) + (z2 - z1) * (z2 - z1));

        /// <summary>
        /// Returns Euler's number e (2.71828...) raised to the power of 
        /// the n parameter.
        /// </summary>
        /// <param name="n">float: exponent to raise</param>
        /// <returns>float</returns>
        public float exp(float n) => (float)Exp(n);

        /// <summary>
        /// Calculates the closest int value that is less than or equal 
        /// to the value of the parameter.
        /// </summary>
        /// <param name="n">float: number to round down</param>
        /// <returns>int</returns>
        public int floor(float n) => (int)Floor(n);

        /// <summary>
        /// Calculates a number between two numbers at a specific increment. 
        /// The amt parameter is the amount to interpolate between the two 
        /// values where 0.0 equal to the first point, 0.1 is very near the 
        /// first point, 0.5 is half-way in between, etc. The lerp function 
        /// is convenient for creating motion along a straight path and for 
        /// drawing dotted lines.
        /// </summary>
        /// <param name="start">float: first value</param>
        /// <param name="stop">float: second value</param>
        /// <param name="amp">float: float between 0.0 and 1.0</param>
        /// <returns>float</returns>
        public float lerp(float start, float stop, float amp) => start + (stop - start) * amp;

        /// <summary>
        /// Calculates the natural logarithm (the base-e logarithm) of a number. 
        /// This function expects the n parameter to be a value greater than 0.0.
        /// </summary>
        /// <param name="n">float: number greater than 0.0</param>
        /// <returns>float</returns>
        public float log(float n) => (float)Log(n);

        /// <summary>
        /// Calculates the magnitude (or length) of a vector. A vector is a direction 
        /// in space commonly used in computer graphics and linear algebra. Because it 
        /// has no "start" position, the magnitude of a vector can be thought of as the 
        /// distance from the coordinate 0,0 to its x,y value. Therefore, mag() is a 
        /// shortcut for writing dist(0, 0, x, y).
        /// </summary>
        /// <param name="a">float: first value</param>
        /// <param name="b">float: second value</param>
        /// <param name="c">float: third value</param>
        /// <returns>float</returns>
        public float mag(float a, float b, float c = 0) => dist(0, 0, 0, a, b, c);

        /// <summary>
        /// Re-maps a number from one range to another.
        /// </summary>
        /// <param name="value">float: the incoming value to be converted</param>
        /// <param name="start1">float: lower bound of the value's current range</param>
        /// <param name="stop1">float: upper bound of the value's current range</param>
        /// <param name="start2">float: lower bound of the value's target range</param>
        /// <param name="stop2">float: upper bound of the value's target range</param>
        /// <returns></returns>
        public float map(float value, float start1, float stop1, float start2, float stop2)
        {
            var amp = (value - start1) / (stop1 - start1);
            return start2 + (stop2 - start2) * amp;
        }

        /// <summary>
        /// Determines the largest value in a sequence of numbers, and then returns that value. 
        /// max() accepts either two or three float or int values as parameters, or an array 
        /// of any length.
        /// </summary>
        /// <param name="a">float: first number to compare</param>
        /// <param name="b">float: second number to compare</param>
        /// <param name="c">float: third number to compare</param>
        /// <returns>float</returns>
        public float max(float a, float b, float c = float.NegativeInfinity)
        {
            if (a > b && a > c) return a;
            if (b > c && b > a) return b;
            return c;
        }

        /// <summary>
        /// Determines the largest value in a sequence of numbers, and then returns that value. 
        /// max() accepts either two or three float or int values as parameters, or an array 
        /// of any length.
        /// </summary>
        /// <param name="a">int: first number to compare</param>
        /// <param name="b">int: second number to compare</param>
        /// <param name="c">int: third number to compare</param>
        /// <returns>int</returns>
        public int max(int a, int b, int c = int.MinValue)
        {
            if (a > b && a > c) return a;
            if (b > c && b > a) return b;
            return c;
        }

        /// <summary>
        /// Determines the largest value in a sequence of numbers, and then returns that value. 
        /// max() accepts either two or three float or int values as parameters, or an array 
        /// of any length.
        /// </summary>
        /// <param name="list">float[]: array of numbers to compare</param>
        /// <returns>float</returns>
        public float max(float[] list) => list.Max();

        /// <summary>
        /// Determines the largest value in a sequence of numbers, and then returns that value. 
        /// max() accepts either two or three float or int values as parameters, or an array 
        /// of any length.
        /// </summary>
        /// <param name="list">int[]: array of numbers to compare</param>
        /// <returns>int</returns>
        public int max(int[] list) => list.Max();

        /// <summary>
        /// Determines the smallest value in a sequence of numbers, and then returns that value. 
        /// min() accepts either two or three float or int values as parameters, or an array 
        /// of any length.
        /// </summary>
        /// <param name="a">float: first number to compare</param>
        /// <param name="b">float: second number to compare</param>
        /// <param name="c">float: third number to compare</param>
        /// <returns>float</returns>
        public float min(float a, float b, float c = float.PositiveInfinity)
        {
            if (a < b && a < c) return a;
            if (b < c && b < a) return b;
            return c;
        }

        /// <summary>
        /// Determines the smallest value in a sequence of numbers, and then returns that value. 
        /// min() accepts either two or three float or int values as parameters, or an array 
        /// of any length.
        /// </summary>
        /// <param name="a">int: first number to compare</param>
        /// <param name="b">int: second number to compare</param>
        /// <param name="c">int: third number to compare</param>
        /// <returns>int</returns>
        public int min(int a, int b, int c = int.MaxValue)
        {
            if (a < b && a < c) return a;
            if (b < c && b < a) return b;
            return c;
        }

        /// <summary>
        /// Determines the smallest value in a sequence of numbers, and then returns that value. 
        /// min() accepts either two or three float or int values as parameters, or an array 
        /// of any length.
        /// </summary>
        /// <param name="list">float[]: array of numbers to compare</param>
        /// <returns>float</returns>
        public float min(float[] list) => list.Min();

        /// <summary>
        /// Determines the smallest value in a sequence of numbers, and then returns that value. 
        /// min() accepts either two or three float or int values as parameters, or an array 
        /// of any length.
        /// </summary>
        /// <param name="list">int[]: array of numbers to compare</param>
        /// <returns>int</returns>
        public int min(int[] list) => list.Min();

        /// <summary>
        /// Normalizes a number from another range into a value between 0 and 1. Identical to 
        /// map(value, low, high, 0, 1). Numbers outside of the range are not clamped to 0 and 1, 
        /// because out-of-range values are often intentional and useful.
        /// </summary>
        /// <param name="value">float: the incoming value to be converted</param>
        /// <param name="start">float: lower bound of the value's current range</param>
        /// <param name="stop">	float: upper bound of the value's current range</param>
        /// <returns>float</returns>
        public float norm(float value, float start, float stop) => map(value, start, stop, 0, 1);

        /// <summary>
        /// Facilitates exponential expressions. The pow() function is an efficient way of 
        /// multiplying numbers by themselves (or their reciprocals) in large quantities. 
        /// For example, pow(3, 5) is equivalent to the expression 3*3*3*3*3 and pow(3, -5) 
        /// is equivalent to 1 / 3*3*3*3*3.
        /// </summary>
        /// <param name="n">float: base of the exponential expression</param>
        /// <param name="e">float: power by which to raise the base</param>
        /// <returns></returns>
        public float pow(float n, float e) => (float)Pow(n, e);

        /// <summary>
        /// Calculates the integer closest to the n parameter. 
        /// For example, round(133.8) returns the value 134.
        /// </summary>
        /// <param name="n">float: number to round</param>
        /// <returns>int</returns>
        public int round(float n) => (int)Round(n);

        /// <summary>
        /// Squares a number (multiplies a number by itself). 
        /// The result is always a positive number, as multiplying 
        /// two negative numbers always yields a positive result. 
        /// For example, -1 * -1 = 1.
        /// </summary>
        /// <param name="n">float: number to square</param>
        /// <returns>float</returns>
        public float sq(float n) => n * n;

        /// <summary>
        /// Calculates the square root of a number. The square root of a 
        /// number is always positive, even though there may be a valid 
        /// negative root. The square root s of number a is such that 
        /// s*s = a. It is the opposite of squaring.
        /// </summary>
        /// <param name="n">float: non-negative number</param>
        /// <returns>float</returns>
        public float sqrt(float n) => (float)Sqrt(n);

        #endregion
    }
}
