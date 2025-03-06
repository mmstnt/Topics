using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimation : MonoBehaviour
{
    public Animator ani;
    public Rigidbody2D rb;
    private Skeleton skeleton;


    private void Awake()
    {
        ani = GetComponent<Animator>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
        skeleton = transform.parent.GetComponent<Skeleton>();
    }

    private void Update()
    {
        SetAnimation();
    }

    public void SetAnimation()
    {

        ani.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
        ani.SetBool("isDead", skeleton.isDead);
    }

    public void Skeletoncall()
    {
        ani.SetTrigger("call");
    }

    public void Skeletoncut()
    {
        ani.SetTrigger("cut");
    }

    public void Skeletonwave()
    {
        ani.SetTrigger("wave");
    }
}
