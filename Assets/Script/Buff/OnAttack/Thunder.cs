using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : ScriptableObject, IBuff
{
    private GameObject thunderObject;

    public Thunder(GameObject thunderObject)
    {
        this.thunderObject = thunderObject;
    }

    public void Apply(Character character)
    {
        character.onAttack.AddListener(thunder);
    }

    public void Remove(Character character)
    {
        character.onAttack.RemoveListener(thunder);
    }

    public void thunder(Transform target, Character character, Attack attacker)
    {
        if (Random.value < 0.25f) 
        {
            Vector2 vector = target.position;
            vector.y += 1.0f;
            GameObject thunderGameObject = Instantiate(thunderObject, vector, target.rotation);
            thunderGameObject.GetComponent<AttackSource>().attackSource = attacker.damageSource.transform;
        }
    }
}
