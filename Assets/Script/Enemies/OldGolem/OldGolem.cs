using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldGolem : MonoBehaviour
{
    public Rigidbody2D rb;
    private GameObject player;
    private OldGolemAnimation ani;
    private PhysicsCheck physicsCheck;
    public LayerMask GroundLayer;

    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;
    public VoidEventSO cameraLensEvent;

    [Header("角色參數")]
    public GameObject oldGolemBomb;
    public float moveSpeed;
    public float moveTimeMin;
    public float moveTimeMax;
    public float attackDistance;
    public float spitDistance;
    public RaycastHit2D hit;

    public float slideForce;

    [Header("角色狀態")]
    public float distanceToPlayer;
    public bool isDead;
    public bool action;
    public enum actionKind { move, hit1, hit2, spit,slide}
    public actionKind actionMode;
    public List<actionKind> actionList;
    private float moveDuring;
    public bool isJump;
    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        
        ani = transform.Find("Ani").GetComponent<OldGolemAnimation>();
        physicsCheck = transform.GetComponent<PhysicsCheck>();
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
        if (isDead || player == null) return;
        oldgolemAction();
        CliffTurn();
        distanceToPlayer = Mathf.Abs(player.transform.position.x - transform.position.x);
    }

    private void FixedUpdate()
    {
        if (isDead || player == null) return;
        move();
    }

    public void oldgolemAction()
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
                getPlayerSite();
                moveDuring = Random.Range(moveTimeMin, moveTimeMax);
                break;
            case actionKind.hit1:
                if (distanceToPlayer > attackDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.hit1();
                break;
            case actionKind.hit2:
                if (distanceToPlayer > attackDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.hit2();
                break;
            case actionKind.spit:
                if (distanceToPlayer < attackDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.spit();
                break;
            case actionKind.slide:
                if (distanceToPlayer < attackDistance)
                    break;
                action = true;
                getPlayerSite();
                slide();
                ani.slide();
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
                actionList.Add(actionKind.slide);
                actionList.Add(actionKind.hit2);
                
                break;
            case 2:
                actionList.Add(actionKind.spit);
                break;

            case 3:
                actionList.Add(actionKind.slide);
                
                 
                break;


        }
    }

    private void getPlayerSite()
    {
        float dir = (player.transform.position.x - transform.position.x) > 0 ? -1 : 1;
        transform.localScale = new Vector3(dir, 1, 1);
    }

    public void move()
    {
        if (actionMode != actionKind.move )
            return;
        rb.velocity = new Vector2(transform.localScale.x * moveSpeed * Time.deltaTime, rb.velocity.y);
        if (moveDuring < 0)
        {
            action = false;
        }
        moveDuring -= Time.deltaTime; ;

    }
    private void slide()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
        Vector2 dirForce = new Vector2(transform.localScale.x, 0).normalized;
        rb.AddForce(-dirForce * slideForce, ForceMode2D.Impulse);
    }
    public void spitBomb()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector3 vector = transform.position;
            vector.x += transform.localScale.x * -2;
            vector.y += 1;

            GameObject bombObject = Instantiate(oldGolemBomb, vector, transform.rotation);
            bombObject.GetComponent<Bullet>().isThrow(transform.localScale.x);
            bombObject.GetComponent<AttackSource>().attackSource = this.transform;
        }

    }

    public void dead()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
    }

    public void CliffTurn()
    {
        // 計算射線的方向
        Vector3 rayDirection = transform.localScale.x * new Vector3(-1f, 0, 0);
        // 射線的起點
        Vector3 rayStart = transform.position + new Vector3(0, -1, 0);
        hit = Physics2D.Raycast(rayStart, rayDirection, 3, GroundLayer);
        // 繪製射線（用於調試，可視化射線）
        Debug.DrawRay(rayStart, rayDirection * 3, Color.red);

    }

}
