using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonLighter : MonoBehaviour
{
    public Rigidbody2D rb;
    private SkeletonLighterAni ani;
    private GameObject player;

    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("角色參數")]
    public GameObject blueFireCast;
    public float moveSpeed;
    public float moveTimeMin;
    public float moveTimeMax;
    public float attackMinDistance;
    public float attackMaxDistance;

    [Header("角色狀態")]
    public float distanceToPlayer;
    public bool isDead;
    public bool action;
    public enum actionKind { move, attack01, attack02}
    public actionKind actionMode;
    public List<actionKind> actionList;
    private float moveDuring;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        ani = transform.Find("Ani").GetComponent<SkeletonLighterAni>();
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
        skeletonLighterAction();
        distanceToPlayer = Mathf.Abs(player.transform.position.x - transform.position.x);
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        move();
    }

    public void skeletonLighterAction()
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
                if (distanceToPlayer < attackMinDistance || distanceToPlayer > attackMaxDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.isAttack01();
                break;
            case actionKind.attack02:
                if (distanceToPlayer < attackMinDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.isAttack02();
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
                actionList.Add(actionKind.attack01);
                break;
            case 2:
                actionList.Add(actionKind.attack02);
                break;
            case 3:
                actionList.Add(actionKind.move);
                actionList.Add(actionKind.attack01);
                break;

        }
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
        moveDuring -= Time.deltaTime;
    }

    public void releaseBlueFire() 
    {
        GameObject blueFiretObject = Instantiate(blueFireCast, transform.position, transform.rotation);
        blueFiretObject.GetComponent<BlueFireCast>().attackSource = this.transform;
    }

    public void dead()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
    }
}
