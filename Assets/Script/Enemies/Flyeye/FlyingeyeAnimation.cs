using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingeyeAnimation : MonoBehaviour
{
     
    public Animator ani;
    public Rigidbody2D rb;
    
    private void Awake()
    {
        ani = GetComponent<Animator>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
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
}
