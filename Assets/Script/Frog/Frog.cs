using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.InteropServices;
using UnityEngine;

public class Frog : MonoBehaviour
{
    public float MoveSpeed;  // 角色移動速度
    public float jumpForce;  // 跳躍力度
    public int direction;
    public float minDistance;  // 最小距離，不跳向玩家的閾值
    public float bigDistance;
    public float distanceToPlayer;
    public GameObject bullet;
    private GameObject player;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private float jumpWaitTime;  // 跳躍等待時間
    private float timer;  // 計時器
    private bool canJump;  // 是否可以跳躍
    public float minFireRate = 1f;   // 最小發射間隔
    public float maxFireRate = 3f;   // 最大發射間隔

    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        physicsCheck = transform.GetComponent<PhysicsCheck>();
        player = GameObject.Find("Game/Player");  // 找到 Player 物件
        direction = Random.Range(0, 2) * 2 - 1;  // 隨機初始化方向（-1 或 1）
        canJump = false;  // 初始時不可跳躍
        SetJumpWaitTime();  // 初始化跳躍等待時間
        ScheduleNextShot();
    }

    void Update()
    {
        Jump();
        MoveTowardsPlayer();
    }

    void Jump()
    {
        distanceToPlayer = Mathf.Abs(player.transform.position.x - transform.position.x);

        // 如果角色接觸地面，開始計時
        if (physicsCheck.isGround)
        {
            timer += Time.deltaTime;  // 計時器開始累加

            // 當計時器超過隨機等待時間時，才允許跳躍
            if (timer >= jumpWaitTime && !canJump)
            {
                canJump = true;  // 標記為可跳躍
                timer = 0;  // 重置計時器
                SetJumpWaitTime();  // 重新設置下一次的隨機等待時間
            }

            if (canJump)
            {
                if (distanceToPlayer < bigDistance)
                {
                    // 施加跳躍力，X 軸的方向會根據隨機跳躍方向變化
                    rb.AddForce(new Vector2((Random.Range(0, 2) * 2 - 1) * MoveSpeed * 10, jumpForce), ForceMode2D.Impulse);
                    canJump = false; // 跳躍後重置跳躍狀態
                }
                else
                {
                    rb.AddForce(new Vector2(-direction * MoveSpeed * 10, jumpForce), ForceMode2D.Impulse);
                    canJump = false; // 跳躍後重置跳躍狀態
                }
            }
        }
    }

    void MoveTowardsPlayer()
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

    // 設置一個隨機的跳躍等待時間
    void SetJumpWaitTime()
    {
        jumpWaitTime = Random.Range(0f, 3f);  // 隨機等待時間在 0 到 3 秒之間
    }

    void FireBullet()
    { 
        if (physicsCheck.isGround)
        {
            GameObject bulletGameObject = Instantiate(bullet, transform.position, transform.rotation);
            bulletGameObject.GetComponent<BulletMover>().direction = new Vector3(-direction, 0, 0);
        }
        ScheduleNextShot();
    }
    void ScheduleNextShot()
    {
        // 在1到3秒之間的隨機時間後發射子彈
        float randomFireRate = Random.Range(minFireRate, maxFireRate);
        Invoke("FireBullet", randomFireRate);
    }
}




