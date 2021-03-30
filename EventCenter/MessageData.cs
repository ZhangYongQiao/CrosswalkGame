using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MessageData
{
    public int valueInt;
    public bool valueBool;
    public float valueFloat;

    public MessageData(int value)
    {
        valueInt = value;
    }
    public MessageData(float value)
    {
        valueFloat = value;
    }
    public MessageData(bool value)
    {
        valueBool = value;
    }

}
