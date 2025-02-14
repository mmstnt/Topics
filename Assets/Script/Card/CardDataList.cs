using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "CardDataList", menuName = "List/CardDataList")]
public class CardDataList : ScriptableObject
{
    public List<CardData> cardDataList;
    public Dictionary<string, int> cardIDList = new Dictionary<string, int>();

    public void cardInitialize() 
    {
        for(int i=0;i<cardDataList.Count;i++) 
        {
            if (!cardIDList.ContainsKey(cardDataList[i].cardID))
            {
                cardIDList.Add(cardDataList[i].cardID, i);
            }
        }
    }

    public string getCardName(string ID) 
    {
        return cardDataList[cardIDList[ID]].cardName;
    }

    public string getCradIntroduce(string ID) 
    {
        return cardDataList[cardIDList[ID]].cardIntroduce;
    }

    public Sprite getCradImage(string ID) 
    {
        return cardDataList[cardIDList[ID]].cardImage;
    }
}
