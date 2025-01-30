using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

#nullable enable

namespace TestInputMonitoring
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Enable visual styles and run the application
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new RawInputWindow());
        }
    }

    // Basic Windows form window with Start and Stop buttons
    public class RawInputWindow : Form
    {
        private Button startRawInputButton;
        private Button stopRawInputButton;

        private Button startLLKeyboardHookButton;
        private Button stopLLKeyboardHookButton;

        private Button startKeyboardHookButton;
        private Button stopKeyboardHookButton;

        private Button startMonitorMessages;
        private Button stopMonitorMessages;

        private Label rawInputActiveLabel;
        private Label LLKeyboardHookActiveLabel;
        private Label KeyboardHookActiveLabel;
        private Label MonitorActiveLabel;

        private IncomingMessageHandler msgHandler;
        private LowLevelKeyboardHookHandler mainLowLevelKeyboardHookHandler;

        public RawInputWindow()
        {
            // Initialize components
            InitializeComponent();
            msgHandler = new IncomingMessageHandler(this, this.MonitorActiveLabel);
            mainLowLevelKeyboardHookHandler = new LowLevelKeyboardHookHandler();
        }

        private void InitializeComponent()
        {
            // Labels
            this.rawInputActiveLabel = new Label();
            this.LLKeyboardHookActiveLabel = new Label();
            this.KeyboardHookActiveLabel = new Label();
            this.MonitorActiveLabel = new Label();

            // Initialize the buttons
            this.startRawInputButton = new Button();
            this.stopRawInputButton = new Button();

            this.startLLKeyboardHookButton = new Button();
            this.stopLLKeyboardHookButton = new Button();

            this.startKeyboardHookButton = new Button();
            this.stopKeyboardHookButton = new Button();

            this.startMonitorMessages = new Button();
            this.stopMonitorMessages = new Button();

            // ----------------------------------------

            int btnYStart = 20;
            int vSpacing = 35;

            int xMargin = 20;
            int buttonWidth = 150;

            int labelSpacing = 20;
            int labelX = 120;

            int leftButtonX = xMargin;
            int rightButtonX = xMargin + buttonWidth + 20;
            int windowWidth = rightButtonX+buttonWidth+xMargin;

            // Start raw input button properties
            this.startRawInputButton.Text = "Start Raw Input";
            this.startRawInputButton.Location = new Point(leftButtonX, btnYStart);
            this.startRawInputButton.Click += StartButton_Click;
            this.startRawInputButton.Width = buttonWidth;

            // Stop raw input button properties
            this.stopRawInputButton.Text = "Stop Raw Input";
            this.stopRawInputButton.Location = new Point(rightButtonX, btnYStart);
            this.stopRawInputButton.Click += StopButton_Click;
            this.stopRawInputButton.Width = buttonWidth;
            int prevY = btnYStart;

            // Start low level keyboard hook button properties
            this.startLLKeyboardHookButton.Text = "Start Low-Level KB Hook";
            this.startLLKeyboardHookButton.Location = new Point(leftButtonX, prevY+vSpacing);
            this.startLLKeyboardHookButton.Click += StartLLKeyboardHookButton_Click;
            this.startLLKeyboardHookButton.Width = buttonWidth;

            // Stop low level keyboard hook button properties
            this.stopLLKeyboardHookButton.Text = "Stop Low-Level KB Hook";
            this.stopLLKeyboardHookButton.Location = new Point(rightButtonX, prevY + vSpacing);
            this.stopLLKeyboardHookButton.Click += StopLLKeyboardHookButton_Click;
            this.stopLLKeyboardHookButton.Width = buttonWidth;
            prevY += vSpacing;

            // Start regular keyboard hook button properties
            this.startKeyboardHookButton.Text = "Start KB Hook";
            this.startKeyboardHookButton.Location = new Point(leftButtonX, prevY + vSpacing);
            this.startKeyboardHookButton.Click += StartKeyboardHookButton_Click;
            this.startKeyboardHookButton.Width = buttonWidth;

            // Stop regular keyboard hook button properties
            this.stopKeyboardHookButton.Text = "Stop KB Hook";
            this.stopKeyboardHookButton.Location = new Point(rightButtonX, prevY + vSpacing);
            this.stopKeyboardHookButton.Click += StopKeyboardHookButton_Click;
            this.stopKeyboardHookButton.Width = buttonWidth;
            prevY += vSpacing;

            // Start monitoring messages
            this.startMonitorMessages.Text = "Start WM Monitor";
            this.startMonitorMessages.Location = new Point(leftButtonX, prevY + vSpacing);
            this.startMonitorMessages.Click += StartMonitorMessages_Click;
            this.startMonitorMessages.Width = buttonWidth;

            // Stop monitoring messages
            this.stopMonitorMessages.Text = "Stop WM Monitor";
            this.stopMonitorMessages.Location = new Point(rightButtonX, prevY + vSpacing);
            this.stopMonitorMessages.Click += StopMonitorMessages_Click;
            this.stopMonitorMessages.Width = buttonWidth;
            prevY += vSpacing;

            // Add buttons to the form
            this.Controls.Add(this.startRawInputButton);
            this.Controls.Add(this.stopRawInputButton);
            this.Controls.Add(this.startLLKeyboardHookButton);
            this.Controls.Add(this.stopLLKeyboardHookButton);
            this.Controls.Add(this.startKeyboardHookButton);
            this.Controls.Add(this.stopKeyboardHookButton);
            this.Controls.Add(this.startMonitorMessages);
            this.Controls.Add(this.stopMonitorMessages);

            // Extra space for labels
            prevY += 20;

            // Set the labels
            this.rawInputActiveLabel.Text = LabelStrings.RawInputInactive;
            this.rawInputActiveLabel.Location = new Point(labelX, prevY+ labelSpacing);
            this.rawInputActiveLabel.AutoSize = true;
            this.rawInputActiveLabel.ForeColor = LabelColors.InactiveColor;
            prevY += labelSpacing;

            this.LLKeyboardHookActiveLabel.Text = LabelStrings.LLKeyboardHookInactive;
            this.LLKeyboardHookActiveLabel.Location = new Point(labelX, prevY + labelSpacing);
            this.LLKeyboardHookActiveLabel.AutoSize = true;
            this.LLKeyboardHookActiveLabel.ForeColor = LabelColors.InactiveColor;
            prevY += labelSpacing;

            this.KeyboardHookActiveLabel.Text = LabelStrings.KeyboardHookInactive;
            this.KeyboardHookActiveLabel.Location = new Point(labelX, prevY + labelSpacing);
            this.KeyboardHookActiveLabel.AutoSize = true;
            this.KeyboardHookActiveLabel.ForeColor = LabelColors.InactiveColor;
            prevY += labelSpacing;

            this.MonitorActiveLabel.Text = LabelStrings.MonitorInactive;
            this.MonitorActiveLabel.Location = new Point(labelX, prevY + labelSpacing);
            this.MonitorActiveLabel.AutoSize = true;
            this.MonitorActiveLabel.ForeColor = LabelColors.InactiveColor;
            prevY += labelSpacing;

            this.Controls.Add(this.rawInputActiveLabel);
            this.Controls.Add(this.LLKeyboardHookActiveLabel);
            this.Controls.Add(this.KeyboardHookActiveLabel);
            this.Controls.Add(this.MonitorActiveLabel);

            // Form properties
            this.Text = "Raw Input Window";
            this.Size = new Size(windowWidth + 16, 300); // Account for window border
        }

        private void StartMonitorMessages_Click(object sender, EventArgs e)
        {
            // Start monitoring messages
            msgHandler.StartMessageMonitoring();
        }

        private void StopMonitorMessages_Click(object sender, EventArgs e)
        {
            // Stop monitoring messages
            msgHandler.StopMessageMonitoring();
        }

        private void StartKeyboardHookButton_Click(object sender, EventArgs e)
        {
            // Initialize keyboard hook handling
            KeyboardHookHandler.InitializeKeyboardHook(this.KeyboardHookActiveLabel);
        }

        private void StopKeyboardHookButton_Click(object sender, EventArgs e)
        {
            // Clean up keyboard hook handling
            KeyboardHookHandler.StopHook();
        }

        private void StartLLKeyboardHookButton_Click(object sender, EventArgs e)
        {
            // Initialize keyboard hook handling
            mainLowLevelKeyboardHookHandler.InitializeKeyboardHook(this.LLKeyboardHookActiveLabel);
        }

        private void StopLLKeyboardHookButton_Click(object sender, EventArgs e)
        {
            // Clean up keyboard hook handling
            mainLowLevelKeyboardHookHandler.StopHook();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            // Initialize raw input handling
            RawInputHandler.InitializeRawInput(this.Handle, this.rawInputActiveLabel);
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            // Clean up raw input handling
            RawInputHandler.CleanupInputWatcher();
        }
    }

    // Class to store strings
    public static class LabelStrings
    {
        public const string RawInputActive = "RawInput Watcher: Started";
        public const string RawInputInactive = "RawInput Watcher: Stopped";

        public const string LLKeyboardHookActive = "Low-Level Hook: Started";
        public const string LLKeyboardHookInactive = "Low-Level Hook: Stopped";

        public const string KeyboardHookActive = "Keyboard Hook: Started";
        public const string KeyboardHookInactive = "Keyboard Hook: Stopped";

        public const string MonitorActive = "Monitoring Messages: Started";
        public const string MonitorInactive = "Monitoring Messages: Stopped";
    }

    public static class LabelColors
    {
        public static Color ActiveColor = Color.Green;
        public static Color InactiveColor = Color.Red;
    }
}
