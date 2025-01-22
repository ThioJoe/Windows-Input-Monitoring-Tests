﻿global using static TestRawInput.WinEnums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

#nullable enable

namespace TestRawInput
{
    internal static class WinEnums
    {
        // See: https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-rawinputdevice
        [StructLayout(LayoutKind.Sequential)]
        public struct RAWINPUTDEVICE
        {
            public ushort usUsagePage;
            public ushort usUsage;
            public _dwFlags dwFlags;
            public IntPtr hwndTarget;

            public enum _dwFlags : uint
            {
                RIDEV_REMOVE = 0x00000001,
                RIDEV_EXCLUDE = 0x00000010,
                RIDEV_PAGEONLY = 0x00000020,
                RIDEV_NOLEGACY = 0x00000030,
                RIDEV_INPUTSINK = 0x00000100,
                RIDEV_CAPTUREMOUSE = 0x00000200,
                RIDEV_NOHOTKEYS = 0x00000200,
                RIDEV_APPKEYS = 0x00000400,
                RIDEV_EXINPUTSINK = 0x00001000,
                RIDEV_DEVNOTIFY = 0x00002000,

                //RID_INPUT = 0x10000003,
                //RIM_TYPEKEYBOARD = 1,
                //WM_INPUT = 0x00FF,
            }
        }

        // For use in GetRawInputData: https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getrawinputdata
        public enum uiCommand : uint
        {
            RID_HEADER = 0x10000005,
            RID_INPUT = 0x10000003
        }

        public enum WM_INPUT_wParam : uint
        {
            RIM_INPUT = 0,
            RIM_INPUTSINK = 1
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RAWINPUTDEVICELIST
        {

        }

        // See: https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-rawinputheader
        [StructLayout(LayoutKind.Sequential)]
        public struct RAWINPUTHEADER
        {
            public _dwType dwType;
            public uint dwSize;
            public IntPtr hDevice;
            public IntPtr wParam;

            public enum _dwType : uint
            {
                RIM_TYPEMOUSE = 0,
                RIM_TYPEKEYBOARD = 1,
                RIM_TYPEHID = 2
            }
        }

        // See: https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-rawkeyboard
        [StructLayout(LayoutKind.Sequential)]
        public struct RAWKEYBOARD
        {
            public ushort MakeCode;
            public _Flags Flags;
            public ushort Reserved;
            public ushort VKey;
            public uint Message;
            public uint ExtraInformation;

            [Flags]
            public enum _Flags : ushort
            {
                RI_KEY_MAKE = 0,
                RI_KEY_BREAK = 1,
                RI_KEY_E0 = 2,
                RI_KEY_E1 = 4,
            }
        }

        // See: https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-rawinput
        [StructLayout(LayoutKind.Sequential)]
        public struct RAWINPUT
        {
            public RAWINPUTHEADER header;
            public RAWKEYBOARD keyboard;
        }

        // For SetWindowLongPtr: https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowlongptra#parameters
        public enum nIndex : int
        {
            GWLP_WNDPROC = -4,  // Sets a new address for the window procedure.
            GWLP_HINSTANCE = -6,
            GWL_EXSTYLE = -20,
            GWLP_ID = -12,
            GWL_STYLE = -16,
            GWLP_USERDATA = -21,
        }

        // For use in the NOTIFYICONDATA structure
        // See: https://learn.microsoft.com/en-us/windows/win32/api/shellapi/ns-shellapi-notifyicondataa
        // Values from shellapi.h
        public enum uVersion : uint
        {
            _zero = 0, // Not named but a possible value
            NOTIFYICON_VERSION = 3, // Use this or else the context menu will not work
            NOTIFYICON_VERSION_4 = 4
        }

        [Flags]
        public enum NOTIFYICONDATAA_uFlags : uint
        {
            NIF_MESSAGE = 0x00000001,
            NIF_ICON = 0x00000002,
            NIF_TIP = 0x00000004,
            NIF_STATE = 0x00000008,
            NIF_INFO = 0x00000010,
            NIF_GUID = 0x00000020,
            NIF_REALTIME = 0x00000040,
            NIF_SHOWTIP = 0x00000080
        }

        // See: https://learn.microsoft.com/en-us/windows/win32/api/shellapi/nf-shellapi-shell_notifyiconw
        public enum NotifyIcon_dwMessage
        {
            NIM_ADD = 0x00000000,
            NIM_MODIFY = 0x00000001,
            NIM_DELETE = 0x00000002,
            NIM_SETFOCUS = 0x00000003,
            NIM_SETVERSION = 0x00000004
        }

        public enum KeyboardHook : int
        {
            WH_KEYBOARD_LL = 13,
            WM_KEYDOWN = 0x0100,
            WM_KEYUP = 0x0101,
            VK_NUMLOCK = 0x90,
            VK_CAPSLOCK = 0x14,
            VM_SYSKEYDOWN = 0x104,
            VM_SYSKEYUP = 0x105,
        }

        public enum ShellNotifyIconMessage : uint
        {
            NIM_ADD = 0x00000000,
            NIM_MODIFY = 0x00000001,
            NIM_DELETE = 0x00000002,
            NIM_SETFOCUS = 0x00000003,
            NIM_SETVERSION = 0x00000004,
        }

        // See: https://learn.microsoft.com/en-us/uwp/api/windows.devices.lights.lamparraykind
        private enum LampArrayKind : int
        {
            Undefined = 0,
            Keyboard = 1,
            Mouse = 2,
            GameController = 3,
            Peripheral = 4,
            Scene = 5,
            Notification = 6,
            Chassis = 7,
            Wearable = 8,
            Furniture = 9,
            Art = 10,
            Headset = 11,
            Microphone = 12,
            Speaker = 13
        }

        public enum HIDUsagePage : ushort
        {
            HID_USAGE_PAGE_GENERIC = 0x01,
            HID_USAGE_PAGE_GAME = 0x05,
            HID_USAGE_PAGE_LED = 0x08,
            HID_USAGE_PAGE_BUTTON = 0x09
        }

        public enum HIDGenericDesktopUsage : ushort
        {
            HID_USAGE_GENERIC_POINTER = 0x01,
            HID_USAGE_GENERIC_MOUSE = 0x02,
            HID_USAGE_GENERIC_JOYSTICK = 0x04,
            HID_USAGE_GENERIC_GAMEPAD = 0x05,
            HID_USAGE_GENERIC_KEYBOARD = 0x06,
            HID_USAGE_GENERIC_KEYPAD = 0x07,
            HID_USAGE_GENERIC_MULTI_AXIS_CONTROLLER = 0x08
        }

        public enum WM_MESSAGE : uint
        {
            WM_NULL = 0x0000,
            WM_CREATE = 0x0001,
            WM_DESTROY = 0x0002,
            WM_MOVE = 0x0003,
            WM_SIZE = 0x0005,
            WM_ACTIVATE = 0x0006,
            WM_SETFOCUS = 0x0007,
            WM_KILLFOCUS = 0x0008,
            WM_ENABLE = 0x000A,
            WM_SETREDRAW = 0x000B,
            WM_SETTEXT = 0x000C,
            WM_GETTEXT = 0x000D,
            WM_GETTEXTLENGTH = 0x000E,
            WM_PAINT = 0x000F,
            WM_CLOSE = 0x0010,
            WM_QUERYENDSESSION = 0x0011,
            WM_QUERYOPEN = 0x0013,
            WM_ENDSESSION = 0x0016,
            WM_QUIT = 0x0012,
            WM_ERASEBKGND = 0x0014,
            WM_SYSCOLORCHANGE = 0x0015,
            WM_SHOWWINDOW = 0x0018,
            WM_WININICHANGE = 0x001A,
            WM_DEVMODECHANGE = 0x001B,
            WM_ACTIVATEAPP = 0x001C,
            WM_FONTCHANGE = 0x001D,
            WM_TIMECHANGE = 0x001E,
            WM_CANCELMODE = 0x001F,
            WM_SETCURSOR = 0x0020,
            WM_MOUSEACTIVATE = 0x0021,
            WM_CHILDACTIVATE = 0x0022,
            WM_QUEUESYNC = 0x0023,
            WM_GETMINMAXINFO = 0x0024,
            WM_PAINTICON = 0x0026,
            WM_ICONERASEBKGND = 0x0027,
            WM_NEXTDLGCTL = 0x0028,
            WM_SPOOLERSTATUS = 0x002A,
            WM_DRAWITEM = 0x002B,
            WM_MEASUREITEM = 0x002C,
            WM_DELETEITEM = 0x002D,
            WM_VKEYTOITEM = 0x002E,
            WM_CHARTOITEM = 0x002F,
            WM_SETFONT = 0x0030,
            WM_GETFONT = 0x0031,
            WM_SETHOTKEY = 0x0032,
            WM_GETHOTKEY = 0x0033,
            WM_QUERYDRAGICON = 0x0037,
            WM_COMPAREITEM = 0x0039,
            WM_GETOBJECT = 0x003D,
            WM_COMPACTING = 0x0041,
            WM_COMMNOTIFY = 0x0044,
            WM_WINDOWPOSCHANGING = 0x0046,
            WM_WINDOWPOSCHANGED = 0x0047,
            WM_POWER = 0x0048,
            WM_COPYDATA = 0x004A,
            WM_CANCELJOURNAL = 0x004B,
            WM_NOTIFY = 0x004E,
            WM_INPUTLANGCHANGEREQUEST = 0x0050,
            WM_INPUTLANGCHANGE = 0x0051,
            WM_TCARD = 0x0052,
            WM_HELP = 0x0053,
            WM_USERCHANGED = 0x0054,
            WM_NOTIFYFORMAT = 0x0055,
            WM_CONTEXTMENU = 0x007B,
            WM_STYLECHANGING = 0x007C,
            WM_STYLECHANGED = 0x007D,
            WM_DISPLAYCHANGE = 0x007E,
            WM_GETICON = 0x007F,
            WM_SETICON = 0x0080,
            WM_NCCREATE = 0x0081,
            WM_NCDESTROY = 0x0082,
            WM_NCCALCSIZE = 0x0083,
            WM_NCHITTEST = 0x0084,
            WM_NCPAINT = 0x0085,
            WM_NCACTIVATE = 0x0086,
            WM_GETDLGCODE = 0x0087,
            WM_SYNCPAINT = 0x0088,
            WM_NCMOUSEMOVE = 0x00A0,
            WM_NCLBUTTONDOWN = 0x00A1,
            WM_NCLBUTTONUP = 0x00A2,
            WM_NCLBUTTONDBLCLK = 0x00A3,
            WM_NCRBUTTONDOWN = 0x00A4,
            WM_NCRBUTTONUP = 0x00A5,
            WM_NCRBUTTONDBLCLK = 0x00A6,
            WM_NCMBUTTONDOWN = 0x00A7,
            WM_NCMBUTTONUP = 0x00A8,
            WM_NCMBUTTONDBLCLK = 0x00A9,
            WM_NCXBUTTONDOWN = 0x00AB,
            WM_NCXBUTTONUP = 0x00AC,
            WM_NCXBUTTONDBLCLK = 0x00AD,
            WM_INPUT_DEVICE_CHANGE = 0x00FE,
            WM_INPUT = 0x00FF,
            WM_KEYFIRST = 0x0100,
            WM_KEYDOWN = 0x0100,
            WM_KEYUP = 0x0101,
            WM_CHAR = 0x0102,
            WM_DEADCHAR = 0x0103,
            WM_SYSKEYDOWN = 0x0104,
            WM_SYSKEYUP = 0x0105,
            WM_SYSCHAR = 0x0106,
            WM_SYSDEADCHAR = 0x0107,
            WM_UNICHAR = 0x0109,
            WM_KEYLAST = 0x0109,    // XP And Later
            //WM_KEYLAST = 0x0108, //  Only for earlier than Windows XP
            WM_IME_STARTCOMPOSITION = 0x010D,
            WM_IME_ENDCOMPOSITION = 0x010E,
            WM_IME_COMPOSITION = 0x010F,
            WM_IME_KEYLAST = 0x010F,
            WM_INITDIALOG = 0x0110,
            WM_COMMAND = 0x0111,
            WM_SYSCOMMAND = 0x0112,
            WM_TIMER = 0x0113,
            WM_HSCROLL = 0x0114,
            WM_VSCROLL = 0x0115,
            WM_INITMENU = 0x0116,
            WM_INITMENUPOPUP = 0x0117,
            WM_GESTURE = 0x0119,
            WM_GESTURENOTIFY = 0x011A,
            WM_MENUSELECT = 0x011F,
            WM_MENUCHAR = 0x0120,
            WM_ENTERIDLE = 0x0121,
            WM_MENURBUTTONUP = 0x0122,
            WM_MENUDRAG = 0x0123,
            WM_MENUGETOBJECT = 0x0124,
            WM_UNINITMENUPOPUP = 0x0125,
            WM_MENUCOMMAND = 0x0126,
            WM_CHANGEUISTATE = 0x0127,
            WM_UPDATEUISTATE = 0x0128,
            WM_QUERYUISTATE = 0x0129,
            WM_CTLCOLORMSGBOX = 0x0132,
            WM_CTLCOLOREDIT = 0x0133,
            WM_CTLCOLORLISTBOX = 0x0134,
            WM_CTLCOLORBTN = 0x0135,
            WM_CTLCOLORDLG = 0x0136,
            WM_CTLCOLORSCROLLBAR = 0x0137,
            WM_CTLCOLORSTATIC = 0x0138,
            WM_MOUSEFIRST = 0x0200,
            WM_MOUSEMOVE = 0x0200,
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_LBUTTONDBLCLK = 0x0203,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205,
            WM_RBUTTONDBLCLK = 0x0206,
            WM_MBUTTONDOWN = 0x0207,
            WM_MBUTTONUP = 0x0208,
            WM_MBUTTONDBLCLK = 0x0209,
            WM_MOUSEWHEEL = 0x020A,
            WM_XBUTTONDOWN = 0x020B,
            WM_XBUTTONUP = 0x020C,
            WM_XBUTTONDBLCLK = 0x020D,
            WM_MOUSEHWHEEL = 0x020E,
            WM_MOUSELAST = 0x020E, // Windows Vista/Server 2008 and later (_WIN32_WINNT >= 0x0600)
            //WM_MOUSELAST = 0x020D, // Windows XP/Server 2003 (_WIN32_WINNT >= 0x0501)
            //WM_MOUSELAST = 0x020A, // Windows 2000 (_WIN32_WINNT >= 0x0500)
            //WM_MOUSELAST = 0x0209, // Windows NT 4.0 (_WIN32_WINNT >= 0x0400)
            WM_PARENTNOTIFY = 0x0210,
            WM_ENTERMENULOOP = 0x0211,
            WM_EXITMENULOOP = 0x0212,
            WM_NEXTMENU = 0x0213,
            WM_SIZING = 0x0214,
            WM_CAPTURECHANGED = 0x0215,
            WM_MOVING = 0x0216,
            WM_POWERBROADCAST = 0x0218,
            WM_DEVICECHANGE = 0x0219,
            WM_MDICREATE = 0x0220,
            WM_MDIDESTROY = 0x0221,
            WM_MDIACTIVATE = 0x0222,
            WM_MDIRESTORE = 0x0223,
            WM_MDINEXT = 0x0224,
            WM_MDIMAXIMIZE = 0x0225,
            WM_MDITILE = 0x0226,
            WM_MDICASCADE = 0x0227,
            WM_MDIICONARRANGE = 0x0228,
            WM_MDIGETACTIVE = 0x0229,
            WM_MDISETMENU = 0x0230,
            WM_ENTERSIZEMOVE = 0x0231,
            WM_EXITSIZEMOVE = 0x0232,
            WM_DROPFILES = 0x0233,
            WM_MDIREFRESHMENU = 0x0234,
            WM_POINTERDEVICECHANGE = 0x238,
            WM_POINTERDEVICEINRANGE = 0x239,
            WM_POINTERDEVICEOUTOFRANGE = 0x23A,
            WM_TOUCH = 0x0240,
            WM_NCPOINTERUPDATE = 0x0241,
            WM_NCPOINTERDOWN = 0x0242,
            WM_NCPOINTERUP = 0x0243,
            WM_POINTERUPDATE = 0x0245,
            WM_POINTERDOWN = 0x0246,
            WM_POINTERUP = 0x0247,
            WM_POINTERENTER = 0x0249,
            WM_POINTERLEAVE = 0x024A,
            WM_POINTERACTIVATE = 0x024B,
            WM_POINTERCAPTURECHANGED = 0x024C,
            WM_TOUCHHITTESTING = 0x024D,
            WM_POINTERWHEEL = 0x024E,
            WM_POINTERHWHEEL = 0x024F,
            WM_POINTERROUTEDTO = 0x0251,
            WM_POINTERROUTEDAWAY = 0x0252,
            WM_POINTERROUTEDRELEASED = 0x0253,
            WM_IME_SETCONTEXT = 0x0281,
            WM_IME_NOTIFY = 0x0282,
            WM_IME_CONTROL = 0x0283,
            WM_IME_COMPOSITIONFULL = 0x0284,
            WM_IME_SELECT = 0x0285,
            WM_IME_CHAR = 0x0286,
            WM_IME_REQUEST = 0x0288,
            WM_IME_KEYDOWN = 0x0290,
            WM_IME_KEYUP = 0x0291,
            WM_MOUSEHOVER = 0x02A1,
            WM_MOUSELEAVE = 0x02A3,
            WM_NCMOUSEHOVER = 0x02A0,
            WM_NCMOUSELEAVE = 0x02A2,
            WM_WTSSESSION_CHANGE = 0x02B1,
            WM_TABLET_FIRST = 0x02c0,
            WM_TABLET_LAST = 0x02df,
            WM_DPICHANGED = 0x02E0,
            WM_DPICHANGED_BEFOREPARENT = 0x02E2,
            WM_DPICHANGED_AFTERPARENT = 0x02E3,
            WM_GETDPISCALEDSIZE = 0x02E4,
            WM_CUT = 0x0300,
            WM_COPY = 0x0301,
            WM_PASTE = 0x0302,
            WM_CLEAR = 0x0303,
            WM_UNDO = 0x0304,
            WM_RENDERFORMAT = 0x0305,
            WM_RENDERALLFORMATS = 0x0306,
            WM_DESTROYCLIPBOARD = 0x0307,
            WM_DRAWCLIPBOARD = 0x0308,
            WM_PAINTCLIPBOARD = 0x0309,
            WM_VSCROLLCLIPBOARD = 0x030A,
            WM_SIZECLIPBOARD = 0x030B,
            WM_ASKCBFORMATNAME = 0x030C,
            WM_CHANGECBCHAIN = 0x030D,
            WM_HSCROLLCLIPBOARD = 0x030E,
            WM_QUERYNEWPALETTE = 0x030F,
            WM_PALETTEISCHANGING = 0x0310,
            WM_PALETTECHANGED = 0x0311,
            WM_HOTKEY = 0x0312,
            WM_PRINT = 0x0317,
            WM_PRINTCLIENT = 0x0318,
            WM_APPCOMMAND = 0x0319,
            WM_THEMECHANGED = 0x031A,
            WM_CLIPBOARDUPDATE = 0x031D,
            WM_DWMCOMPOSITIONCHANGED = 0x031E,
            WM_DWMNCRENDERINGCHANGED = 0x031F,
            WM_DWMCOLORIZATIONCOLORCHANGED = 0x0320,
            WM_DWMWINDOWMAXIMIZEDCHANGE = 0x0321,
            WM_DWMSENDICONICTHUMBNAIL = 0x0323,
            WM_DWMSENDICONICLIVEPREVIEWBITMAP = 0x0326,
            WM_GETTITLEBARINFOEX = 0x033F,
            WM_HANDHELDFIRST = 0x0358,
            WM_HANDHELDLAST = 0x035F,
            WM_AFXFIRST = 0x0360,
            WM_AFXLAST = 0x037F,
            WM_PENWINFIRST = 0x0380,
            WM_PENWINLAST = 0x038F,
            WM_APP = 0x8000,
            WM_USER = 0x0400,
            WM_TOOLTIPDISMISS = 0x0345,

            // Can't find exactly where these are from but apparently it's correct
            WM_TRAYICON = 0x800
        }
    }
}
