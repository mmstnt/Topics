using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardChoose : MonoBehaviour
{
    [Header("²Õ¥ó")]
    public GameObject[] card = new GameObject[3];

    private void OnEnable()
    {
        for(int i = 0; i < card.Length; i++) 
        {
            if (BuffManager.instance.cardPool != null && BuffManager.instance.cardPool.Count > 0)  
            {
                int chooseCard = Random.Range(0, BuffManager.instance.cardPool.Count);
                card[i].GetComponent<Card>().cardID = BuffManager.instance.cardPool[chooseCard];
                BuffManager.instance.cardPool.Remove(BuffManager.instance.cardPool[chooseCard]);
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
