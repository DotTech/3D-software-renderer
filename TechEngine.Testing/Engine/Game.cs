using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace TechEngine.Engine
{
    public class Game : Control
    {
        private Renderer renderer;
        private Model model;
        private FrameBuffer frame;
        private Timer timerUpdate;

        private Vector3 camera = new Vector3(0, 0, 800);
        private double scale = 1;

        public void Start()
        {
            Setup();
            CreateModel();

            timerUpdate.Enabled = true;
        }

        private void Setup()
        {
            frame = new FrameBuffer(Size);

            renderer = new Renderer();
            renderer.Size = this.Size;
            renderer.Init();

            Controls.Add(renderer);

            timerUpdate = new Timer();
            timerUpdate.Tick += new EventHandler(Update);
            timerUpdate.Interval = 5;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            InputHandler.OnKeyDown(this, e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            InputHandler.OnKeyUp(this, e);
        }

        private void CreateModel()
        {
            model = new Model();
            model.Rotation = new Vector3(130, 260, 30);

            model.Vertices.Add(new Vertex(-100, 100, -100));   // 0
            model.Vertices.Add(new Vertex(100, 100, -100));    // 1
            model.Vertices.Add(new Vertex(-100, -100, -100));  // 2
            model.Vertices.Add(new Vertex(-100, 100, 100));    // 3
            model.Vertices.Add(new Vertex(100, 100, 100));     // 4
            model.Vertices.Add(new Vertex(-100, -100, 100));   // 5            
            model.Vertices.Add(new Vertex(100, -100, -100));   // 6
            model.Vertices.Add(new Vertex(100, -100, 100));    // 7

            model.Triangles.Add(new Triangle(0, 1, 2, Color.FromArgb(140, 140, 140).ToArgb()));
            model.Triangles.Add(new Triangle(1, 6, 2, Color.FromArgb(140, 140, 140).ToArgb()));

            model.Triangles.Add(new Triangle(1, 4, 6, Color.FromArgb(120, 120, 120).ToArgb()));
            model.Triangles.Add(new Triangle(4, 7, 6, Color.FromArgb(120, 120, 120).ToArgb()));

            model.Triangles.Add(new Triangle(4, 3, 7, Color.FromArgb(140, 140, 140).ToArgb()));
            model.Triangles.Add(new Triangle(3, 5, 7, Color.FromArgb(140, 140, 140).ToArgb()));

            model.Triangles.Add(new Triangle(3, 0, 5, Color.FromArgb(120, 120, 120).ToArgb()));
            model.Triangles.Add(new Triangle(0, 2, 5, Color.FromArgb(120, 120, 120).ToArgb()));

            model.Triangles.Add(new Triangle(3, 4, 0, Color.FromArgb(100, 100, 100).ToArgb()));
            model.Triangles.Add(new Triangle(4, 1, 0, Color.FromArgb(100, 100, 100).ToArgb()));

            model.Triangles.Add(new Triangle(2, 6, 5, Color.FromArgb(100, 100, 100).ToArgb()));
            model.Triangles.Add(new Triangle(6, 7, 5, Color.FromArgb(100, 100, 100).ToArgb()));
        }

        private void Update(object sender, EventArgs e)
        {
            HandleKeys();
            
            Projection();
            model.BackfaceCulling();

            UpdateLog();

            Render();

            //timerUpdate.Enabled = false;
        }

        private void HandleKeys()
        {
            if (InputHandler.KeyD)
            {
                model.Rotation.Y = model.Rotation.Y < 359 ? model.Rotation.Y + 1 : 0;
            }

            if (InputHandler.KeyA)
            {
                model.Rotation.Y = model.Rotation.Y > 1 ? model.Rotation.Y - 1 : 359;
            }

            if (InputHandler.KeyW)
            {
                model.Rotation.X = model.Rotation.X < 359 ? model.Rotation.X + 1 : 0;
            }

            if (InputHandler.KeyS)
            {
                model.Rotation.X = model.Rotation.X > 1 ? model.Rotation.X - 1 : 359;
            }

            if (InputHandler.KeyQ)
            {
                model.Rotation.Z = model.Rotation.Z < 359 ? model.Rotation.Z + 1 : 0;
            }

            if (InputHandler.KeyE)
            {
                model.Rotation.Z = model.Rotation.Z > 1 ? model.Rotation.Z - 1 : 359;
            }

            if (InputHandler.KeyZ)
            {
                scale += 0.02;
            }

            if (InputHandler.KeyX)
            {
                if (scale > 0.05)
                {
                    scale -= 0.02;
                }
            }

            if (InputHandler.KeyNumPad4)
            {
                model.Position.X--;
            }

            if (InputHandler.KeyNumPad6)
            {
                model.Position.X++;
            }

            if (InputHandler.KeyNumPad8)
            {
                model.Position.Y++;
            }

            if (InputHandler.KeyNumPad5)
            {
                model.Position.Y--;
            }

            if (InputHandler.KeyNumPad6)
            {
                model.Position.X++;
            }

            if (InputHandler.KeyNumPad7)
            {
                model.Position.Z++;
            }

            if (InputHandler.KeyNumPad9)
            {
                model.Position.Z--;
            }

            model.Rotate();
        }

        private void Projection()
        {
            var sw = Stopwatch.StartNew();

            for (int v = 0; v < model.Vertices.Count; v++)
            {
                Vertex vertex = model.Vertices[v];

                // Perspective projection
                double pz = camera.Z + vertex.Transformed.Z;
                vertex.Projected.X = (int)(((camera.Z * (vertex.Transformed.X - camera.X)) / pz * scale) + camera.X);
                vertex.Projected.Y = -(int)(((camera.Z * (vertex.Transformed.Y - camera.Y)) / pz * scale) + camera.Y);

                // Scaling and positioning
                vertex.Projected.X += (int)(model.Position.X + Size.Width / 2);
                vertex.Projected.Y += (int)(model.Position.Y + Size.Height / 2);
            }

            renderer.SetLogValue("Game.Projection()", sw.ElapsedMilliseconds);
        }

        private void Culling()
        {
            var sw = Stopwatch.StartNew();


            renderer.SetLogValue("Game.Culling()", sw.ElapsedMilliseconds);
        }

        private void UpdateLog()
        {
            renderer.SetLogValue("scale", scale);
            renderer.SetLogValue("rotation", model.Rotation);
            renderer.SetLogValue("position", model.Position);
        }

        private void Render()
        {
            var sw = Stopwatch.StartNew();

            frame.Clear();
            frame.DrawModel(model);
            renderer.SetLogValue("frame.DrawModel()", sw.ElapsedMilliseconds);

            sw.Restart();
            renderer.SetBackbuffer(frame);
            renderer.SetLogValue("renderer.SetBackbuffer()", sw.ElapsedMilliseconds);

            sw.Restart();
            renderer.SwapBuffer();
            renderer.SetLogValue("renderer.SwapBuffer()", sw.ElapsedMilliseconds);

            sw.Restart();
            renderer.Update();
            renderer.SetLogValue("renderer.Update()", sw.ElapsedMilliseconds);
        }
    }
}
