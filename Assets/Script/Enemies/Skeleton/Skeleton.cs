using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    private Rigidbody2D rb;
    private SkeletonAnimation ani;
    private GameObject player;
    private Character character;
    private PhysicsCheck physicsCheck;
    public LayerMask GroundLayer;

    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;
    public VoidEventSO cameraLensEvent;

    [Header("角色參數")]
    public GameObject skeleton2;
    public float moveTimeMin;
    public float moveTimeMax;
    public float cutDistance;
    public RaycastHit2D hit;
    public RaycastHit2D hitup;

    [Header("角色狀態")]
    public int direction; // 移動方向（-1 表示左，1 表示右）
    public float distanceToPlayer;
    public float distanceToPlayerY;
    public bool action;
    public enum actionKind { move, call, cut, wave }
    public actionKind actionMode;
    public List<actionKind> actionList;
    private float moveDuring;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        character = transform.GetComponent<Character>();
        physicsCheck = GetComponent<PhysicsCheck>();
        ani = transform.Find("Ani").GetComponent<SkeletonAnimation>();
        direction = Random.Range(0, 2) * 2 - 1; // 隨機初始化方向（-1 或 1）
    }

    private void OnEnable()
    {
        afterSceneLoadEvent.onEventRaised += onAfterSceneLoadEvent;
        cameraLensEvent.onEventRaised += onCameraLensEvent;
    }

    private void OnDisable()
    {
        afterSceneLoadEvent.onEventRaised -= onAfterSceneLoadEvent;
        cameraLensEvent.onEventRaised -= onCameraLensEvent;
    }

    private void onAfterSceneLoadEvent()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
    }

    private void onCameraLensEvent()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (character.isDead || player == null) return;
        CliffTurn();
        updateCharacterFacing();
        characterAction();
        distanceToPlayer = Mathf.Abs(player.transform.position.x - transform.position.x);
        distanceToPlayerY = player.transform.position.y - transform.position.y;
    }

    private void FixedUpdate()
    {
        if (character.isDead || player == null) return;
        move();
    }

    public void characterAction()
    {
        if (action) return;
        if (actionList.Count == 0)
        {
            getActionMode();
            return;
        }
        actionMode = actionList[0];
        actionList.RemoveAt(0);
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
                ani.Skeletoncall();
                break;
            case actionKind.cut:
                if (distanceToPlayer > cutDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                ani.Skeletoncut();
                break;
            case actionKind.wave:
                if (distanceToPlayer > cutDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                ani.Skeletonwave();
                break;
        }
    }
    private void getActionMode()
    {
        int mode = Random.RandomRange(0, 4);
        switch (mode)
        {
            case 0:
                actionList.Add(actionKind.move);
                break;
            case 1:
                actionList.Add(actionKind.call);
                break;
            case 2:
                actionList.Add(actionKind.cut);
                break;
            case 3:
                actionList.Add(actionKind.wave);
                break;
        }
    }

    public void CliffTurn()
    {
        // 計算射線的方向
        Vector3 rayDirection = transform.localScale.x * new Vector3(1f, 0, 0);
        Vector3 rayDirectionup = transform.localScale.x * new Vector3(1f,0, 0);

        // 射線的起點
        Vector3 rayStart = transform.position + new Vector3(0, -1, 0);
        Vector3 rayStartup = transform.position + new Vector3(0, 0.3f, 0);
        hit = Physics2D.Raycast(rayStart, rayDirection, 3, GroundLayer);
        // 繪製射線（用於調試，可視化射線）
        Debug.DrawRay(rayStart, rayDirection * 3, Color.red);  
        hitup = Physics2D.Raycast(rayStartup, rayDirectionup, 4, GroundLayer);
        Debug.DrawRay(rayStartup, rayDirectionup * 4, Color.green);


    }

    public void move()
    {
        if (actionMode != actionKind.move)
            return;
        if (actionMode == actionKind.move)
        {
            rb.velocity = new Vector2(direction * character.speed * Time.deltaTime, rb.velocity.y);
            if ((hit.collider != null && physicsCheck.isGround) ||(hitup.collider != null && physicsCheck.isGround && distanceToPlayerY > 0.5))
            {
                transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 3, 10), ForceMode2D.Impulse);
            } 
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
        int times = 1;
        for (int i = 0; i < times; i++)
        {
            Vector3 newPosition = transform.position;

            float x = Random.Range(-10f, 10f);
            newPosition.x = player.transform.position.x + x;
            newPosition.y = 10;
            GameObject skeleton2Object = Instantiate(skeleton2, newPosition, transform.rotation);
            skeleton2Object.GetComponent<AttackSource>().attackSource = this.transform;
        }
    }
}