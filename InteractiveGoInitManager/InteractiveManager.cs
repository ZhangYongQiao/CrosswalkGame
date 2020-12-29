using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveManager : MonoBehaviour
{   
    public List<BaseInfo> _cherryInfos;                     //设置加载信息
    public List<BaseInfo> _gemInfos;

    private Transform _cherryGo;                            //获得CherryContianer Transform
    private Transform _gemGO;                               //获得GemContianer Transform

    private readonly string _cherryPrePath = "Prefabs/cherry";       //资源加载路径
    private readonly string _gemPrePath = "Prefabs/gem";

    private void Awake()
    {
        _gemGO = GameObject.Find("GemsContianer").transform;
        _cherryGo = GameObject.Find("CherrysContianer").transform;

        InitInteractiveGo(_cherryPrePath, _cherryGo, _cherryInfos);
        InitInteractiveGo(_gemPrePath, _gemGO, _gemInfos);
    }

    /// <summary>
    /// 根据给定信息，实例化交互对象
    /// </summary>
    /// <param name="loadPath">加载资源路径</param>
    /// <param name="setParent">父对象</param>
    /// <param name="goInfo">给定信息</param>
    private void InitInteractiveGo(string loadPath,Transform setParent,List<BaseInfo> goInfo)
    {
        if (!setParent)
            Debug.LogError("未找到要设置的父对象");
        GameObject goPreTmp = Resources.Load<GameObject>(loadPath);
        if (goPreTmp)
        {
            foreach (var info in goInfo)
            {
                for (int i = 0; i < info._muchNumPerGroup; i++)
                {
                    GameObject cherryTmp = GameObject.Instantiate(goPreTmp, setParent);
                    cherryTmp.transform.position = new Vector3(info._startPosition.x + info._offset * i, info._startPosition.y, info._startPosition.z);
                }
            }
        }
    }
}
