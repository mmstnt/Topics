using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldGolemAnimation : MonoBehaviour
{ 
private OldGolem oldgolem;
private Animator ani;
private Rigidbody2D rb;
private PhysicsCheck physicsCheck;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
        oldgolem = transform.parent.GetComponent<OldGolem>();
        physicsCheck = transform.parent.gameObject.GetComponent<PhysicsCheck>();
    }

    private void Update()
    {
        SetAnimation();
    }

    public void SetAnimation()
    {
        ani.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
        ani.SetBool("isDead", oldgolem.isDead);
        ani.SetBool("isGround", physicsCheck.isGround);
    }

    public void hit1()
    {
        ani.SetTrigger("hit1");
    }

    public void hit2()
    {
        ani.SetTrigger("hit2");
    }

    public void spit()
    {
        ani.SetTrigger("spit");
    }

    public void hurt()
    {
        ani.SetTrigger("hurt");
    }

    public void slide()
    {
        ani.SetTrigger("isSlide");
    }

    public void spitOldGuardianBomb()
    {
        oldgolem.spitBomb();
    }
}

