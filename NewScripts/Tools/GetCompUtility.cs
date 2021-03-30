using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GetCompUtility
{   
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

    public static List<Transform> FindChildCompGo<T>(Transform parent)
    {
        List<Transform> list = new List<Transform>();

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform trans = parent.GetChild(i);
            if (trans != null)
                list.Add(trans);
        }
        return list;
    }

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

    public static List<T> ConcatList<T>(List<T> list1,List<T> list2)
    {
        List<T> newList = list1.Concat(list2).ToList<T>();
        return newList;
    }
}
