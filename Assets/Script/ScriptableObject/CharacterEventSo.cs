using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/CharacterEventSO")]
public class CharacterEventSo : ScriptableObject
{
    public UnityAction<Character> onEventRaised;

    public void RaiseEvent(Character character)
    {
        onEventRaised?.Invoke(character);
    }
}
