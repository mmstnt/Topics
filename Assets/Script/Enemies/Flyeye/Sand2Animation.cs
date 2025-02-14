using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sand2Animation : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator ani;
    public Rigidbody2D rb;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
    }

    public void sand2explode()
    {
        ani.SetTrigger("explode");
    }
}
