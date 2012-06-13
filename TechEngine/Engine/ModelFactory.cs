using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TechEngine.Engine
{
    public static class ModelFactory
    {
        /// <summary>
        /// Create model from PLY ascii file
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public static Model CreateFromFile(string filename, double scalar = 1)
        {
            var model = new Model();

            string filepath = Path.Combine(Application.StartupPath, "Resources", filename);
            var lines = File.ReadLines(filepath);

            int readmode = 0;       // 0=header, 1=vertices, 2=triangles, 3=done
            int vertexcount = 0;
            int trianglecount = 0;

            int readvertices = 0;
            int readtriangles = 0;

            int bcolor = Color.Red.ToArgb();
            int? fcolor = Color.Gray.ToArgb();

            foreach (string line in lines)
            {
                if (readmode == 0)
                {
                    if (line.StartsWith("element vertex"))
                    {
                        string rev = ReverseString(line.Trim());
                        string value = ReverseString(rev.Substring(0, rev.IndexOf(" ")));
                        vertexcount = Convert.ToInt32(value);
                    }

                    if (line.StartsWith("element face"))
                    {
                        string rev = ReverseString(line.Trim());
                        string value = ReverseString(rev.Substring(0, rev.IndexOf(" ")));
                        trianglecount = Convert.ToInt32(value);
                    }

                    if (line.StartsWith("end_header"))
                    {
                        readmode = 1;
                        continue;
                    }
                }

                if (readmode == 1)
                {
                    double[] values = line.Trim().Split(' ').Select(x => Math.Round(Convert.ToDouble(x) * scalar)).ToArray();
                    model.AddVertex(values[0], values[1], values[2], (values.Length >= 5) ? (values[4] / scalar) * 2 : 1);

                    if (++readvertices >= vertexcount)
                    {
                        readmode = 2;
                        continue;
                    }
                }

                if (readmode == 2)
                {
                    int[] values = line.Trim().Split(' ').Select(x => Convert.ToInt32(x)).ToArray();
                    model.AddTriangle(values[1], values[2], values[3]);

                    if (++readtriangles >= trianglecount)
                    {
                        readmode = 3;
                        continue;
                    }
                }
            }

            model.CenterModel();

            return model;
        }

        public static Model CreateCube()
        {
            var model = new Model();
            model.Rotation = new Vector3(0, 0, 0); //new Vector3(81, 358, 351);

            model.AddVertex(-100, 100, -100);   // 0
            model.AddVertex(100, 100, -100);    // 1
            model.AddVertex(-100, -100, -100);  // 2
            model.AddVertex(-100, 100, 100);    // 3
            model.AddVertex(100, 100, 100);     // 4
            model.AddVertex(-100, -100, 100);   // 5            
            model.AddVertex(100, -100, -100);   // 6
            model.AddVertex(100, -100, 100);    // 7

            int? fillColor1 = Color.FromArgb(140, 140, 140).ToArgb();
            int? fillColor2 = Color.FromArgb(120, 120, 120).ToArgb();
            int? fillColor3 = Color.FromArgb(100, 100, 100).ToArgb();
            int borderColor = Color.Red.ToArgb();

            model.Triangles.Add(new Triangle(
                model.Vertices[0],
                model.Vertices[1],
                model.Vertices[2],
                borderColor, fillColor1));

            model.Triangles.Add(new Triangle(
                model.Vertices[1],
                model.Vertices[6],
                model.Vertices[2],
                borderColor, fillColor1));

            model.Triangles.Add(new Triangle(
                model.Vertices[1],
                model.Vertices[4],
                model.Vertices[6],
                borderColor, fillColor2));

            model.Triangles.Add(new Triangle(
                model.Vertices[4],
                model.Vertices[7],
                model.Vertices[6],
                borderColor, fillColor2));

            model.Triangles.Add(new Triangle(
                model.Vertices[4],
                model.Vertices[3],
                model.Vertices[7],
                borderColor, fillColor1));

            model.Triangles.Add(new Triangle(
                model.Vertices[3],
                model.Vertices[5],
                model.Vertices[7],
                borderColor, fillColor1));

            model.Triangles.Add(new Triangle(
                model.Vertices[3],
                model.Vertices[0],
                model.Vertices[5],
                borderColor, fillColor2));

            model.Triangles.Add(new Triangle(
                model.Vertices[0],
                model.Vertices[2],
                model.Vertices[5],
                borderColor, fillColor2));

            model.Triangles.Add(new Triangle(
                model.Vertices[3],
                model.Vertices[4],
                model.Vertices[0],
                borderColor, fillColor3));

            model.Triangles.Add(new Triangle(
                model.Vertices[4],
                model.Vertices[1],
                model.Vertices[0],
                borderColor, fillColor3));

            model.Triangles.Add(new Triangle(
                model.Vertices[2],
                model.Vertices[6],
                model.Vertices[5],
                borderColor, fillColor3));

            model.Triangles.Add(new Triangle(
                model.Vertices[6],
                model.Vertices[7],
                model.Vertices[5],
                borderColor, fillColor3));

            model.CenterModel();

            return model;
        }

        public static Model CreateTestObject()
        {
            var model = new Model();
            model.Rotation = new Vector3(0, 0, 0); //new Vector3(81, 358, 351);

            model.AddVertex(-100, 100, -100);   // 0
            model.AddVertex(100, 100, -100);    // 1
            model.AddVertex(-100, -100, -100);  // 2
            model.AddVertex(-100, 100, 100);    // 3
            model.AddVertex(100, 100, 100);     // 4
            model.AddVertex(-100, -100, 100);   // 5            
            model.AddVertex(100, -100, -100);   // 6
            model.AddVertex(100, -100, 100);    // 7
            model.AddVertex(0, 150, 0);         // 8
            model.AddVertex(0, -50, 0);         // 9

            model.AddTriangle(0, 1, 2);
            model.AddTriangle(1, 6, 2);
            model.AddTriangle(1, 4, 6);
            model.AddTriangle(4, 7, 6);
            model.AddTriangle(4, 3, 7);
            model.AddTriangle(3, 5, 7);
            model.AddTriangle(3, 0, 5);
            model.AddTriangle(0, 2, 5);

            model.AddTriangle(0, 8, 1);
            model.AddTriangle(1, 8, 4);
            model.AddTriangle(4, 8, 3);
            model.AddTriangle(3, 8, 0);
            
            model.AddTriangle(6, 9, 2);
            model.AddTriangle(2, 9, 5);
            model.AddTriangle(5, 9, 7);
            model.AddTriangle(7, 9, 6);

            model.CenterModel();

            return model;
        }

        private static string ReverseString(string s)
        {
            return new string(s.ToCharArray().Reverse().ToArray());
        }
    }
}
