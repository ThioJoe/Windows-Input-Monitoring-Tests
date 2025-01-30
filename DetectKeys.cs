using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestInputMonitoring
{
    class DetectKeys
    {
        private List<PressedKey> pressedKeys = [];
        private List<PressedKey> allKeys = [];

        public void Start()
        {
            allKeys = MakeListOfAllKnownKeys();

            // Create a new low level keyboard hook handler
            LowLevelKeyboardHookHandler.InitializeKeyboardHook();
            LowLevelKeyboardHookHandler.EnableBlocking();

            // Subscribe to the KeyPressed event
            LowLevelKeyboardHookHandler.KeyPressed += KeyboardHookHandler_KeyPressed;
            LowLevelKeyboardHookHandler.BlockingDisabled += KeyboardHookHandler_BlockingDisabled;
        }

        public void Stop()
        {
            LowLevelKeyboardHookHandler.DisableBlocking(notify:false);
            FinishAndPrint();
        }

        private void KeyboardHookHandler_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            PressedKey pressedKey = new PressedKey(e.VirtualKeyCode, e.ScanCode);

            // Check if the key is already in the list
            if ( !pressedKeys.Any(k => k.VirtualKeyCode == pressedKey.VirtualKeyCode) )
            {
                pressedKeys.Add(pressedKey);
                Console.WriteLine($"Number of unique keys pressed: {pressedKeys.Count}");
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
            // Print a list of all the names of keys that were pressed along with their scan codes and VK Codes
            foreach ( PressedKey pressedKey in pressedKeys )
            {
                int vkCode = pressedKey.VirtualKeyCode;
                int scanCode = pressedKey.ScanCode;

                string vkHex = vkCode.ToString("X");
                string scanHex = scanCode.ToString("X");

                string keyName = pressedKey.GetKeyName();
                Console.WriteLine($"VK: 0x{vkHex} | Scan: 0x{scanHex} | Key: {keyName}");
            }

            // ---------------------------------------
            Console.WriteLine("\n------------------------------------------------------------\n");
            // Print a list of all the keys that were not pressed
            List<PressedKey> notPressedKeys = GetKeysNotOnKeyboard();
            foreach ( PressedKey notPressedKey in notPressedKeys )
            {
                int vkCode = notPressedKey.VirtualKeyCode;
                int scanCode = notPressedKey.ScanCode;
                string vkHex = vkCode.ToString("X");
                string scanHex = scanCode.ToString("X");
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
                int scanCode = (int)key;
                allKeys.Add(new PressedKey(vkCode, scanCode));
            }
            return allKeys;
        }

        private List<PressedKey> GetKeysNotOnKeyboard()
        {
            return allKeys.Except(pressedKeys).ToList();
        }
    }
}
