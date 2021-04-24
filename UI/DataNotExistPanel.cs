using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataNotExistPanel : BaseUI
{
    public Button Button;

    protected override void Awake()
    {
        base.Awake();
        Button.onClick.AddListener(OnClickPanel);
    }

    private void OnClickPanel()
    {
        UIManager.Instance.HideUI(PrefabConst.DataNotExistPanel,false);
    }
}
