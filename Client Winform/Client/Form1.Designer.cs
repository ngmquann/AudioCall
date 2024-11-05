using System.Drawing;
using System.Windows.Forms;

namespace Client
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

        private System.Windows.Forms.Button introductionButton;
        private System.Windows.Forms.Button callButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button historyButton;
        private System.Windows.Forms.Panel sidePanel;
        private System.Windows.Forms.Panel mainPanel;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.introductionButton = new System.Windows.Forms.Button();
            this.callButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.historyButton = new System.Windows.Forms.Button();
            this.sidePanel = new System.Windows.Forms.Panel();
            this.mainPanel = new System.Windows.Forms.Panel();

            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "Audio Call Control - Client";
            this.BackColor = Color.FromArgb(242, 242, 242);

            this.sidePanel.Dock = DockStyle.Left;
            this.sidePanel.Width = 200;
            this.sidePanel.BackColor = Color.FromArgb(153, 102, 204);
            this.sidePanel.Controls.Add(this.introductionButton);
            this.sidePanel.Controls.Add(this.callButton);
            this.sidePanel.Controls.Add(this.historyButton);
            this.sidePanel.Controls.Add(this.exitButton);

            Bitmap ResizeIcon(string path, int width, int height)
            {
                Bitmap originalIcon = new Bitmap(path);
                return new Bitmap(originalIcon, new Size(width, height));
            }

            this.introductionButton.Image = ResizeIcon("icons/intro.png", 24, 24);
            this.introductionButton.ImageAlign = ContentAlignment.MiddleLeft;
            this.introductionButton.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.introductionButton.Location = new Point(20, 50);
            this.introductionButton.Name = "introductionButton";
            this.introductionButton.Size = new Size(160, 50);
            this.introductionButton.Text = "Introduction";
            this.introductionButton.TextAlign = ContentAlignment.MiddleRight;
            this.introductionButton.BackColor = Color.White;
            this.introductionButton.Font = new Font("Arial", 12, FontStyle.Bold);
            this.introductionButton.FlatStyle = FlatStyle.Flat;
            this.introductionButton.Click += new System.EventHandler(this.IntroductionButton_Click);

            this.callButton.Image = ResizeIcon("icons/call.png", 24, 24);
            this.callButton.ImageAlign = ContentAlignment.MiddleLeft;
            this.callButton.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.callButton.Location = new Point(20, 120);
            this.callButton.Name = "callButton";
            this.callButton.Size = new Size(160, 50);
            this.callButton.Text = "Call";
            this.callButton.TextAlign = ContentAlignment.MiddleRight;
            this.callButton.UseVisualStyleBackColor = true;
            this.callButton.BackColor = Color.White;
            this.callButton.Font = new Font("Arial", 12, FontStyle.Bold);
            this.callButton.FlatStyle = FlatStyle.Flat;
            this.callButton.Click += new System.EventHandler(this.CallButton_Click);

            this.historyButton.Image = ResizeIcon("icons/history.png", 24, 24);
            this.historyButton.ImageAlign = ContentAlignment.MiddleLeft;
            this.historyButton.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.historyButton.Location = new Point(20, 190);
            this.historyButton.Name = "historyButton";
            this.historyButton.Size = new Size(160, 50);
            this.historyButton.Text = "History";
            this.historyButton.TextAlign = ContentAlignment.MiddleRight;
            this.historyButton.UseVisualStyleBackColor = true;
            this.historyButton.BackColor = Color.White;
            this.historyButton.Font = new Font("Arial", 12, FontStyle.Bold);
            this.historyButton.FlatStyle = FlatStyle.Flat;
            this.historyButton.Click += new System.EventHandler(this.HistoryButton_Click);

            this.exitButton.Image = ResizeIcon("icons/exit.png", 24, 24);
            this.exitButton.ImageAlign = ContentAlignment.MiddleLeft;
            this.exitButton.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.exitButton.Location = new Point(20, 260);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new Size(160, 50);
            this.exitButton.Text = "Exit";
            this.exitButton.TextAlign = ContentAlignment.MiddleRight;
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.BackColor = Color.White;
            this.exitButton.Font = new Font("Arial", 12, FontStyle.Bold);
            this.exitButton.FlatStyle = FlatStyle.Flat;
            this.exitButton.Click += new System.EventHandler(this.ExitButton_Click);

            this.mainPanel.Dock = DockStyle.Fill;
            this.mainPanel.BackColor = Color.FromArgb(47, 54, 64);

            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.sidePanel);

            this.ResumeLayout(false);
        }
    }
}