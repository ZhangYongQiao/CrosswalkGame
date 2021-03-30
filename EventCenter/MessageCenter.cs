using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MessageCenter
{
    private static MessageCenter _instance = null;
    public static MessageCenter Instance
    {
        get
        {
            if (_instance == null)
                _instance = new MessageCenter();
            return _instance;
        }
    }

    private Dictionary<string, Action<MessageData>> _dicMessage;

    private MessageCenter()
    {
        _dicMessage = new Dictionary<string, Action<MessageData>>();
    }

    public void Register(string key,Action<MessageData> action)
    {
        if (!_dicMessage.ContainsKey(key))
        {
            _dicMessage.Add(key, action);
        }
        else
        {
            if (_dicMessage[key] == action)
                return;
            else
                _dicMessage[key] += action;
        }

    }

    public void Remove(string key, Action<MessageData> action)
    {
        if(_dicMessage.ContainsKey(key) && _dicMessage[key] != null)
        {
            _dicMessage[key] -= action;
        }
    }

    public void Send(string key , MessageData data = null)
    {
        if(_dicMessage.ContainsKey(key) && _dicMessage[key] != null)
        {
            _dicMessage[key].Invoke(data);
        }
    }

    public void Clear()
    {
        _dicMessage.Clear();
    }

}
