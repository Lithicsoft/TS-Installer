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
using File = System.IO.File;

namespace Lithicsoft_Trainer_Studio_Installer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Closing += OnClosing;

            if (File.Exists("build.txt") || Directory.Exists("Lithicsoft Trainer Studio"))
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

                using (var webClient = new WebClient())
                {
                    richTextBox1.Text = webClient.DownloadString(url);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting change log: {ex.Message}", "Exception Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForUpdates();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            await UpdateTrainerStudio();
            button1.Enabled = true;
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            await InstallTrainerStudio();
        }

        private void CheckForUpdates()
        {
            try
            {
                string url = "https://raw.githubusercontent.com/EndermanPC/test/main/update.txt";
                string localFilePath = "build.txt";

                using (var webClient = new WebClient())
                {
                    string fileContents = webClient.DownloadString(url);
                    string[] lines = fileContents.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    if (lines.Length > 0)
                    {
                        string firstLine = lines[0];

                        if (File.Exists(localFilePath))
                        {
                            string localFileText = File.ReadAllText(localFilePath);

                            if (firstLine != localFileText)
                            {
                                MessageBox.Show("An update is available. Please update the new version.", "Update Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show("An update is available. Please update the new version.", "Update Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking for updates: {ex.Message}", "Exception Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task UpdateTrainerStudio()
        {
            try
            {
                var progress = new Progress<int>(value => progressBar1.Value = value);
                await PerformUpdateOrInstall("update", progress);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating Lithicsoft Trainer Studio: {ex.Message}", "Exception Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task InstallTrainerStudio()
        {
            try
            {
                var progress = new Progress<int>(value => progressBar1.Value = value);
                await PerformUpdateOrInstall("install", progress);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error installing Lithicsoft Trainer Studio: {ex.Message}", "Exception Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task PerformUpdateOrInstall(string mode, IProgress<int> progress)
        {
            progress.Report(0);
            string url = "https://raw.githubusercontent.com/EndermanPC/test/main/update.txt";
            string localFilePath = "build.txt";
            string destinationFolder = "Lithicsoft Trainer Studio";

            using (var webClient = new WebClient())
            {
                string fileContents = await webClient.DownloadStringTaskAsync(url);
                string[] lines = fileContents.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                if (lines.Length > 0)
                {
                    string firstLine = lines[0];
                    string secondLine = lines.Length > 1 ? lines[1] : string.Empty;

                    File.WriteAllText(localFilePath, firstLine);

                    string fileToDownload = secondLine;
                    string zipFilePath = "downloaded.zip";

                    progress.Report(20);
                    await webClient.DownloadFileTaskAsync(new Uri(fileToDownload), zipFilePath);
                    progress.Report(50);
                    ZipFile.ExtractToDirectory(zipFilePath, destinationFolder, overwriteFiles: true);
                    progress.Report(90);

                    File.Delete(zipFilePath);
                    progress.Report(100);

                    if (mode == "install")
                    {
                        CreateShortcut("Lithicsoft Trainer Studio", Path.GetFullPath(Path.Combine(destinationFolder, "Lithicsoft Trainer Studio.exe")));
                        MessageBox.Show("Lithicsoft Trainer Studio has been installed!", "Installer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        button3.Enabled = false;
                        button1.Enabled = true;
                    }
                    else if (mode == "update")
                    {
                        MessageBox.Show("Lithicsoft Trainer Studio has been updated!", "Installer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    label3.Text = "Build: " + File.ReadAllText(localFilePath);
                }
            }
        }

        private void CreateShortcut(string shortcutName, string targetFileLocation)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string startMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);

            string desktopShortcutLocation = Path.Combine(desktopPath, $"{shortcutName}.lnk");
            string startMenuShortcutLocation = Path.Combine(startMenuPath, $"{shortcutName}.lnk");

            WshShell shell = new WshShell();

            IWshShortcut desktopShortcut = (IWshShortcut)shell.CreateShortcut(desktopShortcutLocation);
            desktopShortcut.Description = "Shortcut for Lithicsoft Trainer Studio";
            desktopShortcut.TargetPath = targetFileLocation;
            desktopShortcut.Save();

            IWshShortcut startMenuShortcut = (IWshShortcut)shell.CreateShortcut(startMenuShortcutLocation);
            startMenuShortcut.Description = "Shortcut for Lithicsoft Trainer Studio";
            startMenuShortcut.TargetPath = targetFileLocation;
            startMenuShortcut.Save();
        }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            string msg = "Do you want to exit Trainer Studio Installer?";
            DialogResult result = MessageBox.Show(msg, "Close Confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                Application.Exit();
            else if (result == DialogResult.No)
                cancelEventArgs.Cancel = false;

        }
    }
}
