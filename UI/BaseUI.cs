using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUI : MonoBehaviour
{
    protected CanvasGroup CanvasGroup;

    protected virtual void Awake()
    {
        CanvasGroup = gameObject.AddComponent<CanvasGroup>();
#if UNITY_EDITOR
        Log.Info("BaseUI : 组件添加成功 !");
#endif
    }

}
