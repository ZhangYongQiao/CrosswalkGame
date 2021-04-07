using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void Init()
    {
        ShowUI(PrefabConst.MainPanel);
        LoadUtility.InstantiateOtherPrefabs(PrefabConst.SoundEffects, LoadUtility.SoundPath,null);
        LoadUtility.InstantiateOtherPrefabs(PrefabConst.SoundMusic, LoadUtility.SoundPath,null);
    }

    /// <summary>
    /// 显示UI
    /// </summary>
    /// <param name="name">UI名称</param>
    public void ShowUI(string name)
    {   
        if(_hideUIDic != null && _hideUIDic.ContainsKey(name))
        {
            MoveToShow(name);
            return;
        }

        if (_instantiatedDic == null) _instantiatedDic = new Dictionary<string, GameObject>();
        if (_popUI == null) _popUI = new Stack<GameObject>();

        GameObject go;
        go = LoadUtility.InstantiateUIPrefabs(name,ShowCanvasGo.transform,false);
        _instantiatedDic.Add(name, go);
        PushUI(go);
    }

    /// <summary>
    /// 关闭UI
    /// </summary>
    /// <param name="name">UI名称</param>
    public void HideUI(string name)
    {
        if (_hideUIDic == null) _hideUIDic = new Dictionary<string, GameObject>();
        if (_instantiatedDic.ContainsKey(name))
        {
            MoveToHide(name);
        }
        else
        {
            Debug.LogError("未实例化该物体");
            return;
        }
    }

    /// <summary>
    /// 关闭窗口时，将其移动至HideCanvas下
    /// </summary>
    /// <param name="name">UI名称</param>
    private void MoveToHide(string name)
    {
        _instantiatedDic[name].transform.SetParent(HideCanvasGo.transform);
        _hideUIDic.Add(name, _instantiatedDic[name]);
        _hideUIDic[name].SetActive(false);
        PopUI(_hideUIDic[name]);
    }

    /// <summary>
    /// 显示窗口时，将其移动至ShowCanvas下
    /// </summary>
    /// <param name="name">UI名称</param>
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
    private void PopUI(GameObject go)
    {
        if (_popUI.Count > 1)
        {
            _popUI.Pop();
            _popUI.Peek().GetComponent<CanvasGroup>().interactable = true;
        }
        else if (_popUI.Count == 1)
            _popUI.Pop();
        else
            Debug.LogError("栈为空");
    }
}
