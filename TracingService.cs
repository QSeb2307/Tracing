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
        private List<PhoneEntry> phoneEntries = new List<PhoneEntry>();
        private int currentIndex;
        private bool isPaused;
        private bool isRunning;
        private CancellationTokenSource? cancellationTokenSource;
        private readonly bool isWindowsEnvironment;

        public event Action<string>? OnStatusChanged;

        public TracingService()
        {
            isWindowsEnvironment = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            if (!isWindowsEnvironment)
            {
                OnStatusChanged?.Invoke("Warning: This application requires Windows to function properly.");
            }
        }

        public void SetPhoneEntries(List<PhoneEntry> entries)
        {
            phoneEntries = entries ?? throw new ArgumentNullException(nameof(entries));
            currentIndex = 0;
            OnStatusChanged?.Invoke($"Loaded {entries.Count} phone entries");
        }

        private async Task ExecuteTraceSequence(PhoneEntry entry)
        {
            // Small delay before starting
            await Task.Delay(200); // Reduced from 500ms

            // Press 't' key
            OnStatusChanged?.Invoke($"Tracing number: {entry.PhoneNumber}");
            SendKeys.SendWait("t");
            await Task.Delay(200); // Reduced from 500ms

            // Type the trace command with a space
            SendKeys.SendWait(" /trace ");
            await Task.Delay(100); // Reduced from 300ms

            // Type the phone number
            SendKeys.SendWait(entry.PhoneNumber);
            await Task.Delay(100); // Reduced from 300ms

            // Press Enter
            SendKeys.SendWait("{ENTER}");
            await Task.Delay(1000); // Reduced from 2000ms

            OnStatusChanged?.Invoke($"Traced: {entry.Name} - {entry.PhoneNumber}");
        }

        public async void Start()
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

            if (phoneEntries.Count == 0)
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
                        await ExecuteTraceSequence(entry);
                        currentIndex++;

                        if (!cancellationTokenSource.Token.IsCancellationRequested)
                        {
                            await Task.Delay(500); // Reduced from 1000ms - delay between traces
                        }
                    }

                    if (currentIndex >= phoneEntries.Count)
                    {
                        OnStatusChanged?.Invoke("Tracing completed");
                        isRunning = false;
                    }
                }
                catch (Exception ex)
                {
                    OnStatusChanged?.Invoke($"Error during tracing: {ex.Message}");
                    isRunning = false;
                }
            });
        }

        public void Pause()
        {
            isPaused = true;
            OnStatusChanged?.Invoke("Tracing paused");
        }

        public void Restart()
        {
            currentIndex = 0;
            isPaused = false;
            OnStatusChanged?.Invoke("Tracing restarted");

            if (!isRunning)
            {
                Start();
            }
        }

        public void Stop()
        {
            cancellationTokenSource?.Cancel();
            isRunning = false;
            isPaused = false;
            currentIndex = 0;
            OnStatusChanged?.Invoke("Tracing stopped");
        }
    }
}