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
    public const string MonsterTag = "Monster";

    public const string SoundMusicKey = "SoundMusic";
    public const string SoundEffectKey = "SoundEffect";

    public const string InitPath = "InitDataFolder";

    public static string SceneName = null;

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

        List<Monster> monsterList = GetMonsterList();
        List<Vector3> gemPosList = GetCompUtility.FindAllTag(GemTag);
        List<Vector3> cherryPosList = GetCompUtility.FindAllTag(CherryTag);

        JsonToWrite<Player>(player,RolePath);
        JsonToWrite<List<Monster>>(monsterList, MonsterPath);
        JsonToWrite<List<Vector3>>(gemPosList, GemPath);
        JsonToWrite<List<Vector3>>(cherryPosList, CherryPath);
    }

    private static List<Monster> GetMonsterList()
    {
        List<Monster> monsterList = new List<Monster>();
        GameObject box = GameObject.Find("MonsterBox(Clone)");
        if (box)
        {
            for (int i = 0; i < box.transform.childCount; i++)
            {
                Transform trans = box.transform.GetChild(i);
                Monster monster = new Monster();
                if (trans.name.Remove(trans.name.Length - 7, 7) == "Eagle")
                {
                    monster.monsterType = MonsterType.Eagle;
                    monster.Pos = trans.position;
                    monsterList.Add(monster);
                }
                else if (trans.name.Remove(trans.name.Length - 7, 7) == "Opossum")
                {
                    monster.monsterType = MonsterType.Frog;
                    monster.Pos = trans.position;
                    monsterList.Add(monster);
                }
                else
                {
                    monster.monsterType = MonsterType.Opossum;
                    monster.Pos = trans.position;
                    monsterList.Add(monster);
                }
            }
        }
        else
        {
            Log.Error("DataUtility-GetMonsterList:获取怪物信息失败");
            return null;
        }
        return monsterList;
    }

#endregion

    #region 读取数据

    public static List<Vector3> ReadInitGemData(string level)
    {
        string newStr = string.Format("Level_{0}_Gem.json", level);
        TextAsset initData = Resources.Load<TextAsset>(Path.Combine(InitPath, newStr));
        List<Vector3> postion = JsonConvert.DeserializeObject<List<Vector3>>(initData.text) as List<Vector3>;
        return postion;
    }

    public static List<Vector3> ReadInitCherryData(string level)
    {
        string newStr = string.Format("Level_{0}_Cherry.json", level);
        TextAsset initData = Resources.Load<TextAsset>(Path.Combine(InitPath, newStr));
        List<Vector3> postion = JsonConvert.DeserializeObject<List<Vector3>>(initData.text) as List<Vector3>;
        return postion;
    }

    public static List<Monster> ReadInitMonsterData(string level)
    {
        string newStr = string.Format("Level_{0}_Monster.json", level);
        TextAsset initData = Resources.Load<TextAsset>(Path.Combine(InitPath, newStr));
        List<Monster> monsters = JsonConvert.DeserializeObject<List<Monster>>(initData.text) as List<Monster>;
        return monsters;
    }

    public static Player ReadInitPlayerData(string level)
    {
        string newStr = string.Format("Level_{0}_Player.json", level);
        TextAsset initData = Resources.Load<TextAsset>(Path.Combine(InitPath, newStr));
        Player player = JsonConvert.DeserializeObject<Player>(initData.name) as Player;
        return player;
    }

    public static bool ReadJsonToData()
    {
#if UNITY_EDITOR
        if(!File.Exists(Path.Combine(EditorSavePath,RolePath)))
        {
            UIManager.Instance.ShowUI(PrefabConst.DataNotExistPanel);
            Log.Error("读取文件出错");
            return false;
        }
#else
        if(!File.Exists(Path.Combine(StandardAloneSavePath,RolePath)))
        {
            UIManager.Instance.ShowUI(PrefabConst.DataNotExistPanel);
            return false;
        }
#endif
        Player player = ReadToData<Player>(RolePath);
        List<Monster> monsterList = ReadToData<List<Monster>>(MonsterPath);
        List<Vector3> gemListPos = ReadToData<List<Vector3>>(GemPath);
        List<Vector3> cherryListPos = ReadToData<List<Vector3>>(CherryPath);

        if(player == null || monsterList == null || monsterList.Count == 0 || gemListPos == null 
            || gemListPos.Count ==0 || cherryListPos == null || cherryListPos.Count == 0)
        {
            Log.Error("数据加载出错");
        }

        CurPlayer.Instance.Pos = player.Pos;
        CurPlayer.Instance.Blood = player.Blood;
        CurPlayer.Instance.Scene = player.Scene;
        CurPlayer.Instance.Score = player.Score;

        CurMonster.Instance.MonsterList = monsterList; 

        CurGem.Instance.PosList = gemListPos;
        CurCherry.Instance.PosList = cherryListPos;

        return true;
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
            Log.Error("传入数据为空");
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
            Log.Error("文件中可能不存在数据");
        return default;
    }
}
