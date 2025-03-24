using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burning : ScriptableObject, IBuff
{
    public AttackBurning buffSource;
    public float time;

    public void Apply(Character character)
    {
        character.onTime.AddListener(burning);
        time = 3;
    }

    public void Remove(Character character)
    {
        character.onTime.RemoveListener(burning);
    }

    public void burning(Transform target, Character character)
    {
        character.currentHp -= 0.5f;
        character.onHealthChange?.Invoke(character);
        time -= 1;
        if (time < 0) 
        {
            buffSource.buffCharacterList.Remove(this);
            BuffManager.instance.unRegisterBuff(this, character);
        }
    }
}
