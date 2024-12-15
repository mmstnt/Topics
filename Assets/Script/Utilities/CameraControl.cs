using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Header("®∆•Û∫ ≈•")]
    public VoidEventSO afterSceneLoadEvent;
    private CinemachineConfiner2D confiner2D;

    private void Awake()
    {
        confiner2D = transform.GetComponent<CinemachineConfiner2D>();
    }

    private void OnEnable()
    {
        afterSceneLoadEvent.onEventRaised += onAfterSceneLoadEvent;
    }

    private void OnDisable()
    {
        afterSceneLoadEvent.onEventRaised -= onAfterSceneLoadEvent;
    }

    private void onAfterSceneLoadEvent()
    {
        GetNewCameraBound();
    }

    private void GetNewCameraBound() 
    {
        var obj = GameObject.FindGameObjectWithTag("Bounds");
        if (obj == null) 
            return;
        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
        confiner2D.InvalidateCache();
    }
}
