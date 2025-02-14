using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [Header("�d�P�w")]
    public CardDataList cardDataList;

    [Header("�Ѽ�")]
    public string cardID;
    public bool isChoose;

    [Header("�ե�")]
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
