using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BaseUI
{
    public Slider EffectSlider;
    public Slider MusicSlider;
    public CustomButton ExitBtn;

    public AudioSource Effect;
    public AudioSource Music;

    protected override void Awake()
    {
        base.Awake();
        ExitBtn.onClick.AddListener(OnCloseBtn);
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

    private void OnChangeMusicValue(float value)
    {
        DataUtility.SetSoundValue(DataUtility.SoundMusicKey, (int)value);
        SetVolume(value, Music);
    }

    private void OnChangeEffectValue(float value)
    {
        DataUtility.SetSoundValue(DataUtility.SoundEffectKey, (int)value);
        SetVolume(value, Effect);
    }

    private void SetVolume(float value, AudioSource audio) { audio.volume = value / 100; }

    /// <summary>
    /// 每次打开面板自动设置数据
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

    private void SetSoundValue(int value, Slider slider)
    {
        slider.value = value;
    }

    private void OnCloseBtn()
    {
        UIManager.Instance.HideUI(PrefabConst.SettingPanel);
    }

}
