using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire2animation : MonoBehaviour
{
    public Animator ani;
    public Rigidbody2D rb;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
    }

    public void fire2explode()
    {
        ani.SetTrigger("explode");
    }
}
