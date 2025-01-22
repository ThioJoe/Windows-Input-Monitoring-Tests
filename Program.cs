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
        private Button startButton;
        private Button stopButton;
        private Label watcherActiveLabel;

        public RawInputWindow()
        {
            // Initialize components
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Initialize the buttons
            this.startButton = new Button();
            this.stopButton = new Button();
            this.watcherActiveLabel = new Label();

            // Start button properties
            this.startButton.Text = "Start";
            this.startButton.Location = new Point(50, 50);
            this.startButton.Click += StartButton_Click;

            // Stop button properties
            this.stopButton.Text = "Stop";
            this.stopButton.Location = new Point(150, 50);
            this.stopButton.Click += StopButton_Click;

            // Add buttons to the form
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.stopButton);

            // Add a label
            this.watcherActiveLabel.Text = "";
            this.watcherActiveLabel.Location = new Point(50, 75);
            this.Controls.Add(this.watcherActiveLabel);

            // Form properties
            this.Text = "Raw Input Window";
            this.Size = new Size(300, 150);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            // Initialize raw input handling
            RawInputHandler.InitializeRawInput(this.Handle, this.watcherActiveLabel);
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            // Clean up raw input handling
            RawInputHandler.CleanupInputWatcher();
        }
    }
}
