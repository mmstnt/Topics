using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
  
    private Rigidbody2D rb;   // Rigidbody2D 組件
     

    void Start()
    {
        // 獲取 Rigidbody2D 組件
        rb = GetComponent<Rigidbody2D>();
    }

    

    // 設定移動方向
    public void Move(int direction)
    {
        
        if (direction == -1)
        {
            transform.localScale = new Vector3(-1, 1, 1); // 面向左
        }
        else if (direction == 1)
        {
            transform.localScale = new Vector3(1, 1, 1); // 面向右
        }

    }

    // 實際移動邏輯
     
}
