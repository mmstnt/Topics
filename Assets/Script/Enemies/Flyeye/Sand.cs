using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sand : MonoBehaviour
{
    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("角色參數")]
    private bool isExploded = false; // 防止多次觸發銷毀
    private SandAnimation sandAnimation;

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
        sandAnimation = transform.Find("Ani").GetComponent<SandAnimation>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isExploded) // 確保僅觸發一次
        {
            isExploded = true;
            sandAnimation.sandexplode();
        }
    }
}
    

