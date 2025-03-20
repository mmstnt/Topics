using Mediapipe.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zero : ScriptableObject, IBuff
{
    public void Apply(Character character)
    {
        character.onHitEvent.AddListener(zerodamage);
    }

    public void Remove(Character character)
    {
        character.onHitEvent.RemoveListener(zerodamage);
    }

    public void zerodamage(Transform target, Character character, Attack attacker)
    {
        if (Random.value < 0.1f)
        {
            character.currentHp += (attacker.damageSource.damage * attacker.damageRatio) + (attacker.damageBasic) + (attacker.damagePercentage * character.maxHp); ;
            character.onHealthChange?.Invoke(character);
        }
    }
}
