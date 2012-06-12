using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechEngine.Engine
{
    public class Vector2
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Vector2()
        {
        }

        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector2(Vector2 vector)
        {
            X = vector.X;
            Y = vector.Y;
        }
    }
}
