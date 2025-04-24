using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireKnightAni : MonoBehaviour
{
    private Animator ani;
    private Rigidbody2D rb;
    private Character character;
    private FireKnight fireKnight;
    private PhysicsCheck physicsCheck;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
        character = transform.parent.GetComponent<Character>();
        fireKnight = transform.parent.GetComponent<FireKnight>();
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

    public void isFire()
    {
        ani.SetTrigger("fire");
    }

    public void isBomb()
    {
        ani.SetTrigger("bomb");
    }

    public void isRotate()
    {
        ani.SetTrigger("rotate");
    }

    public void isRotateEnd()
    {
        ani.SetTrigger("rotateEnd");
    }

    public void slide()
    {
        ani.SetTrigger("slide");
    }


}
