using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueFire : MonoBehaviour
{
    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("角色參數")]
    public float speed;
    public float time;
    public bool isAttack;
    public Transform attackSource;
    private GameObject player;
    private float lessSpeed;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lessSpeed = (speed - 2.5f) / time;
        isAttack = false;
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
        
        Vector3 direction = player.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        if (time < 0 || attackSource.GetComponent<SkeletonLighter>().isDead) 
        {
            destroyGameObject();
        }
        else 
        {
            time -= Time.deltaTime;
            speed -= lessSpeed * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (attackSource == null || !isAttack) return;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        if (attackSource.GetComponent<SkeletonLighter>().isDead)
        {
            destroyGameObject();
        }
    }

    public void destroyGameObject()
    {
        transform.Find("Ani").GetComponent<Animator>().SetTrigger("isDisAppear");
        speed = 2.5f;
    }
}
