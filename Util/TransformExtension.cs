using System.Collections.Generic;
using UnityEngine;

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

    /// <summary>
    /// 查找某个物体的子物体的所有子物体并返回（找孙子）
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="index">子物体的下标</param>
    /// <returns>返回Transform集合</returns>
    public static List<Transform> FindGrandsons(this Transform trans,int index)
    {
        List<Transform> lists = new List<Transform>();
        Transform sonTransTmp = trans.GetChild(index);
        if (sonTransTmp)
        {
            for (int i = 0; i < sonTransTmp.childCount; i++)
            {
                lists.Add(sonTransTmp.GetChild(i));
            }
            return lists;
        }
        return null;
    }
}
