using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : ScriptableObject, IBuff
{
    public BuffType buffType => BuffType.OnHit;

    public void Apply(Character character)
    {
        character.onHitEvent.AddListener(onHH);
    }

    public void Remove(Character character)
    {
        character.onHitEvent.RemoveListener(onHH);
    }

    public void onHH(Transform target) 
    {
        target.GetComponent<Character>().currentHp = 0;
    }
}
