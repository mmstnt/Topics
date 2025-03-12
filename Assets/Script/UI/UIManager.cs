using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
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

    [Header("組件")]
    public GameObject gameOverPanel;
    public GameObject Btn;
    public GameObject cardChoose;

    private void OnEnable()
    {
        playerHealthEvent.onEventRaised += onPlayerHealthEvent;
        bossHealthEvent.onEventRaised += onBossHealthEvent;
        unLoadedSceneEvent.LoadRequestEvent += onUnLoadedSceneEvent;
        loadDataEvent.onEventRaised += onLoadDataEvent;
        gameOverEvent.onEventRaised += onGameOverEvent;
        backToMenuEvent.onEventRaised += onLoadDataEvent;
    }

    private void OnDisable()
    {
        playerHealthEvent.onEventRaised -= onPlayerHealthEvent;
        bossHealthEvent.onEventRaised -= onBossHealthEvent;
        unLoadedSceneEvent.LoadRequestEvent -= onUnLoadedSceneEvent;
        loadDataEvent.onEventRaised -= onLoadDataEvent;
        gameOverEvent.onEventRaised -= onGameOverEvent;
        backToMenuEvent.onEventRaised -= onLoadDataEvent;
    }

    private void onGameOverEvent()
    {
        gameOverPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(Btn);
    }

    private void onLoadDataEvent()
    {
        gameOverPanel.SetActive(false);
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
