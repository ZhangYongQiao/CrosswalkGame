using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FloatTextPrefab : MonoBehaviour
{
    public Text DescText;
    private Coroutine coroutine;

    private void Awake()
    {
        MessageCenter.Instance.Register(MessageName.OnTellEnqueue,ToDequeueHandler);
    }

    private void OnDestroy()
    {
        MessageCenter.Instance.Remove(MessageName.OnTellEnqueue, ToDequeueHandler);
    }

    private void ToDequeueHandler(MessageData obj)
    {
        if(FloatTextManager.Instance._queue.Peek() != null)
        {
            coroutine = StartCoroutine(AutoDestroy(this.gameObject));
        }
    }


    IEnumerator AutoDestroy(GameObject go, float time = FloatTextManager.LIFE_TIME)
    {
        yield return new WaitForSeconds(time);
        FloatTextManager.Instance._queue.Dequeue();
        GameObject.Destroy(go);
        SettingPanel.canClick = true;               //限制连续点击出现队列对象出错
        foreach (var item in FloatTextManager.Instance._queue)
        {
            item.gameObject.transform.DOMoveY(item.gameObject.transform.position.y+50f, 0.3f);
        }
        yield return new WaitForSeconds(0.3f);
        if (FloatTextManager.Instance._cacheQueue.Count > 0)
        {
            string info = FloatTextManager.Instance._cacheQueue.Dequeue();
            FloatTextManager.Instance.DoEnqueue(FloatTextManager.Instance._queue, info);
        }

#if UNITY_EDITOR
        Log.Info("对象已出列");
#endif
        StopCoroutine(coroutine);
    }

}
