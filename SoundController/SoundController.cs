using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    private AudioSource _audioGoComponent;
    private AudioSource _soundGoComponent;

    public Slider _slider;

    private bool _isMute = false;
    private float _sliderValue;                 //保存滑块的值

    private void Awake()
    {
        _audioGoComponent = GameObject.Find("Audio Source").GetComponent<AudioSource>();
        _soundGoComponent = GameObject.Find("BgmSource").GetComponent<AudioSource>();
        _slider.value = PlayerPrefs.GetFloat("soundValue");
    }

    public void Update()
    {
        _audioGoComponent.volume = _slider.value;
        _soundGoComponent.volume = _slider.value;
    }

    /// <summary>
    /// 点击图标静音，再次点击则还原
    /// </summary>
    public void SetMute()
    {
        if (!_isMute)
        {
            _sliderValue = _slider.value;
            _slider.value = 0;
            _isMute = !_isMute;
        }
        else
        {
            _slider.value = _sliderValue;
            _audioGoComponent.volume = _slider.value;
            _soundGoComponent.volume = _slider.value;
            _isMute = !_isMute;
        }
    }

}
