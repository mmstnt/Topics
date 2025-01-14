using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;


public class MediaPipeKeyboard : MonoBehaviour
{
    public GameObject player;
    [Header("°Ñ¼Æ")]
    public float horizontalDistance;
    public float verticalDistance;
    public float attackDistance;
    public float attackTime;
    public float attackDruing;

    private Vector2 distance;
    private Keyboard keyboard;
    private bool isJump = false;
    private PhysicsCheck playerPhysicsCheck;

    private void Start()
    {
        keyboard = InputSystem.AddDevice<Keyboard>();
        playerPhysicsCheck = player.GetComponent<PhysicsCheck>();
    }

    private void OnDestroy()
    {
        if (keyboard != null)
        {
            InputSystem.RemoveDevice(keyboard);
        }
    }

    private void Update()
    {
        distance = transform.position - player.transform.position;
        if (attackDruing >= 0) 
        {
            attackDruing -= Time.deltaTime;
        }
        move();
        jump();

        InputSystem.Update();
    }

    private void move()
    {
        if (Mathf.Abs(distance.x) < horizontalDistance) 
        {
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            return;
        }

        if (distance.x < 0)
        {
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.A));
        }
        else
        {
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.D));
        }
    }

    private void jump() 
    {
        if (!playerPhysicsCheck.isGround) 
        {
            isJump = false;
        }
        if (distance.y < verticalDistance)
        {
            isJump = false;
            return;
        }
        if (!isJump && playerPhysicsCheck.isGround)
        {
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.Space));
            isJump = true;
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemies") && other.GetComponent<Character>()?.currentHp > 0)  
        {
            if(Mathf.Abs(distance.x) < attackDistance && attackDruing < 0) 
            {
                player.transform.localScale = new Vector3((other.transform.position.x - player.transform.position.x) > 0 ? 1 : -1, 1, 1);
                InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.J));
                attackDruing = attackTime;
            }
        }
    }
}
