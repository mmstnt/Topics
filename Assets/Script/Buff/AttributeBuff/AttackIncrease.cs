using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIncrease : ScriptableObject, IBuff
{
    public void Apply(Character character)
    {
        character.damage += 1;
    }

    public void Remove(Character character)
    {
        character.damage -= 1;
    }
}
