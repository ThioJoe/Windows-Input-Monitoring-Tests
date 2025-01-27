using System;
//using System.Reflection.Emit;
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
    [DllImport("kernel32.dll")]
    private static extern uint GetCurrentThreadId();

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
                    HookActiveLabelReference.Text = value ? LabelStrings.KeyboardHookActive : LabelStrings.KeyboardHookInactive;
                    HookActiveLabelReference.ForeColor = value ? LabelColors.ActiveColor : LabelColors.InactiveColor;
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

    public struct KeyInfo
    {
        private readonly IntPtr _value;
        private readonly int _vkCode;

        public KeyInfo(IntPtr lParam, int vkCode)
        {
            _value = lParam;
            _vkCode = vkCode;
        }

        public int VirtualKey => _vkCode;

        private long ToInt64() => _value.ToInt64();

        public int RepeatCount => (int)(ToInt64() & 0xFFFF);
        public int ScanCode => (int)((ToInt64() >> 16) & 0xFF);
        public bool IsExtendedKey => ((ToInt64() >> 24) & 0x1) == 1;
        public bool AltPressed => ((ToInt64() >> 29) & 0x1) == 1;
        public bool PreviousState => ((ToInt64() >> 30) & 0x1) == 1;
        public bool TransitionState => ((ToInt64() >> 31) & 0x1) == 1;

        public override string ToString()
        {
            string keyName = "Unknown";
            try
            {
                keyName = ((Keys)VirtualKey).ToString();
            }
            catch { }

            string t = "   ";
            string scanCodeHex = (IsExtendedKey ? "E0" : "00") + ScanCode.ToString("X2");
            string vkCodeHex = VirtualKey.ToString("X2");

            return $"Key: {keyName}" +
                   $"\n{t}ScanCode: \t0x{scanCodeHex}" +
                   $"\n{t}VKey: \t0x{vkCodeHex}" +
                   $"\n{t}Repeat: \t{RepeatCount}" +
                   $"\n{t}Flags: \t" +
                   $"{(IsExtendedKey ? "Extended " : "")}" +
                   $"{(AltPressed ? "Alt " : "")}" +
                   $"{(PreviousState ? "PrevDown " : "")}" +
                   $"{(TransitionState ? "Released" : "Pressed")}\n";
        }
    }

    public static void InitializeKeyboardHook(Label? labelToUpdate = null)
    {
        if ( labelToUpdate != null )
            HookActiveLabelReference = labelToUpdate;

        _proc = HookCallback;
        //uint threadId = 0;
        uint threadId = GetCurrentThreadId();

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
            var keyInfo = new KeyInfo(lParam, vkCode);

            //Console.WriteLine($"VK: {vkCode}, Scan: {keyInfo.ScanCode}");
            Console.WriteLine(keyInfo.ToString());
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