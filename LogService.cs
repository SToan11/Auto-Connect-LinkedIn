using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoConnectLinkedIn
{
    public static class LogService
    {
        private static readonly string LogFolder =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");

        // Hàm để lấy đường dẫn đầy đủ của file log
        private static string GetLogFile(string fileName)
        {
            if (!Directory.Exists(LogFolder))
                Directory.CreateDirectory(LogFolder);

            return Path.Combine(LogFolder, fileName);
        }

        // Log đăng nhập
        public static void LogLogin(string email, bool success)
        {
            string filePath = GetLogFile("login_log.txt");
            string status = success ? "LOGIN SUCCESS" : "LOGIN FAILED";

            string line = $"{email} | {status} | {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
            File.AppendAllText(filePath, line + Environment.NewLine);
        }

        // Log connect
        public static void LogConnect(string email, int connectCount)
        {
            string filePath = GetLogFile("connect_log.txt");
            string line = $"{email} | CONNECT SENT: {connectCount} | {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
            File.AppendAllText(filePath, line + Environment.NewLine);
        }

        // Log chi tiết từng người
        public static void LogConnectedPeople(string detail)
        {
            string filePath = GetLogFile("connected_people.txt");
            string line = $"{detail}";
            File.AppendAllText(filePath, line + Environment.NewLine);
        }
    }
        
    //public static class LogService
    //{
    //    //private static readonly string BaseFolder = @"D:\Auto\AutoConnectLinkedIn\";
    //    private static readonly string BaseFolder = AppDomain.CurrentDomain.BaseDirectory;

    //    private static string GetLogFile(string fileName)
    //    {
    //        if (!Directory.Exists(BaseFolder))
    //            Directory.CreateDirectory(BaseFolder);

    //        return Path.Combine(BaseFolder, fileName);
    //    }

    //    // Log đăng nhập
    //    public static void LogLogin(string email, bool success)
    //    {
    //        string filePath = GetLogFile("login_log.txt");
    //        string status = success ? "LOGIN SUCCESS" : "LOGIN FAILED";

    //        string line = $"{email} | {status} | {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
    //        File.AppendAllText(filePath, line + Environment.NewLine);
    //    }

    //    // Log connect
    //    public static void LogConnect(string email, int connectCount)
    //    {
    //        string filePath = GetLogFile("connect_log.txt");

    //        string line = $"{email} | CONNECT SENT: {connectCount} | {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
    //        File.AppendAllText(filePath, line + Environment.NewLine);
    //    }

    //    // Log chi tiết từng người (nếu muốn)
    //    public static void LogConnectedPeople(string email, string detail)
    //    {
    //        string filePath = GetLogFile("connected_people.txt");

    //        string line = $"{detail}";
    //        File.AppendAllText(filePath, line + Environment.NewLine);
    //    }
    //}
}
