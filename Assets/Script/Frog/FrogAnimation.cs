using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogAnimation : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    public PhysicsCheck physicsCheck;

    private void Awake()
    {
        anim = transform.GetComponent<Animator>();
        rb = transform.parent.gameObject.transform.GetComponent<Rigidbody2D>();
        physicsCheck = transform.parent.gameObject.transform.GetComponent<PhysicsCheck>();
    }
    private void Update()
    {
        SetAnimation();
    }
    public void SetAnimation()
    {
        anim.SetFloat("velocityY", rb.velocity.y);
        anim.SetBool("isGround", physicsCheck.isGround);
    }
}
