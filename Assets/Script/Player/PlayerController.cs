using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("事件監聽")]
    public SceneLoadEventSO loadEvent;
    public VoidEventSO afterSceneLoadedEvent;

    private PlayerInputControl inputControl;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerAnimation playerAnimation;
    private Vector2 inputDirection;
    [Header("參數")]
    public float speed;
    public float jumpForce;
    public float hurtForce;
    [Header("狀態")]
    public bool isHurt;
    public bool isDead;
    public bool isAttack;
    //私人參數
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
    }

    private void OnEnable()
    {
        inputControl.Enable();
        loadEvent.LoadRequestEvent += onLoadEvent;
        afterSceneLoadedEvent.onEventRaised += onAfterSceneLoadedEvent;
    }

    private void OnDisable()
    {
        inputControl.Disable();
        loadEvent.LoadRequestEvent -= onLoadEvent;
        afterSceneLoadedEvent.onEventRaised -= onAfterSceneLoadedEvent;
    }


    private void Update()
    {
        inputDirection = inputControl.GamePlay.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        move();
    }

    private void onLoadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        inputControl.GamePlay.Disable();
    }

    private void onAfterSceneLoadedEvent()
    {
        inputControl.GamePlay.Enable();
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
