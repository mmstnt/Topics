using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
  
    private Rigidbody2D rb;   // Rigidbody2D �ե�
     

    void Start()
    {
        // ��� Rigidbody2D �ե�
        rb = GetComponent<Rigidbody2D>();
    }

    

    // �]�w���ʤ�V
    public void Move(int direction)
    {
        
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
     
}
