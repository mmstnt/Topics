using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPfull : ScriptableObject, IBuff
{
    public void Apply(Character character)
    {
        character.currentHp += character.maxHp;
        character.onHealthChange?.Invoke(character);

    }

    public void Remove(Character character)
    {
        character.onHealthChange?.Invoke(character);
    }
}