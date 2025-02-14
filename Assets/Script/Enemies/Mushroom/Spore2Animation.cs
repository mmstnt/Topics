using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spore2Animation : MonoBehaviour
{
    public Animator ani;
    public Rigidbody2D rb;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
    }

    public void spore2explode()
    {
        ani.SetTrigger("explode");
    }
}