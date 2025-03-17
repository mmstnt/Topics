using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldGuardianAni : MonoBehaviour
{
    private Animator ani;
    private Rigidbody2D rb;
    private OldGuardian oldGuardian;
    private PhysicsCheck physicsCheck;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
        oldGuardian = transform.parent.GetComponent<OldGuardian>();
        physicsCheck = transform.parent.gameObject.GetComponent<PhysicsCheck>();
    }

    private void Update()
    {
        SetAnimation();
    }

    public void SetAnimation()
    {
        ani.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
        ani.SetFloat("velocityY", rb.velocity.y);
        ani.SetBool("isDead", oldGuardian.isDead);
        ani.SetBool("isGround", physicsCheck.isGround);
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

    public void isSpit()
    {
        ani.SetTrigger("isSpit");
    }

    public void spitOldGuardianBomb() 
    {
        oldGuardian.spitBomb();
    }
}
