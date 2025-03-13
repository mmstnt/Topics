using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAnimation : MonoBehaviour
{ 
    public Animator ani;
    public Rigidbody2D rb;
    private Golem golem;


    private void Awake()
    {
        ani = GetComponent<Animator>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
        golem = transform.parent.GetComponent<Golem>();
    }

    private void Update()
    {
        SetAnimation();



    }
    public void SetAnimation()
    {

        
        ani.SetBool("isDead", golem.isDead);

    }

    public void Golemshoot()
    {
        ani.SetTrigger("shoot");
    }

    public void Golemlaser()
    {
        ani.SetTrigger("laser");
    }

    public void Golemtransmit()
    {
        ani.SetTrigger("transmit");
    }
    public void hurt()
    {
        ani.SetTrigger("hurt");
    }
}
