using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("事件監聽")]
    public SceneLoadEventSO sceneLoadEvent;
    public VoidEventSO afterSceneLoadedEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO cardChooseEvent;
    public VoidEventSO cardChooseEndEvent;
    public VoidEventSO cameraLensEvent;

    [Header("參數")]
    public float speed;
    public float jumpForce;
    public float doubleJumpForce;
    public float slideForce;
    public float hurtForce;

    [Header("狀態")]
    public bool isHurt;
    public bool isAttack;
    public bool isSlide;
    private PlayerInputControl inputControl;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerAnimation playerAnimation;
    private Character character;
    private Vector2 inputDirection;
    private int faceDir;
    private bool canDoubleJump;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerAnimation = transform.Find("Ani").GetComponent<PlayerAnimation>();
        //按鍵功能設置
        inputControl = new PlayerInputControl();
        inputControl.GamePlay.Jump.started += jump;
        inputControl.GamePlay.Attack.started += attack;
        inputControl.GamePlay.Slide.started += slide;
        inputControl.GamePlay.MoveInterrupt.started += moveInterrupt;
        inputControl.Enable();
    }

    private void OnEnable()
    {
        sceneLoadEvent.LoadRequestEvent += onLoadEvent;
        afterSceneLoadedEvent.onEventRaised += onAfterSceneLoadedEvent;
        loadDataEvent.onEventRaised += onLoadDataEvent;
        cardChooseEvent.onEventRaised += onCardChooseEvent;
        cardChooseEndEvent.onEventRaised += onCardChooseEndEvent;
        cameraLensEvent.onEventRaised += onCameraLensEvent;
    }

    private void OnDisable()
    {
        inputControl.Disable();
        sceneLoadEvent.LoadRequestEvent -= onLoadEvent;
        afterSceneLoadedEvent.onEventRaised -= onAfterSceneLoadedEvent;
        loadDataEvent.onEventRaised -= onLoadDataEvent;
        cardChooseEvent.onEventRaised -= onCardChooseEvent;
        cardChooseEndEvent.onEventRaised -= onCardChooseEndEvent;
        cameraLensEvent.onEventRaised -= onCameraLensEvent;
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
        //inputControl.GamePlay.Enable();
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
        character.isDead = false;
    }

    private void onCameraLensEvent()
    {
        inputControl.GamePlay.Enable();
    }

    public void move() 
    {
        if (isHurt || isSlide || isAttack)
            return;
        //移動
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        //翻轉
        faceDir = inputDirection.x != 0 ? (inputDirection.x > 0 ? 1 : -1) : (int)transform.localScale.x;
        transform.localScale = new Vector3(faceDir, 1, 1);
    }

    private void jump(InputAction.CallbackContext obj)
    {
        isSlide = false;
        if (physicsCheck.isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            canDoubleJump = true;
        }
        else if (canDoubleJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(transform.up * doubleJumpForce, ForceMode2D.Impulse);
            canDoubleJump = false;
        }
    }

    private void attack(InputAction.CallbackContext obj)
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
        playerAnimation.playerAttack();

        isSlide = false;
        isAttack = true;
    }

    private void slide(InputAction.CallbackContext obj)
    {
        playerAnimation.playerSlide();
        rb.velocity = new Vector2(0, rb.velocity.y);
        Vector2 dirForce = new Vector2(transform.localScale.x, 0).normalized;
        rb.AddForce(dirForce * slideForce, ForceMode2D.Impulse);

        isAttack = false;
        isSlide = true;
    }

    private void moveInterrupt(InputAction.CallbackContext obj)
    {
        isAttack = false;
        isSlide = false;
    }

    public void getHurt(Transform attacker) 
    {
        isHurt = true;
        if(attacker.GetComponent<Attack>().attackImpact == AttackImpact.Normal) 
        {
            rb.velocity = Vector2.zero;
            Vector2 dirForce = new Vector2(transform.position.x - attacker.position.x, 0).normalized;
            rb.AddForce(dirForce * hurtForce, ForceMode2D.Impulse);
        }
    }

    public void playerDead() 
    {
        inputControl.GamePlay.Disable();
    }
}
