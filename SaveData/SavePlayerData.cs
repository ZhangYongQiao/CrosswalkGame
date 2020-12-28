using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using System.Collections.Generic;

public class SavePlayerData : MonoBehaviour
{

    private readonly string _path = UIManager.Instance._pathCur;             //默认存储数据路径

    private AudioSource _soundEffect;
    public AudioSource SoundEffect
    {
        get
        {
            if (_soundEffect == null)
                _soundEffect = GameObject.Find("Audio Source").transform.GetComponent<AudioSource>();
            return _soundEffect;
        }
    }

    /// <summary>
    /// 保存游戏内数据(如：玩家位置、当前场景等)
    /// </summary>
    public void WriteData()
    {
        SoundEffectManager.Instance.SoundEffect.Play();

        PlayerData data = new PlayerData();
        //获取数据
        Scene sceneTmp = SceneManager.GetActiveScene();
        PlayerDataRunTime.Instance._curScene = sceneTmp.name;

        data._curScene = PlayerDataRunTime.Instance._curScene;
        data._blood = PlayerDataRunTime.Instance._curBlood;
        data._getScore = PlayerDataRunTime.Instance._curScore;

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            //序列化PlayerData对象 保存至文件
            data._vecPos = player.transform.position;
            string jsonStrTmp = JsonUtility.ToJson(data);
            using (StreamWriter sw = new StreamWriter(_path))
            {
                sw.WriteLine(jsonStrTmp);
            }

            //回到主界面
            SceneManager.LoadScene("MainPanelScene");
            UIManager.Instance.panels.Clear();
        }
    }

    /// <summary>
    /// 删除存档 开始新游戏
    /// </summary>
    public void NewGameStart()
    {
        SoundEffectManager.Instance.SoundEffect.Play();

        File.Delete(_path);
        SceneManager.LoadScene("1");
    }

    /// <summary>
    /// 重新开始本关卡
    /// </summary>
    public void ReStart()
    {
        SoundEffectManager.Instance.SoundEffect.Play();

        Transform canvasTransTmp = GameObject.Find("Canvas").transform;
        LoadPlayerAndPanel loadPanelTmp = canvasTransTmp.GetComponent<LoadPlayerAndPanel>();
        //重置信息
        PlayerDataRunTime.Instance._curBlood = loadPanelTmp._curSceneOriginalBlood;
        PlayerDataRunTime.Instance._curScore = loadPanelTmp._cur_curSceneOriginalScore;
        SceneManager.LoadScene(loadPanelTmp._sceneCur.name);
        PlayerData.Instance._vecPos = new Vector3(0, 2, 0);
    }

}
