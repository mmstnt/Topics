using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    private GolemAnimation golemAnimation;
    private GameObject player;
    private Transform golemTransform;
    private float timer;
    private float moveDuring;

    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("角色參數")]
    public GameObject laser;
    public GameObject rock;
    public float MoveSpeed; // 角色移動速度
    public float moveTimeMin;
    public float moveTimeMax;

    [Header("角色狀態")]
    public int direction; // 移動方向（-1 表示左，1 表示右）
    public float distanceToPlayer;
    public bool action;
    public enum actionKind { move, transmit ,shoot,laser}
    public actionKind actionMode;

    private void Awake()
    {
        golemTransform = this.transform;
        rb = transform.GetComponent<Rigidbody2D>();
        golemAnimation = transform.Find("Ani").GetComponent<GolemAnimation>();
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
        mushroomAction();
        distanceToPlayer = Mathf.Abs(player.transform.position.x - transform.position.x);
    }

    private void FixedUpdate()
    {
        move();
    }
    public void transmit()
    {
        golemTransform = this.transform;
        float randomX = Random.Range(-20f, 20f); // 隨機生成 -20 到 20 的數值
        Vector3 newPosition = new Vector3(randomX, golemTransform.position.y, golemTransform.position.z); // 保持 y 和 z 不變
        golemTransform.position = newPosition; // 更新位置
    }

    public void shootlaser()
    {
        GameObject laserObject = Instantiate(laser, transform.position, transform.rotation);
        laserObject.GetComponent<Laser>().Move(direction);
    }  

    public void shootrock()
    {
        GameObject rockObject = Instantiate(rock, transform.position, transform.rotation);
        rockObject.GetComponent<Rock>().Move(direction);
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
        else if (moveDuring < 0)
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


    public void mushroomAction()
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
             
            case actionKind.transmit:
                action = true;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                golemAnimation.Golemtransmit();
                break;

            case actionKind.shoot:
                action = true;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                golemAnimation.Golemshoot();
                break;

            case actionKind.laser:
                action = true;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                golemAnimation.Golemlaser();
                break;







        }
    }
}
