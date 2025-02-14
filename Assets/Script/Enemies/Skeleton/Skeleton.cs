using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    private Rigidbody2D rb;
    private SkeletonAnimation skeletonAnimation;
    private GameObject player;

    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("角色參數")]
    public GameObject skeleton2;
    public float MoveSpeed; // 角色移動速度
    public float moveTimeMin;
    public float moveTimeMax;
    public float cutDistance;

    [Header("角色狀態")]
    public int direction; // 移動方向（-1 表示左，1 表示右）
    public float distanceToPlayer;
    public bool action;
    public enum actionKind {move,call,cut,wave}
    public actionKind actionMode;
    private float moveDuring;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        skeletonAnimation = transform.Find("Ani").GetComponent<SkeletonAnimation>();
        direction = Random.Range(0, 2) * 2 - 1; // 隨機初始化方向（-1 或 1）
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
        updateCharacterFacing();
        skeletonAction();
        distanceToPlayer = Mathf.Abs(player.transform.position.x - transform.position.x);
    }

    private void FixedUpdate()
    {
        move();
    }

    public void move()
    {
        if (actionMode != actionKind.move)
            return;
        if (actionMode == actionKind.move)
        {
            rb.velocity = new Vector2(direction * MoveSpeed * Time.deltaTime, rb.velocity.y); // 僅在 X 軸上移動
            if (moveDuring < 0)
            {
                action = false;
            }
        }
        else if (moveDuring < 0 || distanceToPlayer < cutDistance)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        moveDuring -= Time.deltaTime;
    }

    public void updateCharacterFacing()
    {
        // 根據移動方向改變角色的面向
        if (direction == -1)
        {
            transform.localScale = new Vector3(-1, 1, 1); // 面向左
        }
        else if (direction == 1)
        {
            transform.localScale = new Vector3(1, 1, 1); // 面向右
        }
    }

    public void call()
    {
        int times = 2;
        for (int i = 0; i < times; i++)
        {
            Vector3 newPosition = transform.position;
            float x = Random.Range(-10f, 10f);
            newPosition.x = player.transform.position.x + x;
            newPosition.y = 1;
            GameObject skeleton2Object = Instantiate(skeleton2, newPosition, transform.rotation);
            skeleton2Object.GetComponent<Skeleton2>().findPlayer();
        }
    }

     


    public void skeletonAction()
    {
        if (action) return;
        actionMode = (actionKind)Random.Range(0, 4); 

        switch (actionMode)
        {
            case actionKind.move:
                action = true;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                moveDuring = Random.Range(moveTimeMin, moveTimeMax);
                break;
            case actionKind.call:
                if (distanceToPlayer < cutDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                skeletonAnimation.Skeletoncall();
                break;

            case actionKind.cut:
                if (distanceToPlayer > cutDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                skeletonAnimation.Skeletoncut();
                break;

            case actionKind.wave:
                if (distanceToPlayer > cutDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                skeletonAnimation.Skeletonwave();
                break;
        }
    }





}
