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
    public static bool isContinue = false;
    public static Vector3 PlayerPos;

    #region PlayerPrefs

    public static void SetSoundValue(string key,float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    public static float GetSoundValue(string key)
    {
        return PlayerPrefs.GetFloat(key);
    }

    #endregion

    #region 存储数据

    /// <summary>
    /// 以json方式写入文件
    /// </summary>
    public static void WriteDataToJson()
    {
        CurPlayer.Instance.Pos = PlayerPos;
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
        Log.Error("保存数据成功");
    }

    private static List<Monster> GetMonsterList()
    {
        List<Monster> monsterList = new List<Monster>();
        GameObject box = GameObject.Find("MonsterBox");
        if (box.transform.childCount == 0)
        {
            return monsterList;
        }
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
                    monster.monsterType = MonsterType.Opossum;
                    monster.Pos = trans.position;
                    monsterList.Add(monster);
                }
                else
                {
                    monster.monsterType = MonsterType.Frog;
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
        Player player = JsonConvert.DeserializeObject<Player>(initData.text) as Player;
        return player;
    }

    public static bool FileIsExist(bool isShow = true)
    {
#if UNITY_EDITOR
        if(!File.Exists(Path.Combine(EditorSavePath,RolePath)) && !File.Exists(Path.Combine(EditorSavePath, MonsterPath))&&
            !File.Exists(Path.Combine(EditorSavePath, CherryPath)) && !File.Exists(Path.Combine(EditorSavePath, GemPath)))
        {   
            if(isShow)
                UIManager.Instance.ShowUI(PrefabConst.DataNotExistPanel);
            Log.Error("暂无可读取文件");
            return false;
        }
#else
        if(!File.Exists(Path.Combine(StandardAloneSavePath,RolePath))&& !File.Exists(Path.Combine(StandardAloneSavePath, MonsterPath))&&!File.Exists(Path.Combine(StandardAloneSavePath, CherryPath)) && !File.Exists(Path.Combine(StandardAloneSavePath, GemPath)))
        {
        if(isShow)
                UIManager.Instance.ShowUI(PrefabConst.DataNotExistPanel);
            return false;
        }
#endif
        return true;
    }

    public static void ReadData()
    {   
        if(!FileIsExist(true))
        {
            Log.Error("文件不存在");
            return;
        }
        Player player = ReadToData<Player>(RolePath);
        List<Monster> monsterList = ReadToData<List<Monster>>(MonsterPath);
        List<Vector3> gemListPos = ReadToData<List<Vector3>>(GemPath);
        List<Vector3> cherryListPos = ReadToData<List<Vector3>>(CherryPath);

        if (player == null || monsterList == null || gemListPos == null || cherryListPos == null)
        {
            Log.Error("数据加载出错");
            return;
        }

        CurPlayer.Instance.Pos = player.Pos;
        CurPlayer.Instance.Blood = player.Blood;
        CurPlayer.Instance.Scene = player.Scene;
        CurPlayer.Instance.Score = player.Score;
        CurMonster.Instance.MonsterList = monsterList;
        CurGem.Instance.PosList = gemListPos;
        CurCherry.Instance.PosList = cherryListPos;

        InstantiateData(player, monsterList, gemListPos, cherryListPos);
    }

    private static void InstantiateData(Player player, List<Monster> monsterList, List<Vector3> gemListPos, List<Vector3> cherryListPos)
    {
        GameObject role = LoadUtility.InstantiateOtherPrefabs(PrefabConst.Player, LoadUtility.OtherPath);
        role.transform.position = player.Pos;

        GameObject monster = null;
        if (GameObject.Find("MonsterBox") == null)
            monster = new GameObject("MonsterBox");
        foreach (var item in monsterList)
        {
            if (item.monsterType == MonsterType.Eagle)
            {
                GameObject go = LoadUtility.InstantiateMonsterPrefabs(PrefabConst.EaglePrefab, monster.transform, false);
                go.transform.position = item.Pos;
            }
            else if (item.monsterType == MonsterType.Opossum)
            {
                GameObject go = LoadUtility.InstantiateMonsterPrefabs(PrefabConst.OpossumPrefab, monster.transform, false);
                go.transform.position = item.Pos;
            }
            else
            {
                GameObject go = LoadUtility.InstantiateMonsterPrefabs(PrefabConst.FrogPrefab, monster.transform, false);
                go.transform.position = item.Pos;
            }
        }

        GameObject gems = null;
        if (GameObject.Find("GemBox") == null)
            gems = new GameObject("GemBox");
        foreach (var item in gemListPos)
        {
            
                GameObject go = LoadUtility.InstantiateOtherPrefabs(PrefabConst.GemPrefab, LoadUtility.GemPath, gems.transform);
                go.transform.position = item;
        }

        GameObject cherrys = null;
        if (GameObject.Find("CherryBox") == null)
            cherrys = new GameObject("CherryBox");
        foreach (var item in cherryListPos)
        {
            GameObject go = LoadUtility.InstantiateOtherPrefabs(PrefabConst.CherryPrefab, LoadUtility.CherryPath, cherrys.transform);
            go.transform.position = item;
        }

        Log.Info("初始化成功");
    }

    public static string ReadSaveScene()
    {
        if (!FileIsExist(false))
        {
            Log.Error("文件不存在");
            return null;
        }
        Player player = ReadToData<Player>(RolePath);
        return player.Scene;
    }

    #endregion

    public static void DeleteAllData(bool isDelete = true)
    {
        if (FileIsExist(false))
        {
#if UNITY_EDITOR
            File.Delete(Path.Combine(EditorSavePath, RolePath));
            File.Delete(Path.Combine(EditorSavePath, MonsterPath));
            File.Delete(Path.Combine(EditorSavePath, GemPath));
            File.Delete(Path.Combine(EditorSavePath, CherryPath));
            FloatTextManager.Instance.ShowFT("删除存档成功");
            Log.Info("删档成功");
#else
            File.Delete(Path.Combine(StandardAloneSavePath, RolePath));    
            File.Delete(Path.Combine(StandardAloneSavePath, MonsterPath));    
            File.Delete(Path.Combine(StandardAloneSavePath, GemPath));    
            File.Delete(Path.Combine(StandardAloneSavePath, CherryPath)); 
            FloatTextManager.Instance.ShowFT("删除存档成功");
#endif
        }
    }

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
            FileOperation.WriteFile(Path.Combine(StandardAloneSavePath, fileName),jsonTxt,false);
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
