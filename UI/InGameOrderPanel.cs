using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;

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
        UIManager.Instance.ShowUI(PrefabConst.SettingPanel);
    }

    private void OnBackMainBtn()
    {
        FloatTextManager.Instance._queue.Clear();
        FloatTextManager.Instance._cacheQueue.Clear();

        Destroy(GameObject.Find("SoundEffect(Clone)"));
        Destroy(GameObject.Find("SoundMusic(Clone)"));
        Destroy(GameObject.Find("UICanvas(Clone)"));
        UIManager.Instance.Clear();
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        DataUtility.SceneName = "MainPanel";
        SceneManager.LoadScene("Loading");
        Log.Info("OnBackMainBtn");
    }

    private void OnSaveBtn()
    {
        MessageCenter.Instance.Send(MessageName.OnGetPlayerPos);
        DataUtility.WriteDataToJson();
    }

    private void OnDestroy()
    {
        SaveBtn.onClick.RemoveListener(OnSaveBtn);
        BackMainBtn.onClick.RemoveListener(OnBackMainBtn);
        Setting.onClick.RemoveListener(OnSettingBtn);
    }
}
