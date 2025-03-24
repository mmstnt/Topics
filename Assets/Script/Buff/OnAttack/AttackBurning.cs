using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBurning : ScriptableObject, IBuff
{
    public Dictionary<Burning, Character> buffCharacterList = new Dictionary<Burning, Character>();

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
        foreach(var buffCharacter in buffCharacterList) 
        {
            if (character == buffCharacter.Value) 
            {
                buffCharacter.Key.time = 3;
                return;
            }
        }
        Burning newBuff = new Burning();
        newBuff.buffSource = this;
        BuffManager.instance.registerBuff(newBuff, character);
        buffCharacterList.Add(newBuff, character);
    }
}
