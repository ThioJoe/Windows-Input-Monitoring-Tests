using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#nullable enable

namespace TestRawInput;
internal static class KeyboardHookHandler
{
    // Win32 API imports
    [DllImport("user32.dll")]
    private static extern IntPtr SetWindowsHookEx(int idHook, KeyboardProc callback, IntPtr hInstance, uint threadId);

    [DllImport("user32.dll")]
    private static extern bool UnhookWindowsHookEx(IntPtr hInstance);

    [DllImport("user32.dll")]
    private static extern IntPtr CallNextHookEx(IntPtr idHook, int code, IntPtr wParam, IntPtr lParam);

    private delegate IntPtr KeyboardProc(int code, IntPtr wParam, IntPtr lParam);
    private static KeyboardProc _proc;
    private static IntPtr _hookID = IntPtr.Zero;

    // Hook status
    private static bool _keyboardHookActive = false;
    public static bool KeyboardHookActive
    {
        get => _keyboardHookActive;
        set
        {
            if ( _keyboardHookActive != value )
            {
                _keyboardHookActive = value;
                if ( HookActiveLabelReference != null )
                {
                    HookActiveLabelReference.Text = value ? "Hook Active" : "Hook Inactive";
                }
            }
        }
    }
    private static Label? HookActiveLabelReference = null;

    // Constants
    private const int WH_KEYBOARD = 2;
    private const int HC_ACTION = 0;
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_KEYUP = 0x0101;

    [StructLayout(LayoutKind.Explicit)]
    private struct KeyInfo
    {
        [FieldOffset(0)]
        private readonly int _value;

        public int RepeatCount => _value & 0xFFFF;
        public int ScanCode => (_value >> 16) & 0xFF;
        public bool IsExtendedKey => ((_value >> 24) & 0x1) == 1;
        public bool AltPressed => ((_value >> 29) & 0x1) == 1;
        public bool PreviousState => ((_value >> 30) & 0x1) == 1;
        public bool TransitionState => ((_value >> 31) & 0x1) == 1;

        public KeyInfo(int value)
        {
            _value = value;
        }
    }

    public static void InitializeKeyboardHook(Label? labelToUpdate = null)
    {
        if ( labelToUpdate != null )
            HookActiveLabelReference = labelToUpdate;

        _proc = HookCallback;
        uint threadId = (uint)System.Threading.Thread.CurrentThread.ManagedThreadId;
        _hookID = SetWindowsHookEx(WH_KEYBOARD, _proc, IntPtr.Zero, threadId);

        if ( _hookID == IntPtr.Zero )
        {
            if ( labelToUpdate != null )
                labelToUpdate.Text = "Hook failed to start";
        }
        else
        {
            KeyboardHookActive = true;
        }


        
    }

    private static IntPtr HookCallback(int code, IntPtr wParam, IntPtr lParam)
    {
        if ( code >= 0 )
        {
            int vkCode = (int)wParam;
            var keyInfo = new KeyInfo(lParam.ToInt32());
            Console.WriteLine($"VK: {vkCode}, Scan: {keyInfo.ScanCode}, Alt: {keyInfo.AltPressed}");
        }
        return CallNextHookEx(_hookID, code, wParam, lParam);
    }

    public static void StopHook()
    {
        if ( _hookID != IntPtr.Zero )
        {
            UnhookWindowsHookEx(_hookID);
            _hookID = IntPtr.Zero;
        }
        KeyboardHookActive = false;
    }
}