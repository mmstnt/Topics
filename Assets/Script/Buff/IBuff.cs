using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuff
{
    BuffType buffType { get; }

    void Apply(Character character); 
    void Remove(Character character); 
}



