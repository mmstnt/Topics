using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBurning : ScriptableObject, IBuff
{
    public void Apply(Character character)
    {
        character.onAttack.AddListener(attackBurning);
    }

    public void Remove(Character character)
    {
        character.onAttack.RemoveListener(attackBurning);
    }

    public void attackBurning(Transform target, Character character, Attack attacker)
    {
        //if (BuffManager.instance.allBuffs) 
        //{
            IBuff newBuff = BuffManager.instance.getCardBuff("");
            BuffManager.instance.registerBuff(newBuff, character);
        //}
    }
}
