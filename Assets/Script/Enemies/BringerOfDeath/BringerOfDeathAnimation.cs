using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeathAnimation : MonoBehaviour
{
    private Animator ani;
    private Rigidbody2D rb;
    private Character character;
    private BringerOfDeath bringerOfDeath;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
        character = transform.parent.GetComponent<Character>();
        bringerOfDeath = transform.parent.GetComponent<BringerOfDeath>();
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

    public void isHurt()
    {
        ani.SetTrigger("hurt");
    }

    public void isAttack01()
    {
        ani.SetTrigger("isAtk01");
    }

    public void isAttack01Move() 
    {
        if (bringerOfDeath.distanceToPlayer < bringerOfDeath.attack01MoveDistance) return;
        bringerOfDeath.rb.AddForce(new Vector2(-transform.parent.transform.localScale.x * bringerOfDeath.attack01MoveSpeed, 0), ForceMode2D.Impulse);
    }

    public void isAttack02()
    {
        ani.SetTrigger("isAtk02");
    }

    public void isCast01()
    {
        ani.SetTrigger("isCast01");
    }

    public void isCast02()
    {
        ani.SetTrigger("isCast02");
    }
    
    public void isTeleport()
    {
        ani.SetTrigger("isTeleport");
    }

    public void isTeleportEnd()
    {
        ani.SetTrigger("isTeleportEnd");
    }

    public void isTeleportTrue() 
    {
        bringerOfDeath.isTeleport = true;
    }
}
