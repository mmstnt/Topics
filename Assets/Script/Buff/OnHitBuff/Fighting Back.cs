using Mediapipe.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingBack : ScriptableObject, IBuff

{
    private float accumulatedDamage = 0f;
    private float originalDamage = 0f;
    private bool isbuff = false;

    public void Apply(Character character)
    {
        if (!isbuff)
        {
            originalDamage = character.damage;
            isbuff = true;
        }
        accumulatedDamage = 0f;
        character.onHitEvent.AddListener(OnHit);
        character.onAttack.AddListener(OnAttack);
    }

    public void Remove(Character character)
    {
        character.onHitEvent.RemoveListener(OnHit);
        character.onAttack.RemoveListener(OnAttack);
        character.damage = originalDamage;  // 清除時恢復原始攻擊力
        isbuff = false;
    }

    private void OnHit(Transform target, Character character, Attack attacker)
    {
        float damageTaken = attacker.damageSource.damage;
        accumulatedDamage += damageTaken;
        character.damage = originalDamage + accumulatedDamage;
    }
    private void OnAttack(Transform target, Character character, Attack attacker)
    {
        // 攻擊後清空累積傷害，恢復原始攻擊力
        accumulatedDamage = 0f;
        attacker.damageSource.damage = originalDamage;
    }
}