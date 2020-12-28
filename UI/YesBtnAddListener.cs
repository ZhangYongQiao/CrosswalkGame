using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YesBtnAddListener : MonoBehaviour
{
    Button _yesBtn;

    private void Awake()
    {
        _yesBtn = GetComponent<Button>();
        if (_yesBtn)
        {
            _yesBtn.onClick.AddListener(UIManager.Instance.ClickYes);
        }
    }

   
}
