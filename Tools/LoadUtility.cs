using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;

public class LoadUtility
{
    public const string UIPath = "Prefabs/UIPrefabs";
    public const string MonstersPath = "Prefabs/Monster";
    public const string SoundPath = "Prefabs/SoundPrefabs";
    public const string GemPath = "Prefabs";
    public const string CherryPath = "Prefabs";
    public const string OtherPath = "Prefabs";
    public const string LoadingScene = "Loading";
    public const string FirstScene = "1";


    public static Dictionary<string,GameObject> m_ObjectPrefabsDic;

    #region Resources
    public static void LoadPrefabs(string name, string path)
    {
        GameObject go = Resources.Load<GameObject>(Path.Combine(path, name));
        if (go == null)
            Log.Error(string.Format("加载预制体{0}失败。", name));
        else
            AddDic(name, go);
    }

    /// <summary>
    /// 加载UI预制体
    /// </summary>
    /// <param name="name">预制体名称</param>
    /// <param name="path">预制体路径</param>
    public static void LoadUIPrefabs(string name,string path = UIPath)
    {
         GameObject go = Resources.Load<GameObject>(Path.Combine(path, name));
        if (go == null)
            Log.Error(string.Format("加载预制体{0}失败。", name));
        else
            AddDic(name, go);
    }
    
    /// <summary>
    /// 加载怪物预制体
    /// </summary>
    /// <param name="name"></param>
    /// <param name="path"></param>
    public static void LoadMonsterPrefabs(string name, string path = MonstersPath)
    {
        GameObject go = Resources.Load<GameObject>(Path.Combine(path, name));
        if (go == null)
            Log.Error(string.Format("加载预制体{0}失败。", name));
        else
            AddDic(name, go);
    }

    /// <summary>
    /// 加入到字典
    /// </summary>
    /// <param name="name">预制体名称</param>
    /// <param name="go">加载后的对象</param>
    private static void AddDic(string name,GameObject go)
    {
        if (m_ObjectPrefabsDic == null)
            m_ObjectPrefabsDic = new Dictionary<string, GameObject>();

        if (!m_ObjectPrefabsDic.ContainsKey(name))
        {
            m_ObjectPrefabsDic.Add(name, go);
        }
        else
            return;
    }
    #endregion

    #region Instantiate

    /// <summary>
    /// UI实例化
    /// </summary>
    /// <param name="name"></param>
    /// <param name="parent"></param>
    /// <param name="worldPosStay"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static GameObject InstantiateUIPrefabs(string name,Transform parent,bool worldPosStay,string path= UIPath)
    {
        if (m_ObjectPrefabsDic == null) m_ObjectPrefabsDic = new Dictionary<string, GameObject>();
        if(!m_ObjectPrefabsDic.ContainsKey(name))
            LoadUIPrefabs(name, path);

        if (!m_ObjectPrefabsDic.ContainsKey(name))
        {
            Log.Error("实例化失败，字典中不存在此预制体");
            return null;
        }
        GameObject go = GameObject.Instantiate(m_ObjectPrefabsDic[name], parent, worldPosStay);
        return go;
    }

    /// <summary>
    /// 怪物实例化
    /// </summary>
    /// <param name="name"></param>
    /// <param name="parent"></param>
    /// <param name="worldPosStay"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static GameObject InstantiateMonsterPrefabs(string name, Transform parent, bool worldPosStay, string path=MonstersPath)
    {
        if (!m_ObjectPrefabsDic.ContainsKey(name))
            LoadMonsterPrefabs(name, path);

        if (!m_ObjectPrefabsDic.ContainsKey(name))
        {
            Log.Error("实例化失败，字典中不存在此预制体");
            return null;
        }
        return GameObject.Instantiate(m_ObjectPrefabsDic[name], parent, worldPosStay);
    }

    /// <summary>
    /// 通用实例化
    /// </summary>
    /// <returns></returns>
    public static GameObject InstantiateOtherPrefabs(string name,string path,Transform parent = null)
    {
        if (m_ObjectPrefabsDic == null) m_ObjectPrefabsDic = new Dictionary<string, GameObject>();
        if (m_ObjectPrefabsDic.ContainsKey(name))
            return GameObject.Instantiate(m_ObjectPrefabsDic[name], parent, false);
        
        LoadPrefabs(name, path);
        return GameObject.Instantiate(m_ObjectPrefabsDic[name], parent, false);
    }
    #endregion
}
