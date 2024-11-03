using System;
using System.Drawing;
using System.Windows.Forms;

namespace UartLedControl
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ShowIntroduction();
        }

        private void ShowIntroduction()
        {
            mainPanel.Controls.Clear();
            Label introLabel = new Label
            {
                Text = "Ứng dụng gọi Audio giữa Server và Client",
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 20, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = Color.White // Set background to white
            };
            mainPanel.Controls.Add(introLabel);
        }

        private void ShowCallInterface()
        {
            mainPanel.Controls.Clear();

            Button connectButton = new Button
            {
                Location = new Point(200, 50),
                Name = "connectButton",
                Size = new Size(120, 60),
                Text = "Connect",
                UseVisualStyleBackColor = false,
                BackColor = Color.FromArgb(50, 205, 50), // Lime Green
                Font = new Font("Arial", 14, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            connectButton.Anchor = AnchorStyles.Top;
            connectButton.MouseEnter += (s, e) => connectButton.BackColor = Color.Green;
            connectButton.MouseLeave += (s, e) => connectButton.BackColor = Color.FromArgb(50, 205, 50);
            connectButton.MouseDown += (s, e) => connectButton.BackColor = Color.DarkGreen; // Darker on click
            connectButton.MouseUp += (s, e) => connectButton.BackColor = Color.Green; // Return to original
            connectButton.Click += StartButton_Click;

            Button endButton = new Button
            {
                Location = new Point(350, 50),
                Name = "endButton",
                Size = new Size(120, 60),
                Text = "End Call",
                UseVisualStyleBackColor = false,
                BackColor = Color.FromArgb(255, 69, 0), // Red-Orange
                Font = new Font("Arial", 14, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            endButton.Anchor = AnchorStyles.Top;
            endButton.MouseEnter += (s, e) => endButton.BackColor = Color.Red;
            endButton.MouseLeave += (s, e) => endButton.BackColor = Color.FromArgb(255, 69, 0);
            endButton.MouseDown += (s, e) => endButton.BackColor = Color.DarkRed; // Darker on click
            endButton.MouseUp += (s, e) => endButton.BackColor = Color.Red; // Return to original
            endButton.Click += EndButton_Click;

            Label myLabel = new Label
            {
                Location = new Point(200, 130),
                Name = "myLabel",
                Size = new Size(270, 50),
                Text = "Status: Idle",
                Font = new Font("Arial", 16, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Black
            };
            myLabel.Anchor = AnchorStyles.Top;

            TrackBar volumeBar = new TrackBar
            {
                Location = new Point(200, 200),
                Name = "volumeBar",
                Size = new Size(270, 45),
                Minimum = 0,
                Maximum = 100,
                Value = 50,
                TickFrequency = 10,
                BackColor = Color.White,
                ForeColor = Color.Black
            };
            volumeBar.Anchor = AnchorStyles.Top;
            volumeBar.Scroll += VolumeBar_Scroll; // Event to handle volume adjustment

            // Label to display volume percentage
            Label volumeLabel = new Label
            {
                Location = new Point(480, 200),
                Name = "volumeLabel",
                Size = new Size(60, 45),
                Text = "50%",
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleLeft
            };
            volumeLabel.Anchor = AnchorStyles.Top;

            mainPanel.Controls.Add(connectButton);
            mainPanel.Controls.Add(endButton);
            mainPanel.Controls.Add(myLabel);
            mainPanel.Controls.Add(volumeBar);
            mainPanel.Controls.Add(volumeLabel); // Add the volume label to the panel
        }

        private void VolumeBar_Scroll(object? sender, EventArgs e)
        {
            // Update the volume label to reflect the current value of the volume bar
            if (sender is TrackBar volumeBar)
            {
                Label volumeLabel = (Label?)mainPanel.Controls["volumeLabel"];
                if (volumeLabel != null)
                {
                    volumeLabel.Text = $"{volumeBar.Value}%"; // Display current volume percentage
                }
            }
        }

        private void IntroductionButton_Click(object? sender, EventArgs e)
        {
            ShowIntroduction();
        }

        private void CallButton_Click(object? sender, EventArgs e)
        {
            ShowCallInterface();
        }

        private void ExitButton_Click(object? sender, EventArgs e)
        {
            Application.Exit();
        }

        private void StartButton_Click(object? sender, EventArgs e)
        {
            foreach (Control control in mainPanel.Controls)
            {
                if (control is Label label && label.Name == "myLabel")
                {
                    label.Text = "Status: Connecting...";
                    label.ForeColor = Color.Lime; // Change text color to Lime when connecting
                }
            }
        }

        private void EndButton_Click(object? sender, EventArgs e)
        {
            foreach (Control control in mainPanel.Controls)
            {
                if (control is Label label && label.Name == "myLabel")
                {
                    label.Text = "Status: Call Ended";
                    label.ForeColor = Color.Red; // Change text color to Red when ended
                }
            }
        }
    }
}
