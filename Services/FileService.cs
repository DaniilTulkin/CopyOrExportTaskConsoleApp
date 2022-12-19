using System;
using System.IO;
using System.Text.RegularExpressions;

namespace CopyOrExportTaskConsoleApp
{
    public static class FileService
    {
        public static void CopyFiles()
        {
            string title = IniService.ReadKey(Consts.CopySettingsSection, Consts.CopySettingsTitleKey);
            string projectCode = IniService.ReadKey(Consts.CopySettingsSection, Consts.CopySettingsProjectCodeKey);
            string destPath = IniService.ReadKey(Consts.CopySettingsSection, Consts.CopySettingsDestinationKey);
            string suffix = IniService.ReadKey(Consts.CopySettingsSection, Consts.CopySettingsSuffixKey);
            string log = IniService.ReadKey(Consts.CopySettingsSection, Consts.CopySettingsLogKey);
            string[] filesPaths = IniService.ReadSection(Consts.CopyFilesSection);

            LogService.Initialize(log);
            LogService.Info("CopyFiles started");

            if (filesPaths != null)
            {
                foreach (string filePath in filesPaths)
                {
                    if (filePath.Contains(projectCode))
                    {
                        string fileName = $"{Path.GetFileNameWithoutExtension(filePath)}{suffix}.rvt";
                        string sharedFilePath = Path.Combine(destPath, fileName);

                        if (IsValidFileName(sharedFilePath))
                        {
                            try
                            {
                                if (!Directory.Exists(destPath)) Directory.CreateDirectory(destPath);
                                File.Copy(filePath, sharedFilePath, true);
                                LogService.Info($"File copied.\nNew file path {sharedFilePath}");
                            }
                            catch (Exception ex)
                            {
                                LogService.Error($"Copy file error.\nFile path {filePath}\n{ex.Message}");
                            }
                        }
                    }
                }
            }
            LogService.Info("CopyFiles ended");
        }

        public static void DeleteByPath(params string[] paths)
        {
            foreach (string path in paths)
            {
                if (File.Exists(path))
                {
                    try
                    {
                        File.Delete(path);
                    }
                    catch (Exception ex)
                    {
                        LogService.Error($"Delete file error.\n{ex.Message}");
                    }
                }
            }
        }

        private static bool IsValidFileName(string fileName)
        {
            Regex containsBadChar = new Regex("["
                  + Regex.Escape(new string(Path.GetInvalidPathChars())) + "]");
            if (containsBadChar.IsMatch(fileName)) return false;
            return true;
        }
    }
}
