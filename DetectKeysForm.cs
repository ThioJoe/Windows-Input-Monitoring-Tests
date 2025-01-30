using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestInputMonitoring
{
    public partial class DetectKeysForm: Form
    {
        DetectKeys detectKeys = new DetectKeys();

        public DetectKeysForm()
        {
            InitializeComponent();
        }

        private void buttonStartDetection_Click(object sender, EventArgs e)
        {
            detectKeys.Start();
        }

        private void buttonStopDetection_Click(object sender, EventArgs e)
        {
            detectKeys.Stop();
        }
    }
}
