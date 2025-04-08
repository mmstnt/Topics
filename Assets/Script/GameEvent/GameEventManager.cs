using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameEventManager : MonoBehaviour,ISaveable
{
    private bool startSave;
    private bool load;
    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadedEvent;
    public VoidEventSO portalGenerateEvent;
    public VoidEventSO cardChooseEvent;
    public VoidEventSO cardChooseEndEvent;
    public VoidEventSO newGameEvent;

    [Header("廣播")]
    public VoidEventSO saveDataEvent;

    [Header("組件")]
    public List<GameObject> portalList;
    public GameObject portalObject;
    public GameObject cardChoose;
    public SceneLoadManager sceneLoadManager;
    public Transform player;

    [Header("關卡池")]
    public List<GameSceneSO> curLevelPool;
    public List<GameSceneSO> levelPool_01;

    [Header("卡牌庫")]
    public CardDataList cardDataList;

    private void OnEnable()
    {
        newGameEvent.onEventRaised += newGame;
        afterSceneLoadedEvent.onEventRaised += onAfterSceneLoadedEvent;
        portalGenerateEvent.onEventRaised += onPortalGenerat;
        cardChooseEvent.onEventRaised += onCardChoose;
        cardChooseEndEvent.onEventRaised += onCardChooseEnd;

        ISaveable saveable = this;
        saveable.registerSaveDate();
    }

    private void OnDisable()
    {
        newGameEvent.onEventRaised -= newGame;
        afterSceneLoadedEvent.onEventRaised -= onAfterSceneLoadedEvent;
        portalGenerateEvent.onEventRaised -= onPortalGenerat;
        cardChooseEvent.onEventRaised -= onCardChoose;
        cardChooseEndEvent.onEventRaised -= onCardChooseEnd;

        ISaveable saveable = this;
        saveable.unregisterSaveDate();
    }

    private void newGame() 
    {
        startSave = true;
        cardDataList.cardInitialize();
        levelInitialize(levelPool_01);

        player.GetComponent<Character>().newGame();
        player.GetComponent<Character>().isDead = false;
    }

    private void onAfterSceneLoadedEvent()
    {
        if (startSave)
        {
            portalGenerateEvent.raiseEvent();
            saveDataEvent.raiseEvent();
            startSave = false;
        }
        else if(load)
        {
            load = false;
            player.GetComponent<Character>().isDead = false;
        }
        else 
        {
            removePortal();
        }
    }

    private void onPortalGenerat()
    {
        for (int i = 0; i < 3; i++)
        {
            if (curLevelPool != null && curLevelPool.Count > 0)
            {
                Vector3 vector = sceneLoadManager.currentLoadedScene.portalPosition;
                vector.x += i==1?4:(i==2?-4:0);
                int levelPoolChoose = Random.Range(0, curLevelPool.Count);
                Transform portalObjectTrans = Instantiate(portalObject, vector, Quaternion.identity).transform;
                portalObjectTrans.GetComponent<Portal>().sceneToGo = curLevelPool[levelPoolChoose];
                portalObjectTrans.GetComponent<Portal>().changeImage();
                portalList.Add(portalObjectTrans.gameObject);
                curLevelPool.Remove(curLevelPool[levelPoolChoose]);
            }
        }
    }

    private void removePortal() 
    {
        for (int i = 0; i < portalList.Count; i++)
        {
            Portal portalScript = portalList[i].GetComponent<Portal>();
            if (!portalScript.isChoose)
            {
                curLevelPool.Add(portalScript.sceneToGo);
            }
            Destroy(portalList[i]);
        }
        portalList = new List<GameObject>();
    }

    private void onCardChoose()
    {
        if(!player.GetComponent<Character>().isDead)
            cardChoose.SetActive(true);
    }

    private void onCardChooseEnd()
    {
        cardChoose.SetActive(false);
        portalGenerateEvent.onEventRaised();
    }

    private void levelInitialize(List<GameSceneSO> pool) 
    {
        curLevelPool = new List<GameSceneSO>();
        for (int i = 0; i < pool.Count; i++) 
        {
            curLevelPool.Add(pool[i]);
        }
    }

    public DataDefinition getDataID()
    {
        return GetComponent<DataDefinition>();
    }

    public void getSaveDate(Data data)
    {
        data.saveGameSceneList(curLevelPool);
        data.savePortal(portalList);
    }

    public void loadData(Data data)
    {
        load = true;
        curLevelPool = data.getSaveGameSceneList();
        portalList = new List<GameObject>();
        foreach(var portal in data.getSavePortal()) 
        {
            Transform portalObjectTrans = Instantiate(portalObject, portal.Value, Quaternion.identity).transform;
            portalObjectTrans.GetComponent<Portal>().sceneToGo = portal.Key;
            portalObjectTrans.GetComponent<Portal>().changeImage();
            portalList.Add(portalObjectTrans.gameObject);
        }

        player.GetComponent<Character>().loadData(data);
    }
}
