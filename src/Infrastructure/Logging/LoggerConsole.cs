namespace Infrastructure.Logging;

using Application.Interfaces;
using System;

public class LoggerConsole : ILog
{
    public void Log(string message)
    {
        Console.WriteLine($"[INFO] {DateTime.Now} - {message}");
    }

    public void Error(string message)
    {
        Console.WriteLine($"[ERROR] {DateTime.Now} - {message}");
    }
}