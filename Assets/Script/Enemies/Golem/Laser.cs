using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("角色參數")]
    public Transform site;

    private void OnEnable()
    {
        afterSceneLoadEvent.onEventRaised += onAfterSceneLoadEvent;
    }

    private void OnDisable()
    {
        afterSceneLoadEvent.onEventRaised -= onAfterSceneLoadEvent;
    }

    private void onAfterSceneLoadEvent()
    {
        Destroy(this.gameObject);
    }

    private void Update()
    {
        transform.position = site.position;
        transform.localScale = site.localScale;
    }
}
