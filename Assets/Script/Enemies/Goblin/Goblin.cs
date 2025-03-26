using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Goblin : MonoBehaviour
{
    private Rigidbody2D rb;
    private GoblinAnimation ani;
    private GameObject player;
    private Character character;

    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;
    public VoidEventSO cameraLensEvent;

    [Header("角色參數")]
    public GameObject bomb;
    public float FastSpeed;
    public float moveTimeMin;
    public float moveTimeMax;
    public float sprintTime;
    public float cutDistance;
    public float bombDistance;

    [Header("角色狀態")]
    public float distanceToPlayer;
    public bool action;
    public enum actionKind { move, cut, throwBomb, slide}
    public actionKind actionMode;
    public List<actionKind> actionList;
    public float moveDuring;
    
    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        character = transform.GetComponent<Character>();
        ani = transform.Find("Ani").GetComponent<GoblinAnimation>();
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
            case actionKind.throwBomb:
                if (distanceToPlayer < cutDistance) 
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.isThrow();
                break;
            case actionKind.slide:
                if (distanceToPlayer < cutDistance) 
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.isSlide();
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
                actionList.Add(actionKind.throwBomb);
                break;
            case 3:
                actionList.Add(actionKind.slide);
                actionList.Add(actionKind.cut);
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
        if (actionMode != actionKind.slide && actionMode != actionKind.move)  
            return;
        if (actionMode == actionKind.move) 
        {
            rb.velocity = new Vector2(transform.localScale.x * character.speed * Time.deltaTime, rb.velocity.y); // 僅在 X 軸上移動
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

    public void slide()
    {
        rb.AddForce(new Vector2(transform.localScale.x * FastSpeed * 10, 0), ForceMode2D.Impulse);
    }

    public void ThrowBomb()
    {
        GameObject bombObject = Instantiate(bomb, transform.position, transform.rotation);
        bombObject.GetComponent<AttackSource>().attackSource = this.transform;
        if (distanceToPlayer < bombDistance)
        {
            bombObject.GetComponent<Bomb>().startSpeedMin.x /= 2;
            bombObject.GetComponent<Bomb>().startSpeedMax /= 2;
        }
        bombObject.GetComponent<Bomb>().isThrow(transform.localScale.x);
    }
}
