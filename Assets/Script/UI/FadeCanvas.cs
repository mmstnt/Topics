using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeCanvas : MonoBehaviour
{
    [Header("®∆•Û∫ ≈•")]
    public FadeEventSO fadeEventSO;
    public Image fabeImage;

    private void OnEnable()
    {
        fadeEventSO.OnEventRaised += onFadeEvent;
    }

    private void OnDisable()
    {
        fadeEventSO.OnEventRaised -= onFadeEvent;
    }

    private void onFadeEvent(Color target, float duration, bool fadeIn) 
    {
        fabeImage.DOBlendableColor(target, duration);
    }
}
