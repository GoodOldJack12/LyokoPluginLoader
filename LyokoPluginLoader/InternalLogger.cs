using System;
using System.IO;

namespace LyokoPluginLoader
{
    public class InternalLogger
    {
        private DirectoryInfo logDir;
        private FileInfo logFile;
        public InternalLogger(string logDirPath)
        {
            try
            {
                EnsureLogDir(logDirPath, out this.logDir);
                EnsureLogFile(Path.Combine(logDir.FullName,"lyokoPluginLoader.log"));

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }


        public void Log(string issue)
        {
            File.WriteAllText(logFile.FullName, $"[{DateTime.Now}] {issue}");
        }

        private bool EnsureLogFile(string logPath)
        {
            logFile = new FileInfo(logPath);
            if (File.Exists(logFile.FullName))
            {
                return false;
            }

            logFile.Create();
            return true;
        }

        private bool EnsureLogDir(string path, out DirectoryInfo directory)
        {
            if (Directory.Exists(path))
            {
                directory = new DirectoryInfo(path);
                return false;
            }
            else
            {
                directory =  Directory.CreateDirectory(path);
                return true;
            }
        }
    }
}