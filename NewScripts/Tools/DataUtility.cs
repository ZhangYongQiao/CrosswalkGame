using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;

public class DataUtility
{

    private static string EditorSavePath = Application.dataPath;
    private static string StandardAloneSavePath = Application.persistentDataPath;

    public const string RolePath = "RoleData.txt";
    public const string MonsterPath = "MonsterData.txt";
    public const string GemAndCherryPath = "GemAndCherryData.txt";

    public const string GemTag = "Gem";
    public const string CherryTag = "Cherry";

    #region 存储数据

    public static void WriteDataToJson()
    {
        Player player = new Player
        {
            Pos = CurPlayer.Instance.Pos,
            Blood = CurPlayer.Instance.Blood,
            Score = CurPlayer.Instance.Score,
            Scene = CurPlayer.Instance.Scene
        };

        List<Transform> monsterPosList = GetCompUtility.GetAllSameCompGo<EagleAndOpossumMove>();
        List<Transform> gemPosList = GetCompUtility.FindAllTag(GemTag);
        List<Transform> cherryPosList = GetCompUtility.FindAllTag(CherryTag);
        List<Transform> concatListPos = GetCompUtility.ConcatList<Transform>(gemPosList, cherryPosList,false);

        JsonToWrite<Player>(player,RolePath);
        JsonToWrite<List<Transform>>(concatListPos, GemAndCherryPath);
        JsonToWrite<List<Transform>>(monsterPosList, MonsterPath);
    }

#endregion

#region 读取数据

    public static void ReadJsonToData()
    {
        Player player = ReadToData<Player>(RolePath);
        List<Transform> monsterPosList = ReadToData<List<Transform>>(MonsterPath);
        List<Transform> concatListPos = ReadToData<List<Transform>>(GemAndCherryPath);

        CurPlayer.Instance.Pos = player.Pos;
        CurPlayer.Instance.Blood = player.Blood;
        CurPlayer.Instance.Scene = player.Scene;
        CurPlayer.Instance.Score = player.Score;

        CurMonster.Instance.PosList = monsterPosList;
    }

    #endregion

    /// <summary>
    /// 序列化，写入文件
    /// </summary>
    /// <typeparam name="T">序列化的类型</typeparam>
    /// <param name="data">数据</param>
    /// <param name="fileName">文件名</param>
    public static void JsonToWrite<T>(T data, string fileName) where T : new()
    {
        if (data != null)
        {
            string jsonTxt = JsonConvert.SerializeObject(data);
#if UNITY_EDITOR
            FileOperation.WriteFile(Path.Combine(EditorSavePath, fileName), jsonTxt);
#else
            FileOperation.WriteFile(Path.Combine(StandardAloneSavePath, fileName),jsonTxt);
#endif
        }

    }

    /// <summary>
    /// 反序列化文件
    /// </summary>
    /// <typeparam name="T">对应类型</typeparam>
    /// <param name="fileName">文件名</param>
    /// <returns>返回类型数据</returns>
    public static T ReadToData<T>(string fileName) where T : new()
    {
        string jsonStr;
#if UNITY_EDITOR
        jsonStr = FileOperation.ReadFile(Path.Combine(EditorSavePath, fileName));
#else
        jsonStr = FileOperation.ReadFile(Path.Combine(StandardAloneSavePath, fileName));
#endif
        if (!String.IsNullOrEmpty(jsonStr))
        {
            T data = JsonConvert.DeserializeObject<T>(jsonStr);
            return data;
        }
        return default;
    }

}
