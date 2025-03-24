using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OldGuardian : MonoBehaviour
{
    public Rigidbody2D rb;
    private OldGuardianAni ani;
    private GameObject player;
    private Character character;
    private PhysicsCheck physicsCheck;

    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;
    public VoidEventSO cameraLensEvent;

    [Header("角色參數")]
    public GameObject oldGuardianBomb;
    public float moveTimeMin;
    public float moveTimeMax;
    public float attack01MinDistance;
    public float attack01MaxDistance;
    public Vector2 attack02Force;
    public int oldGuardianBombMinCount;
    public int oldGuardianBombMaxCount;
    public Vector2 jumpForceMin;
    public Vector2 jumpForceMax;

    [Header("角色狀態")]
    public float distanceToPlayer;
    public bool isDead;
    public bool action;
    public enum actionKind { move , attack01 , attack02 , spit , jump }
    public actionKind actionMode;
    public List<actionKind> actionList;
    public bool isJump;
    private float moveDuring;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        character = transform.GetComponent<Character>();
        physicsCheck = transform.GetComponent<PhysicsCheck>();
        ani = transform.Find("Ani").GetComponent<OldGuardianAni>();
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
        if (isDead || player == null) return;
        characterAction();
        distanceToPlayer = Mathf.Abs(player.transform.position.x - transform.position.x);
    }

    private void FixedUpdate()
    {
        if (isDead || player == null) return;
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
                getPlayerSite();
                moveDuring = Random.Range(moveTimeMin, moveTimeMax);
                break;
            case actionKind.attack01:
                if (distanceToPlayer < attack01MinDistance || distanceToPlayer > attack01MaxDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.isAttack01();
                break;
            case actionKind.attack02:
                if (distanceToPlayer > attack01MaxDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.isAttack02();
                break;
            case actionKind.spit:
                if (distanceToPlayer < attack01MaxDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.isSpit();
                break;
            case actionKind.jump:
                if (distanceToPlayer < attack01MaxDistance || !physicsCheck.isGround || isJump)
                    break;
                action = true;
                getPlayerSite();
                jump();
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
                actionList.Add(actionKind.attack01);
                break;
            case 2:
                actionList.Add(actionKind.attack02);
                actionList.Add(actionKind.attack01);
                break;
            case 3:
                actionList.Add(actionKind.attack01);
                actionList.Add(actionKind.attack02);
                break;
            case 4:
                for (int i = Random.Range(oldGuardianBombMinCount,oldGuardianBombMaxCount+1); i > 0; i--) 
                {
                    actionList.Add(actionKind.spit);
                }
                break;
            case 5:
                actionList.Add(actionKind.jump);
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
        if (actionMode != actionKind.move || isJump)
            return;
        rb.velocity = new Vector2(transform.localScale.x * character.speed * Time.deltaTime, rb.velocity.y);
        if (moveDuring < 0)
        {
            action = false;
        }
        moveDuring -= Time.deltaTime;
    }

    public void jump()
    {
        isJump = true;
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(transform.localScale.x * Random.Range(jumpForceMin.x, jumpForceMax.x), Random.Range(jumpForceMin.y, jumpForceMax.y)), ForceMode2D.Impulse);
    }

    public void attack02(Transform damageSource, Transform target) 
    {
        Vector2 dirForce = new Vector2(target.position.x - damageSource.position.x, 0).normalized;
        dirForce.y = 1;
        target.GetComponent<Rigidbody2D>()?.AddForce(dirForce * attack02Force, ForceMode2D.Impulse);
    }

    public void spitBomb()
    {
        Vector3 vector = transform.position;
        vector.x += transform.localScale.x * 2;
        vector.y += 1;
        GameObject bombObject = Instantiate(oldGuardianBomb, vector, transform.rotation);
        bombObject.GetComponent<OldGuardianBomb>().isThrow(transform.localScale.x);
        bombObject.GetComponent<AttackSource>().attackSource = this.transform;
    }

    public void dead()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
    }
}
