using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;

namespace PhoneTracer
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                // Create log directory if it doesn't exist
                string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
                Directory.CreateDirectory(logPath);

                // Log startup
                File.AppendAllText(Path.Combine(logPath, "app.log"), 
                    $"{DateTime.Now}: Application starting\n");

                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    MessageBox.Show("This application is designed for Windows only.\n" +
                                  "It requires Windows Forms and keyboard simulation capabilities.",
                                  "Environment Error",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                    return;
                }

                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                string errorMessage = $"Application crashed: {ex.Message}\n" +
                                    $"Stack Trace: {ex.StackTrace}";

                // Log error
                try
                {
                    string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
                    Directory.CreateDirectory(logPath);
                    File.AppendAllText(Path.Combine(logPath, "error.log"), 
                        $"{DateTime.Now}: {errorMessage}\n");
                }
                catch
                {
                    // If logging fails, at least show the error
                }

                MessageBox.Show($"Application Error:\n\n{errorMessage}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }
    }
}