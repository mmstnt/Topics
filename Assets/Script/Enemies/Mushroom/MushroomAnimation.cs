using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomAnimation : MonoBehaviour
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
        SetAnimation();
    }

    public void SetAnimation()
    {

        ani.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));

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
