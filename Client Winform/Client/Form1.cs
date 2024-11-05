using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        private List<string> connectionHistory;

        public Form1()
        {
            InitializeComponent();
            connectionHistory = new List<string>();
            ShowIntroduction();
        }

        private void ShowIntroduction()
        {
            mainPanel.Controls.Clear();
            Label introLabel = new Label
            {
                Text = "Ứng dụng gọi Audio giữa Client và Server",
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(47, 54, 64) 
            };
            mainPanel.Controls.Add(introLabel);
        }

        private void ShowCallInterface()
        {
            mainPanel.Controls.Clear();

            TextBox ipAddressTextBox = new TextBox
            {
                Location = new Point(100, 50),
                Name = "ipAddressTextBox",
                Size = new Size(250, 30),
                Font = new Font("Arial", 12),
                ForeColor = Color.Gray,
                Text = "Nhập địa chỉ IP của Server"
            };

            ipAddressTextBox.GotFocus += (s, e) =>
            {
                if (ipAddressTextBox.Text == "Nhập địa chỉ IP của Server")
                {
                    ipAddressTextBox.Text = "";
                    ipAddressTextBox.ForeColor = Color.Black;
                }
            };

            ipAddressTextBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(ipAddressTextBox.Text))
                {
                    ipAddressTextBox.Text = "Nhập địa chỉ IP của Server";
                    ipAddressTextBox.ForeColor = Color.Gray;
                }
            };

            Button connectButton = new Button
            {
                Location = new Point(100, 100),
                Name = "connectButton",
                Size = new Size(100, 50),
                Text = "Connect",
                UseVisualStyleBackColor = true,
                BackColor = Color.LightGreen,
                Font = new Font("Arial", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            connectButton.Anchor = AnchorStyles.Top;
            connectButton.MouseEnter += (s, e) => connectButton.BackColor = Color.Green;
            connectButton.MouseLeave += (s, e) => connectButton.BackColor = Color.LightGreen;
            connectButton.Click += new EventHandler(this.StartButton_Click);

            Button endButton = new Button
            {
                Location = new Point(250, 100),
                Name = "endButton",
                Size = new Size(100, 50),
                Text = "End Call",
                UseVisualStyleBackColor = true,
                BackColor = Color.IndianRed,
                Font = new Font("Arial", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            endButton.Anchor = AnchorStyles.Top;
            endButton.MouseEnter += (s, e) => endButton.BackColor = Color.Red;
            endButton.MouseLeave += (s, e) => endButton.BackColor = Color.IndianRed;
            endButton.Click += new EventHandler(this.EndButton_Click);

            Label myLabel = new Label
            {
                Location = new Point(100, 180),
                Name = "myLabel",
                Size = new Size(250, 40),
                Text = "Status: Idle",
                Font = new Font("Arial", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.White
            };
            myLabel.Anchor = AnchorStyles.Top;

            TrackBar volumeBar = new TrackBar
            {
                Location = new Point(100, 240),
                Name = "volumeBar",
                Size = new Size(250, 45),
                Minimum = 0,
                Maximum = 100,
                Value = 50,
                TickFrequency = 10,
                BackColor = Color.FromArgb(47, 54, 64) 
            };
            volumeBar.Anchor = AnchorStyles.Top;

            Label volumeLabel = new Label
            {
                Location = new Point(360, 240),
                Name = "volumeLabel",
                Size = new Size(50, 30),
                Text = volumeBar.Value + "%",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft
            };
            volumeBar.Scroll += (s, e) => volumeLabel.Text = volumeBar.Value + "%";

            mainPanel.Controls.Add(ipAddressTextBox);
            mainPanel.Controls.Add(connectButton);
            mainPanel.Controls.Add(endButton);
            mainPanel.Controls.Add(myLabel);
            mainPanel.Controls.Add(volumeBar);
            mainPanel.Controls.Add(volumeLabel);
        }


        private void ShowHistory()
        {
            mainPanel.Controls.Clear();
            ListBox historyListBox = new ListBox
            {
                Location = new Point(100, 50),
                Name = "historyListBox",
                Size = new Size(400, 300),
                Font = new Font("Arial", 12),
                BackColor = Color.Black 
            };

            foreach (var ip in connectionHistory)
            {
                historyListBox.Items.Add(ip);
            }

            Button backButton = new Button
            {
                Location = new Point(100, 370),
                Name = "backButton",
                Size = new Size(100, 50),
                Text = "Back",
                UseVisualStyleBackColor = true,
                BackColor = Color.LightGray,
                Font = new Font("Arial", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            backButton.Click += (s, e) => ShowCallInterface();

            mainPanel.Controls.Add(historyListBox);
            mainPanel.Controls.Add(backButton);
        }

        private void IntroductionButton_Click(object sender, EventArgs e)
        {
            ShowIntroduction();
        }

        private void CallButton_Click(object sender, EventArgs e)
        {
            ShowCallInterface();
        }

        private void HistoryButton_Click(object sender, EventArgs e)
        {
            ShowHistory();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            string ipAddress = string.Empty;
            foreach (Control control in mainPanel.Controls)
            {
                if (control is TextBox textBox && textBox.Name == "ipAddressTextBox")
                {
                    ipAddress = textBox.Text;
                }
                if (control is Label label && label.Name == "myLabel")
                {
                    label.Text = "Status: Connecting...";
                    label.ForeColor = Color.Green;
                }
            }

            if (!string.IsNullOrWhiteSpace(ipAddress) && !connectionHistory.Contains(ipAddress))
            {
                connectionHistory.Add(ipAddress);
            }
        }

        private void EndButton_Click(object sender, EventArgs e)
        {
            foreach (Control control in mainPanel.Controls)
            {
                if (control is Label label && label.Name == "myLabel")
                {
                    label.Text = "Status: Call Ended";
                    label.ForeColor = Color.Red;
                }
            }
        }
    }
}