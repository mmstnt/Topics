using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("�ݩ�")]
    public float maxHp;
    public float currentHp;
    [Header("�L��")]
    public bool invulnerable;
    public float invulnerableTime;
    public float invulnerableDuration;
    [Header("�ƥ�")]
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
            //����L��
            currentHp -= attacker.damage;
            triggerInvulnerable();
            //�������
            onTakeDamage?.Invoke(attacker.transform);
        }
        else 
        {
            currentHp = 0;
            //���`
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
