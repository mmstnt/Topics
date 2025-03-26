using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiremancerAnimation : MonoBehaviour
{
    private Animator ani;
    private Rigidbody2D rb;
    private Character character;
    private Firemancer fire;


    private void Awake()
    {
        ani = GetComponent<Animator>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
        character = transform.parent.GetComponent<Character>();
        fire = transform.parent.GetComponent<Firemancer>();
    }
    private void Update()
    {
        SetAnimation();
    }

    public void SetAnimation()
    {
         
        ani.SetBool("isDead", character.isDead);
    }

    public void Fire()
    {
        ani.SetTrigger("fire");
    }

    public void Spout()
    {
        ani.SetTrigger("spout");
    }

    public void Fire2()
    {
        ani.SetTrigger("fire2");
    }
    public void hurt()
    {
        ani.SetTrigger("hurt");
    }

}
