using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Frog : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("參數")]
    public float MoveSpeed;  // 角色移動速度
    public float jumpForce;  // 跳躍力度
    public float minDistance;  // 最小距離，不跳向玩家的閾值
    public float bigDistance;
    public float attackDistance;
    public float minTime;
    public float maxTime;

    [Header("狀態")]
    public bool action;
    public actionKind actionMode;
    public enum actionKind { jump,attack };

    [Header("物件")]
    public GameObject bullet;
    //私人參數
    private int direction = 1;
    private float distanceToPlayer;
    private float time;


    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        physicsCheck = transform.GetComponent<PhysicsCheck>();
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
        player = GameObject.FindGameObjectWithTag("Player");  // 找到 Player 物件
    }

    private void Update()
    {
        frogActionMode();
        MoveTowardsPlayer();
    }

    private void frogActionMode() 
    {
        if (action) return;
        if (time > 0) 
        {
            time -= Time.deltaTime;
            return;
        }
        actionMode = (actionKind)Random.Range(0, 2);
        switch (actionMode)
        {
            case actionKind.jump:
                Jump();
                idleTime();
                break;
            case actionKind.attack:
                action = true;
                attack();
                idleTime();
                break;
            default:
                break;
        }
    }

    private void MoveTowardsPlayer()
    {
        transform.localScale = new Vector3(direction, 1, 1);
        if (!physicsCheck.isGround)
        {
            direction = rb.velocity.x == 0 ? direction : rb.velocity.x > 0 ? -1 : 1;
        }
        else
        {
            direction = player.transform.position.x - transform.position.x > 0 ? -1 : 1;
        }
    }

    private void Jump()
    {
        if (physicsCheck.isGround)
        {
            action = true;
            Debug.Log(111);
            distanceToPlayer = Mathf.Abs(player.transform.position.x - transform.position.x);
            if (distanceToPlayer < bigDistance)
            {
                // 施加跳躍力，X 軸的方向會根據隨機跳躍方向變化
                rb.AddForce(new Vector2((Random.Range(0, 2) * 2 - 1) * MoveSpeed , jumpForce), ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(new Vector2(-direction * MoveSpeed, jumpForce), ForceMode2D.Impulse);
            }
            attack();
            //action = false;
        }
    }

    private void attack()
    {
        GameObject bulletGameObject = Instantiate(bullet, transform.position, transform.rotation, transform);
        Vector3 attackSite = player.transform.position;
        if (Vector3.Distance(transform.position, attackSite) > attackDistance)
        {
            attackSite = transform.position + (attackSite - transform.position).normalized * attackDistance;
        }
        bulletGameObject.GetComponent<BulletMover>().frogAttack(attackSite);
    }

    private void idleTime() 
    {
        time = Random.Range(minTime, maxTime);
    }
}




