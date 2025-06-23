using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("卡牌庫")]
    public CardDataList cardDataList;

    [Header("物件監聽")]
    public PlayerHPUI playerHp;
    public PlayerHPUI bossHp;

    [Header("事件監聽")]
    public CharacterEventSO playerHealthEvent;
    public CharacterEventSO bossHealthEvent;
    public SceneLoadEventSO unLoadedSceneEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO gameOverEvent;
    public VoidEventSO backToMenuEvent;
    public VoidEventSO mainMenuEvent;
    public VoidEventSO newGameEvent;

    [Header("組件")]
    public GameObject gameOverPanel;
    public GameObject Btn;
    public GameObject cardChoose;
    public GameObject menu;
    public List<GameObject> endLevelImageGameObject;
    public List<GameObject> endCardChooseGameObject;

    private void OnEnable()
    {
        playerHealthEvent.onEventRaised += onPlayerHealthEvent;
        bossHealthEvent.onEventRaised += onBossHealthEvent;
        unLoadedSceneEvent.LoadRequestEvent += onUnLoadedSceneEvent;
        loadDataEvent.onEventRaised += onLoadDataEvent;
        gameOverEvent.onEventRaised += onGameOverEvent;
        backToMenuEvent.onEventRaised += onLoadDataEvent;
        mainMenuEvent.onEventRaised += onMainMenuEvent;
        newGameEvent.onEventRaised += onLoadDataEvent;
    }

    private void OnDisable()
    {
        playerHealthEvent.onEventRaised -= onPlayerHealthEvent;
        bossHealthEvent.onEventRaised -= onBossHealthEvent;
        unLoadedSceneEvent.LoadRequestEvent -= onUnLoadedSceneEvent;
        loadDataEvent.onEventRaised -= onLoadDataEvent;
        gameOverEvent.onEventRaised -= onGameOverEvent;
        backToMenuEvent.onEventRaised -= onLoadDataEvent;
        mainMenuEvent.onEventRaised -= onMainMenuEvent;
        newGameEvent.onEventRaised -= onLoadDataEvent;
    }

    private void onGameOverEvent()
    {
        gameOverPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(Btn);
        foreach (GameObject endLevel in endLevelImageGameObject) 
        {
            endLevel.GetComponent<Image>().enabled = false;
        }
        foreach (GameObject endCardChoose in endCardChooseGameObject)
        {
            endCardChoose.GetComponent<Image>().enabled = false;
        }
        for (int i=0;i<SceneLoadManager.instance.passSceneList.Count;i++) 
        {   
            if(SceneLoadManager.instance.passSceneList[i].image != null) 
            {
                endLevelImageGameObject[i].GetComponent<Image>().sprite = SceneLoadManager.instance.passSceneList[i].image;
                endLevelImageGameObject[i].GetComponent<Image>().enabled = true;
            }
        }
        for (int i = 0; i < BuffManager.instance.cardChooseList.Count; i++)
        {
            if (BuffManager.instance.cardChooseList[i] != null)
            {
                endCardChooseGameObject[i].GetComponent<Image>().sprite = cardDataList.getCradImage(BuffManager.instance.cardChooseList[i]);
                endCardChooseGameObject[i].GetComponent<Image>().enabled = true;
            }
        }
    }

    private void onMainMenuEvent() 
    { 
        menu.SetActive(true);
    }

    private void onLoadDataEvent()
    {
        gameOverPanel.SetActive(false); 
        menu.SetActive(false);
    }

    private void onUnLoadedSceneEvent(GameSceneSO sceneToload, Vector3 arg1, bool arg2, bool n)
    {
        var isMenu = (sceneToload.sceneType == SceneType.Menu);
        var isBoss = (sceneToload.sceneType == SceneType.Boss);
        playerHp.gameObject.SetActive(!isMenu);
        bossHp.gameObject.SetActive(isBoss);
    }

    private void onPlayerHealthEvent(Character character)
    {
        float persentage = character.currentHp / character.maxHp;
        playerHp.playerHealthChange(persentage);
    }
    
    private void onBossHealthEvent(Character character)
    {
        float persentage = character.currentHp / character.maxHp;
        bossHp.playerHealthChange(persentage);
    }
}
