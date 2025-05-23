using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartoonVikingAni : MonoBehaviour
{
    private Animator ani;
    private Rigidbody2D rb;
    private Character character;
    private CartoonViking cartoonViking;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
        character = transform.parent.GetComponent<Character>();
        cartoonViking = transform.parent.GetComponent<CartoonViking>();
    }

    private void Update()
    {
        SetAnimation();
    }

    public void SetAnimation()
    {
        ani.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
        ani.SetFloat("velocityY", rb.velocity.y);
        ani.SetBool("isDead", character.isDead);
        ani.SetBool("isGun", cartoonViking.isGun);
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

    public void axeToGun()
    {
        ani.SetTrigger("AxeToGun");
    }

    public void gunToAxe()
    {
        ani.SetTrigger("GunToAxe");
    }

    public void slide()
    {
        ani.SetTrigger("isSlide");
    }

    public void slideEnd()
    {
        ani.SetTrigger("SlideEnd");
    }

    public void slideAttack()
    {
        ani.SetTrigger("SlideAttack");
    }

    public void shoot() 
    {
        cartoonViking.shoot();
    }

    public void wave()
    {
        ani.SetTrigger("Wave");
    }
}
