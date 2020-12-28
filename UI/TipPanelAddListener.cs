using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipPanelAddListener : MonoBehaviour
{
    Button _tipBtn;

    private void Awake()
    {
        _tipBtn = GetComponent<Button>();
        if (_tipBtn)
        {
            _tipBtn.onClick.AddListener(()=> { UIManager.Instance.ClickNo("NoDataTipPanel"); });
        }
    }

    
}
