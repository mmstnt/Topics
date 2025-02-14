using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sand : MonoBehaviour
{
     
    private Rigidbody2D rb;
    private bool isExploded = false; // ����h��Ĳ�o�P��
    private SandAnimation sandAnimation;
    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        sandAnimation = transform.Find("Ani").GetComponent<SandAnimation>();
         

    }

     

    // ��o�͸I����Ĳ�o
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isExploded) // �T�O��Ĳ�o�@��
        {
            isExploded = true;
             
            sandAnimation.sandexplode();

        }
    }

}
    

