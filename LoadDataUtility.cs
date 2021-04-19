using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using DG.Tweening;
using System;

public class LoadDataUtility : MonoBehaviour
{
    private void Awake()
    {
        LoadAll();
    }

    bool isOut = false;
    InGameOrderPanel info = null;
    public void PopInGameOrder()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) && !isOut)
        {
            info = UIManager.Instance.ShowUI<InGameOrderPanel>(PrefabConst.InGameOrderPanel);
            Vector3 vec = info.transform.position;
            info.transform.DOMove(vec + new Vector3(-420, 0, 0), 0.5f);
            isOut = true;
            MessageData data = new MessageData(EffectType.Pop);
            MessageCenter.Instance.Send(MessageName.OnPlaySoundEffect, data);
        }
        else if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) && isOut)
        {
            Vector3 vec = info.transform.position;
            info.transform.DOMove(vec + new Vector3(420, 0, 0), 0.5f);
            isOut = false;
            StartCoroutine(CallBackHide());
            MessageData data = new MessageData(EffectType.Pop);
            MessageCenter.Instance.Send(MessageName.OnPlaySoundEffect, data);
        }
    }

    private void Update()
    {
        PopInGameOrder();
    }

    IEnumerator CallBackHide()
    {
        yield return new WaitForSeconds(0.5f);
        UIManager.Instance.HideUI(PrefabConst.InGameOrderPanel);
    }

    private void LoadAll()
    {
        InitUI();
        InitPlayer();
        InitCamera();
        InitMonster();
        InitCherry();
        InitGem();
    }

    GameObject show;
    GameObject hide;
    private void InitUI()
    {
        //show = GameObject.FindGameObjectWithTag("ShowCanvas");
        //hide= GameObject.FindGameObjectWithTag("HideCanvas");

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
            GameObject go = LoadUtility.InstantiateOtherPrefabs(PrefabConst.GemPrefab, LoadUtility.CherryPath, gems.transform);
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
        camera.transform.position = new Vector3(10, 10, 10);
    }

    private void InitPlayer()
    {
        GameObject player = LoadUtility.InstantiateOtherPrefabs(PrefabConst.Player, LoadUtility.OtherPath);
        player.transform.position = new Vector3(10, 5, 10);
    }

}
