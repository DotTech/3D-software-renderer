using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechEngine.Engine
{
    public class Model
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        
        public List<Vertex> Vertices { get; private set; }
        public List<Triangle> Triangles { get; private set; }
        
        public Model()
        {
            Vertices = new List<Vertex>();
            Triangles = new List<Triangle>();

            Position = new Vector3(0, 0, 0);
            Rotation = new Vector3(0, 0, 0);
        }
        
        public void Rotate()
        {
            Vector3 pivot = new Vector3(0, 0, 0);
            
            /*
            var zx = parseInt(px * Math.cos(TD.Global.rotation.z.radians) - py * Math.sin(TD.Global.rotation.z.radians) - px),
				zy = parseInt(px * Math.sin(TD.Global.rotation.z.radians) + py * Math.cos(TD.Global.rotation.z.radians) - py),
				yx = parseInt((px + zx) * Math.cos(TD.Global.rotation.y.radians) - pz * Math.sin(TD.Global.rotation.y.radians) - (px + zx)),
				yz = parseInt((px + zx) * Math.sin(TD.Global.rotation.y.radians) + pz * Math.cos(TD.Global.rotation.y.radians) - pz),
				xy = parseInt((py + zy) * Math.cos(TD.Global.rotation.x.radians) - (pz + yz) * Math.sin(TD.Global.rotation.x.radians) - (py + zy)),
				xz = parseInt((py + zy) * Math.sin(TD.Global.rotation.x.radians) + (pz + yz) * Math.cos(TD.Global.rotation.x.radians) - (pz + yz)),
				rotationOffset = {
					x: yx + zx,
					y: zy + xy,
					z: xz + yz
				};
            */

            foreach (Vertex v in Vertices)
            {
                double rx = Rotation.X.ToRadians();
                double ry = Rotation.Y.ToRadians();
                double rz = Rotation.Z.ToRadians();

                double px = v.X - pivot.X;
                double py = v.Y - pivot.Y;
                double pz = v.Z - pivot.Z;

                double zx = px * Math.Cos(rz) - py * Math.Sin(rz) - px;
                double zy = px * Math.Sin(rz) + py * Math.Cos(rz) - py;
                double yx = (px + zx) * Math.Cos(ry) - pz * Math.Sin(ry) - (px + zx);
                double yz = (px + zx) * Math.Sin(ry) + pz * Math.Cos(ry) - pz;
                double xy = (py + zy) * Math.Cos(rx) - (pz + yz) * Math.Sin(rx) - (py + zy);
                double xz = (py + zy) * Math.Sin(rx) + (pz + yz) * Math.Cos(rx) - (pz + yz);

                double offX = yx + zx;
                double offY = zy + xy;
                double offZ = xz + yz;

                v.Transformed.X = v.X + offX;
                v.Transformed.Y = v.Y + offY;
                v.Transformed.Z = v.Z + offZ;
            }
        }

        public void Rotate(Vector3 angles)
        {
            Rotation = angles;
            Rotate();
        }

        /// <summary>
        /// Flag triangles that are facing backwards and dont need to be rendered
        /// </summary>
        public void BackfaceCulling()
        {
            foreach (Triangle t in Triangles)
            {
                Vector3 v1 = Vertices[t.Vertices[0]].Projected;
                Vector3 v2 = Vertices[t.Vertices[1]].Projected;
                Vector3 v3 = Vertices[t.Vertices[2]].Projected;

                // Edge vectors
                Vector3 e1 = v2 - v1;
                Vector3 e2 = v3 - v1;

                // Calculate the triangle normal
                Vector3 normal = e1.CrossProduct(e2) / e1.CrossProduct(e2).Magnitude();

                // If it's negative, the triangle is backfacing and can be ignored
                t.BackFacing = normal.Z < 0;
            }
        }
    }
}
