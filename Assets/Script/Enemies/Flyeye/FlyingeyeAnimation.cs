using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingeyeAnimation : MonoBehaviour
{
     
    public Animator ani;
    public Rigidbody2D rb;
    private Flyingeye flyingeye;
   
    
    private void Awake()
    {
        ani = GetComponent<Animator>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
        flyingeye = transform.parent.GetComponent<Flyingeye>();
    }


    private void Update()
    {
        SetAnimation();
    }

    public void SetAnimation()
    {
         
        ani.SetBool("isDead",flyingeye.isDead);
    }

    public void attack1()
    {
        ani.SetTrigger("attack1");
    }

    public void attack2()
    {
        ani.SetTrigger("attack2");
    }

    public void attack3()
    {
        ani.SetTrigger("attack3");
    }

    public void hurt()
    {
        ani.SetTrigger("hurt");
    }
}
