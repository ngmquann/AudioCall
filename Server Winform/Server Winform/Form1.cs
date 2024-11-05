using System;
using System.Drawing;
using System.Windows.Forms;


namespace Server
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            InitializeAudio();
            ShowIntroduction();
        }

        private void InitializeAudio()
        {
            waveIn = new WaveIn();
            waveIn.WaveFormat = new WaveFormat(44100, 1);
            waveIn.DataAvailable += WaveIn_DataAvailable;
            waveIn.BufferMilliseconds = 50;
        }


        private void ShowIntroduction()
        {
            mainPanel.Controls.Clear();
            Label introLabel = new Label
            {
                Text = "Ứng dụng gọi Audio giữa Server và Client (Server)",
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Black
            };
            mainPanel.Controls.Add(introLabel);
        }

        private void ShowCallInterface()
        {
            mainPanel.Controls.Clear();

            Button connectButton = new Button
            {
                Location = new Point(200, 50),
                Size = new Size(100, 50),
                Text = "Connect",
                BackColor = Color.LightGreen,
                Font = new Font("Arial", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            connectButton.Anchor = AnchorStyles.Top;
            connectButton.MouseEnter += (s, e) => connectButton.BackColor = Color.Green;
            connectButton.MouseLeave += (s, e) => connectButton.BackColor = Color.LightGreen;
            connectButton.Click += StartButton_Click;

            Button endButton = new Button
            {
                Location = new Point(350, 50),
                Size = new Size(100, 50),
                Text = "End Call",
                BackColor = Color.IndianRed,
                Font = new Font("Arial", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            endButton.Anchor = AnchorStyles.Top;
            endButton.MouseEnter += (s, e) => endButton.BackColor = Color.Red;
            endButton.MouseLeave += (s, e) => endButton.BackColor = Color.IndianRed;
            endButton.Click += EndButton_Click;

            Label statusLabel = new Label
            {
                Location = new Point(170, 120),
                Name = "statusLabel",
                Size = new Size(350, 50),
                Text = "Status: Idle",
                Font = new Font("Arial", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.White
            };
            statusLabel.Anchor = AnchorStyles.Top;

            TrackBar volumeBar = new TrackBar
            {
                Location = new Point(200, 210),
                Size = new Size(250, 45),
                Minimum = 0,
                Maximum = 100,
                Value = 50,
                TickFrequency = 10,
                BackColor = Color.Black
            };
            volumeBar.Anchor = AnchorStyles.Top;

            mainPanel.Controls.Add(connectButton);
            mainPanel.Controls.Add(endButton);
            mainPanel.Controls.Add(statusLabel);
            mainPanel.Controls.Add(volumeBar);
        }

        private void IntroductionButton_Click(object sender, EventArgs e)
        {
            ShowIntroduction();
        }

        private void CallButton_Click(object sender, EventArgs e)
        {
            ShowCallInterface();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            foreach (Control control in mainPanel.Controls)
            {
                if (control is Label label && label.Name == "myLabel")
                {
                    label.Text = "Status: Connecting...";
                    label.ForeColor = Color.Green;
                }
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
