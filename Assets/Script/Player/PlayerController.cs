using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("�ƥ��ť")]
    public SceneLoadEventSO loadEvent;
    public VoidEventSO afterSceneLoadedEvent;

    private PlayerInputControl inputControl;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerAnimation playerAnimation;
    private Vector2 inputDirection;
    [Header("�Ѽ�")]
    public float speed;
    public float jumpForce;
    public float hurtForce;
    [Header("���A")]
    public bool isHurt;
    public bool isDead;
    public bool isAttack;
    //�p�H�Ѽ�
    private int faceDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerAnimation = transform.Find("Ani").GetComponent<PlayerAnimation>();
        //����\��]�m
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
        //����
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        //½��
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
