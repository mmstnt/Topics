using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuff : ScriptableObject, IBuff
{
    public BuffType buffType => BuffType.Attribute;

    public void Apply(Character character)
    {
        character.maxHp += 6;
    }

    public void Remove(Character character)
    {
        character.maxHp -= 6;
    }

}
