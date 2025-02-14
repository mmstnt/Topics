using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firemancer : MonoBehaviour
{
    private Rigidbody2D rb;
    private FiremancerAnimation firemancerAnimation;
    private GameObject player;

    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("角色參數")]
    public GameObject fire;
    public GameObject fire2;
    public float MoveSpeed; // 角色移動速度
    public float moveTimeMin;
    public float moveTimeMax;
    public float spoutDistance;
    public float y;
     

    [Header("角色狀態")]
    public int direction; // 移動方向（-1 表示左，1 表示右）
    public float distanceToPlayer;
    public bool action;
    public enum actionKind { move, fire,fire2,spout }
    public actionKind actionMode;
    private float moveDuring;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        firemancerAnimation = transform.Find("Ani").GetComponent<FiremancerAnimation>();
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
        firemancerAction();
        distanceToPlayer = Mathf.Abs(player.transform.position.x - transform.position.x);
    }

    public void dofire()
    {
            // 計算隨機位置
            Vector3 newPosition = transform.position;
            float x = Random.Range(-5f, 5f);
            newPosition.x = player.transform.position.x + x;
            newPosition.y = 3;

            // 生成 Fire 物件
            Instantiate(fire, newPosition, transform.rotation);
    }


    public void dofire2()
    {
        // 計算隨機位置
        Vector3 newPosition = transform.position;
        
        newPosition.x = player.transform.position.x + y * direction;
        newPosition.y = 3;
        y = y + 10;

        Instantiate(fire2, newPosition, transform.rotation);
        if (y > 20)
        {
            y = 0;
        }
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
        else if (moveDuring < 0 || distanceToPlayer < spoutDistance)
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


    public void firemancerAction()
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

            case actionKind.fire:
                if (distanceToPlayer < spoutDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                firemancerAnimation.Fire();
                break;

            case actionKind.fire2:
                if (distanceToPlayer < spoutDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                firemancerAnimation.Fire2();
                break;

            case actionKind.spout:
                if (distanceToPlayer > spoutDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                firemancerAnimation.Spout();
                break;
        }
    }
}
