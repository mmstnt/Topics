using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("屬性")]
    public float maxHp;
    public float currentHp;
    [Header("無敵")]
    public bool invulnerable;
    public float invulnerableTime;
    public float invulnerableDuration;
    [Header("事件")]
    public UnityEvent<Character> onHealthChange;
    public UnityEvent<Transform> onTakeDamage;
    public UnityEvent onDead;
    
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
        if (invulnerable || currentHp == 0) 
            return;
        if (currentHp - attacker.damage > 0) 
        {
            //扣血無敵
            currentHp -= attacker.damage;
            triggerInvulnerable();
            //執行受傷
            onTakeDamage?.Invoke(attacker.transform);
        }
        else 
        {
            currentHp = 0;
            //死亡
            onDead?.Invoke();
        }
        onHealthChange?.Invoke(this);
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
