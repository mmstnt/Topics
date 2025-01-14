using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAnimation : MonoBehaviour
{
    public Animator ani;
    public Rigidbody2D rb;
     
    private void Awake()
    {
        ani = GetComponent<Animator>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
    }

    public void bombexplode()
    {
        ani.SetTrigger("explode");
    }
}
