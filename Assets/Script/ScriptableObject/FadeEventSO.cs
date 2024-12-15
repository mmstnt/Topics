using OpenCvSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/FadeEventSO")] 
public class FadeEventSO : ScriptableObject
{
    public UnityAction<Color, float, bool> OnEventRaised;

    public void FadeIn(float duration) 
    {
        raiseEvent(Color.black, duration, true);
    }

    public void FadeOut(float duration) 
    {
        raiseEvent(Color.clear, duration, false);
    }

    public void raiseEvent(Color target, float duration, bool fadeIn) 
    {
        OnEventRaised?.Invoke(target, duration, fadeIn);
    }
}
