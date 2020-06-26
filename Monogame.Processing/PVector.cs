using System;
using Microsoft.Xna.Framework;

namespace Monogame.Processing
{
    public class PVector
    {
        private Vector3 _internal = new Vector3();

        public float x
        {
            get => _internal.X;
            set => _internal.X = value;
        }

        public float y
        {
            get => _internal.Y;
            set => _internal.Y = value;
        }
        public float z
        {
            get => _internal.Z;
            set => _internal.Z = value;
        }

        private PVector(Vector3 v) => _internal = v;

        public PVector() => _internal = Vector3.Zero;

        public PVector(float x, float y, float z = 0.0f)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public void set(PVector v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }

        public void set(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public void set(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public void set(float[] source)
        {
            if (source.Length > 0) x = source[0];
            if (source.Length > 1) y = source[1];
            if (source.Length > 2) z = source[2];
        }

        public PVector add(PVector v) => add(v.x, v.y, v.z);

        public PVector add(float x, float y, float z = 0f)
        {
            _internal = new Vector3(x + this.x, y + this.y, z + this.z);
            return this;
        }

        public static PVector add(PVector v1, PVector v2) => new PVector(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        public static void add(PVector v1, PVector v2, out PVector target) => target = new PVector(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);

        public PVector sub(PVector v) => sub(v.x, v.y, v.z);

        public PVector sub(float x, float y, float z = 0f)
        {
            _internal = new Vector3(-x + this.x, -y + this.y, -z + this.z);
            return this;
        }

        public static PVector sub(PVector v1, PVector v2) => new PVector(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        public static void sub(PVector v1, PVector v2, out PVector target) => target = new PVector(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);

        public PVector mult(float n)
        {
            _internal *= n;
            return this;
        }
        public static PVector mult(PVector v, float n) => new PVector(v._internal * n);
        public static void mult(PVector v, float n, out PVector target) => target = new PVector(v._internal * n);

        public PVector div(float n)
        {
            _internal /= n;
            return this;
        }

        public static PVector div(PVector v, float n) => new PVector(v._internal / n);
        public static void div(PVector v, float n, out PVector target) => target = new PVector(v._internal / n);

        public float dist(PVector v) => Vector3.Distance(this, v);
        public static float dist(PVector v1, PVector v2) => Vector3.Distance(v1, v2);

        public float dot(PVector v) => Vector3.Dot(this, v);
        public float dot(float x, float y, float z) => dot(new PVector(x, y, z));
        public static float dot(PVector v1, PVector v2) => Vector3.Dot(v1, v2);

        public PVector cross(PVector v) => Vector3.Cross(this, v);

        public PVector cross(PVector v, out PVector target)
        {
            target = new PVector(Vector3.Cross(this, v));
            return this;
        }

        public static void cross(PVector v1, PVector v2, out PVector target) =>
            target = new PVector(Vector3.Cross(v1, v2));

        public PVector normalize()
        {
            _internal.Normalize();
            return this;
        }

        public void normalize(out PVector target) => target = Vector3.Normalize(_internal);

        public PVector limit(float max)
        {
            if (!(_internal.Length() > max)) return this;
            _internal.Normalize();
            _internal *= max;
            return this;
        }

        public PVector setMag(float len)
        {
            _internal.Normalize();
            _internal *= len;
            return this;
        }

        public void setMag(float len, out PVector target)
        {
            var aux = Vector3.Normalize(_internal);
            target = new PVector(aux * len);
        }

        public float heading() => (float) Math.Acos(x / (x * x + y * y));

        public PVector rotate(float theta)
        {
            _internal = Vector3.Transform(_internal, Matrix.CreateRotationX(theta));
            return this;
        }

        public PVector lerp(PVector v, float amt)
        {
            _internal = Vector3.Lerp(_internal, v, amt);
            return this;
        }

        public PVector lerp(float x, float y, float z, float amt) => lerp(new PVector(x, y, z), amt);

        public static PVector lerp(PVector v1, PVector v2, float amt) => Vector3.Lerp(v1._internal, v2._internal, amt);

        public static float angleBetween(PVector v1, PVector v2) => (float) Math.Acos(Vector3.Dot(v1, v2));

        public float[] array() => new[] {x, y, z};

        public float magSq() => x * x + y * y + z * z;

        public float mag() => _internal.Length();

        public PVector copy() => new PVector(x, y, z);

        public static PVector fromAngle(float angle) => 
            Vector3.Transform(Vector3.UnitX, Matrix.CreateRotationX(angle));

        public static void fromAngle(float angle, out PVector target) =>
            target = Vector3.Transform(Vector3.UnitX, Matrix.CreateRotationX(angle));

        public override string ToString() => $"[{x}, {y}, {z}]";
        public static implicit operator Vector3(PVector v) => v._internal;
        public static implicit operator PVector(Vector3 v) => new PVector(v.X, v.Y, v.Z);

        public static implicit operator (float x, float y, float z)(PVector v) => (v.x, v.y, v.z);
        public static implicit operator PVector((float x, float y, float z) v) => new PVector(v.x, v.y, v.z);

        public void Deconstruct(out float x, out float y, out float z)
        {
            x = this.x;
            y = this.y;
            z = this.z;
        }
    }
}
