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
        private List<KeyPressedEventArgs> pressedKeys = [];
        private LowLevelKeyboardHookHandler keyboardHookHandler = new LowLevelKeyboardHookHandler();

        public void Start()
        {
            // Create a new low level keyboard hook handler
            keyboardHookHandler.InitializeKeyboardHook();
            keyboardHookHandler.EnableBlocking();

            // Subscribe to the KeyPressed event
            keyboardHookHandler.KeyPressed += KeyboardHookHandler_KeyPressed;
            keyboardHookHandler.BlockingDisabled += KeyboardHookHandler_BlockingDisabled;
        }

        public void Stop()
        {
            keyboardHookHandler.DisableBlocking();
        }

        private void KeyboardHookHandler_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            // Check if the key is already in the list
            if ( !pressedKeys.Contains(e) )
            {
                pressedKeys.Add(e);
                Console.WriteLine($"Number of unique keys pressed: {pressedKeys.Count}");
            }
        }

        // When blocking is disabled, stop the hook and process the list of pressed keys
        private void KeyboardHookHandler_BlockingDisabled(object sender, EventArgs e)
        {
            keyboardHookHandler.StopHook();
            // Print a list of all the names of keys that were pressed along with their scan codes and VK Codes
            foreach ( KeyPressedEventArgs pressedKey in pressedKeys )
            {
                int vkCode = pressedKey.VirtualKeyCode;
                int scanCode = pressedKey.ScanCode;

                string vkHex = vkCode.ToString("X");
                string scanHex = scanCode.ToString("X");

                string keyName = Enum.GetName(typeof(Keys), vkCode) ?? vkCode.ToString();
                Console.WriteLine($"VK: 0x{vkHex} | Scan: 0x{scanHex} | Key: {keyName}");
            }
        }
    }
}
