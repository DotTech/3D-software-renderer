using System;
using System.Windows.Forms;

namespace TechEngine
{
    public class Program
    {
        /// <summary>
        /// Entry point of this application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
