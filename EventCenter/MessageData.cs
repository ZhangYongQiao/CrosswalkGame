using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MessageData
{
    public object _type;

    public object _data;

    /// <summary>
    /// 数据传输
    /// </summary>
    /// <param name="value"></param>
    /// <param name="data"></param>
    public MessageData(object type,object data)
    {
        _type = type;
        _data = data;
    }

    public MessageData(object info)
    {
        _data = info;
    }

}
