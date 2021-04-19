using DG.Tweening;
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
    public RotateButton SettingButton;


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
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnResumeBtn()
    {
        bool isExist = DataUtility.ReadJsonToData();

        if (isExist)
        {
            string resumeScene = CurPlayer.Instance.Scene;
            DataUtility.SceneName = resumeScene;
            SceneManager.LoadScene(LoadUtility.LoadingScene);
        }
    }

    private void OnBeginBtn()
    {   
        UIManager.Instance.MoveAllChildToHide(UIManager.Instance.ShowCanvasGo.transform);
        string name = LoadUtility.FirstScene;
        DataUtility.SceneName = name;
        SceneManager.LoadScene(LoadUtility.LoadingScene);
    }

    private void OnDestroy()
    {
        BeginButton.onClick.RemoveListener(OnBeginBtn);
        ResumeButton.onClick.RemoveListener(OnResumeBtn);
        ExitButton.onClick.RemoveListener(OnExitBtn);
        SettingButton.onClick.RemoveListener(OnSettingBtn);
    }
}
