using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton2 : MonoBehaviour
{
    private Rigidbody2D rb;
    private Skeleton2Animation skeleton2Animation;
    private GameObject player;
    public LayerMask GroundLayer;
    private PhysicsCheck physicsCheck;

    [Header("角色參數")]
    public float MoveSpeed; // 角色移動速度
    public RaycastHit2D hit;

    [Header("角色狀態")]
    public int direction; // 移動方向（-1 表示左，1 表示右）
    public float distanceToPlayer;
    public bool action;
    public bool isDead;
    public enum actionKind { move,call }
    public actionKind actionMode;
    private float moveDuring;

    private void Awake()
    {
        physicsCheck = GetComponent<PhysicsCheck>();
        rb = transform.GetComponent<Rigidbody2D>();
        skeleton2Animation = transform.Find("Ani").GetComponent<Skeleton2Animation>();
        direction = Random.Range(0, 2) * 2 - 1; // 隨機初始化方向（-1 或 1）
        player = GameObject.FindGameObjectWithTag("Player");  // 找到 Player 物件
        isDead = false;
    }

    private void Update()
    {
        CliffTurn();
        updateCharacterFacing();
        distanceToPlayer = Mathf.Abs(player.transform.position.x - transform.position.x);
    }

    private void FixedUpdate()
    {
        move();
    }

    public void move()
    {
        if (isDead) return;
        if (distanceToPlayer > 2)
        {
            direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
            rb.velocity = new Vector2(direction * MoveSpeed * Time.deltaTime, rb.velocity.y); // 僅在 X 軸上移動
            if (   physicsCheck.isGround& hit.collider != null)
            {
                transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 3, 20), ForceMode2D.Impulse);
            }
        }
        else
        {
            direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
            skeleton2Animation.Skeleton2attack();
            isDead = true;
        }     
    }

    public void CliffTurn()
    {
        // 計算射線的方向
        Vector3 rayDirection = transform.localScale.x * new Vector3(1f, 0, 0);
        // 射線的起點
        Vector3 rayStart = transform.position;
        hit = Physics2D.Raycast(rayStart, rayDirection, 3, GroundLayer);
        // 繪製射線（用於調試，可視化射線）
        Debug.DrawRay(rayStart, rayDirection * 3, Color.red); // 紅色射線，長度為 2
    }

    public void updateCharacterFacing()
    {
        if (isDead) return;
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
}
