using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
     
    public float speed; // 移動速度
    private Rigidbody2D rb;   // Rigidbody2D 組件
    private int currentDirection; // 當前移動方向

    void Start()
    {
        // 獲取 Rigidbody2D 組件
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 1.0f);
    }

    void Update()
    {
        // 持續移動
        Go();
    }

    // 設定移動方向
    public void Move(int direction)
    {
        currentDirection = direction; // 更新當前方向
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
    private void Go()
    {
        // 根據方向設定速度
        rb.velocity = new Vector2(currentDirection * speed * Time.deltaTime, rb.velocity.y);
    }
}

 
