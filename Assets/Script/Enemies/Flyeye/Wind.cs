using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public Vector2 attackSite;
    public float distance;
    private Rigidbody2D rb;
    private bool isExploded = false; // ¨¾¤î¦h¦¸Ä²µo¾P·´
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        float angle = transform.eulerAngles.z;

        Vector2 direction = new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad), 
            Mathf.Sin(angle * Mathf.Deg2Rad)  
        );
        attackSite = (Vector2)transform.position + direction * distance;
        rb = transform.GetComponent<Rigidbody2D>();

        Destroy(gameObject, 1.0f);



    }

    private void Update()
    {
        Move();
    }

    void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, attackSite, speed * Time.deltaTime);         
    }

     


     
}

 
