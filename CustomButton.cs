﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

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
        MessageData data = new MessageData(EffectType.Button);
        MessageCenter.Instance.Send(MessageName.OnPlaySoundEffect,data);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        MessageData data = new MessageData(EffectType.ButtonEnter);
        MessageCenter.Instance.Send(MessageName.OnPlaySoundEffect, data);
        DOScale(new Vector3(1.1f, 1.1f, 1f), 0.2f);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        DOScale(Vector3.one, 0.15f);
    }

    private void DOScale(Vector3 vec,float t)
    {
        this.transform.DOScale(vec, t);
    }

}
