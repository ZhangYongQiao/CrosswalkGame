using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShowInfoPanel : BaseUI
{
    public RectTransform BloodList;
    public Text Score;
    public Image Blood;

    private static int scoreValue = 0;
    private static int bloodValue = 0;
    private Stack<GameObject> _bloodStack;

    protected override void Awake()
    {
        base.Awake();
        MessageCenter.Instance.Register(MessageName.OnAddScore, OnAddScoreHandler);
        MessageCenter.Instance.Register(MessageName.OnNoticeInitPlayerData, OnInitPlayerDataHandler);
        MessageCenter.Instance.Register(MessageName.OnPlayerHurt, OnPlayerHurtHandler);
    }


    private void OnDestroy()
    {
        MessageCenter.Instance.Remove(MessageName.OnAddScore, OnAddScoreHandler);
        MessageCenter.Instance.Remove(MessageName.OnNoticeInitPlayerData, OnInitPlayerDataHandler);
        MessageCenter.Instance.Remove(MessageName.OnPlayerHurt, OnPlayerHurtHandler);
    }

    private void OnPlayerHurtHandler(MessageData obj)
    {
        UpdateBlood();
    }

    private void UpdateBlood()
    {
        bloodValue -= 1;
        if(bloodValue <= 0)
        {   
            Log.Info("TODO:角色死亡");
            //TODO  

        }
        CurPlayer.Instance.Blood = bloodValue;
        GameObject go = _bloodStack.Pop();
        StartCoroutine(SubBlood(1f,go));
    }

    private IEnumerator SubBlood(float time,GameObject go)
    {
        Image image = go.GetComponent<Image>();
        if (image != null)
        {
            image.DOFade(0f, time);
            go.transform.DOScale(0, time);
        }
        yield return new WaitForSeconds(time);
        DestroyImmediate(go);
    }

    private void AddBlood(float time, GameObject go)
    {   
        Image image = go.GetComponent<Image>();
        image.color = new Color(255, 255, 255, 0);
        if (image != null)
        {
            image.DOFade(1f, time);
            go.transform.DOScale(1, time);
            SetAnimForBlood(go);
        }
    }

    private void OnInitPlayerDataHandler(MessageData obj)
    {
        Player data = obj._data as Player;
        InitData(data);
    }

    private void InitData(Player data)
    {
        scoreValue = data.Score;
        bloodValue = data.Blood;

        CurPlayer.Instance.Blood = bloodValue;
        CurPlayer.Instance.Score = scoreValue;
        CurPlayer.Instance.Scene = data.Scene;

        Score.text = scoreValue.ToString();
        if (bloodValue > 5 || bloodValue < 5)
        {
            Log.Error("初始化血量出错");
            return;
        }
        if (_bloodStack == null) _bloodStack = new Stack<GameObject>();
        for (int i = 0; i < bloodValue; i++)
        {
            GameObject blood = Instantiate(Blood.gameObject, BloodList);
            blood.SetActive(true);
            _bloodStack.Push(blood);
            SetAnimForBlood(blood);
        }
    }

    private void SetAnimForBlood(GameObject blood)
    {
        Tweener tweener = blood.transform.DOScale(Vector3.one * 1.1f, 1f);
        tweener.SetLoops(-1, LoopType.Yoyo);
    }

    private void OnAddScoreHandler(MessageData obj)
    {
        ScoreType type = (ScoreType)obj._data;
        switch (type)
        {
            case ScoreType.Gem:
                scoreValue += 1;
                CurPlayer.Instance.Score = scoreValue;
                Score.text = scoreValue.ToString();
                FloatTextManager.Instance.ShowFT("<color=#2894FF>宝石得分 +1</color>");
                break;
            case ScoreType.Cherry:
                if(CurPlayer.Instance.Blood == 5)
                {
                    scoreValue += 3;
                    CurPlayer.Instance.Score = scoreValue;
                    Score.text = scoreValue.ToString();
                    FloatTextManager.Instance.ShowFT("<color=#FF2D2D>樱桃得分 +3</color>");
                }
                else
                {
                    bloodValue += 1;
                    CurPlayer.Instance.Blood = bloodValue;
                    GameObject go = GameObject.Instantiate(Blood.gameObject, BloodList);
                    if (_bloodStack == null)
                        _bloodStack = new Stack<GameObject>();
                    FloatTextManager.Instance.ShowFT("<color=#BF0060>樱桃加血 +1</color>");
                    _bloodStack.Push(go);
                    go.SetActive(true);
                    AddBlood(1f, go);
                    if (_bloodStack.Count > 5)
                    {
                        Log.Error("栈溢出");
                    }
                }
                break;
            case ScoreType.DieMonster:
                scoreValue += 5;
                CurPlayer.Instance.Score = scoreValue;
                Score.text = scoreValue.ToString();
                FloatTextManager.Instance.ShowFT("<color=#2828FF>怪物得分 +5</color>");
                break;
            default:
                Log.Error("加分出错");
                break;
        }
    }
}
