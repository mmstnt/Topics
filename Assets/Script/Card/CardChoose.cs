using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardChoose : MonoBehaviour
{
    [Header("¥d¦À")]
    public CardDataList cardDataList;
    public List<string> cardPool;

    [Header("²Õ¥ó")]
    public GameObject[] card = new GameObject[3];
    
    public string[] curCardID = new string[3];

    private void Awake()
    {
        for(int i = 0; i < cardDataList.cardDataList.Count; i++) 
        {
            if (cardDataList.cardDataList[i].cardKind == CardKind.normal) 
            {
                cardPool.Add(cardDataList.cardDataList[i].cardID);
            }
        }
    }

    private void OnEnable()
    {
        for(int i = 0; i < curCardID.Length; i++) 
        {
            if (cardPool != null && cardPool.Count > 0)  
            {
                int chooseCard = Random.Range(0, cardPool.Count);
                card[i].GetComponent<Card>().cardID = cardPool[chooseCard];
                cardPool.Remove(cardPool[chooseCard]);
            }
            else 
            {
                card[i].GetComponent<Card>().cardID = "0";
            }
            card[i].SetActive(true);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < card.Length; i++)
        {
            card[i].SetActive(false);
        }
    }
}
