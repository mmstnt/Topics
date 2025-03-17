using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAnimation : MonoBehaviour
{
    private Animator ani;
    private Rigidbody2D rb;
    private Goblin goblin;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
        goblin = transform.parent.GetComponent<Goblin>();
    }

    private void Update()
    {
        SetAnimation();
    }

    public void SetAnimation()
    {
        ani.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
        ani.SetBool("isDead", goblin.isDead);
    }

    public void isThrow()
    {
        ani.SetTrigger("isThrow");
    }

    public void isCut()
    {
        ani.SetTrigger("isCut");
    }

    public void isSlide()
    {
        ani.SetTrigger("isSlide");
    }

    public void hurt()
    {
        ani.SetTrigger("hurt");
    }
}


