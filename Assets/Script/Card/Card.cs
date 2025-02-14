using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [Header("卡牌庫")]
    public CardDataList cardDataList;

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
        if (!isChoose) 
        {
            cardChoose.cardPool.Add(cardID);
        }
    }
}
