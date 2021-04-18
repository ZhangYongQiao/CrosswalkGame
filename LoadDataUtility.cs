using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class LoadDataUtility : MonoBehaviour
{
    private void Awake()
    {
        LoadAll();
    }

    private void LoadAll()
    {
        InitPlayer();
        InitCamera();
        InitMonster();
        InitCherry();
        InitGem();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject go = UIManager.Instance.ShowUI(PrefabConst.InGameOrder);
            transform.DOMoveX(-420, 0.5f);
        }
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
