using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class FloatTextManager
{
    private FloatTextManager() { }

    private static FloatTextManager _floatTextManager;
    public static FloatTextManager Instance
    {
        get
        {
            if(_floatTextManager == null)
            {
                _floatTextManager = new FloatTextManager();
            }
            return _floatTextManager;
        }
    }

    public const float LIFE_TIME = 1.3f;                     //生存时间
    public const int MAX_EXIST_NUM = 3;                     //同时存在最大数

    public Queue<FloatTextPrefab> _queue;
    public Queue<string> _cacheQueue;

    public void ShowFT(string desc)
    {   
        if (_queue == null) _queue = new Queue<FloatTextPrefab>();
        if (_cacheQueue == null) _cacheQueue = new Queue<string>();

        if(_queue.Count == MAX_EXIST_NUM)
        {
            _cacheQueue.Enqueue(desc);
        }
        else
        {
            DoEnqueue(_queue, desc);
        }
    }

    public void DoEnqueue(Queue<FloatTextPrefab> queue ,string desc)
    {
        GameObject go = LoadUtility.InstantiateUIPrefabs(PrefabConst.FloatTextPrefab, UIManager.Instance.ShowCanvasGo.transform, true);
        go.GetComponent<FloatTextPrefab>().DescText.text = desc;
        queue.Enqueue(go.GetComponent<FloatTextPrefab>());

        MessageCenter.Instance.Send(MessageName.OnTellEnqueue);

        DoShowFT(go, 590f-50 * (queue.Count-1), 0.7f);
    }

    private void DoShowFT(GameObject go,float endValue,float duration)
    {
        go.transform.localPosition = Vector3.zero;
        go.transform.DOMoveY(endValue,duration);
    }

}
