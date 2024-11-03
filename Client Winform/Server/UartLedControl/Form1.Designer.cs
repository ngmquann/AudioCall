namespace UartLedControl
{
    partial class Form1
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

        #region Windows Form Designer generated code

        private System.Windows.Forms.Button introductionButton;
        private System.Windows.Forms.Button callButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Panel sidePanel;
        private System.Windows.Forms.Panel mainPanel;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.introductionButton = new System.Windows.Forms.Button();
            this.callButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.sidePanel = new System.Windows.Forms.Panel();
            this.mainPanel = new System.Windows.Forms.Panel();

            // Set up the form properties
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "Audio Call Control";
            this.BackColor = Color.White; // Set background to white

            // 
            // sidePanel
            // 
            this.sidePanel.Dock = DockStyle.Left;
            this.sidePanel.Width = 200;
            this.sidePanel.BackColor = Color.LightGray; // Changed to lighter color for better contrast
            this.sidePanel.Controls.Add(this.introductionButton);
            this.sidePanel.Controls.Add(this.callButton);
            this.sidePanel.Controls.Add(this.exitButton);

            // 
            // introductionButton
            // 
            this.introductionButton.Location = new Point(20, 50);
            this.introductionButton.Name = "introductionButton";
            this.introductionButton.Size = new Size(160, 50);
            this.introductionButton.Text = "Introduction";
            this.introductionButton.UseVisualStyleBackColor = true;
            this.introductionButton.BackColor = Color.White;
            this.introductionButton.Font = new Font("Arial", 12, FontStyle.Bold);
            this.introductionButton.FlatStyle = FlatStyle.Flat; 
            this.introductionButton.Click += new System.EventHandler(this.IntroductionButton_Click);

            // 
            // callButton
            // 
            this.callButton.Location = new Point(20, 120);
            this.callButton.Name = "callButton";
            this.callButton.Size = new Size(160, 50);
            this.callButton.Text = "Call";
            this.callButton.UseVisualStyleBackColor = true;
            this.callButton.BackColor = Color.White;
            this.callButton.Font = new Font("Arial", 12, FontStyle.Bold);
            this.callButton.FlatStyle = FlatStyle.Flat; 
            this.callButton.Click += new System.EventHandler(this.CallButton_Click);

            // 
            // exitButton
            // 
            this.exitButton.Location = new Point(20, 190);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new Size(160, 50);
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.BackColor = Color.White;
            this.exitButton.Font = new Font("Arial", 12, FontStyle.Bold);
            this.exitButton.FlatStyle = FlatStyle.Flat; 
            this.exitButton.Click += new System.EventHandler(this.ExitButton_Click);

            // 
            // mainPanel
            // 
            this.mainPanel.Dock = DockStyle.Fill;
            this.mainPanel.BackColor = Color.White; // Set background to white

            // 
            // Form1
            // 
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.sidePanel);
            this.ResumeLayout(false);
        }

        #endregion
    }
}
