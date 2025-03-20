using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPfixed : ScriptableObject, IBuff
{
    public void Apply(Character character)
    {
        character.maxHp = character.maxHp * 0.5f;
        character.damage = character.damage*2;
        character.onHealthChange?.Invoke(character);
    }

    public void Remove(Character character)
    {
        character.maxHp = character.maxHp*2f;
        character.damage = character.damage*0.5f;
        character.onHealthChange?.Invoke(character);
    }
}