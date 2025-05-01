using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeKeeperAni : MonoBehaviour
{
    private Animator ani;
    private Rigidbody2D rb;
    private Character character;
    private BladeKeeper bladeKeeper;
    private PhysicsCheck physicsCheck;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
        character = transform.parent.GetComponent<Character>();
        bladeKeeper = transform.parent.GetComponent<BladeKeeper>();
        physicsCheck = transform.parent.gameObject.GetComponent<PhysicsCheck>();
    }

    private void Update()
    {
        SetAnimation();
    }

    public void SetAnimation()
    {
        ani.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
        ani.SetBool("isDead", character.isDead);
        ani.SetBool("isGround", physicsCheck.isGround);

    }

    public void isHurt()
    {
        ani.SetTrigger("hurt");
    }

    public void isShoot()
    {
        ani.SetTrigger("shoot");
    }

    public void isCut()
    {
        ani.SetTrigger("cut");
    }

    public void isSpike()
    {
        ani.SetTrigger("spike");
    }

    public void throwKnife()
    {
        bladeKeeper.throwKnife();
    }

    public void isAttack01Move()
    {
        if (bladeKeeper.distanceToPlayer < bladeKeeper.attack01MoveDistance) return;
        bladeKeeper.rb.AddForce(new Vector2(transform.parent.transform.localScale.x * bladeKeeper.attack01MoveSpeed, 0), ForceMode2D.Impulse);
    }

}
