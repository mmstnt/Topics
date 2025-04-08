using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimation : MonoBehaviour
{
    public Animator ani;
    public Rigidbody2D rb;
    private Character character;


    private void Awake()
    {
        ani = GetComponent<Animator>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
        character = transform.parent.GetComponent<Character>();
    }

    private void Update()
    {
        SetAnimation();
    }

    public void SetAnimation()
    {

        ani.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
        ani.SetBool("isDead", character.isDead);
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
    public void hurt()
    {
        ani.SetTrigger("hurt");
    }
}
