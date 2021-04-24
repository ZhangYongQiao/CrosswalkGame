﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BaseUI
{
    public Slider EffectSlider;
    public Slider MusicSlider;
    public CustomButton ExitBtn;
    public CustomButton DeleteFileBtn;

    private AudioSource Effect;
    private AudioSource Music;

    protected override void Awake()
    {
        base.Awake();
        ExitBtn.onClick.AddListener(OnCloseBtn);
        DeleteFileBtn.onClick.AddListener(OnDleteFileBtn);
        EffectSlider.onValueChanged.AddListener(OnChangeEffectValue);
        MusicSlider.onValueChanged.AddListener(OnChangeMusicValue);

        GameObject effect= GameObject.FindGameObjectWithTag("SoundEffect");
        GameObject music= GameObject.FindGameObjectWithTag("SoundMusic");

        if (effect != null && music != null)
        {
            Effect = effect.GetComponent<AudioSource>();
            Music = music.GetComponent<AudioSource>();
        }
    }

    public static bool canClick = true;
    private void OnDleteFileBtn()
    {
        if (!DataUtility.FileIsExist(false) && canClick)
        {
            canClick = false;
            FloatTextManager.Instance.ShowFT("存档已删除");
            return;
        }
        else
        {
            DataUtility.DeleteAllData();
        }
    }

    private void OnChangeMusicValue(float value)
    {
        DataUtility.SetSoundValue(DataUtility.SoundMusicKey, value);
        SetVolume(value, Music);
    }

    private void OnChangeEffectValue(float value)
    {
        DataUtility.SetSoundValue(DataUtility.SoundEffectKey, value);
        SetVolume(value, Effect);
    }

    private void SetVolume(float value, AudioSource audio) { audio.volume = value / 100; }

    /// <summary>
    /// 打开面板加载数据
    /// </summary>
    private void OnEnable()
    { 
        if (PlayerPrefs.HasKey(DataUtility.SoundEffectKey))
        {
            SetSoundValue(DataUtility.GetSoundValue(DataUtility.SoundEffectKey), EffectSlider);
            SetVolume(EffectSlider.value, Effect);
        }
        if (PlayerPrefs.HasKey(DataUtility.SoundMusicKey))
        {
            SetSoundValue(DataUtility.GetSoundValue(DataUtility.SoundMusicKey), MusicSlider);
            SetVolume(MusicSlider.value, Music);
        }
    }

    private void SetSoundValue(float value, Slider slider)
    {
        slider.value = (int)value;
    }

    private void OnCloseBtn()
    {
        UIManager.Instance.HideUI(PrefabConst.SettingPanel,false);
    }

}
