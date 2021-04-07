using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MessageData
{
    public object data;

    public object param;

    /// <summary>
    /// 数据传输
    /// </summary>
    /// <param name="value"></param>
    /// <param name="info"></param>
    public MessageData(object value,object info)
    {
        data = value;
        param = info;
    }

    public MessageData(object info)
    {
        param = info;
    }

}
