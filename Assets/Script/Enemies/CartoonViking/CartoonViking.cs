using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CartoonViking : MonoBehaviour
{
    public Rigidbody2D rb;
    private CartoonVikingAni ani;
    private GameObject player;
    private Character character;

    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;
    public VoidEventSO cameraLensEvent;

    [Header("角色參數")]
    public GameObject gunBullet;
    public float moveTimeMin;
    public float moveTimeMax;
    public float attackMinDistance;
    public float attackMaxDistance;
    public float slideForce;
    public int gunAttackMinCount;
    public int gunAttackMaxCount;

    [Header("角色狀態")]
    public float distanceToPlayer;
    public bool isGun;
    public bool action;
    public enum actionKind { move, attack01, attack02, axeToGun, gunToAxe, slide , slideAttack , slideGunAttack }
    public actionKind actionMode;
    public List<actionKind> actionList;
    private float moveDuring;
    private int switchActionCount;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        character = transform.GetComponent<Character>();
        ani = transform.Find("Ani").GetComponent<CartoonVikingAni>();
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
        slideAttack();
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
        switchActionCount--;
        switch (actionMode)
        {
            case actionKind.move:
                action = true;
                getPlayerSite();
                moveDuring = Random.Range(moveTimeMin, moveTimeMax);
                break;
            case actionKind.attack01:
                if (distanceToPlayer < attackMinDistance || distanceToPlayer > attackMaxDistance || isGun)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.isAttack01();
                break;
            case actionKind.attack02:
                if (distanceToPlayer < attackMaxDistance || !isGun) 
                {
                    if (isGun) 
                    {
                        actionList.Add(actionKind.slideGunAttack);
                        actionList.Add(actionKind.attack02);
                    }
                    break;
                }
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.isAttack02();
                break;
            case actionKind.axeToGun:
                if (isGun || switchActionCount > 0) 
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                switchActionCount = 3;
                ani.axeToGun();
                break;
            case actionKind.gunToAxe:
                if (!isGun || switchActionCount > 0)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                switchActionCount = 3;
                ani.gunToAxe();
                break;
            case actionKind.slide:
                if (distanceToPlayer < attackMaxDistance || isGun)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                slide();
                ani.slide();
                break;
            case actionKind.slideAttack:
                if (distanceToPlayer < attackMinDistance || distanceToPlayer > attackMaxDistance || isGun)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.slideAttack();
                break;
            case actionKind.slideGunAttack:
                if (!isGun)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
                slide();
                ani.slide();
                break;
        }
    }

    private void getActionMode()
    {
        int mode = Random.RandomRange(0, 8);
        switch (mode)
        {
            case 0:
                actionList.Add(actionKind.move);
                break;
            case 1:
                actionList.Add(actionKind.attack01);
                break;
            case 2:
                for (int i = Random.RandomRange(gunAttackMinCount, gunAttackMaxCount+1); i > 0; i--) 
                {
                    actionList.Add(actionKind.attack02);
                }
                break;
            case 3:
                actionList.Add(actionKind.axeToGun);
                break;
            case 4:
                actionList.Add(actionKind.gunToAxe);
                break;
            case 5:
                actionList.Add(actionKind.slide);
                break;
            case 6:
                actionList.Add(actionKind.slide);
                actionList.Add(actionKind.attack01);
                break;
            case 7:
                actionList.Add(actionKind.slideGunAttack);
                actionList.Add(actionKind.attack02);
                break;
        }
    }

    private void getPlayerSite()
    {
        float dir = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
        transform.localScale = new Vector3(dir, 1, 1);
    }

    private void move()
    {
        if (actionMode != actionKind.move)
            return;
        rb.velocity = new Vector2(transform.localScale.x * character.speed * Time.deltaTime, rb.velocity.y);
        if (moveDuring < 0)
        {
            action = false;
        }
        moveDuring -= Time.deltaTime;
    }

    private void slide()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
        Vector2 dirForce = new Vector2(transform.localScale.x, 0).normalized;
        rb.AddForce(dirForce * slideForce, ForceMode2D.Impulse);
    }

    private void slideAttack()
    {
        if (actionMode != actionKind.slide) return;
        if(distanceToPlayer < attackMaxDistance) 
        {
            actionList.Add(actionKind.slideAttack);
            ani.slideEnd();
        }
    }

    public void shoot() 
    {
        GameObject bullet = Instantiate(gunBullet, transform.position, transform.rotation);
        bullet.GetComponent<AttackSource>().attackSource = this.transform;
        bullet.transform.localScale = this.transform.localScale;
    }
}
