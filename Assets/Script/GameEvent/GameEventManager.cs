using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameEventManager : MonoBehaviour
{
    private bool startSave;
    [Header("�ƥ��ť")]
    public VoidEventSO portalGenerateEvent;
    public VoidEventSO afterSceneLoadedEvent;

    [Header("�s��")]
    public VoidEventSO saveDataEvent;

    [Header("�ե�")]
    public GameObject portal;

    [Header("���d��")]
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
