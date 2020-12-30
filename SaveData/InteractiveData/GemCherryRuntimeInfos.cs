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

    private Dictionary<string, List<Vector3>> _savePosInfos;

    private string _loadConfigPath;

    public List<Vector3> InitPos
    {
        get
        {
            switch (_curScene)
            {
                case "1":
                    _loadConfigPath = "Config/GemCherryConfig_1";
                    break;
                case "2":
                    _loadConfigPath = "Config/GemCherryConfig_2";
                    break;
                default:
                    break;
            }
            if (_savePosInfos == null)
                _savePosInfos = new Dictionary<string, List<Vector3>>();
            //字典中有，则直接从字典中取
            if (_savePosInfos.ContainsKey(_curScene)) { return _savePosInfos[_curScene]; }

            List<Vector3> _lists = new List<Vector3>();
            //读取配置文件，反序列化
            TextAsset txtAssetTmp = Resources.Load<TextAsset>(_loadConfigPath);
            BaseInfos infosTmp = JsonUtility.FromJson<BaseInfos>(txtAssetTmp.text);

            foreach (var baseInfo in infosTmp._baseInfos)
            {
                _lists.Add(baseInfo._position);
            }
            //将每个场景加载的交互对象位置集合加入到字典
            _savePosInfos.Add(_curScene, _lists);

            return _lists;
        }
    }

    public List<Vector3> _curPos;

    public string _curScene;

}
