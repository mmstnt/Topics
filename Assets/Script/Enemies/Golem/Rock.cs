using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
     
    public float speed; // ���ʳt��
    private Rigidbody2D rb;   // Rigidbody2D �ե�
    private int currentDirection; // ��e���ʤ�V

    void Start()
    {
        // ��� Rigidbody2D �ե�
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 1.0f);
    }

    void Update()
    {
        // ���򲾰�
        Go();
    }

    // �]�w���ʤ�V
    public void Move(int direction)
    {
        currentDirection = direction; // ��s��e��V
        if (direction == -1)
        {
            transform.localScale = new Vector3(-1, 1, 1); // ���V��
        }
        else if (direction == 1)
        {
            transform.localScale = new Vector3(1, 1, 1); // ���V�k
        }

    }

    // ��ڲ����޿�
    private void Go()
    {
        // �ھڤ�V�]�w�t��
        rb.velocity = new Vector2(currentDirection * speed * Time.deltaTime, rb.velocity.y);
    }
}

 
