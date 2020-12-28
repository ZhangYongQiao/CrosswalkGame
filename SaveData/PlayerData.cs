using UnityEngine;

/// <summary>
/// 用户数据
/// </summary>
[System.Serializable]
public class PlayerData
{
    [System.NonSerialized]
    private static PlayerData _instance;                     //单例
    public static PlayerData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PlayerData();
            }
            return _instance;
        }
    }

    public Vector3 _vecPos;                                  //退出前位置
    public string  _curScene;                                //退出前场景
    public int     _blood;
    public int     _getScore;

}
