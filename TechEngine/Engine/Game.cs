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

            var sw = Stopwatch.StartNew();
            //model = ModelFactory.CreateTestObject();
            model = ModelFactory.CreateFromFile("bunny.ply", 1000);
            //model = ModelFactory.CreateFromFile(@"C:\Users\Ruud\Desktop\Armadillo2.ply", 5);
            //model = ModelFactory.CreateFromFile(@"g:\Data\Projects\3D\3dconverter\models\horseasc.ply", 1000);
            //model = ModelFactory.CreateFromFile(@"g:\Data\Projects\3D\3dconverter\models\ateneav.ply", 0.1);
            //model = ModelFactory.CreateFromFile(@"g:\Data\Projects\3D\3dconverter\models\pots.ply", 1);
            //model = ModelFactory.CreateFromFile(@"g:\Data\Projects\3D\3dconverter\models\balls.ply", 1);
            Logger.Value("model load", sw.ElapsedMilliseconds);

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
            timerUpdate.Interval = 10;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            InputHandler.OnKeyDown(this, e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            InputHandler.OnKeyUp(this, e);
        }
        
        private void Update(object sender, EventArgs e)
        {
            HandleKeys();

            model.Transform(camera, scale);

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
        }

        private void UpdateLog()
        {
            Logger.Value("scale", scale);
            Logger.Value("camera", camera);
            Logger.Value("rotation", model.Rotation);
            Logger.Value("position", model.Position);
            Logger.Value("pivot", model.Pivot);
        }

        private void Render()
        {
            var sw = Stopwatch.StartNew();

            frame.Clear();
            frame.DrawModel(model);
            Logger.Value("frame.DrawModel()", sw.ElapsedMilliseconds);

            sw.Restart();
            renderer.SetBackbuffer(frame);
            Logger.Value("renderer.SetBackbuffer()", sw.ElapsedMilliseconds);

            sw.Restart();
            renderer.SwapBuffer();
            Logger.Value("renderer.SwapBuffer()", sw.ElapsedMilliseconds);

            sw.Restart();
            renderer.Update();
            Logger.Value("renderer.Update()", sw.ElapsedMilliseconds);
        }
    }
}
