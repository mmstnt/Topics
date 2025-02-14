using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiremancerAnimation : MonoBehaviour
{
    public Animator ani;
    public Rigidbody2D rb;


    private void Awake()
    {
        ani = GetComponent<Animator>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
    }

     

    public void Fire()
    {
        ani.SetTrigger("fire");
    }

    public void Spout()
    {
        ani.SetTrigger("spout");
    }

    public void Fire2()
    {
        ani.SetTrigger("fire2");
    }

}
