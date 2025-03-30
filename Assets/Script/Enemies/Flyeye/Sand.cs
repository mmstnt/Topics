using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sand : MonoBehaviour
{
    [Header("ㄆン菏钮")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("à︹把计")]
    private bool isExploded = false; // ňゎΩ牟祇綪反
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
        if (!isExploded) // 絋玂度牟祇Ω
        {
            isExploded = true;
            sandAnimation.sandexplode();
        }
    }
}
    

