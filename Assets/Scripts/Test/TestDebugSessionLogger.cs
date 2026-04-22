using System;
using System.IO;
using UnityEngine;

public static class TestDebugSessionLogger
{
    private const string SessionId = "14ed85";
    private const string LogFileName = "debug-14ed85.log";

    public static void Log(string runId, string hypothesisId, string location, string message, string data)
    {
        try
        {
            string root = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
            string path = Path.Combine(root, LogFileName);
            long ts = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            string json = "{"
                + "\"sessionId\":\"" + Escape(SessionId) + "\","
                + "\"runId\":\"" + Escape(runId) + "\","
                + "\"hypothesisId\":\"" + Escape(hypothesisId) + "\","
                + "\"location\":\"" + Escape(location) + "\","
                + "\"message\":\"" + Escape(message) + "\","
                + "\"data\":\"" + Escape(data) + "\","
                + "\"timestamp\":" + ts
                + "}";
            File.AppendAllText(path, json + Environment.NewLine);
        }
        catch
        {
        }
    }

    private static string Escape(string value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;
        return value.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "\\r").Replace("\n", "\\n");
    }
}
