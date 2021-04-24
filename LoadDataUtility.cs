using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LoadDataUtility : MonoBehaviour
{
    private void Awake()
    {
        LoadAll();
    }

    private void Update()
    {
        PopInGameOrder();
    }

    bool isOut = false;
    InGameOrderPanel info = null;
    bool canClick = true;
    Coroutine coroutine;
    public void PopInGameOrder()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) && !isOut && canClick)
        {
            canClick = false;
            if (info == null)
                info = UIManager.Instance.ShowUI<InGameOrderPanel>(PrefabConst.InGameOrderPanel);
            else
                UIManager.Instance.ShowUI(PrefabConst.InGameOrderPanel);
            Vector3 vec = info.transform.position;
            info.transform.DOMove(vec + new Vector3(-420, 0, 0), 0.5f);
            isOut = true;
            MessageData data = new MessageData(EffectType.Pop);
            MessageCenter.Instance.Send(MessageName.OnPlaySoundEffect, data);
        }
        else if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) && isOut && !canClick)
        {
            Vector3 vec = info.transform.position;
            info.transform.DOMove(vec + new Vector3(420, 0, 0), 0.5f);
            isOut = false;
            coroutine = StartCoroutine(CallBackHide());
            MessageData data = new MessageData(EffectType.Pop);
            MessageCenter.Instance.Send(MessageName.OnPlaySoundEffect, data);
        }
    }

    IEnumerator CallBackHide()
    {
        yield return new WaitForSeconds(0.5f);
        UIManager.Instance.HideUI(PrefabConst.InGameOrderPanel,false);
        canClick = true;
        StopCoroutine(coroutine);
    }

    private void LoadAll()
    {
        InitUI();
        if (!DataUtility.isContinue)
        {
            InitPlayer();
            InitCamera();
            InitMonster();
            InitCherry();
            InitGem();
            InitDescPanel();
        }
        else
        {
            DataUtility.ReadData();
            InitCamera();
        }
        
        PlayBgmOfActiveScene();
        //手动创建事件系统
        List<Transform> list = GetCompUtility.FindCompInAll<EventSystem>();
        if (list == null || list.Count == 0)
        {
            UIManager.Instance.CreateEventSystem();
        }
        CurPlayer.Instance.Scene = SceneManager.GetActiveScene().name;
    }

    private void PlayBgmOfActiveScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        switch (scene.name)
        {
            case "1":
                MessageData data = new MessageData(LevelBgm.one);
                MessageCenter.Instance.Send(MessageName.OnPlaySoundBgm, data);
                break;
            default:
                break;
        }
    }
    
    private void InitUI()
    {
        UIManager.Instance.ShowUI(PrefabConst.ShowInfoPanel);
    }

    private void InitGem()
    {
        GameObject gems = null;
        if (GameObject.Find("GemBox") == null)
            gems = new GameObject("GemBox");
        List<Vector3> lists = DataUtility.ReadInitGemData("1");
        for (int i = 0; i < lists.Count; i++)
        {
            GameObject go = LoadUtility.InstantiateOtherPrefabs(PrefabConst.GemPrefab, LoadUtility.GemPath, gems.transform);
            go.transform.position = lists[i];
        }
    }

    private void InitCherry()
    {
        GameObject cherrys = null;
        if (GameObject.Find("CherryBox") == null)
            cherrys = new GameObject("CherryBox");
        List<Vector3> lists = DataUtility.ReadInitCherryData("1");
        for (int i = 0; i < lists.Count; i++)
        {
            GameObject go = LoadUtility.InstantiateOtherPrefabs(PrefabConst.CherryPrefab, LoadUtility.CherryPath, cherrys.transform);
            go.transform.position = lists[i];
        }
    }

    private void InitMonster()
    {
        GameObject monster = null;
        if (GameObject.Find("MonsterBox") == null)
            monster = new GameObject("MonsterBox");

        List<Monster> monsters = DataUtility.ReadInitMonsterData("1");
        foreach (var item in monsters)
        {
            if (item.monsterType == MonsterType.Eagle)
            {
                GameObject go = LoadUtility.InstantiateMonsterPrefabs(PrefabConst.EaglePrefab, monster.transform, false);
                go.transform.position = item.Pos;
            }
            else if (item.monsterType == MonsterType.Opossum)
            {
                GameObject go = LoadUtility.InstantiateMonsterPrefabs(PrefabConst.OpossumPrefab, monster.transform, false);
                go.transform.position = item.Pos;
            }
            else
            {
                GameObject go = LoadUtility.InstantiateMonsterPrefabs(PrefabConst.FrogPrefab, monster.transform, false);
                go.transform.position = item.Pos;
            }
        }
    }

    private void InitCamera()
    {
        GameObject camera = LoadUtility.InstantiateOtherPrefabs(PrefabConst.FollowCamera, LoadUtility.OtherPath);
        camera.transform.position = new Vector3(3, -3, 10);
    }

    private void InitPlayer()
    {
        GameObject player = LoadUtility.InstantiateOtherPrefabs(PrefabConst.Player, LoadUtility.OtherPath);
        Player data = DataUtility.ReadInitPlayerData("1");
        player.transform.position = data.Pos;

        MessageData info = new MessageData(data);
        MessageCenter.Instance.Send(MessageName.OnNoticeInitPlayerData, info);

    }

    private void InitDescPanel()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        switch (sceneName)
        {
            case "1":
                Level_one_Desc level = UIManager.Instance.ShowUI<Level_one_Desc>(PrefabConst.Level_one_Desc);
                AddUIScaleAnim(level.transform, Vector3.one,0.5f);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 添加动画
    /// </summary>
    /// <param name="trans">组件的Transform</param>
    /// <param name="targetValue">缩放目标值</param>
    /// <param name="duration">所用时长</param>
    private void AddUIScaleAnim(Transform trans, Vector3 targetValue, float duration, float scaleX = 0, float scaleY = 0, float scaleZ = 0)
    {
        trans.localScale = new Vector3(scaleX, scaleY, scaleZ);
        trans.DOScale(targetValue, duration);
    }
}
