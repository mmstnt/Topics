using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [Header("卡牌庫")]
    public CardDataList cardDataList;

    [Header("廣播")]
    public VoidEventSO cardChooseEndEvent;

    [Header("參數")]
    public string cardID;
    public bool isChoose;

    [Header("組件")]
    public TMP_Text cardName;
    public TMP_Text cardIntroduce;
    public Image cardImage;
    public CardChoose cardChoose;

    private void OnEnable()
    {
        isChoose = false;
        cardName.text = cardDataList.getCardName(cardID);
        cardIntroduce.text = cardDataList.getCradIntroduce(cardID);
        cardImage.sprite = cardDataList.getCradImage(cardID);
    }

    private void OnDisable()
    {
        if (!isChoose && cardID != "0") 
        {
            BuffManager.instance.cardPool.Add(cardID);
        }
    }

    public void cardIsChoose() 
    {
        isChoose = true;
        IBuff newBuff = BuffManager.instance.getCardBuff(cardID);
        BuffManager.instance.registerBuff(newBuff, BuffManager.instance.player);
        cardChooseEndEvent.raiseEvent();
    }

}
