using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{

    private string _playerDataPath;                                           //默认数据存储路径
    private string _interactiveDataPath;

    private Transform _canvasTrans;                                           //提供Canvas组件Transform信息
    private Transform CanvasTrans
    {
        get
        {
            if (_canvasTrans == null)
            {
                _canvasTrans = GameObject.Find("Canvas").transform;
                if (_canvasTrans == null)
                {
                    Debug.LogError("获取Canvas对象失败");
                    return null;
                }
            }
            return _canvasTrans;
        }
    }

    private AudioSource _soundEffect;

    private void Awake()
    {
        _playerDataPath = UIManager.Instance.PlayerDataPath;
        _interactiveDataPath = UIManager.Instance.InteractiveDataPath;
        AudioBGMManager.Instance.PlayBgm();

        _soundEffect = GameObject.Find("Audio Source").GetComponent<AudioSource>();

        PlayerPrefs.SetFloat("soundValue", _soundEffect.volume);
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void BeginBtn()
    {
        SoundEffectManager.Instance.PlaySoundEffect();
        PlayerPrefs.SetFloat("soundValue",_soundEffect.volume);

        if (!File.Exists(_playerDataPath))
        {
            SceneManager.LoadScene("Loading");
        }
        else
        {
            GameObject go = UIManager.Instance.LoadPanel("WarningPanel", CanvasTrans);
            List<Transform> childsTransTmp = go.transform.GetChildContainComp<Button>();
            foreach (var child in childsTransTmp)
            {
                Button btnTmp = child.GetComponent<Button>();
                btnTmp.onClick.AddListener(SoundEffectManager.Instance.PlaySoundEffect);
            }
        }
    }

    /// <summary>
    /// 读取存档
    /// </summary>
    public void ReadData()
    {
        SoundEffectManager.Instance.PlaySoundEffect();

        //文件存在 则读取信息 并反序列化为对象 并给PlayerData字段赋值
        if (File.Exists(_playerDataPath))
        {
            string jsonStrTmp = null;
            using (StreamReader sr = new StreamReader(_playerDataPath))
            {
                jsonStrTmp = sr.ReadToEnd();
            }
            PlayerData dataTmp = JsonUtility.FromJson<PlayerData>(jsonStrTmp);
            if(dataTmp != null)
            {
                PlayerData.Instance._vecPos = dataTmp._vecPos;
                PlayerData.Instance._curScene = dataTmp._curScene;
                PlayerData.Instance._blood = dataTmp._blood;
                PlayerData.Instance._getScore = dataTmp._getScore;
            }

            string interactiveJsonStrTmp = null;
            using (StreamReader sr = new StreamReader(_interactiveDataPath))
            {
                interactiveJsonStrTmp = sr.ReadToEnd();
            }
            GemCherryInfos infosTmp = JsonUtility.FromJson<GemCherryInfos>(interactiveJsonStrTmp);
            if (infosTmp != null)
            {
                GemCherryInfos.Instance._cherryVecLists = infosTmp._cherryVecLists;
                GemCherryInfos.Instance._gemVecLists = infosTmp._gemVecLists;
                GemCherryInfos.Instance._sceneName = infosTmp._sceneName;
            }
        }
        else
        {   //提示面板
            UIManager.Instance.LoadPanel("NoDataTipPanel",CanvasTrans);
            return;
        }
        //加载存档场景
        PlayerPrefs.SetFloat("soundValue", _soundEffect.volume);
        SceneManager.LoadSceneAsync(PlayerData.Instance._curScene);
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void ExitBtn()
    {
        SoundEffectManager.Instance.PlaySoundEffect();
        PlayerPrefs.SetFloat("soundValue", _soundEffect.volume);
        StartCoroutine(ExitDelay());
    }

    IEnumerator ExitDelay()
    {
        yield return new WaitForSeconds(0.2f);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        StartCoroutine(ExitDelay());
#endif
    }

}
