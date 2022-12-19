using System.IO;
using System.Diagnostics;

namespace CopyOrExportTaskConsoleApp
{
    public static class BatService
    {
        public static void ExportProjects()
        {
            string projectCode = IniService.ReadKey(Consts.CopySettingsSection, Consts.CopySettingsProjectCodeKey);
            string log = IniService.ReadKey(Consts.CopySettingsSection, Consts.CopySettingsLogKey);
            string nwcVersion = IniService.ReadKey(Consts.NwcSettingsSection, Consts.NwcSettingsVersionKey);
            string taskRunnerFilePath = IniService.ReadKey(Consts.NwcSettingsSection, Consts.NwcSettingsPathKey);
            string tempPath = IniService.ReadKey(Consts.NwcSettingsSection, Consts.NwcSettingsTempKey);
            string destPath = IniService.ReadKey(Consts.NwcSettingsSection, Consts.NwcSettingsDestinationKey);
            string[] filesPaths = IniService.ReadSection(Consts.ExportFilesSection);

            LogService.Initialize(log);
            LogService.Info("ExportProjects started");

            if (!Directory.Exists(destPath)) Directory.CreateDirectory(destPath);
            if (filesPaths != null)
            {
                foreach (string filePath in filesPaths)
                {
                    if (filePath.Contains(projectCode) && File.Exists(filePath))
                    {
                        string txtFilePath = TxtService.CreateTxtWithRvtFilePath(filePath, tempPath);
                        if (txtFilePath != null)
                        {
                            string batFilePath = CreateBatWithCommands(filePath, tempPath, taskRunnerFilePath, txtFilePath, destPath);
                            RunBatProcess(batFilePath);

                            FileService.DeleteByPath(txtFilePath, batFilePath, Path.Combine(tempPath, $"{Path.GetFileNameWithoutExtension(filePath)}.nwd"));
                        }
                    }
                }
            }

            LogService.Info("ExportProjects ended");
        }

        private static void RunBatProcess(string batFilePath)
        {
            string command = $"/c \"{batFilePath}\"";
            ProcessStartInfo processInfo = new ProcessStartInfo("cmd", command);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            Process process = Process.Start(processInfo);

            process.OutputDataReceived += (object sender, DataReceivedEventArgs e) => 
                { if (!string.IsNullOrEmpty(e.Data)) LogService.Info($"Console info: {e.Data}"); };
            process.BeginOutputReadLine();

            process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) => 
                { if (!string.IsNullOrEmpty(e.Data)) LogService.Error($"Console error: {e.Data}"); };
            process.BeginErrorReadLine();

            process.WaitForExit();
            LogService.Info($"Process exit code: {process.ExitCode}");
            process.Close();
        }

        private static string CreateBatWithCommands(string filePath, 
                                                    string tempPath,
                                                    string taskRunnerFilePath,
                                                    string txtFilePath,
                                                    string destPath)
        {
            string batFilePath = Path.Combine(tempPath, $"{Path.GetFileNameWithoutExtension(filePath)}.bat");
            string command1 = $"start /wait \"Navisworks Batch Utility\" \"{taskRunnerFilePath}\" /i \"{txtFilePath}\" /od \"{tempPath}\"";
            string command2 = $"move /y \"{filePath.Replace("rvt", "nwc")}\" \"{destPath}\"";
            using (StreamWriter batFile = new StreamWriter(batFilePath))
            {
                batFile.WriteLine("chcp 65001");
                batFile.WriteLine(command1);
                batFile.WriteLine(command2);
            }

            return batFilePath;
        }
    }
}
