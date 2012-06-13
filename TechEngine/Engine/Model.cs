﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;

namespace TechEngine.Engine
{
    public class Model
    {
        /// <summary>
        /// Position in world space
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Rotation around model axis
        /// </summary>
        public Vector3 Rotation { get; set; }

        /// <summary>
        /// Rotation point (automatically centered in object space)
        /// </summary>
        public Vector3 Pivot { get; set; }

        public List<Vertex> Vertices { get; private set; }
        public List<Triangle> Triangles { get; private set; }

        public int? LineColor { get; set; }
        public int? FillColor { get; set; }
        
        public Model()
        {
            Vertices = new List<Vertex>();
            Triangles = new List<Triangle>();

            Position = new Vector3(0, 0, 0);
            Rotation = new Vector3(0, 0, 0);
            Pivot = null;
        }

        public void AddVertex(double x, double y, double z, double intensity = 1)
        {
            Vertices.Add(new Vertex(x, y, z, intensity));
        }

        public void AddTriangle(int v0, int v1, int v2)
        {
            Triangles.Add(new Triangle(Vertices[v0], Vertices[v1], Vertices[v2]));
        }

        /// <summary>
        /// Center the pivot in object space
        /// </summary>
        private void CenterPivot()
        {
            Pivot = new Vector3(0, 0, 0);

            // Find minimum and maximum coord for each axis
            /*Vector3 min = new Vector3(
                Vertices.Min(v => v.X),
                Vertices.Min(v => v.Y),
                Vertices.Min(v => v.Z)
            );

            Vector3 max = new Vector3(
                Vertices.Max(v => v.X),
                Vertices.Max(v => v.Y),
                Vertices.Max(v => v.Z)
            );

            Vector3 offset = (max - min) / 2;
            Pivot = min + offset;*/
        }

        /// <summary>
        /// Adjust vertex coordinates so that point 0,0,0 is the center of the model
        /// </summary>
        public void CenterModel()
        {
            Vector3 min = new Vector3(
                Vertices.Min(v => v.X),
                Vertices.Min(v => v.Y),
                Vertices.Min(v => v.Z)
            );

            Vector3 max = new Vector3(
                Vertices.Max(v => v.X),
                Vertices.Max(v => v.Y),
                Vertices.Max(v => v.Z)
            );

            Vector3 offset = (max - min) / 2;

            Vertices.ForEach(delegate(Vertex v)
            {
                v.X += 0 - min.X - offset.X;
                v.Y += 0 - min.Y - offset.Y;
                v.Z += 0 - min.Z - offset.Z;
            });
        }

        /// <summary>
        /// Perform rotation
        /// </summary>
        private void Rotate()
        {
            if (Pivot == null)
            {
                CenterPivot();
            }

            foreach (Vertex v in Vertices)
            {
                double rx = Rotation.X.ToRadians();
                double ry = Rotation.Y.ToRadians();
                double rz = Rotation.Z.ToRadians();

                double px = v.X - Pivot.X;
                double py = v.Y - Pivot.Y;
                double pz = v.Z - Pivot.Z;

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

        private void Rotate(Vector3 angles)
        {
            Rotation = angles;
            Rotate();
        }

        private void TransformToCameraSpace(Vector3 camera)
        {
            foreach (Vertex vertex in Vertices)
            {
                vertex.Transformed -= camera;
            }
        }

        private void BackfaceCulling()
        {
            foreach (Triangle triangle in Triangles)
            {
                Vector3 n = triangle.SurfaceNormal().Normalize();
                triangle.IsBackFaced = Math.Round(n.Z, 1) >= 0;
            }

            //Logger.Value("culled triangles", Triangles.Count(x => x.IsBackFaced));
            Logger.Value("valid triangles", Triangles.Count(x => !x.IsBackFaced));
        }

        /// <summary>
        /// Sort the triangles descending by the sum of all vertices Z value
        /// </summary>
        private void SortDepthBuffer()
        {
            Triangles.Sort(delegate(Triangle a, Triangle b)
            {
                double zSumA = a.Vertices.Max(t => t.Transformed.Z);
                double zSumB = b.Vertices.Max(t => t.Transformed.Z);

                return zSumB.CompareTo(zSumA);
            });
        }

        /// <summary>
        /// Call once after loading the model
        /// </summary>
        public void CalculateColors()
        {
            // Calculate color value using vertex intensity value
            if (FillColor != null)
            {
                Triangles
                    .Where(x => !x.IsBackFaced)
                    .ToList()
                    .ForEach(x => x.CalculateFillColor(FillColor.Value));
            }
        }

        private void ProjectVertices(Vector3 camera, double scale)
        {
            foreach (Vertex vertex in Vertices)
            {
                // Perspective projection
                double pz = camera.Z + vertex.Transformed.Z;
                vertex.Projected.X = (int)(((camera.Z * (vertex.Transformed.X - camera.X)) / pz * scale) + camera.X);
                vertex.Projected.Y = -(int)(((camera.Z * (vertex.Transformed.Y - camera.Y)) / pz * scale) + camera.Y);

                // Scaling and positioning
                vertex.Projected.X += (int)(640 / 2);
                vertex.Projected.Y += (int)(480 / 2);
            }
        }

        /// <summary>
        /// Performs all transformations required for one rendering cycle
        /// </summary>
        /// <param name="camera"></param>
        public void Transform(Vector3 camera, double scale)
        {
            Rotate();

            // Translate model coordinates from world space to camera space
            // The camera will then be fixed at 0,0,0 in world space
            //TransformToCameraSpace(camera);

            BackfaceCulling();

            SortDepthBuffer();

            ProjectVertices(camera, scale);
        }
    }
}
