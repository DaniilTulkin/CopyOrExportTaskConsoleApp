using System;
using System.Collections.Generic;
using System.IO;

namespace CopyOrExportTaskConsoleApp
{
    public static class TxtService
    {
        public static bool Write(string filePath, List<string> contents)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                File.Create(filePath).Close();
                File.AppendAllLines(filePath, contents);
                return true;
            }
            catch (Exception ex)
            {
                LogService.Error($"Write txt error.\n{ex.Message}");
                return false;
            }
        }

        public static string CreateTxtWithRvtFilePath(string filePath, string tempPath)
        {
            string txtFilePath = Path.Combine(tempPath, $"{Path.GetFileNameWithoutExtension(filePath)}.txt");
            List<string> files = new List<string>();
            files.Add(filePath);

            if (Write(txtFilePath, files))
            {
                LogService.Info($"Txt with rvt file path created");
                return txtFilePath;
            }
            return null;
        }
    }
}
