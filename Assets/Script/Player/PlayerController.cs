using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputControl inputControl;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private Vector2 inputDirection;
    [Header("�Ѽ�")]
    public float speed;
    public float jumpForce;
    public float hurtForce;
    public bool isHurt;
    public bool isDead;
    //�p�H�Ѽ�
    private int faceDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        //����\��]�m
        inputControl = new PlayerInputControl();
        inputControl.GamePlay.Jump.started += jump;
    }

    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }

    private void Update()
    {
        inputDirection = inputControl.GamePlay.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        move();
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
