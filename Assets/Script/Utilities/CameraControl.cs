using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Header("廣播")]
    public VoidEventSO cameraLensEvent;

    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("組件")]
    public GameObject playerCamera;
    public GameObject bossCamera;
    public SceneLoadManager sceneLoadManager;

    private CinemachineConfiner2D playerConfiner2D;
    private CinemachineConfiner2D bossConfiner2D;
    private CinemachineVirtualCamera playerVirtualCamera;
    private CinemachineVirtualCamera bossVirtualCamera;

    private void Awake()
    {
        playerConfiner2D = playerCamera.transform.GetComponent<CinemachineConfiner2D>();
        bossConfiner2D = bossCamera.transform.GetComponent<CinemachineConfiner2D>();
        playerVirtualCamera = playerCamera.transform.GetComponent<CinemachineVirtualCamera>();
        bossVirtualCamera = bossCamera.transform.GetComponent<CinemachineVirtualCamera>();
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
        if(sceneLoadManager.currentLoadedScene.sceneType == SceneType.Boss) 
        {
            Transform boss = GameObject.FindGameObjectWithTag("Enemies").transform;
            bossVirtualCamera.Follow = boss;
            bossVirtualCamera.LookAt = boss;
            Invoke("getBoss", 0.5f);
        }
        else 
        {
            cameraLensEventEnd();
        }
    }

    private void GetNewCameraBound() 
    {
        var obj = GameObject.FindGameObjectWithTag("Bounds");
        if (obj == null) 
            return;
        playerConfiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
        playerConfiner2D.InvalidateCache();
        bossConfiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
        bossConfiner2D.InvalidateCache();
    }

    private void getBoss() 
    {
        playerVirtualCamera.Priority = 0;
        bossVirtualCamera.Priority = 10;
        Invoke("getPlayer", 1.5f);
    }

    private void getPlayer()
    {
        playerVirtualCamera.Priority = 10;
        bossVirtualCamera.Priority = 0;
        Invoke("cameraLensEventEnd", 1.5f);
    }

    private void cameraLensEventEnd() 
    {
        cameraLensEvent.raiseEvent();
    }
}
