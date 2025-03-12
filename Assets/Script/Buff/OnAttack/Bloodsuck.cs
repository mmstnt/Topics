using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSuck : ScriptableObject, IBuff
{
    public void Apply(Character character)
    {
        character.onAttack.AddListener(Bloodsucking);
    }

    public void Remove(Character character)
    {
        character.onAttack.RemoveListener(Bloodsucking);
    }

    public void Bloodsucking(Transform target, Character character, Attack attacker)
    {
        float bloodsuck = (attacker.damageSource.damage * attacker.damageRatio) + (attacker.damageBasic) + (attacker.damagePercentage * character.maxHp);
        attacker.damageSource.currentHp += bloodsuck * 0.2f;
        attacker.damageSource.onHealthChange?.Invoke(attacker.damageSource);
    }
}
