using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace PhoneTracer
{
    public partial class MainForm : Form
    {
        private TracingService? tracingService;
        private KeyboardHook? keyboardHook;
        private List<PhoneEntry> phoneEntries;
        private readonly bool isWindowsEnvironment;

        public MainForm()
        {
            InitializeComponent();
            isWindowsEnvironment = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            phoneEntries = new List<PhoneEntry>();

            try
            {
                tracingService = new TracingService();
                keyboardHook = new KeyboardHook();

                // Set up event handlers
                if (tracingService != null && keyboardHook != null)
                {
                    tracingService.OnStatusChanged += UpdateStatus;
                    keyboardHook.OnHotkeyDetected += UpdateStatus;

                    // Register global hotkeys
                    keyboardHook.RegisterHotKey(Keys.Control, Keys.O, StartTracing);
                    keyboardHook.RegisterHotKey(Keys.Control, Keys.H, PauseTracing);
                    keyboardHook.RegisterHotKey(Keys.Control, Keys.R, RestartTracing);
                }

                CheckEnvironment();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing application: {ex.Message}", 
                    "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void CheckEnvironment()
        {
            if (!isWindowsEnvironment)
            {
                btnStartTracing.Enabled = false;
                btnLoadFile.Enabled = false;
                MessageBox.Show("This application requires Windows to function properly.",
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
                        btnStartTracing.Enabled = phoneEntries.Count > 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading file: {ex.Message}", 
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (string.IsNullOrWhiteSpace(line)) continue;

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

            if (tracingService != null)
            {
                tracingService.SetPhoneEntries(phoneEntries);
            }
        }

        private void StartTracing()
        {
            if (phoneEntries.Count > 0 && tracingService != null)
            {
                tracingService.Start();
            }
            else
            {
                MessageBox.Show("Please load phone numbers first.", 
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void PauseTracing()
        {
            tracingService?.Pause();
        }

        private void RestartTracing()
        {
            tracingService?.Restart();
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
            keyboardHook?.Dispose();
            base.OnFormClosing(e);
        }
    }
}