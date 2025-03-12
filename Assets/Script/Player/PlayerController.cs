using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("事件監聽")]
    public SceneLoadEventSO sceneLoadEvent;
    public VoidEventSO afterSceneLoadedEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO backToMenuEvent;
    public VoidEventSO cardChooseEvent;
    public VoidEventSO cardChooseEndEvent;

    [Header("參數")]
    public float speed;
    public float jumpForce;
    public float hurtForce;

    [Header("狀態")]
    public bool isHurt;
    public bool isDead;
    public bool isAttack;
    //私人參數
    private PlayerInputControl inputControl;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerAnimation playerAnimation;
    private Vector2 inputDirection;
    private int faceDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerAnimation = transform.Find("Ani").GetComponent<PlayerAnimation>();
        //按鍵功能設置
        inputControl = new PlayerInputControl();
        inputControl.GamePlay.Jump.started += jump;
        inputControl.GamePlay.Attack.started += attack;
        inputControl.Enable();
    }

    private void OnEnable()
    {
        sceneLoadEvent.LoadRequestEvent += onLoadEvent;
        afterSceneLoadedEvent.onEventRaised += onAfterSceneLoadedEvent;
        loadDataEvent.onEventRaised += onLoadDataEvent;
        backToMenuEvent.onEventRaised += onLoadDataEvent;
        cardChooseEvent.onEventRaised += onCardChooseEvent;
        cardChooseEndEvent.onEventRaised += onCardChooseEndEvent;
    }

    private void OnDisable()
    {
        inputControl.Disable();
        sceneLoadEvent.LoadRequestEvent -= onLoadEvent;
        afterSceneLoadedEvent.onEventRaised -= onAfterSceneLoadedEvent;
        loadDataEvent.onEventRaised -= onLoadDataEvent;
        backToMenuEvent.onEventRaised -= onLoadDataEvent;
        cardChooseEvent.onEventRaised -= onCardChooseEvent;
        cardChooseEndEvent.onEventRaised -= onCardChooseEndEvent;
    }

    private void Update()
    {
        inputDirection = inputControl.GamePlay.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        move();
    }

    //場景加載停止控制
    private void onLoadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2, bool n)
    {
        inputControl.GamePlay.Disable();
    }

    private void onAfterSceneLoadedEvent()
    {
        inputControl.GamePlay.Enable();
    }

    private void onCardChooseEvent()
    {
        inputControl.GamePlay.Disable();
    }

    private void onCardChooseEndEvent()
    {
        inputControl.GamePlay.Enable();
    }

    //讀取遊戲進度
    private void onLoadDataEvent()
    {
        isDead = false;
    }

    public void move() 
    {
        if (isHurt)
            return;
        //移動
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        //翻轉
        faceDir = inputDirection.x != 0 ? (inputDirection.x > 0 ? 1 : -1) : (int)transform.localScale.x;
        transform.localScale = new Vector3(faceDir, 1, 1);
    }

    private void jump(InputAction.CallbackContext obj)
    {
        if (physicsCheck.isGround)
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void attack(InputAction.CallbackContext obj)
    {
        playerAnimation.playerAttack();

        isAttack = true;
    }

    public void getHurt(Transform attacker) 
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dirForce = new Vector2(transform.position.x - attacker.position.x, 0).normalized;
        rb.AddForce(dirForce * hurtForce, ForceMode2D.Impulse);
    }

    public void playerDead() 
    {
        isDead = true;
        inputControl.GamePlay.Disable();
    }
}
