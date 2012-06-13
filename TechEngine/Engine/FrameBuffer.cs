using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TechEngine.Engine
{
    public class FrameBuffer
    {
        public int[] Buffer { get; private set; }
        public Size Size { get; set; }

        public FrameBuffer(Size size)
        {
            Size = size;
            Clear();
        }

        public void Clear()
        {
            Buffer = new int[Size.Width * Size.Height];
        }

        /// <summary>
        /// Draw a line from point x0,y0 to x1,y1 in the framebuffer
        /// </summary>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="color">argb value for color</param>
        public void DrawLine(int x0, int y0, int x1, int y1, int color)
        {
            if (x0 == x1 && y0 == y1)
            {
                DrawPixel(x0, y0, color);
                return;
            }

            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);

            int sx = (x0 < x1) ? 1 : -1;
            int sy = (y0 < y1) ? 1 : -1;

            int err = dx - dy;

            while (!(x0 == x1 && y0 == y1))
            {
                DrawPixel(x0, y0, color);

                int err2 = 2 * err;

                if (err2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }

                if (err2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }

            DrawPixel(x0, y0, color);
        }

        /// <summary>
        /// Draw a line from point0 to point1 in the framebuffer
        /// </summary>
        /// <remarks>http://en.wikipedia.org/wiki/Bresenham's_line_algorithm</remarks>
        /// <param name="point0"></param>
        /// <param name="point1"></param>
        /// <param name="color"></param>
        public void DrawLine(Vector3 point0, Vector3 point1, int color)
        {
            DrawLine((int)point0.X, (int)point0.Y, (int)point1.X, (int)point1.Y, color);
        }

        /// <summary>
        /// Draws a pixel to the framebuffer
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color">argb value for color</param>
        public void DrawPixel(int x, int y, int color)
        {
            if (x < 0 || y < 0 || x > Size.Width - 1 || y > Size.Height - 1)
            {
                return;
            }

            int index = y * Size.Width + x;

            if (index < Buffer.Length)
            {
                Buffer[index] = color;
            }
        }

        /// <summary>
        /// Draws a pixel to the framebuffer
        /// </summary>
        /// <param name="point"></param>
        /// <param name="color"></param>
        public void DrawPixel(Vector3 point, int color)
        {
            DrawPixel((int)point.X, (int)point.Y, color);
        }

        /// <summary>
        /// Draw a mesh model after it has been processed (transformed and projected)
        /// </summary>
        /// <param name="model"></param>
        public void DrawModel(Model model)
        {
            //foreach (Vertex v in model.Vertices)
            //{
            //    DrawPixel(v.Projected, Color.Red.ToArgb());
            //}
            
            int c = 0;

            foreach (Triangle t in model.Triangles.Where(x => !x.IsBackFaced))
            {
                DrawTriangle(model, t);
                c++;
            }

            Logger.Value("triangles drawn", c);
        }

        /// <summary>
        /// Draw a (filled) triangle to the framebuffer
        /// </summary>
        /// <remarks>http://www-users.mat.uni.torun.pl/~wrona/3d_tutor/tri_fillers.html</remarks>
        /// <param name="vertices"></param>
        /// <param name="color"></param>
        /// <param name="fill"></param>
        public void DrawTriangle(Model model, Triangle triangle)
        {
            Vertex[] sortedv = new Vertex[3];
            triangle.Vertices.CopyTo(sortedv, 0);

            Array.Sort(sortedv, delegate(Vertex v1, Vertex v2)
            {
                return v1.Projected.Y.CompareTo(v2.Projected.Y);
            });

            Vector3 a = sortedv[0].Projected;
            Vector3 b = sortedv[1].Projected;
            Vector3 c = sortedv[2].Projected;

            double dx1 = (b.Y - a.Y > 0) ? (double)(b.X - a.X) / (double)(b.Y - a.Y) : 0;
            double dx2 = (c.Y - a.Y > 0) ? (double)(c.X - a.X) / (double)(c.Y - a.Y) : 0;
            double dx3 = (c.Y - b.Y > 0) ? (double)(c.X - b.X) / (double)(c.Y - b.Y) : 0;

            double ex = (double)a.X;
            double sx = (double)a.X;
            double sy = (double)a.Y;

            for (; sy <= b.Y; sy++, sx += dx1, ex += dx2)
            {
                DrawTriangleScanline(sx, ex, sy, model.LineColor, triangle.ShadedFillColor);
            }

            sx = b.X;

            for (; sy <= c.Y; sy++, sx += dx3, ex += dx2)
            {
                DrawTriangleScanline(sx, ex, sy, model.LineColor, triangle.ShadedFillColor);
            }
        }

        /// <summary>
        /// Draw one horizontal scanline of a triangle
        /// </summary>
        /// <param name="x0"></param>
        /// <param name="x1"></param>
        /// <param name="y"></param>
        /// <param name="bordercolor"></param>
        /// <param name="fillcolor"></param>
        private void DrawTriangleScanline(double x0, double x1, double y, int? linecolor, int? fillcolor)
        {
            int sx = (int)x0;
            int ex = (int)x1;
            int sy = (int)y;

            if (fillcolor.HasValue)
            {
                DrawLine(sx, sy, ex, sy, fillcolor.Value);
            }

            if (linecolor.HasValue)
            {
                DrawPixel(sx, sy, linecolor.Value);
                DrawPixel(ex, sy, linecolor.Value);
            }
        }
    }
}
