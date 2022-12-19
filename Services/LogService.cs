using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;

namespace CopyOrExportTaskConsoleApp
{
    public static class LogService
    {
        private static Mutex mutex = new Mutex(false, "ID_LOG_SYNC_MUTEX");
        private static string logFileName = typeof(LogService).Namespace;
        private static string applicationPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static string logFolderPath = $@"{applicationPath}\{logFileName}";
        private static bool isInitialized;
        public static string LogFilePath { get; set; }
        public static void Initialize(string filePath = null)
        {
            if (filePath != null)
            {
                FileInfo fi = null;
                try
                {
                    fi = new FileInfo(filePath);
                }
                catch { }
                if (fi != null)
                {
                    logFileName = fi.Name;
                    logFolderPath = fi.DirectoryName;
                }
            }
            mutex.WaitOne();
            if (!Directory.Exists(logFolderPath)) Directory.CreateDirectory(logFolderPath);
            mutex.ReleaseMutex();

            string dateStr = DateTime.Now.ToString("dd-MM-yyyy_HH-mm");
            LogFilePath = Path.Combine(logFolderPath, $"{dateStr}_{logFileName}");

            mutex.WaitOne();
            if (!File.Exists(LogFilePath)) File.Create(LogFilePath).Close();
            mutex.ReleaseMutex();

            List<string> startString = new List<string>();
            startString.Add("====================================================================================");
            startString.Add($"{logFileName}_start_{dateStr}");
            startString.Add("====================================================================================");
            File.AppendAllLines(LogFilePath, startString);
            isInitialized = true;
        }

        /// <summary>
        /// Information that highlights progress or application lifetime events.
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            List<string> stringListInfo = new List<string>();
            stringListInfo.Add("Info");
            stringListInfo.Add(message);
            WriteLog(stringListInfo);
        }

        /// <summary>
        /// Warnings about validation issues or temporary failures that can be recovered.
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(string message)
        {
            List<string> stringListInfo = new List<string>();
            stringListInfo.Add("Warn");
            stringListInfo.Add(message);
            WriteLog(stringListInfo);
        }

        /// <summary>
        /// Errors where functionality has failed or Exception have been caught.
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message)
        {
            List<string> stringListInfo = new List<string>();
            stringListInfo.Add("Error");
            stringListInfo.Add(Environment.StackTrace);
            stringListInfo.Add(message);
            WriteLog(stringListInfo);
        }

        private static void WriteLog(List<string> message)
        {
            if (!isInitialized) return;
            try
            {
                message.Insert(0, DateTime.Now.ToLocalTime().ToString());
                message.Insert(0, "");

                mutex.WaitOne();
                File.AppendAllLines(LogFilePath, message);
                mutex.ReleaseMutex();
            }
            catch { }
        }
    }
}
