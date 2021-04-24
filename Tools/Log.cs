using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;

public static class Log
{
    private static string FilePath = Application.dataPath + "/MyLog";

    private static string WriteToThisFile = DateTime.UtcNow.ToString("yyyy-MM-dd")+".txt";

    public static void Info(string message)
    {
        Debug.Log(string.Format("{0}:<color=#FFD39B>{1}</color>", TimeNow(), message));
        WriteLog("[Info]"+message, Path.Combine(FilePath, WriteToThisFile));
    }

    public static void Warning(string message)
    {
        Debug.Log(string.Format("{0}:<color=#FF8247>{1}</color>", TimeNow(), message));
        WriteLog("[Warning]"+message, Path.Combine(FilePath, WriteToThisFile));
    }

    public static void Error(string message)
    {
        Debug.Log(string.Format("{0}:<color=#FF0000>{1}</color>", TimeNow(),message));
        WriteLog("[Error]"+message,Path.Combine(FilePath,WriteToThisFile));
    }

    private static void WriteLog(string messages,string path)
    {
        JudgeDirectoryAndFile(path);
        FileOperation.WriteFileNow(path,TimeNow()+":"+messages,true);
    }

    private static void JudgeDirectoryAndFile(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(Application.dataPath + "/Mylog");
        }
        if (!File.Exists(Path.Combine(FilePath, WriteToThisFile)))
        {
            File.Create(Path.Combine(FilePath, WriteToThisFile)).Dispose();
        }
    }

    public static string TimeNow()
    {
        DateTime now = DateTime.UtcNow;
        return now.ToString("T");
    }

    public static string DateNow()
    {
        DateTime now = DateTime.UtcNow;
        return now.ToString("yyyy-MM-dd");
    }

}
