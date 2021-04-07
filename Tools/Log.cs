using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;

public class Log
{
    private static string LogPath = Application.dataPath + "/MyLog/Log.txt";

    public static void LogInfo(string message)
    {
        Debug.Log("<color=#FFD39B>"+message+"</color>");
        WriteLog(message, LogPath);
    }

    public static void LogWarning(string message)
    {
        Debug.Log("<color=#FF8247>" + message + "</color>");
        WriteLog(message, LogPath);
    }

    public static void LogError(string message)
    {
        Debug.Log("<color=#FF0000>" + message + "</color>");
        WriteLog(message, LogPath);
    }

    private static void WriteLog(string messages,string path)
    {
        JudgeFile(path);
        FileOperation.WriteFile(path, messages,true);
    }

    private static void JudgeFile(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(Application.dataPath + "/Mylog");
        }
    }

    private static void JudgeDate()
    {
    }

}
