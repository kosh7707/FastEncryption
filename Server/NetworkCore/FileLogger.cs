using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkCore
{
    public struct CipherLogObject
    {
        public string algorithmName;
        public string operationModeName;
        public long tick;

        public string GetLogFilePath(string baseDir)
        {
            return Path.Combine(baseDir, algorithmName, $"{operationModeName}.txt");
        }

        public string GetLogContent()
        {
            return tick.ToString() + Environment.NewLine;
        }
    }

    public class FileLogger
    {
        private readonly BlockingCollection<CipherLogObject> logQueue = new BlockingCollection<CipherLogObject>();
        private readonly Thread logThread;
        private readonly string baseDir;
        private bool isRunning = true;

        static FileLogger _instance = new FileLogger();
        public static FileLogger Instance { get { return _instance; } }

        FileLogger()
        {
            baseDir = "./AlgorithmTimeLog/";
            logThread = new Thread(ProcessLogQueue) { IsBackground = true };
            logThread.Start();
        }

        public void Log(CipherLogObject log)
        {
            if (!isRunning) throw new InvalidOperationException("Logger is not running.");
            logQueue.Add(log);
        }

        private void ProcessLogQueue()
        {
            while (isRunning)
            {
                try
                {
                    CipherLogObject logObject = logQueue.Take();
                    WriteLog(logObject);
                }
                catch (InvalidOperationException)
                {
                    break;
                }
            }
                
            while (logQueue.TryTake(out CipherLogObject remainingLog))
            {
                WriteLog(remainingLog);
            }
            
        }

        private void WriteLog(CipherLogObject logObject)
        {
            var fullPath = logObject.GetLogFilePath(baseDir);
            var directoryPath = Path.GetDirectoryName(fullPath);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            File.AppendAllText(fullPath, logObject.GetLogContent());
        }

        public void Stop()
        {
            isRunning = false;
            logQueue.CompleteAdding();
            logThread.Join();
        }

    }
}
