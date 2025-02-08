using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [Header("物件監聽")]
    public PlayerHPUI playerHp;

    [Header("事件監聽")]
    public CharacterEventSo healthEvent;
    public SceneLoadEventSO unLoadedSceneEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO gameOverEvent;
    public VoidEventSO backToMenuEvent;

    [Header("組件")]
    public GameObject gameOverPanel;
    public GameObject Btn;

    private void OnEnable()
    {
        healthEvent.onEventRaised += onHealthEvent;
        unLoadedSceneEvent.LoadRequestEvent += onUnLoadedSceneEvent;
        loadDataEvent.onEventRaised += onLoadDataEvent;
        gameOverEvent.onEventRaised += onGameOverEvent;
        backToMenuEvent.onEventRaised += onLoadDataEvent;
    }

    private void OnDisable()
    {
        healthEvent.onEventRaised -= onHealthEvent;
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
        playerHp.gameObject.SetActive(!isMenu);
    }

    private void onHealthEvent(Character character)
    {
        float persentage = character.currentHp / character.maxHp;
        playerHp.playerHealthChange(persentage);
    }

}
