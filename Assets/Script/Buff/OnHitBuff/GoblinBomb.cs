using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GoblinBomb: ScriptableObject, IBuff
{
    private GameObject bombObject;

    public GoblinBomb(GameObject bomb) 
    {
        bombObject = bomb;
    }

    public void Apply(Character character)
    {
        character.onHitEvent.AddListener(dropBomb);
    }

    public void Remove(Character character)
    {
        character.onHitEvent.RemoveListener(dropBomb);
    }

    public void dropBomb(Transform target, Character character, Attack attacker) 
    {
        GameObject bomb = Instantiate(bombObject, target.position, target.rotation);
        bomb.GetComponent<AttackSource>().attackSource = target;
    }
}
