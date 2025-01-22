using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

#nullable enable

namespace TestRawInput
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
        private Button startKeyboardHookButton;
        private Button stopKeyboardHookButton;
        private Label rawInputActiveLabel;

        public RawInputWindow()
        {
            // Initialize components
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Initialize the buttons
            this.startRawInputButton = new Button();
            this.stopRawInputButton = new Button();
            this.rawInputActiveLabel = new Label();
            this.startKeyboardHookButton = new Button();
            this.stopKeyboardHookButton = new Button();

            // Start raw input button properties
            this.startRawInputButton.Text = "Start Raw";
            this.startRawInputButton.Location = new Point(50, 50);
            this.startRawInputButton.Click += StartButton_Click;

            // Stop raw input button properties
            this.stopRawInputButton.Text = "Stop Raw";
            this.stopRawInputButton.Location = new Point(150, 50);
            this.stopRawInputButton.Click += StopButton_Click;

            // Start keyboard hook button properties
            this.startKeyboardHookButton.Text = "Start Hook";
            this.startKeyboardHookButton.Location = new Point(50, 100);
            this.startKeyboardHookButton.Click += StartKeyboardHookButton_Click;

            // Stop keyboard hook button properties
            this.stopKeyboardHookButton.Text = "Stop Hook";
            this.stopKeyboardHookButton.Location = new Point(150, 100);
            this.stopKeyboardHookButton.Click += StopKeyboardHookButton_Click;

            // Add buttons to the form
            this.Controls.Add(this.startRawInputButton);
            this.Controls.Add(this.stopRawInputButton);
            this.Controls.Add(this.startKeyboardHookButton);
            this.Controls.Add(this.stopKeyboardHookButton);

            // Add a label
            this.rawInputActiveLabel.Text = "";
            this.rawInputActiveLabel.Location = new Point(75, 125);
            this.Controls.Add(this.rawInputActiveLabel);

            // Form properties
            this.Text = "Raw Input Window";
            this.Size = new Size(300, 300);
        }

        private void StartKeyboardHookButton_Click(object sender, EventArgs e)
        {
            // Initialize keyboard hook handling
            LowLevelKeyboardHookHandler.InitializeKeyboardHook(this.rawInputActiveLabel);
        }

        private void StopKeyboardHookButton_Click(object sender, EventArgs e)
        {
            // Clean up keyboard hook handling
            LowLevelKeyboardHookHandler.StopHook();
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
}
