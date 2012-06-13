using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace TechEngine.Engine
{
    public class Renderer : PictureBox
    {
        private int[] backBuffer;
        private Bitmap screenBuffer;

        
        private Stopwatch swUpdate;

        public int FPS { get; set; }
        public bool Initialized { get; set; }

        /// <summary>
        /// Must be called before any methods are invoked
        /// </summary>
        public void Init()
        {
            backBuffer = new int[Size.Width * Size.Height];
            screenBuffer = new Bitmap(Size.Width, Size.Height, PixelFormat.Format24bppRgb);

            swUpdate = new Stopwatch();
            swUpdate.Start();

            Initialized = true;
        }

        /// <summary>
        /// Draws an entire frame to the backbuffer
        /// </summary>
        /// <param name="buffer"></param>
        public void SetBackbuffer(FrameBuffer buffer)
        {
            buffer.Buffer.CopyTo(backBuffer, 0);
        }

        /// <summary>
        /// Swap framebuffer
        /// </summary>
        public void SwapBuffer()
        {
            // Write the backBuffer color data directly to the screenBuffer Bitmap object
            BitmapData bmp = screenBuffer.LockBits(new Rectangle(0, 0, screenBuffer.Width, screenBuffer.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmp.Stride;
            IntPtr scan0 = bmp.Scan0;

            unsafe
            {
                // Pointer to bitmap data
                byte* p = (byte*)(void*)scan0;

                // Loop through screenBuffer pixels
                for (int i = 0; i < backBuffer.Length; i++)
                {
                    p[0] = (byte)(backBuffer[i] & 0xFF); 
                    p[1] = (byte)((backBuffer[i] >> 8) & 0xFF);
                    p[2] = (byte)((backBuffer[i] >> 16) & 0xFF);
                    p += 3;
                }
            }

            screenBuffer.UnlockBits(bmp);
        }

        /// <summary>
        /// Draw the screenbuffer and update fps count
        /// </summary>
        public new void Update()
        {
            FPS = 1000 / (int)swUpdate.ElapsedMilliseconds;
            Logger.Value("FPS", FPS);

            swUpdate.Restart();
            
            base.Invalidate();
            base.Update();
        }

        /// <summary>
        /// Paint the contents of the screenBuffer on the control surface
        /// </summary>
        /// <param name="pe"></param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            var g = pe.Graphics;

            g.DrawImageUnscaled(screenBuffer, new Point(0, 0));

            TextRenderer.DrawText(g, Logger.Report(), new Font(FontFamily.GenericSansSerif, 8), new Point(10, 10), Color.White);
        }
    }
}
