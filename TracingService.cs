using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;

namespace PhoneTracer
{
    public class TracingService
    {
        private List<PhoneEntry> phoneEntries;
        private int currentIndex;
        private bool isPaused;
        private bool isRunning;
        private CancellationTokenSource? cancellationTokenSource;
        private readonly bool isWindowsEnvironment;

        public event Action<string>? OnStatusChanged;

        public TracingService()
        {
            try
            {
                phoneEntries = new List<PhoneEntry>();
                currentIndex = 0;
                isPaused = false;
                isRunning = false;
                isWindowsEnvironment = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

                if (!isWindowsEnvironment)
                {
                    OnStatusChanged?.Invoke("Warning: Running in non-Windows environment. Keyboard simulation may not work properly.");
                }
            }
            catch (Exception ex)
            {
                OnStatusChanged?.Invoke($"TracingService initialization error: {ex.Message}");
                throw;
            }
        }

        public void SetPhoneEntries(List<PhoneEntry> entries)
        {
            try
            {
                if (entries == null)
                {
                    throw new ArgumentNullException(nameof(entries), "Phone entries cannot be null");
                }

                phoneEntries = entries;
                currentIndex = 0;
                OnStatusChanged?.Invoke($"Loaded {entries.Count} phone entries");
            }
            catch (Exception ex)
            {
                OnStatusChanged?.Invoke($"Error setting phone entries: {ex.Message}");
                throw;
            }
        }

        private async Task ExecuteTraceSequence(PhoneEntry entry)
        {
            try
            {
                if (entry == null)
                {
                    throw new ArgumentNullException(nameof(entry), "Phone entry cannot be null");
                }

                // Send 't' key for trace command
                OnStatusChanged?.Invoke("Pressing 't' key...");
                SendKeys.SendWait("t");
                await Task.Delay(500);

                // Prepare trace command with proper formatting
                string traceCommand = $"/trace {entry.PhoneNumber}";
                OnStatusChanged?.Invoke($"Preparing command: {traceCommand}");

                try
                {
                    // Clear clipboard first
                    Clipboard.Clear();
                    await Task.Delay(200);

                    // Set new text
                    Clipboard.SetText(traceCommand);
                    await Task.Delay(300);
                }
                catch (Exception clipboardEx)
                {
                    OnStatusChanged?.Invoke($"Clipboard error: {clipboardEx.Message}");
                    return;
                }

                // Paste command and press enter
                OnStatusChanged?.Invoke("Pasting command...");
                SendKeys.SendWait("^v");
                await Task.Delay(500);

                OnStatusChanged?.Invoke("Sending Enter key...");
                SendKeys.SendWait("{ENTER}");
                await Task.Delay(3000); // Wait for trace to complete
            }
            catch (Exception ex)
            {
                OnStatusChanged?.Invoke($"Trace sequence error: {ex.Message}");
            }
        }

        public async void Start()
        {
            try
            {
                if (!isWindowsEnvironment)
                {
                    OnStatusChanged?.Invoke("Error: Cannot start tracing in non-Windows environment");
                    return;
                }

                if (isRunning && !isPaused)
                {
                    OnStatusChanged?.Invoke("Tracing is already running");
                    return;
                }

                if (phoneEntries == null || phoneEntries.Count == 0)
                {
                    OnStatusChanged?.Invoke("No phone entries loaded");
                    return;
                }

                if (isPaused)
                {
                    isPaused = false;
                    OnStatusChanged?.Invoke("Resuming tracing");
                    return;
                }

                isRunning = true;
                cancellationTokenSource = new CancellationTokenSource();

                await Task.Run(async () =>
                {
                    try
                    {
                        while (currentIndex < phoneEntries.Count && !cancellationTokenSource.Token.IsCancellationRequested)
                        {
                            if (isPaused)
                            {
                                await Task.Delay(100);
                                continue;
                            }

                            var entry = phoneEntries[currentIndex];
                            OnStatusChanged?.Invoke($"Tracing: {entry.Name} - {entry.PhoneNumber} ({currentIndex + 1}/{phoneEntries.Count})");

                            await ExecuteTraceSequence(entry);
                            currentIndex++;

                            // Add delay between entries
                            await Task.Delay(2000);
                        }

                        if (currentIndex >= phoneEntries.Count)
                        {
                            OnStatusChanged?.Invoke("Tracing completed");
                            isRunning = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        OnStatusChanged?.Invoke($"Tracing error: {ex.Message}");
                        isRunning = false;
                    }
                });
            }
            catch (Exception ex)
            {
                OnStatusChanged?.Invoke($"Start error: {ex.Message}");
                isRunning = false;
            }
        }

        public void Pause()
        {
            try
            {
                isPaused = true;
                OnStatusChanged?.Invoke("Tracing paused");
            }
            catch (Exception ex)
            {
                OnStatusChanged?.Invoke($"Pause error: {ex.Message}");
            }
        }

        public void Restart()
        {
            try
            {
                currentIndex = 0;
                isPaused = false;
                OnStatusChanged?.Invoke("Tracing restarted");

                if (!isRunning)
                {
                    Start();
                }
            }
            catch (Exception ex)
            {
                OnStatusChanged?.Invoke($"Restart error: {ex.Message}");
            }
        }

        public void Stop()
        {
            try
            {
                cancellationTokenSource?.Cancel();
                isRunning = false;
                isPaused = false;
                currentIndex = 0;
                OnStatusChanged?.Invoke("Tracing stopped");
            }
            catch (Exception ex)
            {
                OnStatusChanged?.Invoke($"Stop error: {ex.Message}");
            }
        }
    }
}