using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Critical : ScriptableObject, IBuff
{
    private Dictionary<Character, int> attackCounters = new Dictionary<Character, int>();
    private bool isbuff = false;
    public void Apply(Character character)
    {
        if (!attackCounters.ContainsKey(character))
            attackCounters[character] = 0;

        character.onAttack.AddListener(critical);
    }
    public void Remove(Character character)
    {
        character.onAttack.AddListener(critical);
        attackCounters.Remove(character);
    }

    // Start is called before the first frame update
    public void critical(Transform target, Character character, Attack attacker)
    {
        if (!attackCounters.ContainsKey(character))
            attackCounters[character] = 0;

        attackCounters[character]++;

        if (attackCounters[character] >= 4)
        {
            if (!isbuff)
            {
                attacker.damageSource.damage *= 2f; // 暴擊傷害（例如2倍）
                isbuff = true;
            }
            else
            {
                if (isbuff)
                {
                    attacker.damageSource.damage *= 0.5f;
                    isbuff = false;
                }
                attackCounters[character] = 0; // 重置計數
            }
        }
    }
}
