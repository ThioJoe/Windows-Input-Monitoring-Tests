namespace TestInputMonitoring;

partial class DetectKeysForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if ( disposing && (components != null) )
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.buttonStartDetection = new System.Windows.Forms.Button();
            this.buttonStopDetection = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonStartDetection
            // 
            this.buttonStartDetection.Location = new System.Drawing.Point(71, 76);
            this.buttonStartDetection.Name = "buttonStartDetection";
            this.buttonStartDetection.Size = new System.Drawing.Size(134, 40);
            this.buttonStartDetection.TabIndex = 0;
            this.buttonStartDetection.Text = "Start Detection";
            this.buttonStartDetection.UseVisualStyleBackColor = true;
            this.buttonStartDetection.Click += new System.EventHandler(this.buttonStartDetection_Click);
            // 
            // buttonStopDetection
            // 
            this.buttonStopDetection.Location = new System.Drawing.Point(254, 76);
            this.buttonStopDetection.Name = "buttonStopDetection";
            this.buttonStopDetection.Size = new System.Drawing.Size(134, 40);
            this.buttonStopDetection.TabIndex = 1;
            this.buttonStopDetection.Text = "Stop Detection";
            this.buttonStopDetection.UseVisualStyleBackColor = true;
            this.buttonStopDetection.Click += new System.EventHandler(this.buttonStopDetection_Click);
            // 
            // DetectKeysForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 318);
            this.Controls.Add(this.buttonStopDetection);
            this.Controls.Add(this.buttonStartDetection);
            this.Name = "DetectKeysForm";
            this.Text = "DetectKeysForm";
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonStartDetection;
    private System.Windows.Forms.Button buttonStopDetection;
}