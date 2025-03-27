using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : MonoBehaviour
{
    public Rigidbody2D rb;
    private GameObject player;
    private WarriorAnimation ani;
    private PhysicsCheck physicsCheck;

    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("角色參數")]

    public float moveSpeed;
    public float moveTimeMin;
    public float moveTimeMax;
    public float jumpMinDistance;
    public float jumpMaxDistance;
    public float attackMinDistance;
    public float attackMaxDistance;
    public Vector2 jumpForceMin;
    public Vector2 jumpForceMax;

    [Header("角色狀態")]
    public float distanceToPlayer;
    public bool isDead;
    public bool action;
    public enum actionKind { move, cut1, cut2, cut3, jump }
    public actionKind actionMode;
    public List<actionKind> actionList;
    private float moveDuring;
    public bool isJump;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        ani = transform.Find("Ani").GetComponent<WarriorAnimation>();
        physicsCheck = transform.GetComponent<PhysicsCheck>();
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
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (isDead) return;
        oldgolemAction();
        distanceToPlayer = Mathf.Abs(player.transform.position.x - transform.position.x);
    }

    private void FixedUpdate()
    {
        if (isDead) return;
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
                if (distanceToPlayer < attackMaxDistance)
                    break;
                action = true;
                getPlayerSite();
                moveDuring = Random.Range(moveTimeMin, moveTimeMax);
                break;
            case actionKind.cut1:
                if (distanceToPlayer < attackMinDistance || distanceToPlayer > attackMaxDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.cut1();
                break;
            case actionKind.cut2:
                if (distanceToPlayer < attackMinDistance || distanceToPlayer > attackMaxDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.cut2();
                break;
            case actionKind.cut3:
                if (distanceToPlayer < attackMinDistance || distanceToPlayer > attackMaxDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.cut3();
                break;

            case actionKind.jump:
                if (distanceToPlayer < jumpMinDistance|| distanceToPlayer > jumpMaxDistance || !physicsCheck.isGround || isJump)
                    break;
                action = true;
                getPlayerSite();
                jump();
                break;

        }
    }

    private void getActionMode()
    {
        int mode = Random.RandomRange(0, 6);
        switch (mode)
        {
            case 0:
                actionList.Add(actionKind.move);
                 
                break;
            case 1:
                actionList.Add(actionKind.jump);
                actionList.Add(actionKind.cut1);
                
                break;
            case 2:
                actionList.Add(actionKind.jump);
                actionList.Add(actionKind.cut2);

                break;
            case 3:
                actionList.Add(actionKind.jump);
                actionList.Add(actionKind.cut3);
                break;
            case 4:
                actionList.Add(actionKind.jump);
                actionList.Add(actionKind.cut1);
                actionList.Add(actionKind.cut2);
                actionList.Add(actionKind.cut3);
                break;
            case 5:
                actionList.Add(actionKind.jump);
                actionList.Add(actionKind.cut1);
                actionList.Add(actionKind.cut2);
                break;



        }
    }

    public void jump()
    {
        isJump = true;
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(transform.localScale.x * Random.Range(jumpForceMin.x, jumpForceMax.x), Random.Range(jumpForceMin.y, jumpForceMax.y)), ForceMode2D.Impulse);
    }

    private void getPlayerSite()
    {
        float dir = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
        transform.localScale = new Vector3(dir, 1, 1);
    }

    public void move()
    {
        if (actionMode != actionKind.move)
            return;
        rb.velocity = new Vector2(transform.localScale.x * moveSpeed * Time.deltaTime, rb.velocity.y);
        if (moveDuring < 0)
        {
            action = false;
        }
        moveDuring -= Time.deltaTime; ;

    }

    public void dead()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
    }

}

