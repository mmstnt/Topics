using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Net.WebRequestMethods;

public class Mushroom : MonoBehaviour
{
    private Rigidbody2D rb;
    private MushroomAnimation mushroomAnimation;
    private GameObject player;
    private float moveDuring;
    public LayerMask GroundLayer;
    private PhysicsCheck physicsCheck;

    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("角色參數")]
    public GameObject spore;
    public GameObject spore2;
    public float MoveSpeed; // 角色移動速度
    public float moveTimeMin;
    public float moveTimeMax;
    public float biteDistance;
    public RaycastHit2D hit;

    [Header("角色狀態")]
    public int direction; // 移動方向（-1 表示左，1 表示右）
    public float distanceToPlayerX;
    public float distanceToPlayerY;
    public bool action;
    public bool isDead;
    public enum actionKind {move, controlMushroom,bite, throwSpore }
    public actionKind actionMode;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();  
        mushroomAnimation = transform.Find("Ani").GetComponent<MushroomAnimation>();
        direction = Random.Range(0, 2) * 2 - 1; // 隨機初始化方向（-1 或 1）
        physicsCheck = GetComponent<PhysicsCheck>();
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
        if (isDead) return;
        CliffTurn();
        updateCharacterFacing();
        mushroomAction();
        distanceToPlayerX = Mathf.Abs(player.transform.position.x - transform.position.x);
        distanceToPlayerY = Mathf.Abs(player.transform.position.y - transform.position.y);
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        move();
    }

    public void ThrowSpore()
    {
        int times = 10;
        for (int i = 0; i < times; i++)
        {
            GameObject sporeObject = Instantiate(spore, transform.position, transform.rotation);
            sporeObject.GetComponent<AttackSource>().attackSource = this.transform;
            sporeObject.GetComponent<Spore>().attack3(direction);
        }
    }

    public void ControlMushroom()
    {
        Vector3 newPosition = transform.position;
        Vector3 newPosition2 = transform.position;
        newPosition.x = 1;
        newPosition.y = 1;
        newPosition2.x = 28;
        newPosition2.y = 0.5f;
        int times = 10;
        for (int i = 0; i < times; i++)
        {
            GameObject spore2Object = Instantiate(spore2, newPosition, transform.rotation);
            spore2Object.GetComponent<AttackSource>().attackSource = this.transform;
            spore2Object.GetComponent<Spore2>().attack3(direction);
        }

        for (int i = 0; i < times; i++)
        {
            GameObject spore2Object = Instantiate(spore2, newPosition2, transform.rotation);
            spore2Object.GetComponent<AttackSource>().attackSource = this.transform;
            spore2Object.GetComponent<Spore2>().attack3(direction);
        }
    }


    public void move()
    {
        if (hit.collider != null&& physicsCheck.isGround&& distanceToPlayerY < 1)
            direction = direction * -1;
        if (actionMode != actionKind.move)
            return;
        if (actionMode == actionKind.move)
        {
            rb.velocity = new Vector2(direction * MoveSpeed * Time.deltaTime, rb.velocity.y); // 僅在 X 軸上移動
            if (hit.collider != null && physicsCheck.isGround && distanceToPlayerY>1)
            {
                transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 3, 20), ForceMode2D.Impulse);
            }
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

    public void CliffTurn()
    {
        // 計算射線的方向
        Vector3 rayDirection = transform.localScale.x * new Vector3(1f, 0, 0);
        // 射線的起點
        Vector3 rayStart = transform.position + new Vector3(0, -1, 0);
        hit = Physics2D.Raycast(rayStart, rayDirection, 3, GroundLayer);
        // 繪製射線（用於調試，可視化射線）
        Debug.DrawRay(rayStart, rayDirection * 3, Color.red);

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
             
            case actionKind.controlMushroom:
                if (distanceToPlayerX < biteDistance)
                    break;
                action = true;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                mushroomAnimation.attack1();
                break;


            case actionKind.bite:
                if (distanceToPlayerX > biteDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                mushroomAnimation.attack2();
 
                break;
            case actionKind.throwSpore:
                if (distanceToPlayerX < biteDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                mushroomAnimation.attack3();
                break;



        }
    }
    public void MushroomDead()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
    }
}
