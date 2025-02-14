using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sand : MonoBehaviour
{
     
    private Rigidbody2D rb;
    private bool isExploded = false; // 防止多次觸發銷毀
    private SandAnimation sandAnimation;
    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        sandAnimation = transform.Find("Ani").GetComponent<SandAnimation>();
         

    }

     

    // 當發生碰撞時觸發
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isExploded) // 確保僅觸發一次
        {
            isExploded = true;
             
            sandAnimation.sandexplode();

        }
    }

}
    

