using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameEventManager : MonoBehaviour
{
    private bool startSave;
    [Header("�ƥ��ť")]
    public VoidEventSO afterSceneLoadedEvent;
    public VoidEventSO portalGenerateEvent;
    public VoidEventSO cardChooseEvent;
    public VoidEventSO portalChooseEvent;

    [Header("�s��")]
    public VoidEventSO saveDataEvent;

    [Header("�ե�")]
    public List<GameObject> portalList;
    public GameObject portal;
    public GameObject cardChooseGameObject;
    public SceneLoadManager sceneLoadManager;

    [Header("���d��")]
    public List<GameSceneSO> curLevelPool;
    public List<GameSceneSO> levelPool_01;

    [Header("�d�P�w")]
    public CardDataList cardDataList;

    private void Awake() 
    {
        startSave = true;
        cardDataList.cardInitialize();
        levelInitialize(levelPool_01);
    }

    private void OnEnable()
    {
        afterSceneLoadedEvent.onEventRaised += onAfterSceneLoadedEvent;
        portalGenerateEvent.onEventRaised += portalGenerat;
        cardChooseEvent.onEventRaised += cardChoose;
        portalChooseEvent.onEventRaised += portalChoose;
    }

    private void OnDisable()
    {
        afterSceneLoadedEvent.onEventRaised -= onAfterSceneLoadedEvent;
        portalGenerateEvent.onEventRaised -= portalGenerat;
        cardChooseEvent.onEventRaised -= cardChoose;
        portalChooseEvent.onEventRaised -= portalChoose;
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
        for (int i = 0; i < 3; i++)
        {
            if (curLevelPool != null && curLevelPool.Count > 0)
            {
                Vector3 vector = sceneLoadManager.currentLoadedScene.portalPosition;
                vector.x += i==1?4:(i==2?-4:0);
                int levelPoolChoose = Random.Range(0, curLevelPool.Count);
                Transform portalObject = Instantiate(portal, vector, Quaternion.identity).transform;
                portalObject.GetComponent<Portal>().sceneToGo = curLevelPool[levelPoolChoose];
                portalObject.GetComponent<Portal>().changeImage();
                portalList.Add(portalObject.gameObject);
                curLevelPool.Remove(curLevelPool[levelPoolChoose]);
            }
        }
    }

    private void cardChoose()
    {
        cardChooseGameObject.SetActive(true);
    }

    private void levelInitialize(List<GameSceneSO> pool) 
    {
        curLevelPool = new List<GameSceneSO>();
        for (int i = 0; i < pool.Count; i++) 
        {
            curLevelPool.Add(pool[i]);
        }
    }

    private void portalChoose()
    {
        for(int i=0;i< portalList.Count; i++) 
        {
            Portal portalScript = portalList[i].GetComponent<Portal>();
            if (!portalScript.isChoose) 
            {
                curLevelPool.Add(portalScript.sceneToGo);
            }
            Destroy(portalList[i]);
        }
    }
}
