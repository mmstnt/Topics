using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battlelust : ScriptableObject, IBuff
{
    private Dictionary<Character, int> attackCounters = new Dictionary<Character, int>();

    public void Apply(Character character)
    {
        character.onAttack.AddListener(battlelust);
        character.onTime.AddListener(time);
        if (!attackCounters.ContainsKey(character))
            attackCounters[character] = 0;
    }

    public void Remove(Character character)
    {
        character.onAttack.RemoveListener(battlelust);
        character.onTime.RemoveListener(time);
        attackCounters.Remove(character);
    }
    public void battlelust(Transform target, Character character, Attack attacker)
    {
        if (!attackCounters.ContainsKey(character))
            attackCounters[character] = 0;

        attackCounters[character] = Math.Min(attackCounters[character] + 1, 4);

        attacker.damageSource.damage = character.damage + attackCounters[character] * 5;
    }
    public void time(Transform target, Character character)
    {
        if(character.damage >= character.startDamage+1)
        {
            character.damage -= 1f;
        }
    }
}