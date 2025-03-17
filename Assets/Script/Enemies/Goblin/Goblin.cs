using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Goblin : MonoBehaviour
{
    private Rigidbody2D rb;
    private GoblinAnimation goblinAnimation;
    private GameObject player;

    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("角色參數")]
    public GameObject bomb;
    public float FastSpeed;
    public float MoveSpeed; // 角色移動速度
    public float moveTimeMin;
    public float moveTimeMax;
    public float sprintTime;
    public float cutDistance;
    public float bombDistance;

    [Header("角色狀態")]
    public int direction; // 移動方向（-1 表示左，1 表示右）
    public float distanceToPlayer;
    public bool action;
    public bool isDead;
    public enum actionKind { move, cut, throwBomb, slide}
    public actionKind actionMode;
    public float moveDuring;
    public bool cut;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        goblinAnimation = transform.Find("Ani").GetComponent<GoblinAnimation>();
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
        if (isDead) return;
        updateCharacterFacing();
        goblinAction();
        distanceToPlayer = Mathf.Abs(player.transform.position.x - transform.position.x);
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        move();
    }
    public void slide()
    {
        rb.AddForce(new Vector2(direction * FastSpeed * 10, 0), ForceMode2D.Impulse);
    }

    public void ThrowBomb()
    {
        GameObject bombObject = Instantiate(bomb, transform.position, transform.rotation);
        if (distanceToPlayer < bombDistance)
        {
            bombObject.GetComponent<Bomb>().startSpeedMin.x /= 2;
            bombObject.GetComponent<Bomb>().startSpeedMax /= 2;
        }
        bombObject.GetComponent<Bomb>().isThrow(direction);
        bombObject.GetComponent<AttackSource>().attackSource = this.transform;
    }

    public void move()
    {
        if (actionMode != actionKind.slide && actionMode != actionKind.move)  
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
    

    public void goblinAction()
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
            case actionKind.cut:
                if (distanceToPlayer > cutDistance) 
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                goblinAnimation.isCut();
                break;
            case actionKind.throwBomb:
                if (distanceToPlayer < cutDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                goblinAnimation.isThrow();
                break;
            case actionKind.slide:
                if (distanceToPlayer < cutDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                goblinAnimation.isSlide();
                break;
                
        }
         
    }

    public void goblinDead()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
    }
}
