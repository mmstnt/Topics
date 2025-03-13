using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire2 : MonoBehaviour
{
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private fire2animation fire2Animation;
    public GameObject ani;
    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        fire2Animation = transform.Find("Ani").GetComponent<fire2animation>();
        physicsCheck = GetComponent<PhysicsCheck>();
        ani.GetComponent<SpriteRenderer>().enabled = false;

    }
    void Update()
    {
        if (physicsCheck.isGround)  
        {
             

            fire2Animation.fire2explode();
            ani.GetComponent<SpriteRenderer>().enabled = true;

        }
    }

 
    
    
}
