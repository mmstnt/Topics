using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("�����ť")]
    public PlayerHPUI playerHp;
    [Header("�ƥ��ť")]
    public CharacterEventSo healthEvent;
    public SceneLoadEventSO loadEvent;

    private void OnEnable()
    {
        healthEvent.onEventRaised += onHealthEvent;
        loadEvent.LoadRequestEvent += onLoadEvent;
    }

    private void OnDisable()
    {
        healthEvent.onEventRaised -= onHealthEvent;
        loadEvent.LoadRequestEvent -= onLoadEvent;
    }

    private void onLoadEvent(GameSceneSO sceneToload, Vector3 arg1, bool arg2)
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
