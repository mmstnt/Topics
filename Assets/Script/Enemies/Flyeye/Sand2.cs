using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sand2 : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isExploded = false; // ����h��Ĳ�o�P��
    private Sand2Animation sand2Animation;
    private float timer = 0f;


    public float Acceleration = 100000000f; // �[�t��
    private float currentSpeed = 100f; // ��e�t��
    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        sand2Animation = transform.Find("Ani").GetComponent<Sand2Animation>();


    }



    // ��o�͸I����Ĳ�o
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isExploded) // �T�O��Ĳ�o�@��
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
