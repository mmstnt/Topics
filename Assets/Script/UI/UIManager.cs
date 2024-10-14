using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("物件監聽")]
    public PlayerHPUI playerHp;
    [Header("事件監聽")]
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
