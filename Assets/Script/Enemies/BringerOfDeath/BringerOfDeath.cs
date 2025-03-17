using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeath : MonoBehaviour
{
    public Rigidbody2D rb;
    private BringerOfDeathAnimation ani;
    private GameObject player;

    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("角色參數")]
    public GameObject bringerOfDeathSpellCast;
    public GameObject bringerOfDeathBulletCast;
    public float moveSpeed;
    public float moveTimeMin;
    public float moveTimeMax; 
    public float attack01Distance;
    public float attack01MoveDistance;
    public float attack01MoveSpeed;

    [Header("角色狀態")]
    public float distanceToPlayer;
    public bool isDead;
    public bool isTeleport;
    public bool action;
    public enum actionKind { move , attack01, attack02, cast01 , cast02 , teleport }
    public actionKind actionMode;
    public List<actionKind> actionList;
    private float moveDuring;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        ani = transform.Find("Ani").GetComponent<BringerOfDeathAnimation>();
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
        bringerOfDeathAction();
        distanceToPlayer = Mathf.Abs(player.transform.position.x - transform.position.x);
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        move();
    }

    public void bringerOfDeathAction()
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
            case actionKind.attack01:
                if (distanceToPlayer < attack01Distance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.isAttack01();
                break;
            case actionKind.attack02:
                if (distanceToPlayer < attack01Distance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.isAttack02();
                break;
            case actionKind.cast01:
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.isCast01();
                break;
            case actionKind.cast02:
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.isCast02();
                break;
            case actionKind.teleport:
                action = true;
                rb.velocity = Vector2.zero;
                moveDuring = Random.Range(moveTimeMin, moveTimeMax);
                getPlayerSite();
                ani.isTeleport();
                break;
        }
    }

    private void getActionMode() 
    {
        int mode = Random.RandomRange(0, 7);
        switch (mode)
        {
            case 0:
                actionList.Add(actionKind.move);
                break;
            case 1:
                actionList.Add(actionKind.attack01);
                break;
            case 2:
                actionList.Add(actionKind.attack01);
                actionList.Add(actionKind.attack02);
                break;
            case 3:
                actionList.Add(actionKind.cast01);
                break;
            case 4:
                actionList.Add(actionKind.cast02);
                break;
            case 5:
                actionList.Add(actionKind.teleport);
                actionList.Add(actionKind.attack02);
                break;
            case 6:
                actionList.Add(actionKind.move);
                actionList.Add(actionKind.attack01);
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
        if (actionMode != actionKind.move && !isTeleport)
            return;
        if (isTeleport) 
        {
            if(moveDuring > 0) 
            {
                if(distanceToPlayer < attack01Distance && distanceToPlayer > attack01MoveDistance) 
                {
                    getPlayerSite();
                }
                rb.velocity = new Vector2(-transform.localScale.x * moveSpeed * Time.deltaTime, rb.velocity.y);
                moveDuring -= Time.deltaTime;
            }
            else 
            {
                getPlayerSite();
                ani.isTeleportEnd();
            }
        }
        else 
        {
            rb.velocity = new Vector2(-transform.localScale.x * moveSpeed * Time.deltaTime, rb.velocity.y);
            if (moveDuring < 0)
            {
                action = false;
            }
            moveDuring -= Time.deltaTime;
        }
    }

    public void dead()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
    }
}
