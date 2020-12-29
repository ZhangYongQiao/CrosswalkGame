using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AudioBGMManager
{
    private static AudioBGMManager _instance;
    public static AudioBGMManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AudioBGMManager();
            }
            return _instance;
        }
    }

    private AudioSource _audioBgmSource;
    public AudioSource AudioBgmSource
    {
        get
        {
            if (_audioBgmSource == null)
            {
                _audioBgmSource = GameObject.Find("BgmSource").transform.GetComponent<AudioSource>();
                GameObject.DontDestroyOnLoad(_audioBgmSource.gameObject);
            }
            return _audioBgmSource;
        }
    }

    private Dictionary<string, string> _clipsPath;                      //场景名和路径
    private Dictionary<string, AudioClip> _clips;                       //场景名和BGM


    /// <summary>
    /// 运行指定场景的BGM
    /// </summary>
    /// <param name="sceneName">场景名</param>
    public void PlayBgm(string sceneName = "MainPanelScene")
    {   
        //初始化
        if (_clips == null)
            _clips = new Dictionary<string, AudioClip>();

        if (_clipsPath == null)
        {   
            //从配置文件读取反序列化为对象
            _clipsPath = new Dictionary<string, string>();
            TextAsset txTmp = Resources.Load<TextAsset>("Config/AudioConfig");
            //添加至_clipsPath
            Music musicTmp = JsonUtility.FromJson<Music>(txTmp.text);
            foreach (var music in musicTmp._musicInfos)
            {
                _clipsPath.Add(music._sceneName, music._clipName);
            }
        }
        //防止多次加载
        if (_clips.ContainsKey(sceneName))
        {
            AudioBgmSource.clip = _clips[sceneName];
        }
        else
        {
            AudioClip clipTmp = Resources.Load<AudioClip>(_clipsPath[sceneName]);
            if (clipTmp)
                _clips.Add(sceneName, clipTmp);
            AudioBgmSource.clip = clipTmp;
        }
        AudioBgmSource.Play();

    }
}
