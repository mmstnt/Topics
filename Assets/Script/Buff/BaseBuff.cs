using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuff : MonoBehaviour, IBuff
{
    BuffType IBuff.buffType => throw new System.NotImplementedException();

    public void Apply(Character character)
    {
        throw new System.NotImplementedException();
    }

    public void Remove(Character character)
    {
        throw new System.NotImplementedException();
    }
}
