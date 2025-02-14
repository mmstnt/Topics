using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sand2 : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isExploded = false; // 防止多次觸發銷毀
    private Sand2Animation sand2Animation;
    private float timer = 0f;


    public float Acceleration = 100000000f; // 加速度
    private float currentSpeed = 100f; // 當前速度
    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        sand2Animation = transform.Find("Ani").GetComponent<Sand2Animation>();


    }



    // 當發生碰撞時觸發
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isExploded) // 確保僅觸發一次
        {
            isExploded = true;

            sand2Animation.sand2explode();

        }
    }
    private void Update()
    {
        
        timer += Time.deltaTime;
        if (timer >= 2f)
        {
            currentSpeed = Acceleration;
             
        }
        rb.velocity = new Vector2(currentSpeed * Time.deltaTime, rb.velocity.y);
    }

     
    
}
