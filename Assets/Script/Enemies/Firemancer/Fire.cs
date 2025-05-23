using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fire : MonoBehaviour
{
    private PhysicsCheck physicsCheck;

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

    private void Awake()
    {
        physicsCheck = GetComponent<PhysicsCheck>();
        ani.SetActive(false);
        Destroy(gameObject, 3.0f);
    }

    private void Update()
    {
        jump();
    }

    private void jump()
    {
        if (physicsCheck.isGround)
        {
            ani.SetActive(true);
        }
    }
}
