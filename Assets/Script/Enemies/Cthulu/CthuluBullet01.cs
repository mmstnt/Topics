using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CthuluBullet01 : MonoBehaviour
{
    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("角色參數")]
    public float startSpeed;
    public float speed;
    public float dirSpeed;
    public float time;
    public bool isAttack;
    private GameObject player;
    private Vector2 dir;
    private float curSpeed;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        isAttack = false;
        transform.position = new Vector2(Random.Range(-1.0f,1.0f), Random.Range(-1.0f, 1.0f)) + (Vector2)transform.position;
        Vector3 direction = player.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 180);
        curSpeed = startSpeed;
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
        if (!isAttack)
        {
            Vector3 direction = player.transform.position - transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), dirSpeed * Time.deltaTime);
            dir = new Vector2(
            Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad),
            Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad)
            ).normalized;
            curSpeed -= 2 * startSpeed * Time.deltaTime;
            if (transform.rotation == Quaternion.Euler(0, 0, angle))
            {
                isAttack = true;
                curSpeed = speed;
            }
        }
        else 
        {
            time -= Time.deltaTime;
            if (time < 0) 
            {
                destroyGameObject();
            }
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(dir * curSpeed * Time.deltaTime, Space.World);
    }

    public void destroyGameObject()
    {
        transform.Find("Ani").GetComponent<Animator>().SetTrigger("isDisAppear");
        curSpeed = 5;
    }
}
