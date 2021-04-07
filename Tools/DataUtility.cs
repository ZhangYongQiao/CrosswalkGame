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
    public const string GemPath = "GemData.txt";
    public const string CherryPath = "CherryData.txt";

    public const string GemTag = "Gem";
    public const string CherryTag = "Cherry";

    public const string SoundMusicKey = "SoundMusic";
    public const string SoundEffectKey = "SoundEffect";

    #region PlayerPrefs

    public static void SetSoundValue(string key,int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public static int GetSoundValue(string key)
    {
        return PlayerPrefs.GetInt(key);
    }

    #endregion

    #region 存储数据

    /// <summary>
    /// 以json方式写入文件
    /// </summary>
    public static void WriteDataToJson()
    {
        Player player = new Player
        {
            Pos = CurPlayer.Instance.Pos,
            Blood = CurPlayer.Instance.Blood,
            Score = CurPlayer.Instance.Score,
            Scene = CurPlayer.Instance.Scene
        };

        List<Transform> monsterPosList = GetCompUtility.GetAllSameCompGo<MonsterMove>();
        List<Transform> gemPosList = GetCompUtility.FindAllTag(GemTag);
        List<Transform> cherryPosList = GetCompUtility.FindAllTag(CherryTag);

        JsonToWrite<Player>(player,RolePath);
        JsonToWrite<List<Transform>>(monsterPosList, MonsterPath);
        JsonToWrite<List<Transform>>(gemPosList, GemPath);
        JsonToWrite<List<Transform>>(cherryPosList, CherryPath);
    }

#endregion

    #region 读取数据

    public static void ReadJsonToData()
    {
        Player player = ReadToData<Player>(RolePath);
        List<Transform> monsterPosList = ReadToData<List<Transform>>(MonsterPath);
        List<Transform> gemListPos = ReadToData<List<Transform>>(GemPath);
        List<Transform> cherryListPos = ReadToData<List<Transform>>(CherryPath);

        if(player == null || monsterPosList == null || monsterPosList.Count == 0 || gemListPos == null || gemListPos.Count ==0
            || cherryListPos == null || cherryListPos.Count == 0)
        {
            Debug.LogError("数据加载出错");
        }

        CurPlayer.Instance.Pos = player.Pos;
        CurPlayer.Instance.Blood = player.Blood;
        CurPlayer.Instance.Scene = player.Scene;
        CurPlayer.Instance.Score = player.Score;

        CurMonster.Instance.PosList = monsterPosList;
        CurGem.Instance.PosList = gemListPos;
        CurCherry.Instance.PosList = cherryListPos;
    }

    #endregion

    /// <summary>
    /// 序列化，写入文件
    /// </summary>
    /// <typeparam name="T">序列化的类型</typeparam>
    /// <param name="data">数据</param>
    /// <param name="fileName">文件名</param>
    private static void JsonToWrite<T>(T data, string fileName) where T : new()
    {
        if (data != null)
        {
            string jsonTxt = JsonConvert.SerializeObject(data);
#if UNITY_EDITOR
            FileOperation.WriteFile(Path.Combine(EditorSavePath, fileName),jsonTxt,false);
#else
            FileOperation.WriteFile(Path.Combine(StandardAloneSavePath, fileName),jsonTxt);
#endif
        }
        else
            Debug.LogError("传入数据为空");
    }

    /// <summary>
    /// 反序列化文件
    /// </summary>
    /// <typeparam name="T">对应类型</typeparam>
    /// <param name="fileName">文件名</param>
    /// <returns>返回类型数据</returns>
    private static T ReadToData<T>(string fileName) where T : new()
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
        else
            Debug.LogError("文件中可能不存在数据");
        return default;
    }

}
