using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Mediapipe.RenderAnnotation.Types;

public class CthuluBullet01Cast : MonoBehaviour
{
    public Transform attackSource;

    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("角色參數")]
    public GameObject CthuluBullet01;
    public int bulletMinCount;
    public int bulletMaxCount;
    public float Interval;
    private int CurCount;
    private float time;


    public void Awake()
    {
        CurCount = Random.Range(bulletMinCount, bulletMaxCount + 1);
    }

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
        if (attackSource == null) return;
        if (time < 0)
        {
            time = Random.Range(0.0f, Interval);
            GameObject cthuluBullet01GameObject = Instantiate(CthuluBullet01, transform.position, transform.rotation);
            cthuluBullet01GameObject.GetComponent<AttackSource>().attackSource = this.attackSource;
            CurCount -= 1;
            if (CurCount <= 0)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            time -= Time.deltaTime;
        }
    }
}
