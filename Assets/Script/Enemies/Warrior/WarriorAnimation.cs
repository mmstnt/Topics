using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorAnimation : MonoBehaviour
{
    private Character character;
    private Animator ani;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
        character = transform.GetComponent<Character>();
        physicsCheck = transform.parent.gameObject.GetComponent<PhysicsCheck>();
    }

    private void Update()
    {
        SetAnimation();
    }

    public void SetAnimation()
    {
        ani.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
        ani.SetFloat("velocityY", rb.velocity.y);
        ani.SetBool("isDead", character.isDead);
        ani.SetBool("isGround", physicsCheck.isGround);
    }

    public void cut1()
    {
        ani.SetTrigger("cut1");
    }

    public void cut2()
    {
        ani.SetTrigger("cut2");
    }

    public void cut3()
    {
        ani.SetTrigger("cut3");
    }

    public void hurt()
    {
        ani.SetTrigger("hurt");
    }

    public void slide()
    {
        ani.SetTrigger("isSlide");
    }
}

