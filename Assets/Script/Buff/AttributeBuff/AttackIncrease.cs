using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIncrease : ScriptableObject, IBuff
{
    public void Apply(Character character)
    {
        character.startDamage += 1;
    }

    public void Remove(Character character)
    {
        character.startDamage -= 1;
    }
}
