using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GetCompUtility
{
    /// <summary>
    /// 找出所有带有特定（T）组件的Transform
    /// </summary>
    /// <typeparam name="T">特定组件</typeparam>
    /// <param name="parent">指定父物体</param>
    /// <returns>满足条件的Transform集合</returns>
    public static List<Transform> GetAllSameCompGo<T>(Transform parent = null)
    {
        List<Transform> list = new List<Transform>();

        if (parent == null)
        {
            list = FindCompInAll<T>();
            return list;
        }
        else
        {
            list = FindChildCompGo<T>(parent);
            return list;
        }
    }

    /// <summary>
    /// 全局搜索带有T组件的Transform
    /// </summary>
    /// <typeparam name="T">指定组件</typeparam>
    /// <returns>满足条件的Transform集合</returns>
    public static List<Transform> FindCompInAll<T>()
    {
        List<Transform> list = new List<Transform>();
        GameObject[] allGo = GameObject.FindObjectsOfType<GameObject>();
        foreach (var item in allGo)
        {
            if (item.GetComponent<T>() != null)
            {
                list.Add(item.transform);
            }
        }
        return list;
    }

    /// <summary>
    /// 找出子物体带T组件的Transform(该方法父物体不能为空)
    /// </summary>
    /// <typeparam name="T">指定组件</typeparam>
    /// <param name="parent">指定父物体</param>
    /// <returns>满足条件的Transform集合</returns>
    public static List<Transform> FindChildCompGo<T>(Transform parent)
    {
        if (parent == null) 
        {
            Debug.LogError("此时父物体不能为空");
            return null;
        }
        List<Transform> list = new List<Transform>();
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform trans = parent.GetChild(i);
            if (trans.GetComponent<T>() != null)
                list.Add(trans);
        }
        return list;
    }

    /// <summary>
    /// 找出所有带特定标签的Transform集合
    /// </summary>
    /// <param name="tag">标签</param>
    /// <returns>返回集合</returns>
    public static List<Transform> FindAllTag(string tag)
    {
        List<Transform> list = new List<Transform>();
        GameObject[] arr = GameObject.FindGameObjectsWithTag(tag);
        for (int i = 0; i < arr.Length; i++)
        {
            list.Add(arr[i].transform);
        }
        return list;
    }

    /// <summary>
    /// 合并两个链表
    /// </summary>
    /// <typeparam name="T">链表内置类型</typeparam>
    /// <param name="list1">1</param>
    /// <param name="list2">2</param>
    /// <param name="isRepeated">链表元素是否重复</param>
    /// <returns>合并后的链表</returns>
    public static List<T> ConcatList<T>(List<T> list1,List<T> list2,bool isRepeated)
    {
        List<T> newList;
        if (!isRepeated)
            newList = list1.Union(list2).ToList<T>();
        else
            newList = list1.Concat(list2).ToList<T>();

        return newList;
    }
}
