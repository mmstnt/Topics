using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fire2 : MonoBehaviour
{
    private PhysicsCheck physicsCheck;
    private fire2animation fire2Animation;
    private bool explode;

    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("角色參數")]
    public GameObject ani;

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

    void Awake()
    {
        explode = false;
        fire2Animation = transform.Find("Ani").GetComponent<fire2animation>();
        physicsCheck = GetComponent<PhysicsCheck>();
    }

    void Update()
    {
        if (physicsCheck.isGround && !explode)  
        {
            explode = true;
            fire2Animation.fire2explode();
        }
    }

 
    
    
}
