using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestInputMonitoring
{
    

    class DetectKeys
    {
        // PInvoke for map virtual key
        [DllImport("user32.dll")]
        static extern ushort MapVirtualKey(uint uCode, uint uMapType);


        private List<PressedKey> pressedKeys = [];
        private List<PressedKey> allKeys = [];

        public void Start()
        {
            allKeys = MakeListOfAllKnownKeys();

            // Create a new low level keyboard hook handler
            LowLevelKeyboardHookHandler.InitializeKeyboardHook();
            LowLevelKeyboardHookHandler.EnableBlocking();
            LowLevelKeyboardHookHandler.SetNoPrintMode(true);

            // Subscribe to the KeyPressed event
            LowLevelKeyboardHookHandler.KeyPressed += KeyboardHookHandler_KeyPressed;
            LowLevelKeyboardHookHandler.BlockingDisabled += KeyboardHookHandler_BlockingDisabled;

            // Print instructions
            Console.WriteLine("\n\nPress all the keys on the keyboard. Press ESC to stop detection and see a list of keys and which ones weren't pressed.\n\n");
        }

        public void Stop()
        {
            LowLevelKeyboardHookHandler.DisableBlocking(notify:false);
            FinishAndPrint();
            LowLevelKeyboardHookHandler.SetNoPrintMode(false);
        }

        private void KeyboardHookHandler_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            PressedKey pressedKey = new PressedKey(e.VirtualKeyCode, e.ScanCode);

            // Check if the key is already in the list
            if ( !pressedKeys.Any(k => k.VirtualKeyCode.Equals(pressedKey.VirtualKeyCode)) )
            {
                pressedKeys.Add(pressedKey);
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"Number of unique keys pressed: {pressedKeys.Count}   ");

                //Console.WriteLine($"\rNumber of unique keys pressed: {pressedKeys.Count}");
            }
        }

        // When blocking is disabled, stop the hook and process the list of pressed keys
        private void KeyboardHookHandler_BlockingDisabled(object sender, EventArgs e)
        {
            FinishAndPrint();
        }

        private void FinishAndPrint()
        {
            LowLevelKeyboardHookHandler.StopHook();
            Console.WriteLine("\n\n-------------------------- Known Keys Pressed: ----------------------------------\n");

            // Sort the list of pressed keys by Virtual Key Code
            pressedKeys.Sort((a, b) => a.VirtualKeyCode.CompareTo(b.VirtualKeyCode));

            // Print a list of all the names of keys that were pressed along with their scan codes and VK Codes
            foreach ( PressedKey pressedKey in pressedKeys )
            {
                int vkCode = pressedKey.VirtualKeyCode;
                int scanCode = pressedKey.ScanCode;

                string vkHex = vkCode.ToString("X");
                string scanHex = scanCode.ToString("X4");

                string keyName = pressedKey.GetKeyName();
                Console.WriteLine($"VK: 0x{vkHex} | Scan: 0x{scanHex} | Key: {keyName}");
            }

            // ---------------------------------------
            Console.WriteLine("\n-------------------------- Known Keys Not Pressed: ----------------------------------\n");
            // Print a list of all the keys that were not pressed
            List<PressedKey> notPressedKeys = GetKeysNotOnKeyboard();
            foreach ( PressedKey notPressedKey in notPressedKeys )
            {
                int vkCode = notPressedKey.VirtualKeyCode;
                int scanCode = notPressedKey.ScanCode;
                string vkHex = vkCode.ToString("X2");
                string scanHex = scanCode.ToString("X4");
                string keyName = notPressedKey.GetKeyName();
                Console.WriteLine($"VK: 0x{vkHex} | Scan: 0x{scanHex} | Key: {keyName}");
            }

        }

        private class PressedKey
        {
            public int VirtualKeyCode { get; set; }
            public int ScanCode { get; set; }
            public Keys Key { get; set; }

            public PressedKey(int vkCode, int scanCode)
            {
                VirtualKeyCode = vkCode;
                ScanCode = scanCode;
                Key = (Keys)vkCode;
            }
            public string GetKeyName()
            {
                return Enum.GetName(typeof(Keys), VirtualKeyCode) ?? VirtualKeyCode.ToString();
            }
        }

        private List<PressedKey> MakeListOfAllKnownKeys()
        {
            List<PressedKey> allKeys = new List<PressedKey>();
            foreach ( Keys key in Enum.GetValues(typeof(Keys)) )
            {
                int vkCode = (int)key;
                int scanCode = getScanCode((ushort)vkCode);
                allKeys.Add(new PressedKey(vkCode, scanCode));
            }
            return allKeys;
        }

        private List<PressedKey> GetKeysNotOnKeyboard()
        {
            List<PressedKey> notPressedKeys = new List<PressedKey>();
            foreach ( PressedKey key in allKeys )
            {
                if ( !pressedKeys.Any(k => k.VirtualKeyCode.Equals(key.VirtualKeyCode)) )
                {
                    notPressedKeys.Add(key);
                }
            }

            return notPressedKeys;
        }

        public enum MapVirtualKeyType : uint
        {
            MAPVK_VK_TO_VSC = 0,
            MAPVK_VSC_TO_VK = 1,
            MAPVK_VK_TO_CHAR = 2,
            MAPVK_VSC_TO_VK_EX = 3,
            MAPVK_VK_TO_VSC_EX = 4
        }

        private ushort getScanCode(ushort vkCode)
        {
            return MapVirtualKey((uint)vkCode, (uint)MapVirtualKeyType.MAPVK_VK_TO_VSC);
        }
    }
}
