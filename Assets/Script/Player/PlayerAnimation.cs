using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator ani;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private Character character;
    private PlayerController playerController;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        rb = transform.parent.gameObject.GetComponent<Rigidbody2D>();
        character = transform.parent.gameObject.GetComponent<Character>();
        physicsCheck = transform.parent.gameObject.GetComponent<PhysicsCheck>();
        playerController = transform.parent.gameObject.GetComponent<PlayerController>();
    }

    private void Update()
    {
        setAnimation();
    }

    public void setAnimation() 
    {
        ani.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
        ani.SetFloat("velocityY", rb.velocity.y);
        ani.SetBool("isGround", physicsCheck.isGround);
        ani.SetBool("isDead", character.isDead);
        ani.SetBool("isAttack", playerController.isAttack);
        ani.SetBool("isSlide", playerController.isSlide);
    }

    public void playerHurt() 
    {
        ani.SetTrigger("hurt");
    }

    public void playerAttack() 
    {
        ani.SetTrigger("attack");
    }

    public void playerSlide()
    {
        ani.SetTrigger("slide");
    }
}
