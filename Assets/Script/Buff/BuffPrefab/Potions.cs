using Mediapipe.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potions : MonoBehaviour
{
    public float healingHP;
    public float time; // �p�ɾ��ܼ�

    void Update()
    {
        time -= Time.deltaTime; // �C�V��ָg�L���ɶ��A�T�O�˼ƭp�ɷǽT

        if (time < 0)
        {
            Destroy(gameObject); // �P����e�C������
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Character character = other.gameObject.GetComponent<Character>(); // �������ե�
            if (character != null)
            {
                character.currentHp += healingHP;
                character.onHealthChange?.Invoke(character);
                Destroy(gameObject); // ������P���Ĥ�����
            }
        }
    }
}