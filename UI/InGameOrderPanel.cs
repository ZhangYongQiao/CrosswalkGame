using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class InGameOrderPanel : BaseUI
{
    public CustomButton SaveBtn;
    public CustomButton BackMainBtn;
    public RotateButton Setting;

    protected override void Awake()
    {
        base.Awake();
        SaveBtn.onClick.AddListener(OnSaveBtn);
        BackMainBtn.onClick.AddListener(OnBackMainBtn);
        Setting.onClick.AddListener(OnSettingBtn);
    }

    private void OnSettingBtn()
    {
        Log.Info("OnSettingBtn");

    }

    private void OnBackMainBtn()
    {
        Log.Info("OnBackMainBtn");
    }

    private void OnSaveBtn()
    {
        Log.Info("OnSaveBtn");
    }

    private void OnDestroy()
    {
        SaveBtn.onClick.RemoveListener(OnSaveBtn);
        BackMainBtn.onClick.RemoveListener(OnBackMainBtn);
        Setting.onClick.RemoveListener(OnSettingBtn);
    }
}
