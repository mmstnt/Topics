﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Flyingeye : MonoBehaviour
{
    private Rigidbody2D rb;
    private FlyingeyeAnimation ani;
    private GameObject player;
    private Character character;
    public LayerMask GroundLayer;
    public LayerMask WallLayer;

    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;
    public VoidEventSO cameraLensEvent;

    [Header("角色參數")]
    public GameObject sand;
    public GameObject wind;
    public GameObject laser;
    public float MoveSpeedY;
    public float moveTimeMin;
    public float moveTimeMax;
    public float flyDistance;
    public float laserDistance;
    public RaycastHit2D up;
    public RaycastHit2D back;

    [Header("角色狀態")]
    public float distanceToPlayer;
    public bool action;
    public enum actionKind { move, throwSand, throwWind, laser }
    public actionKind actionMode;
    public List<actionKind> actionList;
    public float moveDuring;
    public float upDuring;
    public float downDuring;
    
    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        character = transform.GetComponent<Character>();
        ani = transform.Find("Ani").GetComponent<FlyingeyeAnimation>();
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
        distanceToPlayer = Mathf.Abs(player.transform.position.y - transform.position.y);
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
            case actionKind.throwSand:
                action = true;
                getPlayerSite();
                rb.velocity = Vector2.zero;
                ani.attack1();
                break;
            case actionKind.throwWind:
                action = true;
                getPlayerSite();
                rb.velocity = Vector2.zero;
                ani.attack2();
                break;
            case actionKind.laser:
                if (distanceToPlayer > laserDistance) 
                    break;
                action = true;
                getPlayerSite();
                rb.velocity = Vector2.zero;
                ani.attack3();
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
                actionList.Add(actionKind.throwSand);
                break;
            case 2:
                actionList.Add(actionKind.throwWind);
                break;
            case 3:
                actionList.Add(actionKind.laser);
                break;
        }
    }

    private void getPlayerSite()
    {
        float dir = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
        transform.localScale = new Vector3(dir, 1, 1);
    }

    public void ThrowSand()
    {
        float randomValueX;
        float randomValueY;
        int times = 5;
        for (int i = 0; i < times; i++)
        {
            Vector3 newPosition = transform.position;
            randomValueX = Random.Range(-10f, 10f);
            randomValueY = Random.Range(5f, 10f);
            newPosition.x = player.transform.position.x + randomValueX;
            newPosition.y += randomValueY;
            GameObject sandObject = Instantiate(sand, newPosition, transform.rotation);
            sandObject.GetComponent<AttackSource>().attackSource = this.transform;
        }
    }

    public void ThrowSand2()
    {
        GameObject laserObject = Instantiate(laser, transform.position, transform.rotation);
        laserObject.GetComponent<Laser>().site = this.transform;
        laserObject.GetComponent<AttackSource>().attackSource = this.transform;
    }

    public void ThrowWind()
    {
        for(int i = 0; i < 8; i++) 
        {
            GameObject windObject = Instantiate(wind, transform.position, Quaternion.Euler(0, 0, 45*i));
            windObject.GetComponent<AttackSource>().attackSource = this.transform;
        }
    }

    public void CliffTurn()
    {
        // 計算射線的方向
        Vector3 rayDirection = transform.localScale.x * new Vector3(1f, 0, 0);
        // 射線的起點
        Vector3 rayStart = transform.position + new Vector3(0, -1f, 0);
        up = Physics2D.Raycast(rayStart, rayDirection, 3, GroundLayer);
        back = Physics2D.Raycast(rayStart, rayDirection, 3, WallLayer);
        // 繪製射線（用於調試，可視化射線）
        Debug.DrawRay(rayStart, rayDirection * 3, Color.red); // 紅色射線，長度為 2
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
            
            rb.velocity = new Vector2(transform.localScale.x * character.speed * Time.deltaTime, MoveSpeedY * Time.deltaTime); // 僅在 X 軸上移動
            if (moveDuring < 0)
            {
                action = false;
            }
        }
        else if (moveDuring < 0 )
        {
            rb.velocity = Vector2.zero;
            return;
        }
        moveDuring -= Time.deltaTime;
    }
}


