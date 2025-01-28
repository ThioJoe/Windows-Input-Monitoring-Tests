using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable enable

using DWORD = System.UInt32;        // 4 Bytes, aka uint, uint32

namespace TestInputMonitoring;
internal static class LowLevelKeyboardHookHandler
{
    // Win32 API imports
    [DllImport("user32.dll")]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc callback, IntPtr hInstance, uint threadId);

    [DllImport("user32.dll")]
    private static extern bool UnhookWindowsHookEx(IntPtr hInstance);

    [DllImport("user32.dll")]
    private static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("user32.dll")]
    private static extern short GetKeyState(int keyCode);

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
    private static LowLevelKeyboardProc _proc;
    private static IntPtr _hookID = IntPtr.Zero;

    // Keyboard hook constants
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_KEYUP = 0x0101;
    private const int VK_NUMLOCK = 0x90;
    private const int VK_CAPSLOCK = 0x14;
    private const int VK_SCROLL = 0x91;
    private const int VM_SYSKEYDOWN = 0x104;
    private const int VM_SYSKEYUP = 0x105;

    // -----------------------------------------------------------------------------------------------

    private static bool _keyboardHookActive = false;
    public static bool KeyboardHookActive
    {
        get => _keyboardHookActive;
        set
        {
            if ( _keyboardHookActive != value )
            {
                _keyboardHookActive = value;
                // Custom logic when the property is set
                if ( HookActiveLabelReference != null )
                {
                    HookActiveLabelReference.Text = value ? LabelStrings.LLKeyboardHookActive : LabelStrings.LLKeyboardHookInactive;
                    HookActiveLabelReference.ForeColor = value ? LabelColors.ActiveColor : LabelColors.InactiveColor;
                }
            }
        }
    }
    private static Label? HookActiveLabelReference = null;

    // -----------------------------------------------------------------------------------------------

    // Initialize the keyboard hook. Optionally provide a Windows Forms Label to update with the hook status.
    public static void InitializeKeyboardHook(Label? labelToUpdate = null)
    {
        // Set up keyboard hook
        _proc = HookCallback;
        _hookID = SetHook(_proc);

        // Set the label reference
        if ( labelToUpdate != null )
        {
            HookActiveLabelReference = labelToUpdate;
        }

        KeyboardHookActive = true;
    }

    private static IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using ( var curProcess = System.Diagnostics.Process.GetCurrentProcess() )
        using ( var curModule = curProcess.MainModule )
        {
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
        }
    }

    // This gets run every time a key is pressed. It is called by the Windows API as a callback in response to a key press.
    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if ( nCode >= 0 )
        {
            // Get the data from the struct as an object
            KBDLLHOOKSTRUCT kbd = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
            int vkCode = (int)kbd.vkCode;
            string vkHex = vkCode.ToString("X");
            int scanCode = (int)kbd.scanCode;
            string scanHex = scanCode.ToString("X");
            LowLevelKeyboardHookFlags flags = kbd.flags;

            string keyName = Enum.GetName(typeof(Keys), vkCode) ?? vkCode.ToString();

            uint time = kbd.time;

            // Print the VK code, scan code, key name, flags, and time with formatting
            Console.WriteLine($"VK: 0x{vkHex,-4} | Scan: 0x{scanHex,-4} | Key: {keyName,-15} | Time: {time, -10} | Flags: {flags}");
        }
        // Need to forward the call back to the Windows API or else it will discard the key press.
        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

    public static void StopHook()
    {
        if ( _hookID != IntPtr.Zero )
        {
            UnhookWindowsHookEx(_hookID);
        }
        KeyboardHookActive = false;
    }

    // Returned as pointer in the lparam of the hook callback
    // See: https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-kbdllhookstruct
    private struct KBDLLHOOKSTRUCT
    {
        public DWORD vkCode;          // Virtual key code
        public DWORD scanCode;
        public LowLevelKeyboardHookFlags flags;
        public DWORD time;
        public IntPtr dwExtraInfo;
    }

    // Possible flags returned as part of the KBDLLHOOKSTRUCT "flags" field
    [Flags]
    public enum LowLevelKeyboardHookFlags : uint
    {
        Extended = 0x01,             // Bit 0: Extended key (e.g. function key or numpad)
        LowerILInjected = 0x02,      // Bit 1: Injected from lower integrity level process
        Injected = 0x10,             // Bit 4: Injected from any process
        AltDown = 0x20,              // Bit 5: ALT key pressed
        KeyUp = 0x80                 // Bit 7: Key being released (transition state)
                                     // Bits 2-3, 6 are reserved
    }
}
