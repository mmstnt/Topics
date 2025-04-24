using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireKnight : MonoBehaviour
{
    public Rigidbody2D rb;
    private FireKnightAni ani;
    private GameObject player;
    private Character character;
    private PhysicsCheck physicsCheck;

    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;
    public VoidEventSO cameraLensEvent;

    [Header("角色參數")]
    public float moveTimeMin;
    public float moveTimeMax;
    public float rotateTimeMin;
    public float rotateTimeMax;
    public float attackDistance;

    [Header("角色狀態")]
    public float distanceToPlayer;
    public bool action;
    public enum actionKind { move, rotate, fire, bomb , slide }
    public actionKind actionMode;
    public List<actionKind> actionList;
    public bool isJump;
    private float moveDuring;
    public float slideForce;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        character = transform.GetComponent<Character>();
        physicsCheck = transform.GetComponent<PhysicsCheck>();
        ani = transform.Find("Ani").GetComponent<FireKnightAni>();
        isJump = false;
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
        characterAction();
        distanceToPlayer = Mathf.Abs(player.transform.position.x - transform.position.x);
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
                rb.velocity = Vector2.zero;
                getPlayerSite();
                moveDuring = Random.Range(moveTimeMin, moveTimeMax);
                break;
            case actionKind.rotate:
                 
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                moveDuring = Random.Range(rotateTimeMin, rotateTimeMax);
                ani.isRotate();
                break;
            case actionKind.fire:
                if (distanceToPlayer > attackDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.isFire();
                break;
            case actionKind.bomb:
                if (distanceToPlayer > attackDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.isBomb();
                break;

            case actionKind.slide:
               if (distanceToPlayer < attackDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                slide();
                ani.slide();
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
                actionList.Add(actionKind.bomb);
                break;
            case 2:
                actionList.Add(actionKind.fire);
                 
                break;
            case 3:
                actionList.Add(actionKind.rotate);

                break;
            case 4:
                actionList.Add(actionKind.slide);

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
        if (!(actionMode == actionKind.move || actionMode == actionKind.rotate))
            return;
        rb.velocity = new Vector2(transform.localScale.x * character.speed * Time.deltaTime, rb.velocity.y);
        if (moveDuring < 0 && actionMode == actionKind.move)
        {
            action = false;
        }
        else if (moveDuring < 0 && actionMode == actionKind.rotate) 
        {
            ani.isRotateEnd();
        }
        moveDuring -= Time.deltaTime;
    }


    private void slide()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
        Vector2 dirForce = new Vector2(transform.localScale.x, 0).normalized;
        rb.AddForce(dirForce * slideForce, ForceMode2D.Impulse);
    }

}
