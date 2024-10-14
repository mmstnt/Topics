using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("�����ť")]
    public PlayerHPUI playerHp;
    [Header("�ƥ��ť")]
    public CharacterEventSo healthEvent;

    private void OnEnable()
    {
        healthEvent.onEventRaised += onHealthEvent;
    }

    private void OnDisable()
    {
        healthEvent.onEventRaised -= onHealthEvent;
    }

    private void onHealthEvent(Character character)
    {
        float persentage = character.currentHp / character.maxHp;
        playerHp.playerHealthChange(persentage);
    }

}
