using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;

namespace Client
{
    public partial class Form1 : Form
    {
        private WaveIn waveIn;
        private WaveOut waveOut;
        private BufferedWaveProvider waveProvider;
        private TcpClient client;
        private TcpListener server;
        private NetworkStream stream;
        private bool isServer;
        private bool isConnected = false;
        private List<string> connectionHistory;

        public Form1()
        {
            InitializeComponent();
            connectionHistory = new List<string>();
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
            connectButton.Click += new EventHandler(this.ConnectButton_Click);

            Button endButton = new Button
            {
                Location = new Point(250, 100),
                Name = "endButton",
                Size = new Size(100, 50),
                Text = "End Call",
                UseVisualStyleBackColor = true,
                BackColor = Color.IndianRed,
                Font = new Font("Arial", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            endButton.Anchor = AnchorStyles.Top;
            endButton.MouseEnter += (s, e) => endButton.BackColor = Color.Red;
            endButton.MouseLeave += (s, e) => endButton.BackColor = Color.IndianRed;
            endButton.Click += new EventHandler(this.EndButton_Click);

            Label statusLabel = new Label
            {
                Location = new Point(100, 180),
                Name = "statusLabel",
                Size = new Size(250, 40),
                Text = "Status: Idle",
                Font = new Font("Arial", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.White
            };
            statusLabel.Anchor = AnchorStyles.Top;

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

            volumeBar.ValueChanged += (s, e) =>
            {
                volumeLabel.Text = volumeBar.Value + "%";
                if (waveOut != null)
                {
                    waveOut.Volume = volumeBar.Value / 100f;
                }
            };

            mainPanel.Controls.Add(ipAddressTextBox);
            mainPanel.Controls.Add(connectButton);
            mainPanel.Controls.Add(endButton);
            mainPanel.Controls.Add(statusLabel);
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
                BackColor = Color.FromArgb(47, 54, 64),
                ForeColor = Color.White
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

        private async void ConnectButton_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                var connectButton = mainPanel.Controls.Find("connectButton", false)[0] as Button;
                var statusLabel = mainPanel.Controls.Find("statusLabel", false)[0] as Label;
                var ipAddressTextBox = mainPanel.Controls.Find("ipAddressTextBox", false)[0] as TextBox;
                var endButton = mainPanel.Controls.Find("endButton", false)[0] as Button;

                connectButton.Enabled = false;
                statusLabel.Text = "Status: Connecting...";
                statusLabel.ForeColor = Color.Yellow;

                try
                {
                    await StartClientAsync(ipAddressTextBox.Text);
                    if (!string.IsNullOrWhiteSpace(ipAddressTextBox.Text) &&
                        !connectionHistory.Contains(ipAddressTextBox.Text))
                    {
                        connectionHistory.Add(ipAddressTextBox.Text);
                    }
                    waveIn.StartRecording();
                    endButton.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Connection error: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    statusLabel.Text = "Status: Connection failed";
                    statusLabel.ForeColor = Color.Red;
                    connectButton.Enabled = true;
                    return;
                }
            }
        }

        private async Task StartClientAsync(string serverIP)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(serverIP))
                {
                    throw new Exception("Please enter server IP address");
                }

                client = new TcpClient();
                await client.ConnectAsync(serverIP, 8000);
                stream = client.GetStream();

                SetupSuccessfulConnection();
                var statusLabel = mainPanel.Controls.Find("statusLabel", false)[0] as Label;
                statusLabel.Text = "Status: Connected";
                statusLabel.ForeColor = Color.Green;
            }
            catch (Exception)
            {
                if (client != null) client.Close();
                throw;
            }
        }

        private void SetupSuccessfulConnection()
        {
            isConnected = true;
            var ipAddressTextBox = mainPanel.Controls.Find("ipAddressTextBox", false)[0] as TextBox;
            ipAddressTextBox.Enabled = false;

            InitializeAudioDevices();
            StartReceivingAudio();
        }

        private void InitializeAudioDevices()
        {
            try
            {
                waveOut = new WaveOut();
                waveProvider = new BufferedWaveProvider(new WaveFormat(44100, 1));
                waveOut.Init(waveProvider);
                waveOut.Play();

                var volumeBar = mainPanel.Controls.Find("volumeBar", false)[0] as TrackBar;
                waveOut.Volume = volumeBar.Value / 100f;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing audio devices: {ex.Message}",
                    "Audio Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            try
            {
                if (stream != null && stream.CanWrite)
                {
                    stream.Write(e.Buffer, 0, e.BytesRecorded);
                }
            }
            catch (Exception ex)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    var statusLabel = mainPanel.Controls.Find("statusLabel", false)[0] as Label;
                    statusLabel.Text = $"Status: Audio sending error";
                    statusLabel.ForeColor = Color.Red;
                });
            }
        }

        private void StartReceivingAudio()
        {
            Task.Run(async () =>
            {
                byte[] buffer = new byte[4096];
                while (isConnected && stream != null)
                {
                    try
                    {
                        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        if (bytesRead > 0)
                        {
                            waveProvider.AddSamples(buffer, 0, bytesRead);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (isConnected)
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                var statusLabel = mainPanel.Controls.Find("statusLabel", false)[0] as Label;
                                statusLabel.Text = "Status: Audio receiving error";
                                statusLabel.ForeColor = Color.Red;
                            });
                        }
                        break;
                    }
                }
            });
        }

        private void EndButton_Click(object sender, EventArgs e)
        {
            EndCall();
        }

        private void EndCall()
        {
            isConnected = false;

            if (waveIn != null)
            {
                waveIn.StopRecording();
                waveIn.Dispose();
                waveIn = null;
            }

            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }

            if (stream != null)
            {
                stream.Close();
                stream = null;
            }

            if (client != null)
            {
                client.Close();
                client = null;
            }

            if (server != null)
            {
                server.Stop();
                server = null;
            }

            var connectButton = mainPanel.Controls.Find("connectButton", false)[0] as Button;
            var endButton = mainPanel.Controls.Find("endButton", false)[0] as Button;
            var ipAddressTextBox = mainPanel.Controls.Find("ipAddressTextBox", false)[0] as TextBox;
            var statusLabel = mainPanel.Controls.Find("statusLabel", false)[0] as Label;

            connectButton.Enabled = true;
            endButton.Enabled = false;
            ipAddressTextBox.Enabled = true;
            statusLabel.Text = "Status: Disconnected";
            statusLabel.ForeColor = Color.White;
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
    }
}