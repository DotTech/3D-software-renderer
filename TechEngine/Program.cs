using System;
using System.Windows.Forms;
using System.Globalization;

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
            Application.CurrentCulture = new CultureInfo("en-US");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
