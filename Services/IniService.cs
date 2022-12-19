using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace CopyOrExportTaskConsoleApp
{
    public static class IniService
    {
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileSection(string section, 
                                                           byte[] result, 
                                                           int size, 
                                                           string filePath);
        private static string[] Read(string section, string filePath)
        {
            filePath = new FileInfo(filePath).FullName;
            byte[] buffer = new byte[4096];
            GetPrivateProfileSection(section, buffer, 4096, filePath);
            return Encoding.UTF8.GetString(buffer)?.Trim('\0')?.Split('\0');
        }
        public static string ReadKey(string section, 
                                     string key, 
                                     string filePath = Consts.IniFilePath)
        {
            string[] iniStrings = Read(section, filePath);
            foreach (string iniString in iniStrings)
            {
                string[] parts = iniString.Split('=');
                if (parts.Length > 1 && parts[0] == key) return parts[1];
            }
            return null;
        }

        public static string[] ReadSection(string section, 
                                           bool isSectionWithKeys = false, 
                                           string filePath = Consts.IniFilePath)
        {
            string[] iniStrings = Read(section, filePath);
            if  (isSectionWithKeys && iniStrings != null)
            {
                List<string> result = new List<string>();
                foreach (string iniString in iniStrings)
                {
                    string[] parts = iniString.Split('=');
                    if (parts.Length > 1) result.Add(parts[1]);
                }
                return result.ToArray();
            }
            
            return iniStrings;
        }
    }
}
