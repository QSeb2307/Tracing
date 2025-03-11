using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections.Generic;

namespace PhoneTracer
{
    public class KeyboardHook : IDisposable
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;

        private readonly Dictionary<int, Action> hotkeyActions = new Dictionary<int, Action>();
        private readonly LowLevelKeyboardProc? proc;
        private IntPtr hookId = IntPtr.Zero;
        private readonly bool isWindowsEnvironment;

        public event Action<string>? OnHotkeyDetected;

        public KeyboardHook()
        {
            isWindowsEnvironment = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            if (!isWindowsEnvironment)
            {
                OnHotkeyDetected?.Invoke("Warning: Keyboard hooks are not supported in non-Windows environments");
                return;
            }

            proc = HookCallback;
            hookId = SetHook(proc);
        }

        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            if (!isWindowsEnvironment) return IntPtr.Zero;

            using (var curProcess = System.Diagnostics.Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule?.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                bool alt = (Control.ModifierKeys & Keys.Alt) != 0;

                if (alt)
                {
                    OnHotkeyDetected?.Invoke($"Detected key combination: Alt+{(Keys)vkCode}");

                    foreach (var hotkey in hotkeyActions)
                    {
                        if (vkCode == hotkey.Key)
                        {
                            OnHotkeyDetected?.Invoke($"Executing action for hotkey: Alt+{(Keys)vkCode}");
                            hotkey.Value.Invoke();
                        }
                    }
                }
            }
            return CallNextHookEx(hookId, nCode, wParam, lParam);
        }

        public void RegisterHotKey(Keys modifier, Keys key, Action action)
        {
            if (!isWindowsEnvironment)
            {
                OnHotkeyDetected?.Invoke($"Cannot register hotkey Alt+{key} in non-Windows environment");
                return;
            }

            if (modifier == Keys.Alt)
            {
                hotkeyActions[(int)key] = action;
                OnHotkeyDetected?.Invoke($"Registered hotkey: Alt+{key}");
            }
        }

        public void Dispose()
        {
            if (isWindowsEnvironment && hookId != IntPtr.Zero)
            {
                UnhookWindowsHookEx(hookId);
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn,
            IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string? lpModuleName);
    }
}