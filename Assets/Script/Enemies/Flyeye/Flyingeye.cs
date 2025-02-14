using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Flyingeye : MonoBehaviour
{
    private Rigidbody2D rb;
    private FlyingeyeAnimation flyingeyeAnimation;
    private GameObject player;

    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("角色參數")]
    public GameObject sand;
    public GameObject wind;
    public GameObject laser;
    public float MoveSpeedX; // 角色移動速度
    public float MoveSpeedY;
    public float moveTimeMin;
    public float moveTimeMax;
    public float flyDistance;
    public float laserDistance;

    [Header("角色狀態")]
    public int direction; // 移動方向（-1 表示左，1 表示右）
    public float distanceToPlayer;
    public bool action;
    public enum actionKind { move, throwSand, throwWind, laser }
    public actionKind actionMode;
    public float moveDuring;
    public float upDuring;
    public float downDuring;
    
    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        flyingeyeAnimation = transform.Find("Ani").GetComponent<FlyingeyeAnimation>();
        player = GameObject.Find("Game/Player");
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

    public void ThrowSand()
    {
        float randomValueX;
        float randomValueY;
        int times = 5;
        for (int i = 0; i < times; i++)
        {
            Vector3 newPosition = transform.position;
            randomValueX = Random.Range(-10f, 10f);
            randomValueY = Random.Range(5f, 10f);
            newPosition.x = player.transform.position.x + randomValueX;
            newPosition.y += randomValueY;
            Instantiate(sand, newPosition, transform.rotation);
        }
    }
    public void ThrowSand2()
    {
        GameObject laserObject = Instantiate(laser, transform.position, transform.rotation);
        laserObject.GetComponent<Laser>().Move(direction);
    }
    public void ThrowWind()
    {
        for(int i = 0; i < 8; i++) 
        {
            Instantiate(wind, transform.position, Quaternion.Euler(0, 0, 45*i));
        }
    }
    private void Update()
    {
        updateCharacterFacing();
        flyingeyeAction();
        distanceToPlayer = Mathf.Abs(player.transform.position.y - transform.position.y);

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
            if (upDuring < 0)
            {
                upDuring = Random.Range(1, 2);
                if (flyDistance > distanceToPlayer)
                {
                    MoveSpeedY = Random.Range(-150, 150);
                }
                else if(flyDistance < distanceToPlayer)
                {
                    MoveSpeedY = Random.Range(-100, -300);
                }
            }
            upDuring -= Time.deltaTime;
            rb.velocity = new Vector2(direction * MoveSpeedX * Time.deltaTime, MoveSpeedY * Time.deltaTime); // 僅在 X 軸上移動
            if (moveDuring < 0)
            {
                action = false;
            }
        }
        else if (moveDuring < 0 )
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


    public void flyingeyeAction()
    {
        if (action) return;
        actionMode = (actionKind)Random.Range(0, 4); // 隨機選擇 0, 1, 2
        
        switch (actionMode)
        {
            
            case actionKind.move:
                action = true;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                moveDuring = Random.Range(moveTimeMin, moveTimeMax);
                break;
             
            case actionKind.throwSand:
                action = true;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                rb.velocity = Vector2.zero;
                 
                flyingeyeAnimation.attack1();
                break;
            case actionKind.throwWind:
                action = true;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                rb.velocity = Vector2.zero;
                flyingeyeAnimation.attack2();
                break;
            case actionKind.laser:
                if (distanceToPlayer > laserDistance)
                    break;
                action = true;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                rb.velocity = Vector2.zero;
                flyingeyeAnimation.attack3();
                break;

        }
    }

}


