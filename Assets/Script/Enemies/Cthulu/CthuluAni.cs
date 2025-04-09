using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CthuluAni : MonoBehaviour
{
    private Animator ani;
    private Rigidbody2D rb;
    private Character character;
    private Cthulu cthulu;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
        character = transform.parent.GetComponent<Character>();
        cthulu = transform.parent.GetComponent<Cthulu>();
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

    public void isAttack02()
    {
        ani.SetTrigger("isAtk02");
    }

    public void isSkill01()
    {
        ani.SetTrigger("isSkill01");
    }

    public void isSkill01Cast()
    {
        cthulu.skill01();
    }

    public void isSkill02()
    {
        cthulu.skill02();
    }
}
