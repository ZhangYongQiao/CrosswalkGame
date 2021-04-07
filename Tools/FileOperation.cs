using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class FileOperation
{

    /// <summary>
    /// 向文件写数据
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <param name="txt">输入文本</param>
    public static void WriteFile(string path, string txt,bool isAppend)
    {
        if (File.Exists(path))
        {
            using(StreamWriter sw = new StreamWriter(path, isAppend, Encoding.UTF8))
                sw.Write(txt);
        }
        else
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using(StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.Write(txt);
                }
            }
        }
    }

    /// <summary>
    /// 向文件读数据
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <param name="mode">文件模式</param>
    /// <returns>读取出的字符串</returns>
    public static string ReadFile(string path,FileMode mode = FileMode.Open)
    {
        if (File.Exists(path))
        {
            using (FileStream fs = new FileStream(path, mode))
            {
                using(StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    string readTxt = sr.ReadToEnd();
                    return readTxt;
                }
            }
        }
        else
        {
            Debug.LogError("不存在该路径文件");
            return null;
        }
    }

}
