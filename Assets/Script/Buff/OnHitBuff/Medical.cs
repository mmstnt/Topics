using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medical : ScriptableObject, IBuff
{
    private GameObject MedicalObject;

    public Medical(GameObject Potions)
    {
        MedicalObject = Potions;
    }

    public void Apply(Character character)
    {
        character.onHitEvent.AddListener(medical);
    }

    public void Remove(Character character)
    {
        character.onHitEvent.RemoveListener(medical);
    }

    public void medical(Transform target, Character character, Attack attacker)
    {
        if (Random.value < 0.5f)
        {
            Vector3 randomOffset = new Vector3
            (
            Random.Range(-10f, 10f),  // X 軸隨機偏移
            0f,                     // Y 軸不變（保持在地面）
            0f                      // Z 軸不變
            );

            Vector3 spawnPosition = target.position + randomOffset;  // 計算最終位置
            GameObject Potions = Instantiate(MedicalObject, spawnPosition, target.rotation);
        }
    }
}