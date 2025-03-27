using Mediapipe.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potions : MonoBehaviour
{
    public float healingHP;
    public float time; // 計時器變數

    void Update()
    {
        time -= Time.deltaTime; // 每幀減少經過的時間，確保倒數計時準確

        if (time < 0)
        {
            Destroy(gameObject); // 銷毀當前遊戲物件
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Character character = other.gameObject.GetComponent<Character>(); // 獲取角色組件
            if (character != null)
            {
                character.currentHp += healingHP;
                character.onHealthChange?.Invoke(character);
                Destroy(gameObject); // 撞擊後銷毀藥水物件
            }
        }
    }
}