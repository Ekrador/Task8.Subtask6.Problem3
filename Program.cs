using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task8.Subtask6.Problem1
{
    internal class Program
    {

        static void Main(string[] args)
        {
            TimeSpan interval = TimeSpan.FromMinutes(30);
            Console.WriteLine("Введите путь к папке для очистки.");
            string path = Console.ReadLine();
            int deletedFiles = 0;
            if (Directory.Exists(path))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                long startSize = Getsize(dirInfo);
                Console.WriteLine($"Исходный размер папки {dirInfo.Name}: {startSize} байт");
                LookFiles(dirInfo, interval, ref deletedFiles);
                long newSize = Getsize(dirInfo);
                Console.WriteLine($"Удалено {deletedFiles} файлов и очищено {startSize - newSize} байт");
                Console.WriteLine($"Текущий размер папки: {newSize} байт");
            }
            else Console.WriteLine("Папка по данному пути не найдена.");
            Console.ReadLine();
        }

        static void LookFiles(DirectoryInfo directory, TimeSpan interval, ref int deletedFiles)
        {
            FileInfo[] files = directory.GetFiles();
            foreach (FileInfo file in files)
            {
                CheckFile(file, interval, ref deletedFiles);
            }
            DirectoryInfo[] dirs = directory.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                CheckDir(dir, interval, ref deletedFiles);
            }
        }
        static void CheckFile(FileInfo file, TimeSpan interval, ref int deletedFiles)
        {
            if (DateTime.Now - file.LastAccessTime > interval)
            {
                Console.WriteLine($"Файл {file.Name} не использовался более 30 минут и будет удален.");
                try
                {
                    file.Delete();
                    deletedFiles++;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        static void CheckDir(DirectoryInfo dir, TimeSpan interval, ref int deletedFiles)
        {
            if ((DateTime.Now - dir.LastAccessTime > interval) & !IsFilesUsedInInterval(dir, interval))
            {
                Console.WriteLine($"Папка {dir.Name} не использовалась более 30 минут и будет удалена.");
                try
                {
                    dir.Delete(true);
                    deletedFiles++;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        static bool IsFilesUsedInInterval(DirectoryInfo dir, TimeSpan interval)
        {
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                if (DateTime.Now - dir.LastAccessTime < interval) return true;
            }
            DirectoryInfo[] dirs = dir.GetDirectories();
            foreach (DirectoryInfo directory in dirs)
            {
                IsFilesUsedInInterval(directory, interval);
            }
            return false;
        }

        private static long Getsize(DirectoryInfo dirInfo)
        {
            long size = 0;
            FileInfo[] files = dirInfo.GetFiles();
            try
            {
                foreach (FileInfo file in files)
                {
                    size += file.Length;
                }
                DirectoryInfo[] dirs = dirInfo.GetDirectories();
                foreach (DirectoryInfo dir in dirs)
                {
                    size += Getsize(dir);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return size;
        }
    }
}
