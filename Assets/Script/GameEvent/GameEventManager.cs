using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameEventManager : MonoBehaviour
{
    private bool startSave;
    [Header("事件監聽")]
    public VoidEventSO portalGenerateEvent;
    public VoidEventSO afterSceneLoadedEvent;

    [Header("廣播")]
    public VoidEventSO saveDataEvent;

    [Header("組件")]
    public GameObject portal;

    [Header("關卡池")]
    public List<GameSceneSO> levelPool_01;

    private void Awake() 
    {
        startSave = true;
    }

    private void OnEnable()
    {
        portalGenerateEvent.onEventRaised += portalGenerat;
        afterSceneLoadedEvent.onEventRaised += onAfterSceneLoadedEvent;
    }

    private void OnDisable()
    {
        portalGenerateEvent.onEventRaised -= portalGenerat;
        afterSceneLoadedEvent.onEventRaised -= onAfterSceneLoadedEvent;
    }

    private void onAfterSceneLoadedEvent()
    {
        if (startSave) 
        {
            saveDataEvent.raiseEvent();
            portalGenerateEvent.raiseEvent();
            startSave = false;
        }
    }

    private void portalGenerat()
    {
        Transform portalObject = Instantiate(portal, transform.position, Quaternion.identity).transform;
        int levelPoolChoose = Random.Range(0,levelPool_01.Count);
        
        
        portalObject.GetComponent<Portal>().sceneToGo = levelPool_01[levelPoolChoose];
    }

}
