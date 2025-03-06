using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fire : MonoBehaviour
{
    private PhysicsCheck physicsCheck;
    private Rigidbody2D rb;
     
    public GameObject ani;
    void Start()
    {
        physicsCheck = GetComponent<PhysicsCheck>();
        rb = GetComponent<Rigidbody2D>();
        ani.GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject, 3.0f);
        
    }

    private void Update()
    {
        jump();
    }

    private void jump()
    {
        if (physicsCheck.isGround)
        {
            ani.GetComponent<SpriteRenderer>().enabled = true;
        }
        
         
    }
}
