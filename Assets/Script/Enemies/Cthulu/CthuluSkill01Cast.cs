using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CthuluSkill01Cast : MonoBehaviour
{
    public Transform attackSource;

    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("角色參數")]
    public GameObject CthuluSkill01GameObject;
    public float Interval;
    public int Count;
    public float distance;
    private int CurCount;
    private float time;
    private Vector2 dir;
    private GameObject player;

    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Vector3 direction = player.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        dir = new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad)
        );
        CurCount = Count;
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
            transform.position = (Vector2)transform.position + dir * distance;
            float angle = (CurCount % 3 == 0 ? 0 : (CurCount % 3 == 1 ? 60 : -60)) + transform.eulerAngles.z;
            Vector2 direction = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            );
            direction = direction.normalized;
            Vector2 vector = (Vector2)transform.position + direction;
            GameObject bringerOfDeathBulletObject = Instantiate(CthuluSkill01GameObject, vector, Quaternion.Euler(0,0,angle));
            bringerOfDeathBulletObject.GetComponent<AttackSource>().attackSource = this.attackSource;
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
