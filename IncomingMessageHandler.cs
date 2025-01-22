using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace TestRawInput
{
    internal class IncomingMessageHandler
    {
        private Form _targetForm;
        private IntPtr _originalWndProc;
        private bool _isMonitoring = false;

        // Getter/Setter for isMonitoring
        public bool IsMonitoring
        {
            get => _isMonitoring;
            set
            {
                if ( _isMonitoring != value )
                {
                    _isMonitoring = value;
                    if ( MonitorActiveLabelReference != null )
                    {
                        MonitorActiveLabelReference.Text = value ? "Monitoring Messages" : "Stopped Monitoring Messages";
                    }
                }
            }
        }

        Label MonitorActiveLabelReference = null;

        // Delegate for the new window procedure
        private delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        private WndProcDelegate _newWndProc;

        public IncomingMessageHandler(Form form, Label monitorActiveLabelReference)
        {
            _targetForm = form;
            _newWndProc = new WndProcDelegate(HandleWindowMessage);
            MonitorActiveLabelReference = monitorActiveLabelReference;
        }

        public void StartMessageMonitoring()
        {
            if ( IsMonitoring ) return;

            // Store the original window procedure
            _originalWndProc = GetWindowLongPtr(_targetForm.Handle, -4);

            // Set up the new window procedure
            IntPtr newWndProcPtr = Marshal.GetFunctionPointerForDelegate(_newWndProc);
            SetWindowLongPtr(_targetForm.Handle, -4, newWndProcPtr);

            IsMonitoring = true;
        }

        public void StopMessageMonitoring()
        {
            if ( !IsMonitoring ) return;

            // Restore the original window procedure
            SetWindowLongPtr(_targetForm.Handle, -4, _originalWndProc);

            IsMonitoring = false;
        }

        private IntPtr HandleWindowMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            // Print message information
            string messageName = GetMessageName(msg);
            Console.WriteLine($"Window Message Received:");
            Console.WriteLine($"Message: {messageName} (0x{msg:X4})");
            Console.WriteLine($"wParam: 0x{wParam.ToInt64():X8}");
            Console.WriteLine($"lParam: 0x{lParam.ToInt64():X8}");
            Console.WriteLine(new string('-', 50));

            // Call the original window procedure
            return CallWindowProc(_originalWndProc, hWnd, msg, wParam, lParam);
        }

        private string GetMessageName(uint msg)
        {

            // Look it up in WinEnums.WM_MESSAGE
            string messageName = Enum.GetName(typeof(WM_MESSAGE), msg);
            return messageName ?? $"Unknown Message (0x{msg:X4})";

        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
    }
}