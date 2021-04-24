using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Level_one_Desc : BaseUI
{
    public Button Button;

    protected override void Awake()
    {
        base.Awake();
        Button.onClick.AddListener(OnClosePanel);
    }

    private void OnClosePanel()
    {
        transform.DOScale(Vector3.zero, 0.5f);
        StartCoroutine(DelayCall(0.5f));
    }

    IEnumerator DelayCall(float stamp)
    {   
        yield return new WaitForSeconds(stamp);
        UIManager.Instance.HideUI(PrefabConst.Level_one_Desc, true);
    }
}
