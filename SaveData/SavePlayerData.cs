using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

public class SavePlayerData : MonoBehaviour
{

    private string _playerDataPath;                               //默认存储玩家数据路径
    private string _interactiveDataPath;                          //默认存储交互物品数据路径

    //private AudioSource _soundEffect;
    //public AudioSource SoundEffect
    //{
    //    get
    //    {
    //        if (_soundEffect == null)
    //            _soundEffect = GameObject.Find("Audio Source").transform.GetComponent<AudioSource>();
    //        return _soundEffect;
    //    }
    //}

    private Transform _interactiveManagerGo;

    private void Awake()
    {
        _playerDataPath = UIManager.Instance.PlayerDataPath;             //初始化
        _interactiveDataPath = UIManager.Instance.InteractiveDataPath;
        _interactiveManagerGo = GameObject.Find("InteractiveManager").transform;
    }

    /// <summary>
    /// 保存游戏内数据(如：玩家位置、当前场景等)
    /// </summary>
    public void WriteData()
    {
        Scene sceneTmp = SceneManager.GetActiveScene();

        GemCherryInfos infosTmp = new GemCherryInfos();
        //找出场景当前存在的交互对象
        List<Transform> gemListTmp = _interactiveManagerGo.FindGrandsons(0);         //Gem
        List<Transform> cherryListTmp = _interactiveManagerGo.FindGrandsons(1);      //Cherry
        List<Vector3> gemVecsTmp = new List<Vector3>();
        List<Vector3> cherryVecsTmp = new List<Vector3>();
        if (gemListTmp != null)
        {
            foreach (var e in gemListTmp)
            {
                gemVecsTmp.Add(e.position);
            }
        }
        if (cherryListTmp != null)
        {
            foreach (var e in cherryListTmp)
            {
                cherryVecsTmp.Add(e.position);
            }
        }
        //合并集合
        infosTmp._cherryVecLists = cherryVecsTmp;
        infosTmp._gemVecLists = gemVecsTmp;
        infosTmp._sceneName = sceneTmp.name;
        string interavtiveJsonTmp = JsonUtility.ToJson(infosTmp);
        using (StreamWriter sw = new StreamWriter(_interactiveDataPath))
        {
            sw.WriteLine(interavtiveJsonTmp);
        }

        //播放音效
        SoundEffectManager.Instance.SoundEffect.Play();
        
        //获取数据
        PlayerData data = new PlayerData();
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
            using (StreamWriter sw = new StreamWriter(_playerDataPath))
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

        File.Delete(_playerDataPath);
        File.Delete(_interactiveDataPath);
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

        //GemCherryRuntimeInfos.Instance._curGemPos = 
    }

}
