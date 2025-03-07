using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PhoneTracer
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.WriteLine("This application is designed for Windows only.");
                Console.WriteLine("It requires Windows Forms and keyboard simulation capabilities.");
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}