namespace PhoneTracer
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnLoadFile = new System.Windows.Forms.Button();
            this.btnStartTracing = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblHotkeys = new System.Windows.Forms.Label();

            // Form settings
            this.Text = "Phone Number Tracer";
            this.Size = new System.Drawing.Size(400, 300);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Load File button
            this.btnLoadFile.Location = new System.Drawing.Point(20, 20);
            this.btnLoadFile.Size = new System.Drawing.Size(100, 30);
            this.btnLoadFile.Text = "Load File";
            this.btnLoadFile.Click += new System.EventHandler(this.btnLoadFile_Click);

            // Start Tracing button
            this.btnStartTracing.Location = new System.Drawing.Point(130, 20);
            this.btnStartTracing.Size = new System.Drawing.Size(100, 30);
            this.btnStartTracing.Text = "Start Tracing";
            this.btnStartTracing.Enabled = false;
            this.btnStartTracing.Click += (s, e) => StartTracing();

            // Status label
            this.lblStatus.Location = new System.Drawing.Point(20, 70);
            this.lblStatus.Size = new System.Drawing.Size(360, 60);
            this.lblStatus.Text = "Ready to load phone numbers...";

            // Hotkeys label - Updated to show Alt instead of Ctrl
            this.lblHotkeys.Location = new System.Drawing.Point(20, 140);
            this.lblHotkeys.Size = new System.Drawing.Size(360, 100);
            this.lblHotkeys.Text = "Hotkeys:\n" +
                                "Alt+O: Start Tracing\n" +
                                "Alt+H: Pause Tracing\n" +
                                "Alt+R: Restart Tracing";

            // Add controls to form
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.btnLoadFile,
                this.btnStartTracing,
                this.lblStatus,
                this.lblHotkeys
            });
        }

        private System.Windows.Forms.Button btnLoadFile;
        private System.Windows.Forms.Button btnStartTracing;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblHotkeys;
    }
}