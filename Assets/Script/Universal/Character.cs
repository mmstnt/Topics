using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("�ݩ�")]
    public float maxHp;
    public float currentHp;
    [Header("�L��")]
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
            //����L��
            currentHp -= attacker.damage;
            triggerInvulnerable();
            //������˰ʵe(�s�@��)

        }
        else 
        {
            currentHp = 0;
            //���`(���s�@)
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
