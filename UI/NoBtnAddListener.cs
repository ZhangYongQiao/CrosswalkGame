using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoBtnAddListener : MonoBehaviour
{
    Button _noBtn;

    private void Awake()
    {
        _noBtn = GetComponent<Button>();
        if (_noBtn)
        {
            _noBtn.onClick.AddListener(()=> { UIManager.Instance.ClickNo("WarningPanel"); });
        }

    }
}
