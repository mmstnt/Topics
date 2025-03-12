using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIncrease : ScriptableObject, IBuff
{
    public void Apply(Character character)
    {
        character.maxHp += 6;
        character.currentHp += 6;
        character.onHealthChange?.Invoke(character);
    }

    public void Remove(Character character)
    {
        character.maxHp -= 6;
        character.onHealthChange?.Invoke(character);
    }
}
