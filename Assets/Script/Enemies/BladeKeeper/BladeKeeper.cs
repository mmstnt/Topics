using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeKeeper : MonoBehaviour
{
    public Rigidbody2D rb;
    private BladeKeeperAni ani;
    private GameObject player;
    private Character character;
    private PhysicsCheck physicsCheck;

    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;
    public VoidEventSO cameraLensEvent;

    [Header("角色參數")]

    public GameObject bladeKeeperThrow;
    public float moveTimeMin;
    public float moveTimeMax;
     
    public float throwDistance;
    public float spikeDistance;
    public float cutDistance;


    [Header("角色狀態")]
    public float distanceToPlayer;
    public bool action;
    public enum actionKind { move, cut, shoot, spike }
    public actionKind actionMode;
    public List<actionKind> actionList;
    public bool isJump;
    private float moveDuring;
    private float knifeDuring;
     



    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        character = transform.GetComponent<Character>();
        physicsCheck = transform.GetComponent<PhysicsCheck>();
        ani = transform.Find("Ani").GetComponent<BladeKeeperAni>();
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
            case actionKind.cut:
                if (distanceToPlayer > cutDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.isCut();
                break;
            case actionKind.shoot:
                if (distanceToPlayer < throwDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.isShoot();
                break;
            case actionKind.spike:
                if (distanceToPlayer > spikeDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.isSpike();
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
                actionList.Add(actionKind.cut);
                break;
            case 2:
                actionList.Add(actionKind.shoot);

                break;
            case 3:
                actionList.Add(actionKind.spike);

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
        if (actionMode != actionKind.move )
            return;
        rb.velocity = new Vector2(transform.localScale.x * character.speed * Time.deltaTime, rb.velocity.y);
        if (moveDuring < 0)
        {
            action = false;
        }
        moveDuring -= Time.deltaTime;
    }


    public void throwKnife()
    {
        float dir = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
        transform.localScale = new Vector3(dir, 1, 1);

        Vector3 vector = transform.position;
        knifeDuring = Random.Range(1f, 3f);  
        vector.x += dir;  
        vector.y += knifeDuring;

         
        Quaternion bombRotation = dir < 0 ? Quaternion.identity : Quaternion.Euler(0, 180f, 0);

        GameObject bombObject = Instantiate(bladeKeeperThrow, vector, bombRotation);

         
        bombObject.GetComponent<AttackSource>().attackSource = this.transform;



    }

}
