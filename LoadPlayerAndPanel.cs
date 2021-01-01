﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class LoadPlayerAndPanel : MonoBehaviour
{
    //GameObject player;
    GameObject createdPanel;

    private string _savePath;

    private Transform _canvasTrans;
    public Transform CanvasTrans
    {
        get
        {
            if (_canvasTrans == null)
                _canvasTrans = GameObject.Find("Canvas").GetComponent<Transform>();
            return _canvasTrans;
        }
    }                 //获得当前场景Canvas对象

    [HideInInspector]
    public GameObject _bloodImg;                    //血量图标
    [HideInInspector]
    public Transform _childBloodImg;                //存放血量图标

    private Text _childShowScoreTxt;                

    [HideInInspector]
    public Stack<GameObject> _bloods;               //存放血量图标对象 Stack

    private Transform _infoTrans;                   //用户信息面板Trans
    private Transform _orderPanelInGame;            //菜单面板Trans
    [HideInInspector]
    public Scene _sceneCur;                        //当前场景

    private bool _isOpen = false;

    [HideInInspector]
    public int _curSceneOriginalBlood;              //进入新场景时立即记录玩家信息
    [HideInInspector]
    public int _cur_curSceneOriginalScore;

    public GameObject _gemContainer;                
    public GameObject _cherryContainer;

    private GameObject _deathLoadPanel;

    /// <summary>
    /// 加载player 并启动加载面板
    /// </summary>
    void Awake()
    {
        Init();

        Vector3 loadPosTmp = Vector3.zero;
        //场景加载时数据的四种情况
        if (File.Exists(_savePath) && PlayerData.Instance._curScene == _sceneCur.name)
        {
            PlayerDataRunTime.Instance._curPos = PlayerData.Instance._vecPos;
            PlayerDataRunTime.Instance._curBlood = PlayerData.Instance._blood;
            PlayerDataRunTime.Instance._curScore = PlayerData.Instance._getScore;
            loadPosTmp = PlayerDataRunTime.Instance._curPos;

            GemCherryRuntimeInfos.Instance._curScene = GemCherryInfos.Instance._sceneName;
            GemCherryRuntimeInfos.Instance._curCherryPos = GemCherryInfos.Instance._cherryVecLists;
            GemCherryRuntimeInfos.Instance._curGemPos = GemCherryInfos.Instance._gemVecLists;

        }
        else if (!File.Exists(_savePath) && _sceneCur.name == "1")
        {
            PlayerDataRunTime.Instance._curBlood = PlayerDataRunTime.Instance.InitBlood;
            PlayerDataRunTime.Instance._curScore = PlayerDataRunTime.Instance.InitScore;
            PlayerDataRunTime.Instance._curPos = PlayerDataRunTime.Instance.InitPos;
            loadPosTmp = PlayerDataRunTime.Instance._curPos;

            GemCherryRuntimeInfos.Instance._curScene = "1";
            GemCherryRuntimeInfos.Instance._curCherryPos = GemCherryRuntimeInfos.Instance.InitCherryPos;
            GemCherryRuntimeInfos.Instance._curGemPos = GemCherryRuntimeInfos.Instance.InitGemPos;
        }
        else if (File.Exists(_savePath) && PlayerData.Instance._curScene != _sceneCur.name)
        {
            PlayerDataRunTime.Instance._curPos = PlayerData.Instance._vecPos;
            PlayerDataRunTime.Instance._curBlood = PlayerData.Instance._blood;
            PlayerDataRunTime.Instance._curScore = PlayerData.Instance._getScore;
            loadPosTmp = PlayerDataRunTime.Instance.InitPos;

            GemCherryRuntimeInfos.Instance._curScene = _sceneCur.name;
            GemCherryRuntimeInfos.Instance._curCherryPos = GemCherryRuntimeInfos.Instance.InitCherryPos;
            GemCherryRuntimeInfos.Instance._curGemPos = GemCherryRuntimeInfos.Instance.InitGemPos;
        }
        //与角色信息不同的是，每个场景的交互对象位置都不同，所以必须分出另一种情况讨论
        else if (!File.Exists(_savePath) && _sceneCur.name != "1")
        {
            GemCherryRuntimeInfos.Instance._curScene = _sceneCur.name;
            GemCherryRuntimeInfos.Instance._curCherryPos = GemCherryRuntimeInfos.Instance.InitCherryPos;
            GemCherryRuntimeInfos.Instance._curGemPos = GemCherryRuntimeInfos.Instance.InitGemPos;
        }

        //记录第一时间进入当前场景的用户初始血量和分数
        _curSceneOriginalBlood = PlayerDataRunTime.Instance._curBlood;
        _cur_curSceneOriginalScore = PlayerDataRunTime.Instance._curScore;

        //信息填充
        {
            //显示血量
            for (int i = 0; i < PlayerDataRunTime.Instance._curBlood; i++)
            {
                GameObject go = GameObject.Instantiate(_bloodImg, _childBloodImg);
                _bloods.Push(go);
            }
            //显示得分
            _childShowScoreTxt = _infoTrans.Find("ScoreImg").Find("ShowScore").GetComponent<Text>();
            if (_childShowScoreTxt)
                _childShowScoreTxt.text = PlayerDataRunTime.Instance._curScore.ToString();

            //初始化交互对象
            foreach (var go in GemCherryRuntimeInfos.Instance._curCherryPos)
                InteractiveManager.Instance.InitInteractiveGo(InteractiveManager.Instance._cherryPrePath, go, _cherryContainer.transform);
            foreach (var go in GemCherryRuntimeInfos.Instance._curGemPos)
                InteractiveManager.Instance.InitInteractiveGo(InteractiveManager.Instance._gemPrePath, go, _gemContainer.transform);
        }

        //实例化
        GameObject prefabPlayer = Resources.Load<GameObject>("Prefabs\\Player");
        Instantiate<GameObject>(prefabPlayer, loadPosTmp, Quaternion.identity);

        //加载面板
        //StartCoroutine(LoadInGamePanel());
    }

    /// <summary>
    /// 初始化基本的数据
    /// </summary>
    private void Init()
    {
        _sceneCur = SceneManager.GetActiveScene();

        AudioBGMManager.Instance.PlayBgm(_sceneCur.name);
        _savePath = UIManager.Instance.PlayerDataPath;
        _bloods = new Stack<GameObject>();
        //切换场景后清空字典
        UIManager.Instance.panels.Clear();
        //加载面板
        UIManager.Instance.LoadPanel("OrderPanelInGame", CanvasTrans);
        UIManager.Instance.LoadPanel("PlayerInfoPanelInGame", CanvasTrans);

        _infoTrans = CanvasTrans.Find("PlayerInfoPanelInGame" + "(Clone)");
        _orderPanelInGame = CanvasTrans.Find("OrderPanelInGame" + "(Clone)");
        _orderPanelInGame.Find("MusicPanel").Find("Slider").GetComponent<Slider>().value = PlayerPrefs.GetFloat("soundValue");
        _orderPanelInGame.gameObject.SetActive(false);
        PlayerDataRunTime.Instance._curScene = _sceneCur.name;

        _childBloodImg = _infoTrans.Find("BloodGroup");
        _bloodImg = Resources.Load("Prefabs/UIPrefabs/" + "BloodImg") as GameObject;

        float soundValueTmp = PlayerPrefs.GetFloat("soundValue");
        GameObject.Find("Audio Source").GetComponent<AudioSource>().volume = soundValueTmp;
        GameObject.Find("BgmSource").GetComponent<AudioSource>().volume = soundValueTmp;

        _deathLoadPanel = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/UIPrefabs/DeathLoadPanel"), GameObject.Find("Canvas").transform);
        _deathLoadPanel.SetActive(false);
    }

    private void Update()
    {
        //呼出菜单
        if(Input.GetKeyDown(KeyCode.Escape) && !_isOpen)
        {
            _isOpen = !_isOpen;
            _orderPanelInGame.gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && _isOpen)
        {
            _isOpen = !_isOpen;
            _orderPanelInGame.gameObject.SetActive(false);
        }

        //实时更新得分
        _childShowScoreTxt.text = PlayerDataRunTime.Instance._curScore.ToString();

    }


    /// <summary>
    /// 设置面板两秒后显示，并添加点击事件
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadInGamePanel()
    {
        switch (_sceneCur.name)
        {   
            case "1":
                createdPanel = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs\\HistoryDscri\\FirstPosPanel"));
                break;
            case "2":
                createdPanel = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs\\HistoryDscri\\SecondPosPanel"));
                break;
            default:
                Debug.LogError("没有这个面板");
                break;
        }
        //实例化并设置父物体
        createdPanel.transform.SetParent(CanvasTrans, false);

        //添加UI动画
        AddUIScaleAnim(createdPanel.transform,Vector3.one,0.3f);

        //给关闭按钮添加监听
        Transform btnTrans = createdPanel.transform.Find("Button");
        if (btnTrans)
            btnTrans.GetComponent<Button>().onClick.AddListener(ClosePanel);

        yield return null;
    }

    /// <summary>
    /// 为一个ui组件添加缩放动画
    /// </summary>
    /// <param name="trans">组件的Transform</param>
    /// <param name="targetValue">缩放目标值</param>
    /// <param name="duration">所用时长</param>
    /// <param name="scaleX">初始值</param>
    /// <param name="scaleY">初始值</param>
    /// <param name="scaleZ">初始值</param>
    private void AddUIScaleAnim(Transform trans,Vector3 targetValue,float duration,float scaleX = 0, float scaleY = 0, float scaleZ = 0)
    {
        trans.localScale = new Vector3(scaleX,scaleY,scaleZ);
        trans.DOScale(targetValue, duration);
    }

    /// <summary>
    /// 经过一段时间自动销毁面板
    /// </summary>
    /// <returns></returns>
    IEnumerator DestroyPanel()
    {
        GameObject panel = GameObject.Find("FirstPosPanel(Clone)");
        if (panel)
        {
            Destroy(panel, 2f);
        }
        yield return null;
    }

    /// <summary>
    /// 关闭窗口
    /// </summary>
    public void ClosePanel()
    {
        AddUIScaleAnim(createdPanel.transform, Vector3.zero, 0.3f, 1, 1, 1);
        Destroy(createdPanel,0.5f);
    }

}
