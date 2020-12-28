using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BaseInfo
{   
    [Range(1,6)]
    public  int _muchNumPerGroup;            //以组的形式出现，每组1-6个

    public  Vector3 _startPosition;         //每组的起始位置

    [Range(1.5f,2f)]
    public float _offset;                  //间隔
}
