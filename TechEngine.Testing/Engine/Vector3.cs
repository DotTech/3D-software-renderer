using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechEngine.Engine
{
    public class Vector3
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vector3 operator /(Vector3 a, double b)
        {
            return new Vector3(a.X / b, a.Y / b, a.Z / b);
        }

        public Vector3()
        {
        }

        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return String.Format("x={0}, y={1}, z={2}", X, Y, Z);
        }

        /// <summary>
        /// Compute the dotproduct over this vector and vector b
        /// </summary>
        /// <param name="rvalue"></param>
        /// <returns></returns>
        public double DotProduct(Vector3 rvalue)
        {
            return X * rvalue.X + Y * rvalue.Y + Z * rvalue.Z;
        }

        /// <summary>
        /// Compute the crossproduct over this vector and vector b
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public Vector3 CrossProduct(Vector3 rvalue)
        {
            return new Vector3(
                Y * rvalue.Z - Z * rvalue.Y,
                Z * rvalue.X - X * rvalue.Z,
                X * rvalue.Y - Y * rvalue.X
            );
        }

        public double Magnitude()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public Vector3 Normalize()
        {
            var m = Magnitude();
            return new Vector3(X / m, Y / m, Z / m);
        }
    }
}
