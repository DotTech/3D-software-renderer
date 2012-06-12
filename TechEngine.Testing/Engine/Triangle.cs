using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TechEngine.Engine
{
    public class Triangle
    {
        public Vertex[] Vertices { get; set; }
        public int? FillColor { get; set; }
        public int BorderColor { get; set; }
        public bool IsBackFaced { get; set; }

        public Triangle(Vertex a, Vertex b, Vertex c)
        {
            Vertices = new Vertex[3] { a, b, c };
        }

        public Triangle(Vertex a, Vertex b, Vertex c, int borderColor, int? fillColor = null)
            : this(a, b, c)
        {
            FillColor = fillColor;
            BorderColor = borderColor;
        }

        /// <summary>
        /// Calculate the normal for this triangle
        /// </summary>
        /// <returns></returns>
        public Vector3 SurfaceNormal()
        {
            Vector3 a = Vertices[0].Transformed;
            Vector3 b = Vertices[1].Transformed;
            Vector3 c = Vertices[2].Transformed;

            Vector3 e1 = b - a;
            Vector3 e2 = c - a;

            return e1.CrossProduct(e2);
        }
    }
}
