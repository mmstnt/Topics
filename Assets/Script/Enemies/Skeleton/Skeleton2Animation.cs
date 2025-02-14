using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton2Animation : MonoBehaviour
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

    public void Skeleton2attack()
    {
        ani.SetTrigger("attack");
    }
}
