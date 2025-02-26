using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager instance;
    [Header("事件監聽")]
    public CharacterEventSo onHitEvent;

    [Header("受擊事件")]
    public List<IBuff> onHitBuffs;
    public List<IBuff> onAttackBuffs;
    public List<IBuff> timedBuffs;
    public List<IBuff> passiveBuffs;
    public List<IBuff> attributeBuffs;

    public Character player;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        onHitBuffs = new List<IBuff>();
        onAttackBuffs = new List<IBuff>();
        timedBuffs = new List<IBuff>();
        passiveBuffs = new List<IBuff>();
        attributeBuffs = new List<IBuff>();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    public void registerBuff(IBuff newBuff,Character character) 
    {
        switch (newBuff.buffType) 
        {
            case BuffType.OnHit:
                onHitBuffs.Add(newBuff);
                break;
            case BuffType.OnAttack:
                onAttackBuffs.Add(newBuff);
                break;
            case BuffType.Timed:
                timedBuffs.Add(newBuff);
                break;
            case BuffType.Passive:
                passiveBuffs.Add(newBuff);
                break;
            case BuffType.Attribute:
                attributeBuffs.Add(newBuff);
                break;
        }
        newBuff.Apply(character);
    }

    public void unRegisterBuff(IBuff newBuff, Character character)
    {
        switch (newBuff.buffType)
        {
            case BuffType.OnHit:
                onHitBuffs.Remove(newBuff);
                break;
            case BuffType.OnAttack:
                onAttackBuffs.Remove(newBuff);
                break;
            case BuffType.Timed:
                timedBuffs.Remove(newBuff);
                break;
            case BuffType.Passive:
                passiveBuffs.Remove(newBuff);
                break;
            case BuffType.Attribute:
                attributeBuffs.Remove(newBuff);
                break;
        }
        newBuff.Remove(character);
    }

    public IBuff getCardBuff(string cardID)
    {
        switch (cardID)
        {
            case "10001":
                return new Buff();
            default:
                return null;
        }
    }
}
