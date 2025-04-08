using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BringerOfDeathBullet : MonoBehaviour
{
    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("角色參數")]
    public float distance;
    public float speed;
    public bool isAttack;
    public Transform attackSource;
    private Vector2 attackSite;
    private GameObject player;
    private Vector2 directionSite;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
        if (isAttack || attackSource == null)  return;

        Vector3 direction = player.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        directionSite = new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad)
        );
        attackSite = (Vector2)transform.position + directionSite * distance;
    }

    private void FixedUpdate()
    {
        if (!isAttack || attackSource == null) return;
        transform.position = Vector2.MoveTowards(transform.position, attackSite, speed * Time.deltaTime);
        if (new Vector2(transform.position.x, transform.position.y) == attackSite || attackSource.GetComponent<Character>().isDead) 
        {
            destroyGameObject();
            attackSite = (Vector2)transform.position + directionSite * distance;
        }
    }

    public void destroyGameObject()
    {
        transform.Find("Ani").GetComponent<Animator>().SetTrigger("isDisAppear");
        speed = 5;
    }
}
