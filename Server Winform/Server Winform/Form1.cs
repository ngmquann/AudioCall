using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
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
        private bool isMicMuted = false;
        private CancellationTokenSource cancellationTokenSource;
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

            Button muteButton = new Button
            {
                Location = new Point(175, 170),
                Name = "muteButton",
                Size = new Size(100, 40),
                Text = "Mute Mic",
                UseVisualStyleBackColor = true,
                BackColor = Color.LightGray,
                Font = new Font("Arial", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            muteButton.Anchor = AnchorStyles.Top;
            muteButton.Click += MuteButton_Click;

            Label statusLabel = new Label
            {
                Location = new Point(50, 230),
                Name = "statusLabel",
                Size = new Size(360, 70),
                Text = "Status: Idle",
                Font = new Font("Arial", 13, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.White
            };
            statusLabel.Anchor = AnchorStyles.Top;

            TrackBar volumeBar = new TrackBar
            {
                Location = new Point(100, 310),
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
                Location = new Point(360, 310),
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
            mainPanel.Controls.Add(muteButton);
            mainPanel.Controls.Add(statusLabel);
            mainPanel.Controls.Add(volumeBar);
            mainPanel.Controls.Add(volumeLabel);
        }

        private void AddToConnectionHistory(TcpClient client)
        {
            if (client?.Client?.RemoteEndPoint != null)
            {
                string clientAddress = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
                string connectionInfo = $"{clientAddress} - {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}";

                // Add to history if it's not already there
                if (!connectionHistory.Contains(connectionInfo))
                {
                    connectionHistory.Add(connectionInfo);
                }

                // Keep only the last 10 connections
                if (connectionHistory.Count > 10)
                {
                    connectionHistory.RemoveAt(0);
                }
            }
        }

        private void ShowHistory()
        {
            mainPanel.Controls.Clear();

            // Create title label
            Label titleLabel = new Label
            {
                Text = "Connection History",
                Location = new Point(50, 20),
                Size = new Size(500, 30),
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Create history list box
            ListBox historyListBox = new ListBox
            {
                Location = new Point(50, 60),
                Size = new Size(500, 300),
                Font = new Font("Arial", 12),
                BackColor = Color.FromArgb(47, 54, 64),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Add items to history list box
            if (connectionHistory.Count == 0)
            {
                historyListBox.Items.Add("No connection history available");
            }
            else
            {
                foreach (string connection in connectionHistory)
                {
                    historyListBox.Items.Add(connection);
                }
            }

            // Create clear history button
            Button clearButton = new Button
            {
                Text = "Clear History",
                Location = new Point(50, 370),
                Size = new Size(150, 40),
                Font = new Font("Arial", 12, FontStyle.Bold),
                BackColor = Color.IndianRed,
                FlatStyle = FlatStyle.Flat
            };
            clearButton.Click += (s, e) =>
            {
                connectionHistory.Clear();
                historyListBox.Items.Clear();
                historyListBox.Items.Add("No connection history available");
            };

            // Create back button
            Button backButton = new Button
            {
                Text = "Back",
                Location = new Point(400, 370),
                Size = new Size(150, 40),
                Font = new Font("Arial", 12, FontStyle.Bold),
                BackColor = Color.LightGray,
                FlatStyle = FlatStyle.Flat
            };
            backButton.Click += (s, e) => ShowCallInterface();

            // Add all controls to main panel
            mainPanel.Controls.Add(titleLabel);
            mainPanel.Controls.Add(historyListBox);
            mainPanel.Controls.Add(clearButton);
            mainPanel.Controls.Add(backButton);
        }

        private void UpdateButtonStates(bool isConnected)
        {
            foreach (Control control in mainPanel.Controls)
            {
                if (control is Button button)
                {
                    switch (button.Name)
                    {
                        case "connectButton":
                            button.Enabled = !isConnected;
                            break;
                        case "muteButton":
                            button.Enabled = isConnected;
                            if (!isConnected)
                            {
                                button.Text = "Mute Mic";
                                button.BackColor = Color.LightGray;
                            }
                            break;
                        case "endButton":
                            button.Enabled = isConnected;
                            break;
                    }
                }
            }
        }


        private void MuteButton_Click(object sender, EventArgs e)
        {
            Button muteButton = sender as Button;
            isMicMuted = !isMicMuted;

            if (isMicMuted)
            {
                muteButton.Text = "Unmute Mic";
                muteButton.BackColor = Color.Orange;
                waveIn?.StopRecording();
            }
            else
            {
                muteButton.Text = "Mute Mic";
                muteButton.BackColor = Color.LightGray;
                waveIn?.StartRecording();
            }
        }

        private async void StartButton_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                UpdateButtonStates(true);
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
                    UpdateButtonStates(false);
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
            AddToConnectionHistory(client);
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

        private void StartReceivingAudio()
        {
            cancellationTokenSource = new CancellationTokenSource();

            Task.Run(async () =>
            {
                byte[] buffer = new byte[4096];
                while (isConnected && stream != null)
                {
                    try
                    {
                        if (cancellationTokenSource.Token.IsCancellationRequested)
                            break;

                        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationTokenSource.Token);
                        if (bytesRead > 0)
                        {
                            waveProvider?.AddSamples(buffer, 0, bytesRead);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        break;
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
            }, cancellationTokenSource.Token);
        }

        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            try
            {
                if (stream != null && stream.CanWrite && !isMicMuted)
                {
                    stream.Write(e.Buffer, 0, e.BytesRecorded);
                }
            }
            catch (Exception ex)
            {
                if (isConnected)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        UpdateStatus($"Audio sending error: {ex.Message}", Color.Red);
                    });
                }
            }
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
        }

        private void HistoryButton_Click(object sender, EventArgs e)
        {
            ShowHistory();
        }

        private void EndCall()
        {
            if (!isConnected && client == null && server == null)
                return;

            isConnected = false;    
            isMicMuted = false;

            try
            {
                cancellationTokenSource?.Cancel();

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
                Thread.Sleep(100);
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
                cancellationTokenSource?.Dispose();
                cancellationTokenSource = null;
                InitializeAudio();
                UpdateButtonStates(false);
                UpdateStatus("Call ended", Color.Red);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during call termination: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                using (var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5)))
                {
                    try
                    {
                        client = await server.AcceptTcpClientAsync();
                        stream = client.GetStream();
                        SetupSuccessfulConnection();
                        UpdateStatus("Client connected! Ready for call.", Color.Green);
                    }
                    catch (OperationCanceledException)
                    {
                        throw new TimeoutException("Connection attempt timed out");
                    }
                }
            }
            catch (Exception)
            {
                try
                {
                    if (server != null)
                    {
                        server.Stop();
                        server = null;
                    }
                }
                catch { }
                throw;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                EndCall();
            }
            catch { }
            base.OnFormClosing(e);
        }
    }
}
