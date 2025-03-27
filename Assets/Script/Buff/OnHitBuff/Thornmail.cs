using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thornmail : ScriptableObject, IBuff
{
    public void Apply(Character character)
    {
        character.onHitEvent.AddListener(thornmail);
    }

    public void Remove(Character character)
    {
        character.onHitEvent.RemoveListener(thornmail);
    }

    public void thornmail(Transform target, Character character, Attack attacker)
    {
        if (Random.value < 0.5f)
        {
            attacker.damageSource.currentHp -= attacker.damageSource.damage * 0.5f;
            attacker.damageSource.onHealthChange?.Invoke(attacker.damageSource);
        }
    }
}
