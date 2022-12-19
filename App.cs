using System;

namespace CopyOrExportTaskConsoleApp
{
    internal class App
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Файл config.ini необходимо разместить в той-же директории, что и .exe файл");
            Console.WriteLine("");
            Console.WriteLine($"{Consts.CopyTask} - команда для копирования проектов");
            Console.WriteLine($"{Consts.ExportTask} - команда для экспорта проектов в nwc");
            Console.WriteLine("Введите команду");
            string task = Console.ReadLine();
            if (!string.IsNullOrEmpty(task))
            {
                switch (task)
                {
                    case Consts.CopyTask:
                        FileService.CopyFiles();
                        break;
                    case Consts.ExportTask:
                        BatService.ExportProjects();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
