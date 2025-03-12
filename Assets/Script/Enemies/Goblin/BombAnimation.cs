using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAnimation : MonoBehaviour
{
    public Animator ani;
     
    private void Awake()
    {
        ani = GetComponent<Animator>();
    }

    public void bombexplode()
    {
        ani.SetTrigger("explode");
    }
}
