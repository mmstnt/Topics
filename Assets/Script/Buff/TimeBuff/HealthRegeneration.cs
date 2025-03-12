using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegeneration : ScriptableObject, IBuff
{
    public void Apply(Character character)
    {
        character.onTime.AddListener(HPRegeneration);
    }

    public void Remove(Character character)
    {
        character.onTime.RemoveListener(HPRegeneration);
    }

    public void HPRegeneration(Transform target,Character character) 
    {
        character.currentHp += 0.5f;
        character.onHealthChange?.Invoke(character);
    }
}
