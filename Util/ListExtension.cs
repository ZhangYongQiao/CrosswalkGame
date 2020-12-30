using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtension
{
    /// <summary>
    /// 按下标查找元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static T SearchListElemnt<T>(this List<T> list,int index)
    {
        int idxTmp = 0;
        foreach (var e in list)
        {
            idxTmp++;
            if(idxTmp == index)
            {
                return e;
            }
        }
        return default;
    }

}
