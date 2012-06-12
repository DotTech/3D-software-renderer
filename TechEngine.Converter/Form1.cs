using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

namespace TechEngine.Converter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            Application.CurrentCulture = new CultureInfo("en-US");
            InitializeComponent();
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            var output = new StringBuilder();
            var lines = File.ReadLines(txtSourceFile.Text);

            int readmode = 0;       // 0=header, 1=vertices, 2=triangles, 3=done
            double scalar = 1000;
            int vertexcount = 0;
            int trianglecount = 0;

            int readvertices = 0;
            int readtriangles = 0;

            foreach (string line in lines)
            {
                if (readmode == 0)
                {
                    if (line.StartsWith("element vertex"))
                    {
                        string rev = ReverseString(line);
                        string value = ReverseString(rev.Substring(0, rev.IndexOf(" ")));
                        vertexcount = Convert.ToInt32(value);
                    }

                    if (line.StartsWith("element face"))
                    {
                        string rev = ReverseString(line);
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
                    double[] values = line.Trim().Split(' ').Select(x => Convert.ToDouble(x) * scalar).ToArray();
                    output.AppendFormat("model.Vertices.Add(new Vertex({0}, {1}, {2}, {3}));\r\n", values[0], values[1], values[2], values[4] / scalar);

                    if (++readvertices >= vertexcount)
                    {
                        readmode = 2;
                        continue;
                    }
                }

                if (readmode == 2)
                {
                    string[] values = line.Split(' ');
                    output.AppendFormat("model.Triangles.Add(new Triangle(model.Vertices[{0}], model.Vertices[{1}], model.Vertices[{2}], bcolor, fcolor));\r\n", values[1], values[2], values[3]);

                    if (++readtriangles >= trianglecount)
                    {
                        readmode = 3;
                        continue;
                    }
                }
            }

            txtOutput.Text = output.ToString();
        }

        private string ReverseString(string s)
        {
            return new string(s.ToCharArray().Reverse().ToArray());
        }
    }
}
