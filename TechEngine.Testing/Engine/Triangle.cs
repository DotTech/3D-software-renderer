using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TechEngine.Engine
{
    public class Triangle
    {
        public int[] Vertices { get; set; }
        public bool BackFacing { get; set; }
        public int FillColor { get; set; }

        public Triangle(int i0, int i1, int i2)
        {
            Vertices = new int[3] { i0, i1, i2 };
        }

        public Triangle(int i0, int i1, int i2, int fillColor)
            : this(i0, i1, i2)
        {
            FillColor = fillColor;
        }
    }
}
