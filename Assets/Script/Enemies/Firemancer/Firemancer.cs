using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firemancer : MonoBehaviour
{
    private Rigidbody2D rb;
    private FiremancerAnimation ani;
    private GameObject player;
    private Character character;
    public LayerMask GroundLayer;
    public LayerMask WallLayer;

    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;
    public VoidEventSO cameraLensEvent;

    [Header("角色參數")]
    public GameObject fire;
    public GameObject fire2;
    public float MoveSpeedY;
    public float moveTimeMin;
    public float moveTimeMax;
    public float spoutDistance;
    public float flyDistance;
    public RaycastHit2D up;
    public RaycastHit2D back;
    private float y;

    [Header("角色狀態")]
    public float distanceToPlayer;
    public float distanceToPlayerY;
    public bool action;
    public enum actionKind { move, fire,fire2,spout }
    public actionKind actionMode;
    public List<actionKind> actionList;
    private float moveDuring;
    private float upDuring;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        character = transform.GetComponent<Character>();
        ani = transform.Find("Ani").GetComponent<FiremancerAnimation>();
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
        distanceToPlayerY = Mathf.Abs(player.transform.position.y - transform.position.y);
        CliffTurn();
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
            case actionKind.fire:
                if (distanceToPlayer < spoutDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.Fire();
                break;
            case actionKind.fire2:
                if (distanceToPlayer < spoutDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.Fire2();
                break;
            case actionKind.spout:
                if (distanceToPlayer > spoutDistance || distanceToPlayerY > 1)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.Spout();
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
                actionList.Add(actionKind.fire);
                break;
            case 2:
                actionList.Add(actionKind.fire2);
                break;
            case 3:
                actionList.Add(actionKind.spout);
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
        if (back.collider != null)
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
        if (actionMode != actionKind.move)
            return;
        if (actionMode == actionKind.move)
        {
            if (up.collider != null)
            {
                MoveSpeedY = 300;
            }
            if (up.collider == null)
            {
                if (upDuring < 0)
                {
                    upDuring = Random.Range(0.5f, 1);
                    if (flyDistance > distanceToPlayer)
                    {
                        MoveSpeedY = Random.Range(-150, 150);
                    }
                    else if (flyDistance < distanceToPlayer)
                    {
                        MoveSpeedY = Random.Range(-100, -300);
                    }
                }
                upDuring -= Time.deltaTime;
            }
            rb.velocity = new Vector2(transform.localScale.x * character.speed * Time.deltaTime, MoveSpeedY * Time.deltaTime);  
            if (moveDuring < 0)
            {
                action = false;
            }
        }
        else if (moveDuring < 0)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        moveDuring -= Time.deltaTime;
    }
    
    public void dofire()
    {
        // 計算隨機位置
        Vector3 newPosition = transform.position;
        float x = Random.Range(-5f, 5f);
        newPosition.x = player.transform.position.x + x;
        newPosition.y = player.transform.position.y + 5;

        // 生成 Fire 物件
        GameObject fireGameObject = Instantiate(fire, newPosition, transform.rotation);
        fireGameObject.GetComponent<AttackSource>().attackSource = this.transform;
    }

    public void dofire2()
    {
        // 計算隨機位置
        Vector3 newPosition = transform.position;
        
        newPosition.x = player.transform.position.x + y * transform.localScale.x;
        newPosition.y = player.transform.position.y + 5;
        y = y + 10;
        GameObject fire2GameObject = Instantiate(fire2, newPosition, transform.rotation);
        fire2GameObject.GetComponent<AttackSource>().attackSource = this.transform;
        if (y > 20)
        {
            y = 0;
        }
    }

    public void CliffTurn()
    {
        // 計算射線的方向
        Vector3 rayDirection = transform.localScale.x * new Vector3(1f, 0, 0);
        // 射線的起點
        Vector3 rayStart = transform.position + new Vector3(0, -1.5f, 0);
        up = Physics2D.Raycast(rayStart, rayDirection, 3, GroundLayer);
        back = Physics2D.Raycast(rayStart, rayDirection, 3, WallLayer);
        // 繪製射線（用於調試，可視化射線）
        Debug.DrawRay(rayStart, rayDirection * 3, Color.red); // 紅色射線，長度為 2
    }
}
