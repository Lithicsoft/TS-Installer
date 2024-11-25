// License: Apache-2.0
/*
 * Form1.cs: Main form for launcher, installer, updater for Trainer Studio
 *
 * (C) Copyright 2024 Lithicsoft Organization
 * Author: Bui Nguyen Tan Sang <tansangbuinguyen52@gmail.com>
 */

using IWshRuntimeLibrary;
using System.ComponentModel;
using System.IO.Compression;
using System.Net;
using System.Windows.Forms;
using File = System.IO.File;

namespace Lithicsoft_Trainer_Studio_Installer
{
    public partial class Form1 : Form
    {
        private readonly NotifyIcon notifyIcon;

        public Form1()
        {
            InitializeComponent();
            this.Closing += OnClosing;

            notifyIcon = new NotifyIcon
            {
                Icon = SystemIcons.Information,
                Visible = true
            };

            if (File.Exists("build.txt") && Directory.Exists("Lithicsoft Trainer Studio"))
            {
                button3.Enabled = false;
                button1.Enabled = true;
                label3.Text = "Build: " + File.ReadAllText("build.txt");
            }
            else
            {
                button3.Enabled = true;
                button1.Enabled = false;
                label3.Text = "Ready for install";
            }

            try
            {
                string url = "https://raw.githubusercontent.com/EndermanPC/test/main/changelog.txt";
                using var webClient = new WebClient();
                richTextBox1.Text = webClient.DownloadString(url);
            }
            catch (Exception ex)
            {
                ShowNotification("Error", $"Error getting change log: {ex.Message}");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForUpdates();
        }

        private async void Button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            try
            {
                await Task.Run(() => UpdateTrainerStudio());
            }
            catch (Exception ex)
            {
                ShowNotification("Error", $"Update failed: {ex.Message}");
            }
            finally
            {
                button1.Enabled = true;
            }
        }

        private async void Button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            try
            {
                await Task.Run(() => InstallTrainerStudio());
                button1.Enabled = false;
            }
            catch (Exception ex)
            {
                ShowNotification("Error", $"Install failed: {ex.Message}");
            }
        }

        private void CheckForUpdates()
        {
            try
            {
                string url = "https://raw.githubusercontent.com/EndermanPC/test/main/update.txt";
                string localFilePath = "build.txt";

                using var webClient = new WebClient();
                string fileContents = webClient.DownloadString(url);
                string[] lines = fileContents.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                if (lines.Length > 0)
                {
                    string latestBuild = lines[0];

                    if (File.Exists(localFilePath))
                    {
                        string localBuild = File.ReadAllText(localFilePath);
                        if (latestBuild == localBuild)
                        {
                            ShowNotification("Up-to-Date", "Trainer Studio is already up-to-date.");
                            return;
                        }
                    }

                    ShowNotification("Update Available", "An update is available. Please update to the new version.");
                }
            }
            catch (Exception ex)
            {
                ShowNotification("Error", $"Error checking for updates: {ex.Message}");
            }
        }

        private async void UpdateTrainerStudio()
        {
            try
            {
                var progress = new Progress<int>(value => UpdateProgress(value));
                await PerformUpdateOrInstall("update", progress);
            }
            catch (Exception ex)
            {
                ShowNotification("Error", $"Error updating Lithicsoft Trainer Studio: {ex.Message}");
            }
        }

        private async void InstallTrainerStudio()
        {
            try
            {
                var progress = new Progress<int>(value => UpdateProgress(value));
                await PerformUpdateOrInstall("install", progress);
            }
            catch (Exception ex)
            {
                ShowNotification("Error", $"Error installing Lithicsoft Trainer Studio: {ex.Message}");
            }
        }

        private void UpdateProgress(int value)
        {
            if (InvokeRequired)
            {
                Invoke((Action<int>)UpdateProgress, value);
                return;
            }
            progressBar1.Value = value;
        }

        private async Task PerformUpdateOrInstall(string mode, IProgress<int> progress)
        {
            progress.Report(0);
            string url = "https://raw.githubusercontent.com/EndermanPC/test/main/update.txt";
            string localFilePath = "build.txt";
            string destinationFolder = "Lithicsoft Trainer Studio";

            using var webClient = new WebClient();
            string fileContents = await webClient.DownloadStringTaskAsync(url);
            string[] lines = fileContents.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length > 0)
            {
                string latestBuild = lines[0];
                string downloadUrl = lines.Length > 1 ? lines[1] : string.Empty;

                File.WriteAllText(localFilePath, latestBuild);

                string zipFilePath = "downloaded.zip";

                progress.Report(20);
                await webClient.DownloadFileTaskAsync(new Uri(downloadUrl), zipFilePath);
                progress.Report(50);
                ZipFile.ExtractToDirectory(zipFilePath, destinationFolder, overwriteFiles: true);
                progress.Report(90);

                File.Delete(zipFilePath);
                progress.Report(100);

                if (mode == "install")
                {
                    DirectoryPermissionHelper.SetFullControlPermissions(destinationFolder);
                    CreateShortcut("Lithicsoft Trainer Studio", Path.GetFullPath(Path.Combine(destinationFolder, "Lithicsoft Trainer Studio.exe")));
                    ShowNotification("Installation Complete", "Lithicsoft Trainer Studio has been installed!");
                    button3.Enabled = false;
                    button1.Enabled = true;
                }
                else if (mode == "update")
                {
                    ShowNotification("Update Complete", "Lithicsoft Trainer Studio has been updated!");
                }

                label3.Text = "Build: " + File.ReadAllText(localFilePath);
            }
        }

        private void CreateShortcut(string shortcutName, string targetFileLocation)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string startMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);

            string desktopShortcutLocation = Path.Combine(desktopPath, $"{shortcutName}.lnk");
            string startMenuShortcutLocation = Path.Combine(startMenuPath, $"{shortcutName}.lnk");

            WshShell shell = new();

            void ConfigureShortcut(string shortcutPath)
            {
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
                shortcut.Description = "Shortcut for Lithicsoft Trainer Studio";
                shortcut.TargetPath = targetFileLocation;
                string? workingLocation = Path.GetDirectoryName(targetFileLocation);
                if (workingLocation != null)
                {
                    shortcut.WorkingDirectory = Path.GetFullPath(workingLocation);
                }
                shortcut.Save();
            }

            ConfigureShortcut(desktopShortcutLocation);
            ConfigureShortcut(startMenuShortcutLocation);
        }

        private void ShowNotification(string title, string message)
        {
            notifyIcon.BalloonTipTitle = title;
            notifyIcon.BalloonTipText = message;
            notifyIcon.ShowBalloonTip(3000);
        }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            string msg = "Do you want to exit Trainer Studio Installer?";
            DialogResult result = MessageBox.Show(msg, "Close Confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                Application.Exit();
            else if (result == DialogResult.No)
                cancelEventArgs.Cancel = true;
        }
    }
}
