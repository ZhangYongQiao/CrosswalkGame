using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class TransformExtension
{
    /// <summary>
    /// 获得子级所有带指定组件的物体Transform集合
    /// </summary>
    /// <param name="trans">父对象Transform</param>
    /// <returns>返回Transform集合</returns>
    public static List<Transform> GetChildContainComp<T1>(this Transform trans)
    {
        List<Transform> childs = new List<Transform>();
        for (int i = 0; i < trans.childCount; i++)
        {
            Transform childTransTmp =  trans.GetChild(i);
            T1 tTmp = childTransTmp.GetComponent<T1>();
            if (tTmp != null)
            {
                childs.Add(childTransTmp);
            }
        }
        return childs;
    }
}
