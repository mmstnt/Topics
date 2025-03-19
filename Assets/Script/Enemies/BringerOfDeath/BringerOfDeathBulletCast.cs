using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeathBulletCast : MonoBehaviour
{
    public Transform attackSource;

    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("角色參數")]
    public GameObject bringerOfDeathBullet;
    public float Interval;
    public int Count;
    private int CurCount;
    private float time;
    private float dir;

    public void Awake()
    {
        CurCount = Count;
        dir = 360.0f / Count;
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
            time = Interval;
            float angle = dir * CurCount;
            Vector2 direction = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            );
            Vector2 vector = attackSource.position;
            vector.y += 2;
            vector += direction;
            GameObject bringerOfDeathBulletObject = Instantiate(bringerOfDeathBullet, vector, transform.rotation);
            bringerOfDeathBulletObject.GetComponent<AttackSource>().attackSource = this.attackSource;
            bringerOfDeathBulletObject.GetComponent<BringerOfDeathBullet>().attackSource = this.attackSource;
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
