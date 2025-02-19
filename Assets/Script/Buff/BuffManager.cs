using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager instance;
    private List<IBuff> activeBuffs = new List<IBuff>();
    public Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    public void AddBuff(IBuff buff)
    {
        buff.Apply(character);
        activeBuffs.Add(buff);
    }

    public void RemoveBuff(IBuff buff)
    {
        buff.Remove(character);
        activeBuffs.Remove(buff);
    }
}
