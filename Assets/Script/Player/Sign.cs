using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sign : MonoBehaviour
{
    private PlayerInputControl playerInput;
    private Animator ani;
    public Transform playerTransform;
    public GameObject signAni;
    public bool canPress;
    public GameObject pressGameObject;
    public IInteractable targetItem;

    private void Awake()
    {
        ani = signAni.GetComponent<Animator>();
        playerInput = new PlayerInputControl();
        playerInput.Enable();
    }

    private void OnEnable()
    {
        InputSystem.onActionChange += onActionChange;
        playerInput.GamePlay.Confirm.started += onConfirm;
    }

    private void OnDisable()
    {
        canPress = false;
    }

    private void onConfirm(InputAction.CallbackContext obj)
    {
        if (canPress) 
        {
            targetItem.triggerAction();
        }
    }

    private void onActionChange(object obj, InputActionChange change)
    {
        if(change == InputActionChange.ActionStarted) 
        {
            var kind = ((InputAction)obj).activeControl.device;
            switch (kind.device)
            {
                case Keyboard:
                    ani.Play("SignKeyBoard");
                    break;
            }
        }
    }

    private void Update()
    {
        signAni.GetComponent<SpriteRenderer>().enabled = canPress;
        signAni.transform.localScale = playerTransform.localScale;
    }

    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.CompareTag("Interactable"))
        {
            canPress = true;
            targetItem = other.GetComponent<IInteractable>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canPress = false;
    }
}
