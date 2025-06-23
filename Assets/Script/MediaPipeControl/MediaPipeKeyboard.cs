using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;


public class MediaPipeKeyboard : MonoBehaviour
{
    public GameObject player;
    public Sign signGameObject;
    public GameObject ani;
    [Header("°Ñ¼Æ")]
    public float horizontalDistance;
    public float verticalDistance;
    public float attackDistance;
    public float attackTime;
    public float attackDruing;
    public float slideVerticalDistance;
    public float slideTime;
    public float slideDruing;

    private float color;
    private Vector2 distance;
    private Keyboard keyboard;
    private bool isJump = false;
    private bool isSlide = false;
    private PhysicsCheck playerPhysicsCheck;
    
    private void Awake()
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
        if (attackDruing >= 0) attackDruing -= Time.deltaTime;
        if(slideDruing >= 0) slideDruing -= Time.deltaTime;
        else if(isSlide) isSlide = false;
        move();
        jump();
        slide();
        ani.transform.localScale = new Vector2(color, color);
        InputSystem.Update();
    }

    private void move()
    {
        if (isSlide) return;
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

    private void slide() 
    {
        if (Mathf.Abs(distance.x) > horizontalDistance && !isSlide && distance.y < slideVerticalDistance)
        {
            isSlide = true;
            slideDruing = slideTime;
            player.transform.localScale = new Vector3(distance.x > 0 ? 1 : -1, 1, 1);
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.L));
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
        else if (other.CompareTag("Interactable") && signGameObject.canPress) 
        {
            if(other.GetComponent<IInteractable>() == signGameObject.targetItem) 
            {
                color += Time.deltaTime * 1.5f;
                if (color >= 1) 
                {
                    InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.E));
                    color = 0;
                }
            }
        }
        else if (other.CompareTag("Card"))
        {
            color += Time.deltaTime;
            if (color >= 1)
            {
                other.GetComponent<Card>().cardIsChoose();
                color = 0;
            }
        }
        else if (other.CompareTag("Buttle"))
        {
            color += Time.deltaTime * 1.5f;
            if (color >= 1)
            {
                other.GetComponent<Button>().onClick.Invoke();
                color = 0;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        color = 0;
    }
}
