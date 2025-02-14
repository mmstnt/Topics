using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton2 : MonoBehaviour
{
    private Rigidbody2D rb;
    private Skeleton2Animation skeleton2Animation;
    private GameObject player;

    [Header("角色參數")]
    public GameObject skeleton2;
    public float MoveSpeed; // 角色移動速度
     

    [Header("角色狀態")]
    public int direction; // 移動方向（-1 表示左，1 表示右）
    public float distanceToPlayer;
    public bool action;
    public float doing=0;
    public enum actionKind { move,call }
    public actionKind actionMode;
    private float moveDuring;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        skeleton2Animation = transform.Find("Ani").GetComponent<Skeleton2Animation>();
        direction = Random.Range(0, 2) * 2 - 1; // 隨機初始化方向（-1 或 1）
    }


    public void findPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");  // 找到 Player 物件
    }


    private void Update()
    {
        updateCharacterFacing();
        distanceToPlayer = Mathf.Abs(player.transform.position.x - transform.position.x);
    }

    private void FixedUpdate()
    {
        move();
    }

    public void move()
    {
        if (distanceToPlayer > 2 & doing==0)
        {
            direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
            rb.velocity = new Vector2(direction * MoveSpeed * Time.deltaTime, rb.velocity.y); // 僅在 X 軸上移動

        }
        if (distanceToPlayer < 2)
        {
            doing = 1;
            direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
            skeleton2Animation.Skeleton2attack();

        }     
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

     


     
}
