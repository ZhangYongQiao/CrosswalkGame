using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveManager
{
    private static InteractiveManager _instance;
    public static InteractiveManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new InteractiveManager();
            return _instance;
        }
    }

    public readonly string _cherryPrePath = "Prefabs/cherry";       //资源加载路径
    public readonly string _gemPrePath = "Prefabs/gem";

    private Dictionary<string, GameObject> _goes;

    /// <summary>
    /// 根据给定信息，实例化交互对象
    /// </summary>
    /// <param name="loadPath">加载资源路径</param>
    /// <param name="setParent">父对象</param>
    /// <param name="goPos">给定位置信息</param>
    public void InitInteractiveGo(string loadPath, Vector3 goPos, Transform setParent = null)
    {
        if (_goes == null)
        {
            _goes = new Dictionary<string, GameObject>();
        }
        if (_goes.ContainsKey(loadPath))
        {
            GameObject.Instantiate<GameObject>(_goes[loadPath], goPos, Quaternion.identity, setParent);
            return;
        }

        GameObject go = Resources.Load<GameObject>(loadPath);
        GameObject.Instantiate<GameObject>(go, goPos, Quaternion.identity, setParent);
        _goes.Add(loadPath, go);
    }
}
