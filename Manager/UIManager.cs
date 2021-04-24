using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new UIManager();
            return _instance;
        }
    }

    private GameObject _showCanvasGo;
    public GameObject ShowCanvasGo
    {
        get
        {
            if (_showCanvasGo == null)
                _showCanvasGo = GameObject.FindGameObjectWithTag("ShowCanvas");
            return _showCanvasGo;
        }
    }

    private GameObject _hideCanvasGo;
    public GameObject HideCanvasGo
    {
        get
        {
            if (_hideCanvasGo == null)
                _hideCanvasGo = GameObject.FindGameObjectWithTag("HideCanvas");
            return _hideCanvasGo;
        }
    }

    private Dictionary<string, GameObject> _instantiatedDic;
    private Dictionary<string, GameObject> _hideUIDic;

    private Stack<GameObject> _popUI;

    public void Clear()
    {
        if (_instantiatedDic != null)
            _instantiatedDic.Clear();
        if (_hideUIDic != null)
            _hideUIDic.Clear();
        if (_popUI != null)
            _popUI.Clear();
    }


    public void Init()
    {
        GameObject go = LoadUICanvas();
        GameObject.DontDestroyOnLoad(go);
        ShowUI(PrefabConst.MainPanel);
        CreateAudioListener();
        CreateEventSystem();

        //实例化音源
        GameObject soundEffect = LoadUtility.InstantiateOtherPrefabs(PrefabConst.SoundEffects, LoadUtility.SoundPath,null);
        GameObject soundMusic = LoadUtility.InstantiateOtherPrefabs(PrefabConst.SoundMusic, LoadUtility.SoundPath,null);
        GameObject.DontDestroyOnLoad(soundEffect);
        GameObject.DontDestroyOnLoad(soundMusic);
        SetVolume(soundEffect.GetComponent<SoundEffect>(), soundMusic.GetComponent<SoundMusic>());
        MessageData data = new MessageData(LevelBgm.MainPanel);
        MessageCenter.Instance.Send(MessageName.OnPlaySoundBgm, data);
    }

    //设置音效音量大小
    private void SetVolume(SoundEffect effect,SoundMusic music)
    {
        if (PlayerPrefs.HasKey(DataUtility.SoundEffectKey))
        {
            effect.AudioSource.volume = DataUtility.GetSoundValue(DataUtility.SoundEffectKey)/100;
        }
        if (PlayerPrefs.HasKey(DataUtility.SoundMusicKey))
        {
            music.AudioSource.volume = DataUtility.GetSoundValue(DataUtility.SoundMusicKey)/100;
        }
    }


    //隐藏父物体的所有子物体
    public void MoveAllChildToHide(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform trans = parent.GetChild(i);
            HideUI(GetCompUtility.SubClone(trans.name),false);
        }
    }

    /// <summary>
    /// 显示UI
    /// </summary>
    /// <param name="name">UI名称</param>
    public T ShowUI<T>(string name) where T : UnityEngine.Component
    {
        T comp;
        if(_hideUIDic != null && _hideUIDic.ContainsKey(name))
        {
            MoveToShow<T>(name,out comp);
            return comp;
        }

        if (_instantiatedDic == null) _instantiatedDic = new Dictionary<string, GameObject>();
        if (_popUI == null) _popUI = new Stack<GameObject>();

        GameObject go;
        go = LoadUtility.InstantiateUIPrefabs(name,ShowCanvasGo.transform,false);
        comp = go.GetComponent<T>();
        _instantiatedDic.Add(name, go);
        PushUI(go);
        return comp;
    }

    public void ShowUI(string name)
    {
        if (_hideUIDic != null && _hideUIDic.ContainsKey(name))
        {
            MoveToShow(name);
            return ;
        }

        if (_instantiatedDic == null) _instantiatedDic = new Dictionary<string, GameObject>();
        if (_popUI == null) _popUI = new Stack<GameObject>();

        GameObject go;
        go = LoadUtility.InstantiateUIPrefabs(name, ShowCanvasGo.transform, false);
        _instantiatedDic.Add(name, go);
        PushUI(go);
    }

    /// <summary>
    /// 关闭UI
    /// </summary>
    /// <param name="name">UI名称</param>
    public void HideUI(string name,bool isDestroy)
    {   
        if (_hideUIDic == null) _hideUIDic = new Dictionary<string, GameObject>();
        if (_instantiatedDic.ContainsKey(name))
        {   
            MoveToHide(name,isDestroy);
        }
        else
        {
            Log.Error("未实例化该物体");
            return;
        }
    }

    /// <summary>
    /// 关闭窗口时，将其移动至HideCanvas下
    /// </summary>
    /// <param name="name">UI名称</param>
    private void MoveToHide(string name,bool isDestroy)
    {   
        if(isDestroy)
        {
            PopUI();
            GameObject.Destroy(_instantiatedDic[name]);
            return;
        }
        _instantiatedDic[name].transform.SetParent(HideCanvasGo.transform);
        _hideUIDic.Add(name, _instantiatedDic[name]);
        _hideUIDic[name].SetActive(false);
        PopUI();
    }

    /// <summary>
    /// 显示窗口时，将其移动至ShowCanvas下
    /// </summary>
    /// <param name="name">UI名称</param>
    private void MoveToShow<T>(string name,out T comp) where T : Component
    {
        _hideUIDic[name].transform.SetParent(ShowCanvasGo.transform);
        _hideUIDic[name].SetActive(true);
        comp = _hideUIDic[name].GetComponent<T>();
        PushUI(_hideUIDic[name]);
        _hideUIDic.Remove(name);
    }

    private void MoveToShow(string name)
    {
        _hideUIDic[name].transform.SetParent(ShowCanvasGo.transform);
        _hideUIDic[name].SetActive(true);
        PushUI(_hideUIDic[name]);
        _hideUIDic.Remove(name);
    }

    /// <summary>
    /// 每次入栈时，做判断，将上一个对象（若存在）禁用
    /// </summary>
    /// <param name="go"></param>
    private void PushUI(GameObject go)
    {
        if (_popUI.Count > 0)
        {
            _popUI.Peek().GetComponent<CanvasGroup>().interactable = false;
            _popUI.Push(go);
        }
        else if (_popUI.Count == 0)
            _popUI.Push(go);
        go.GetComponent<CanvasGroup>().interactable = true;
    }

    /// <summary>
    /// 关闭窗口时，出栈
    /// </summary>
    /// <param name="go"></param>
    private void PopUI()
    {
        if (_popUI.Count > 1)
        {
            _popUI.Pop();
            _popUI.Peek().GetComponent<CanvasGroup>().interactable = true;
        }
        else if (_popUI.Count == 1)
            _popUI.Pop();
        else
            Log.Error("栈为空");
    }

    #region 创建基本组件
    public GameObject LoadUICanvas()
    {
        GameObject canvas = Resources.Load<GameObject>("Prefabs/UIPrefabs/UICanvas");
        GameObject go = GameObject.Instantiate<GameObject>(canvas);
        return go;
    }

    public void CreateAudioListener()
    {
        GameObject gameObject = new GameObject("AudioListener");
        gameObject.AddComponent<AudioListener>();
    }

    public void CreateEventSystem()
    {
        GameObject gameObject = new GameObject("EventSystem");
        gameObject.AddComponent<EventSystem>();
        gameObject.AddComponent<StandaloneInputModule>();
    }
    #endregion

}
