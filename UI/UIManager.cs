using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class UIManager
{   
    private static UIManager _instance;                                                         //单例
    public static UIManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new UIManager();
            }
            return _instance;
        }
    }

    public Dictionary<string, GameObject> panels = new Dictionary<string, GameObject>();        //保存已加载面板数据

    public string PlayerDataPath
    {
        get
        {
#if(UNITY_EDITOR || UNITY_STANDALONE)
            return Path.Combine(Application.dataPath, "PlayerData.txt");
#else
            return Path.Combine(Application.persistentDataPath, "data.txt");
#endif
        }
    }

    public string InteractiveDataPath
    {
        get
        {
#if (UNITY_EDITOR || UNITY_STANDALONE)
            return Path.Combine(Application.dataPath, "InteractiveData.txt");
#else
            return Path.Combine(Application.persistentDataPath, "InteractiveData.txt");
#endif
        }
    }


    /// <summary>
    /// 加载面板
    /// </summary>
    /// <param name="panelName">需要加载的名称</param>
    /// <param name="parentTrans">认定父物体，可为null</param>
    /// <param name="parentTrans">是否重复创建</param>
    public GameObject LoadPanel(string panelName,Transform parentTrans = null)
    {
        //如果面板字典中存在，则直接设置SetActive
        if (panels.ContainsKey(panelName))
        {
            panels[panelName].SetActive(true);
            return panels[panelName];
        }
        //将资源加载至内存
        var tipPanelPreTmp = Resources.Load("Prefabs/UIPrefabs/"+ panelName) as GameObject;          
        //不为空则实例化、加入字典
        if(tipPanelPreTmp)
        {
            GameObject tipPanelGoTmp = GameObject.Instantiate(tipPanelPreTmp, parentTrans);
            panels.Add(panelName, tipPanelGoTmp);
            return tipPanelGoTmp;
        }
        return null;
    }

    /// <summary>
    /// 删除存档、初始化用户数据、清空字典
    /// </summary>
    public void ClickYes()
    {
        File.Delete(PlayerDataPath);                                        //删除数据

        PlayerData.Instance._vecPos = new Vector3(0,2,0);            //覆盖存档即初始化玩家数据(注：在LoadPanelAndPlayer里面是读取PlayerData数据加载玩家信息的)
        PlayerData.Instance._curScene = "1";
        PlayerData.Instance._blood = PlayerDataRunTime.Instance.InitBlood;
        PlayerData.Instance._getScore = PlayerDataRunTime.Instance.InitScore;
        SceneManager.LoadScene(PlayerData.Instance._curScene);

        panels.Clear();                                              //不清空,再次回到主界面会找不到键值对，报错
    }

    /// <summary>
    /// 关闭窗口(设置SetActive)
    /// </summary>
    /// <param name="panelName">需要关闭面板的名称</param>
    public void ClickNo(string panelName)
    {
        SoundEffectManager.Instance.PlaySoundEffect();
        string nameTmp = panelName + "(Clone)";
        GameObject goTmp = GameObject.Find(nameTmp);
        if (goTmp)
            goTmp.SetActive(false);
    }
}
