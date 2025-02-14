using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAnimation : MonoBehaviour
{ 
    public Animator ani;
    public Rigidbody2D rb;


    private void Awake()
    {
        ani = GetComponent<Animator>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        

     
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
}
