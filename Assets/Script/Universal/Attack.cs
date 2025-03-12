using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("¶Ë®`")]
    public float damageBasic;
    public float damageRatio;
    public float damagePercentage;

    //public float attackRange;
    //public float attackRate;
    public Character damageSource;

    private void Update()
    {
        if (damageSource != null)
            return;
        Transform attackSource = transform.parent.parent.GetComponent<AttackSource>()?.attackSource;
        damageSource = attackSource != null ? attackSource.GetComponent<Character>() : transform.parent.parent.GetComponent<Character>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (damageSource != other.GetComponent<Character>()) 
        {
            other.GetComponent<Character>()?.takeDamage(this);
        }
    }
}
