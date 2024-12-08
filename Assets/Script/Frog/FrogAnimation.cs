using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogAnimation : MonoBehaviour
{
    public Animator ani;
    public Rigidbody2D rb;
    public PhysicsCheck physicsCheck;

    private void Awake()
    {
        ani = transform.GetComponent<Animator>();
        rb = transform.parent.gameObject.transform.GetComponent<Rigidbody2D>();
        physicsCheck = transform.parent.gameObject.transform.GetComponent<PhysicsCheck>();
    }
    private void Update()
    {
        SetAnimation();
    }
    public void SetAnimation()
    {
        ani.SetFloat("velocityY", rb.velocity.y);
        ani.SetBool("isGround", physicsCheck.isGround);
    }
}
