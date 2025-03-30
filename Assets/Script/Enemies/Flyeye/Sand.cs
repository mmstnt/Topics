using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sand : MonoBehaviour
{
    [Header("�ƥ��ť")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("����Ѽ�")]
    private bool isExploded = false; // ����h��Ĳ�o�P��
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
        if (!isExploded) // �T�O��Ĳ�o�@��
        {
            isExploded = true;
            sandAnimation.sandexplode();
        }
    }
}
    

