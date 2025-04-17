using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class BuffManager : MonoBehaviour,ISaveable
{
    public static BuffManager instance;

    [Header("事件監聽")]
    public VoidEventSO newGameEvent;

    [Header("Buff")]
    public Dictionary<IBuff, Character> allBuffs;

    [Header("玩家")]
    public Character player;//讓其他物件調用

    [Header("組件")]
    public GameObject goblinBomb;
    public GameObject thunder;
    public GameObject potions;

    [Header("卡池")]
    public CardDataList cardDataList;
    public List<string> cardPool;

    private void newGame()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        
        foreach (var cardID in cardDataList.cardIDList)
        {
            for (int i = 0; i < cardDataList.cardDataList[cardID.Value].cardCount; i++)
            {
                if (cardDataList.cardDataList[cardID.Value].cardKind == CardKind.Normal)
                {
                    cardPool.Add(cardDataList.cardDataList[cardID.Value].cardID);
                }
            }
        }
        
        allBuffs = new Dictionary<IBuff, Character>();
    }

    private void OnEnable()
    {
        newGameEvent.onEventRaised += newGame;
        ISaveable saveable = this;
        saveable.registerSaveDate();
    }

    private void OnDisable()
    {
        newGameEvent.onEventRaised -= newGame;
        ISaveable saveable = this;
        saveable.unregisterSaveDate();
    }

    public void registerBuff(IBuff newBuff,Character character) 
    {
        allBuffs.Add(newBuff, character);
        newBuff.Apply(character);
    }

    public void unRegisterBuff(IBuff newBuff, Character character)
    {
        allBuffs.Remove(newBuff);
        newBuff.Remove(character);
    }

    public IBuff getCardBuff(string cardID)
    {
        switch (cardID)
        {
            case "10001":
                return new AttackIncrease();
            case "10002":
                return new HealthIncrease();
            case "10003":
                return new HPfull();
            case "10004":
                return new HPfixed();
            case "20001":
                return new HealthRegeneration();
            case "20002":
                return new HPlowattackup();
            case "30001":
                return new GoblinBomb(goblinBomb);
            case "30002":
                return new Zero();
            case "30003":
                return new Medical(potions);
            case "30004":
                return new Thornmail();
            case "40001":
                return new BloodSuck();
            case "40002":
                return new Thunder(thunder);
            case "40003":
                return new AttackBurning();
            case "40004":
                return new Critical();
            default:
                return null;
        }
    }

    public DataDefinition getDataID()
    {
        return GetComponent<DataDefinition>();
    }

    public void getSaveDate(Data data)
    {
        data.cardPool = this.cardPool;
        foreach(var buff in allBuffs) 
        {
            if (data.buffCharacter.ContainsKey(buff.Key))
            {
                data.buffCharacter[buff.Key] = buff.Value;
            }
            else
            {
                data.buffCharacter.Add(buff.Key, buff.Value);
            }
        }
    }

    public void loadData(Data data)
    {
        this.cardPool = data.cardPool;
        foreach (var buff in allBuffs) 
        {
            buff.Key.Remove(buff.Value);
        }
        allBuffs = new Dictionary<IBuff, Character>();
        foreach (var buff in data.buffCharacter)
        {
            allBuffs.Add(buff.Key, buff.Value);
            buff.Key.Apply(buff.Value);
        }
    }
}
