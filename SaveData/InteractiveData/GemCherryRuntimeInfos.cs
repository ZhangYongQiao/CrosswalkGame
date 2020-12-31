using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemCherryRuntimeInfos
{
    private static GemCherryRuntimeInfos _instance;
    public static GemCherryRuntimeInfos Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GemCherryRuntimeInfos();
            return _instance;
        }
    }

    private Dictionary<string, List<Vector3>> _saveCherryPosInfos;
    private Dictionary<string, List<Vector3>> _saveGemPosInfos;

    private string _loadCherryConfigPath;
    private string _loadGemConfigPath;

    public List<Vector3> InitCherryPos
    {
        get
        {
            switch (_curScene)
            {
                case "1":
                    _loadCherryConfigPath = "Config/CherryConfig_1";
                    break;
                case "2":
                    _loadCherryConfigPath = "Config/CherryConfig_2";
                    break;
                default:
                    break;
            }
            if (_saveCherryPosInfos == null)
                _saveCherryPosInfos = new Dictionary<string, List<Vector3>>();
            //字典中有，则直接从字典中取
            if (_saveCherryPosInfos.ContainsKey(_curScene)) { return _saveCherryPosInfos[_curScene]; }

            List<Vector3> cherrylistsTmp = new List<Vector3>();
            //读取配置文件，反序列化
            TextAsset cherryTxtAssetTmp = Resources.Load<TextAsset>(_loadCherryConfigPath);

            CherryInfos cherryInfosTmp = JsonUtility.FromJson<CherryInfos>(cherryTxtAssetTmp.text);

            foreach (var cherry in cherryInfosTmp._cherryInfos)
                cherrylistsTmp.Add(cherry._position);

            //将每个场景加载的交互对象位置集合加入到字典
            _saveCherryPosInfos.Add(_curScene, cherrylistsTmp);

            return cherrylistsTmp;
        }
    }

    public List<Vector3> InitGemPos
    {
        get
        {
            switch (_curScene)
            {
                case "1":
                    _loadGemConfigPath = "Config/GemConfig_1";
                    break;
                case "2":
                    _loadGemConfigPath = "Config/GemConfig_2";
                    break;
                default:
                    break;
            }
            if (_saveGemPosInfos == null)
                _saveGemPosInfos = new Dictionary<string, List<Vector3>>();
            //字典中有，则直接从字典中取
            if (_saveGemPosInfos.ContainsKey(_curScene)) { return _saveGemPosInfos[_curScene]; }

            List<Vector3> gemlistsTmp = new List<Vector3>();
            //读取配置文件，反序列化
            TextAsset gemTxtAssetTmp = Resources.Load<TextAsset>(_loadGemConfigPath);

            GemInfos gemInfosTmp = JsonUtility.FromJson<GemInfos>(gemTxtAssetTmp.text);

            foreach (var gem in gemInfosTmp._gemInfos)
                gemlistsTmp.Add(gem._position);

            //将每个场景加载的交互对象位置集合加入到字典
            _saveGemPosInfos.Add(_curScene, gemlistsTmp);

            return gemlistsTmp;
        }
    }

    public List<Vector3> _curCherryPos;
    public List<Vector3> _curGemPos;

    public string _curScene;

}
