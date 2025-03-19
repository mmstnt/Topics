using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonLighterAni : MonoBehaviour
{
    private Animator ani;
    private Rigidbody2D rb;
    private SkeletonLighter skeletonLighter;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
        skeletonLighter = transform.parent.GetComponent<SkeletonLighter>();
    }

    private void Update()
    {
        SetAnimation();
    }

    public void SetAnimation()
    {
        ani.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
        ani.SetFloat("velocityY", rb.velocity.y);
        ani.SetBool("isDead", skeletonLighter.isDead);
    }

    public void isHurt()
    {
        ani.SetTrigger("hurt");
    }

    public void isAttack01()
    {
        ani.SetTrigger("isAtk01");
    }

    public void isAttack02()
    {
        ani.SetTrigger("isAtk02");
    }

    public void blueFire()
    {
        skeletonLighter.releaseBlueFire();
    }
}
