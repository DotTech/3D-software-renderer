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
        public bool IsBackFaced { get; set; }

        public int? ShadedFillColor { get; private set; }

        public Triangle(Vertex a, Vertex b, Vertex c)
        {
            Vertices = new Vertex[3] { a, b, c };
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

        /// <summary>
        /// Darkens or lightens the fillcolor based on the intesify values of the vertices
        /// </summary>
        /// <param name="color"></param>
        public void CalculateFillColor(int color)
        {
            ShadedFillColor = color;
            double avgintensity = Vertices.Select(v => v.Intensity).Average();

            if (avgintensity < 1)
            {
                // TODO: Use bitshifting for this
                Color c = Color.FromArgb(color);
                ShadedFillColor = Color.FromArgb((int)((double)c.R * avgintensity), (int)((double)c.G * avgintensity), (int)((double)c.B * avgintensity)).ToArgb();
            }
        }
    }
}
