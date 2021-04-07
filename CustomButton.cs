using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[AddComponentMenu("UI/CustomButton",200)]
public class CustomButton : Button
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        MessageCenter.Instance.Send(MessageName.OnPlaySoundEffect);
    }
}
