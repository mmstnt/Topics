using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPlowattackup : ScriptableObject, IBuff
{
    private bool isbuff = false;
    public void Apply(Character character)
    {
        character.onTime.AddListener(HPlowup);
    }

    public void Remove(Character character)
    {
        character.onTime.RemoveListener(HPlowup);
    }
    public void HPlowup(Transform target, Character character)
    {
        if (character.currentHp <= character.maxHp * 0.25f)
        {
            if (!isbuff)
            {
                character.damage += 5;
                isbuff = true;
            }
        }
        else
        {
            if (isbuff)
            {
                character.damage -= 5;
                isbuff = false;
            }
        }
    }
}
