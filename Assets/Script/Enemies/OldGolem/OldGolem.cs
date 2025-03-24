using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldGolem : MonoBehaviour
{
    public Rigidbody2D rb;
    private GameObject player;
    private OldGolemAnimation ani;
    private PhysicsCheck physicsCheck;

    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("角色參數")]
    public GameObject oldGolemBomb;
    public float moveSpeed;
    public float moveTimeMin;
    public float moveTimeMax;
     
    [Header("角色狀態")]
    public float distanceToPlayer;
    public bool isDead;
    public bool action;
    public enum actionKind { move, hit1, hit2, spit}
    public actionKind actionMode;
    public List<actionKind> actionList;
    private float moveDuring;
    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        ani = transform.Find("Ani").GetComponent<OldGolemAnimation>();
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
                action = true;
                getPlayerSite();
                moveDuring = Random.Range(moveTimeMin, moveTimeMax);
                break;
            case actionKind.hit1:
                 
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.hit1();
                break;
            case actionKind.hit2:
                 
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.hit2();
                break;
            case actionKind.spit:
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.spit();
                break;
             
        }
    }

    private void getActionMode()
    {
        int mode = Random.RandomRange(0, 5);
        switch (mode)
        {
            case 0:
                actionList.Add(actionKind.move);
                break;
            case 1:
                actionList.Add(actionKind.hit1);
                break;

            case 2:
                actionList.Add(actionKind.hit2);
                break;
            case 3:
                actionList.Add(actionKind.spit);
                break;

            case 4:
                actionList.Add(actionKind.hit1);
                actionList.Add(actionKind.hit2);
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

    public void spitBomb()
    {
        Vector3 vector = transform.position;
        vector.x += transform.localScale.x * 2;
        vector.y += 1;
        GameObject bombObject = Instantiate(oldGolemBomb, vector, transform.rotation);
        bombObject.GetComponent<Bullet>().isThrow(transform.localScale.x);
        bombObject.GetComponent<AttackSource>().attackSource = this.transform;
    }

    public void dead()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
    }

}
