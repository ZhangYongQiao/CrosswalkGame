using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[AddComponentMenu("UI/RotateButton", 201)]
public class RotateButton : CustomButton
{
    Coroutine _rotate;
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        _rotate = StartCoroutine(RotateBtn(8, transform));
    }

    IEnumerator RotateBtn(float v,Transform trans)
    {
        while (true)
        {
            trans.Rotate(Vector3.forward * v);
            yield return new WaitForSeconds(0.05f);
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        StopCoroutine(_rotate);
    }
}
