using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("屬性")]
    public float maxHp;
    public float currentHp;
    [Header("無敵")]
    public bool invulnerable;
    public float invulnerableTime;
    public float invulnerableDuration;
    
    private void Start()
    {
        currentHp = maxHp;
    }

    private void Update()
    {
        if (invulnerable) 
        {
            invulnerableDuration -= Time.deltaTime;
            if (invulnerableDuration <= 0) 
            {
                invulnerable = false;
            }
        }
    }

    public void takeDamage(Attack attacker) 
    {
        if (invulnerable) 
            return;
        if (currentHp - attacker.damage > 0) 
        {
            //扣血無敵
            currentHp -= attacker.damage;
            triggerInvulnerable();
            //執行受傷動畫(製作中)

        }
        else 
        {
            currentHp = 0;
            //死亡(未製作)
        }
    }

    
    private void triggerInvulnerable() 
    {
        if (!invulnerable) 
        {
            invulnerableDuration = invulnerableTime;
            invulnerable = true;
        }
    }
}
