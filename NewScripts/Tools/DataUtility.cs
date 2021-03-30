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

    public void SaveDataToJson()
    {
        Player player = new Player(CurPlayer.Pos, CurPlayer.Blood, CurPlayer.Score, CurPlayer.Scene);
        List<Transform> monsterPosList = GetCompUtility.GetAllSameCompGo<EagleAndOpossumMove>();
        List<Transform> gemPosList = GetCompUtility.FindAllTag(GemTag);
        List<Transform> cherryPosList = GetCompUtility.FindAllTag(CherryTag);
        List<Transform> concatListPos = GetCompUtility.ConcatList<Transform>(gemPosList, cherryPosList);
    }

    

    #endregion

    #region 读取数据

    #endregion

}
