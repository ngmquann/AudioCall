using NAudio.Wave;
using System;
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Server
{
    public partial class Form1 : Form
    {
        private WaveIn waveIn;
        private WaveOut waveOut;
        private BufferedWaveProvider waveProvider;
        private TcpClient client;
        private TcpListener server;
        private NetworkStream stream;
        private bool isConnected = false;


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
            connectButton.Click += StartButton_Click;

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
            endButton.Click += EndButton_Click;

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

            
            mainPanel.Controls.Add(connectButton);
            mainPanel.Controls.Add(endButton);
            mainPanel.Controls.Add(statusLabel);
            mainPanel.Controls.Add(volumeBar);
            mainPanel.Controls.Add(volumeLabel);
        }
        private async void StartButton_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                Button connectButton = sender as Button;
                if (connectButton != null)
                    connectButton.Enabled = false;

                UpdateStatus("Connecting...", Color.Yellow);

                try
                {
                    await StartServerAsync();
                    waveIn.StartRecording();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Connection error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UpdateStatus("Connection failed", Color.Red);
                    if (connectButton != null)
                        connectButton.Enabled = true;
                }
            }
        }

        private void UpdateStatus(string message, Color color)
        {
            foreach (Control control in mainPanel.Controls)
            {
                if (control is Label label && label.Name == "statusLabel")
                {
                    label.Text = $"Status: {message}";
                    label.ForeColor = color;
                }
            }
        }

        private async Task StartServerAsync()
        {
            string address = GetWifiIPv4Address();
            try
            {
                server = new TcpListener(IPAddress.Any, 8000);
                server.Start();
                UpdateStatus("Waiting for incoming connection... \n Address: " + address, Color.Yellow);

                client = await Task.Run(() => server.AcceptTcpClient());
                stream = client.GetStream();

                SetupSuccessfulConnection();
                UpdateStatus("Client connected! Ready for call.", Color.Green);
            }
            catch (Exception)
            {
                if (server != null) server.Stop();
                throw;
            }
        }

        private string GetWifiIPv4Address()
        {
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && ni.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            return ip.Address.ToString();
                        }
                    }
                }
            }
            return "No Wi-Fi IPv4 address found";
        }


        private void SetupSuccessfulConnection()
        {
            isConnected = true;
            InitializeAudioDevices();
            StartReceivingAudio();
        }

        private void InitializeAudioDevices()
        {
            try
            {
                if (waveOut != null)
                {
                    waveOut.Dispose();
                }

                waveOut = new WaveOut();
                waveProvider = new BufferedWaveProvider(new WaveFormat(44100, 1));
                waveOut.Init(waveProvider);
                waveOut.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing audio devices: {ex.Message}", "Audio Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    UpdateStatus($"Audio sending error: {ex.Message}", Color.Red);
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
                                UpdateStatus($"Audio receiving error: {ex.Message}", Color.Red);
                            });
                        }
                        break;
                    }
                }
            });
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
            EndCall();
            Application.Exit();
        }

        private void EndButton_Click(object sender, EventArgs e)
        {
            EndCall();
            UpdateStatus("Call Ended", Color.Red);
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

            foreach (Control control in mainPanel.Controls)
            {
                if (control is Button button)
                {
                    button.Enabled = true;
                }
            }
        }
    }
}
