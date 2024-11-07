using System.Drawing;
using System.Security.Cryptography.X509Certificates;
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
            this.introductionButton = new System.Windows.Forms.Button();
            this.callButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.historyButton = new System.Windows.Forms.Button();
            this.sidePanel = new System.Windows.Forms.Panel();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.sidePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // introductionButton
            // 
            this.introductionButton.BackColor = System.Drawing.Color.White;
            this.introductionButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.introductionButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.introductionButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.introductionButton.Location = new System.Drawing.Point(20, 50);
            this.introductionButton.Name = "introductionButton";
            this.introductionButton.Size = new System.Drawing.Size(160, 50);
            this.introductionButton.TabIndex = 0;
            this.introductionButton.Text = "Introduction";
            this.introductionButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.introductionButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.introductionButton.UseVisualStyleBackColor = false;
            this.introductionButton.Click += new System.EventHandler(this.IntroductionButton_Click);
            // 
            // callButton
            // 
            this.callButton.BackColor = System.Drawing.Color.White;
            this.callButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.callButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.callButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.callButton.Location = new System.Drawing.Point(20, 120);
            this.callButton.Name = "callButton";
            this.callButton.Size = new System.Drawing.Size(160, 50);
            this.callButton.TabIndex = 1;
            this.callButton.Text = "Call";
            this.callButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.callButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.callButton.UseVisualStyleBackColor = false;
            this.callButton.Click += new System.EventHandler(this.CallButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.BackColor = System.Drawing.Color.White;
            this.exitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exitButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.exitButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.exitButton.Location = new System.Drawing.Point(20, 260);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(160, 50);
            this.exitButton.TabIndex = 3;
            this.exitButton.Text = "Exit";
            this.exitButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.exitButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.exitButton.UseVisualStyleBackColor = false;
            this.exitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // historyButton
            // 
            this.historyButton.BackColor = System.Drawing.Color.White;
            this.historyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.historyButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.historyButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.historyButton.Location = new System.Drawing.Point(20, 190);
            this.historyButton.Name = "historyButton";
            this.historyButton.Size = new System.Drawing.Size(160, 50);
            this.historyButton.TabIndex = 2;
            this.historyButton.Text = "History";
            this.historyButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.historyButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.historyButton.UseVisualStyleBackColor = false;
            this.historyButton.Click += new System.EventHandler(this.HistoryButton_Click);
            // 
            // sidePanel
            // 
            this.sidePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.sidePanel.Controls.Add(this.introductionButton);
            this.sidePanel.Controls.Add(this.callButton);
            this.sidePanel.Controls.Add(this.historyButton);
            this.sidePanel.Controls.Add(this.exitButton);
            this.sidePanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidePanel.Location = new System.Drawing.Point(0, 0);
            this.sidePanel.Name = "sidePanel";
            this.sidePanel.Size = new System.Drawing.Size(200, 450);
            this.sidePanel.TabIndex = 1;
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(54)))), ((int)(((byte)(64)))));
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(200, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(600, 450);
            this.mainPanel.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.sidePanel);
            this.Name = "Form1";
            this.Text = "Audio Call Control - Client";
            this.sidePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private static Bitmap ResizeIcon(string path, int width, int height)
        {
            using (Bitmap originalIcon = new Bitmap(path))
            {
                return new Bitmap(originalIcon, new Size(width, height));
            }
        }

        private void SetButtonIcons()
        {
            this.introductionButton.Image = ResizeIcon("icons/intro.png", 24, 24);
            this.callButton.Image = ResizeIcon("icons/call.png", 24, 24);
            this.historyButton.Image = ResizeIcon("icons/history.png", 24, 24);
            this.exitButton.Image = ResizeIcon("icons/exit.png", 24, 24);
        }
    }
}