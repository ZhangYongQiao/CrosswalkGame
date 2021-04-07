using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainPanel : BaseUI
{
    public CustomButton BeginButton;
    public CustomButton ResumeButton;
    public CustomButton ExitButton;
    public CustomButton SettingButton;


    protected override void Awake()
    {
        base.Awake();

        BeginButton.onClick.AddListener(OnBeginBtn);
        ResumeButton.onClick.AddListener(OnResumeBtn);
        ExitButton.onClick.AddListener(OnExitBtn);
        SettingButton.onClick.AddListener(OnSettingBtn);
    }

    private void OnSettingBtn()
    {
        UIManager.Instance.ShowUI(PrefabConst.SettingPanel);
    }

    private void OnExitBtn()
    {
    }

    private void OnResumeBtn()
    {
    }

    private void OnBeginBtn()
    {
        string name = "1";
        MessageData data = new MessageData(name);
        MessageCenter.Instance.Send(MessageName.OnToloadScene, data);
        SceneManager.LoadScene("Loading");
    }

    private void LoadNextScene()
    {

    }




}
