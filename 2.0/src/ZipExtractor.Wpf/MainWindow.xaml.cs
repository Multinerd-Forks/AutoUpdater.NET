using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Windows;

namespace ZipExtractor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BackgroundWorker _backgroundWorker;
        readonly StringBuilder _logBuilder = new StringBuilder();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _logBuilder.AppendLine(DateTime.Now.ToString("F"));
            _logBuilder.AppendLine();
            _logBuilder.AppendLine("ZipExtractor started with following command line arguments.");

            var args = Environment.GetCommandLineArgs();
            for (var index = 0; index < args.Length; index++)
            {
                var arg = args[index];
                _logBuilder.AppendLine($"[{index}] {arg}");
            }

            _logBuilder.AppendLine();

            if (args.Length >= 3)
            {
                // Extract all the files.
                _backgroundWorker = new BackgroundWorker
                {
                    WorkerReportsProgress = true,
                    WorkerSupportsCancellation = true
                };

                _backgroundWorker.DoWork += (o, eventArgs) =>
                {
                    foreach (var process in Process.GetProcesses())
                    {
                        try
                        {
                            if (process.MainModule.FileName.Equals(args[2]))
                            {
                                _logBuilder.AppendLine("Waiting for application process to Exit...");

                                _backgroundWorker.ReportProgress(0, "Waiting for application to Exit...");
                                process.WaitForExit();
                            }
                        }
                        catch (Exception exception)
                        {
                            Debug.WriteLine(exception.Message);
                        }
                    }

                    _logBuilder.AppendLine("BackgroundWorker started successfully.");

                    var path = Path.GetDirectoryName(args[2]);

                    // Open an existing zip file for reading.
                    var zip = ZipStorer.Open(args[1], FileAccess.Read);

                    // Read the central directory collection.
                    var dir = zip.ReadCentralDir();

                    _logBuilder.AppendLine($"Found total of {dir.Count} files and folders inside the zip file.");

                    for (var index = 0; index < dir.Count; index++)
                    {
                        if (_backgroundWorker.CancellationPending)
                        {
                            eventArgs.Cancel = true;
                            zip.Close();
                            return;
                        }

                        var entry = dir[index];
                        zip.ExtractFile(entry, Path.Combine(path, entry.FilenameInZip));
                        var currentFile = $"Extracting {entry.FilenameInZip}";
                        var progress = (index + 1) * 100 / dir.Count;
                        _backgroundWorker.ReportProgress(progress, currentFile);

                        _logBuilder.AppendLine($"{currentFile} [{progress}%]");
                    }

                    zip.Close();
                };

                _backgroundWorker.ProgressChanged += (o, eventArgs) =>
                {
                    ProgressBar.Value = eventArgs.ProgressPercentage;
                    LabelInformation.Content = eventArgs.UserState.ToString();
                };

                _backgroundWorker.RunWorkerCompleted += (o, eventArgs) =>
                {
                    try
                    {
                        if (eventArgs.Error != null)
                        {
                            throw eventArgs.Error;
                        }

                        if (!eventArgs.Cancelled)
                        {
                            LabelInformation.Content = @"Finished";
                            try
                            {
                                var processStartInfo = new ProcessStartInfo(args[2]);
                                if (args.Length > 3)
                                {
                                    processStartInfo.Arguments = args[3];
                                }

                                Process.Start(processStartInfo);

                                _logBuilder.AppendLine("Successfully launched the updated application.");
                            }
                            catch (Win32Exception exception)
                            {
                                if (exception.NativeErrorCode != 1223)
                                {
                                    throw;
                                }
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        _logBuilder.AppendLine();
                        _logBuilder.AppendLine(exception.ToString());

                        MessageBox.Show(exception.Message, exception.GetType().ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        _logBuilder.AppendLine();
                        Application.Current.Shutdown(0);
                    }
                };

                _backgroundWorker.RunWorkerAsync();
            }
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            _backgroundWorker?.CancelAsync();

            _logBuilder.AppendLine();
            File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ZipExtractor.log"), _logBuilder.ToString());
        }
    }
}
