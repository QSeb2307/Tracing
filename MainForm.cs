using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace PhoneTracer
{
    public partial class MainForm : Form
    {
        private TracingService tracingService = null!;
        private KeyboardHook keyboardHook = null!;
        private List<PhoneEntry> phoneEntries = null!;
        private readonly bool isWindowsEnvironment;

        public MainForm()
        {
            InitializeComponent();
            isWindowsEnvironment = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            InitializeServices();
            CheckEnvironment();
        }

        private void InitializeServices()
        {
            phoneEntries = new List<PhoneEntry>();
            tracingService = new TracingService();
            keyboardHook = new KeyboardHook();

            tracingService.OnStatusChanged += UpdateStatus;
            keyboardHook.OnHotkeyDetected += UpdateStatus;

            // Register global hotkeys
            keyboardHook.RegisterHotKey(Keys.Control, Keys.O, StartTracing);
            keyboardHook.RegisterHotKey(Keys.Control, Keys.H, PauseTracing);
            keyboardHook.RegisterHotKey(Keys.Control, Keys.R, RestartTracing);
        }

        private void CheckEnvironment()
        {
            if (!isWindowsEnvironment)
            {
                btnStartTracing.Enabled = false;
                btnLoadFile.Enabled = false;
                UpdateStatus("⚠️ This application requires Windows to function properly.\n" +
                           "Keyboard simulation and hotkeys are not available in this environment.");
                MessageBox.Show("This application is designed for Windows environments only.\n\n" +
                              "Features like keyboard simulation and global hotkeys will not work properly.",
                              "Environment Warning",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
            }
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        LoadPhoneNumbers(openFileDialog.FileName);
                        UpdateStatus($"Loaded {phoneEntries.Count} numbers successfully");
                        btnStartTracing.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading file: {ex.Message}", "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void LoadPhoneNumbers(string filePath)
        {
            phoneEntries.Clear();
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string[] parts = line.Split('\t');
                if (parts.Length == 2)
                {
                    phoneEntries.Add(new PhoneEntry
                    {
                        Name = parts[0].Trim(),
                        PhoneNumber = parts[1].Trim()
                    });
                }
            }

            tracingService.SetPhoneEntries(phoneEntries);
        }

        private void StartTracing()
        {
            if (phoneEntries.Count > 0)
            {
                tracingService.Start();
                UpdateStatus("Tracing started");
            }
            else
            {
                MessageBox.Show("Please load phone numbers first.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void PauseTracing()
        {
            tracingService.Pause();
            UpdateStatus("Tracing paused");
        }

        private void RestartTracing()
        {
            tracingService.Restart();
            UpdateStatus("Tracing restarted");
        }

        private void UpdateStatus(string status)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(UpdateStatus), status);
                return;
            }

            lblStatus.Text = status;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            keyboardHook.Dispose();
            base.OnFormClosing(e);
        }
    }
}