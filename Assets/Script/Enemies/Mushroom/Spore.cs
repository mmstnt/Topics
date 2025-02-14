using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spore : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 startSpeed; // 炸彈初始速度
    public float jumptime;
    
    private bool isExploded = false; // 防止多次觸發銷毀
    public SporeAnimation sporeAnimation;
    // Start is called before the first frame update
    void Start()
    {
        sporeAnimation = transform.Find("Ani").GetComponent<SporeAnimation>();
    }
    public void attack3(int direction)
    {
        transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * Random.Range(-10, startSpeed.x), Random.Range(10,startSpeed.y)), ForceMode2D.Impulse);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isExploded) // 確保僅觸發一次
        {
            isExploded = true;
            jumptime = jumptime + 1;


            
            
        }
        isExploded = false;
        if (jumptime == 2)
        {
            sporeAnimation.sporeexplode();
        }
    }


}
