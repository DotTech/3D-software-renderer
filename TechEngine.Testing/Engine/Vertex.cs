using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechEngine.Engine
{
    public class Vertex : Vector3
    {
        public Vector3 Projected { get; set; }
        public Vector3 Transformed { get; set; }

        public Vertex()
            : base()
        {
            Projected = new Vector3(0, 0, 0);
            Transformed = new Vector3(0, 0, 0);
        }

        public Vertex(double x, double y, double z)
            : base(x, y, z)
        {
            Projected = new Vector3(0, 0, 0);
            Transformed = new Vector3(x, y, z);
        }
    }
}
