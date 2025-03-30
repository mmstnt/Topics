using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAnimation : MonoBehaviour
{
    private Animator ani;
    private Rigidbody2D rb;
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
        ani.SetBool("isDead", character.isDead);
    }

    public void Golemshoot()
    {
        ani.SetTrigger("shoot");
    }

    public void Golemlaser()
    {
        ani.SetTrigger("laser");
    }

    public void Golemtransmit()
    {
        ani.SetTrigger("transmit");
    }
    public void hurt()
    {
        ani.SetTrigger("hurt");
    }
}
