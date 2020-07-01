using Microsoft.Xna.Framework;
using static System.Math;

namespace Monogame.Processing
{
    partial class Processing
    {
        #region Transform

        /// <summary>
        /// Rotates the amount specified by the angle parameter. Angles must be specified 
        /// in radians (values from 0 to TWO_PI), or they can be converted from degrees 
        /// to radians with the radians() function. 
        /// </summary>
        /// <param name="angle">float: angle of rotation specified in radians</param>
        public void rotate(float angle) => _matrix *= Matrix.CreateRotationZ(angle);

        /// <summary>
        /// Increases or decreases the size of a shape by expanding and contracting vertices. 
        /// Objects always scale from their relative origin to the coordinate system. TransformMat 
        /// values are specified as decimal percentages. For example, the function call scale(2.0) 
        /// increases the dimension of a shape by 200%.
        /// </summary>
        /// <param name="s">float: percentage to scale the object</param>
        public void scale(float s) => _matrix *= Matrix.CreateScale(s);

        /// <summary>
        /// Increases or decreases the size of a shape by expanding and contracting vertices. 
        /// Objects always scale from their relative origin to the coordinate system. TransformMat 
        /// values are specified as decimal percentages. For example, the function call scale(2.0) 
        /// increases the dimension of a shape by 200%. 
        /// </summary>
        /// <param name="x">float: percentage to scale the object in the x-axis</param>
        /// <param name="y">float: percentage to scale the object in the y-axis</param>
        public void scale(float x, float y) => _matrix *= Matrix.CreateScale(x, y, 1);

        /// <summary>
        /// Specifies an amount to displace objects within the display window. The x parameter specifies
        /// left/right translation, the y parameter specifies up/down translation, and the z parameter
        /// specifies translations toward/away from the screen. Using this function with the z parameter
        /// requires using P3D as a parameter in combination with size as shown in the above example.
        ///
        /// Transformations are cumulative and apply to everything that happens after and subsequent calls
        /// to the function accumulates the effect.For example, calling translate(50, 0) and then translate(20, 0)
        /// is the same as translate(70, 0). If translate() is called within draw(), the transformation is reset when
        /// the loop begins again.This function can be further controlled by using pushMatrix() and popMatrix().
        /// </summary>
        /// <param name="x">float: left/right translation</param>
        /// <param name="y">float: up/down translation</param>
        public void translate(float x, float y) => _matrix *= Matrix.CreateTranslation(x, y, 0);

        /// <summary>
        /// Shears a shape around the y-axis the amount specified by the angle parameter. Angles should be specified in
        /// radians (values from 0 to PI*2) or converted to radians with the radians() function. Objects are always sheared
        /// around their relative position to the origin and positive numbers shear objects in a clockwise direction.
        /// Transformations apply to everything that happens after and subsequent calls to the function accumulates the effect.
        /// For example, calling shearY(PI/2) and then shearY(PI/2) is the same as shearY(PI). If shearY() is called within the
        /// draw(), the transformation is reset when the loop begins again.
        ///
        /// Technically, shearY() multiplies the current transformation matrix by a rotation matrix.This function can be further
        /// controlled by the pushMatrix() and popMatrix() functions.
        /// </summary>
        /// <param name="angle">float: angle of shear specified in radians</param>
        public void shearY(float angle)
        {
            var aux = Matrix.Identity;
            aux.M12 = (float)Tan(angle);
            _matrix *= aux;
        }

        /// <summary>
        /// Shears a shape around the x-axis the amount specified by the angle parameter. Angles should be specified in
        /// radians (values from 0 to PI*2) or converted to radians with the radians() function. Objects are always sheared
        /// around their relative position to the origin and positive numbers shear objects in a clockwise direction.
        /// Transformations apply to everything that happens after and subsequent calls to the function accumulates the effect.
        /// For example, calling shearX(PI/2) and then shearX(PI/2) is the same as shearX(PI). If shearX() is called within the
        /// draw(), the transformation is reset when the loop begins again.
        ///
        /// Technically, shearX() multiplies the current transformation matrix by a rotation matrix.This function can be further
        /// controlled by the pushMatrix() and popMatrix() functions.
        /// </summary>
        /// <param name="angle"></param>
        public void shearX(float angle)
        {
            var aux = Matrix.Identity;
            aux.M21 = (float)Tan(angle);
            _matrix *= aux;
        }

        #endregion
    }
}
