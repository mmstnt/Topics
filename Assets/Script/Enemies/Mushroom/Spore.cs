using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spore : MonoBehaviour
{
     
    public Vector2 startSpeed;  
    public float jumptime;
    private PhysicsCheck physicsCheck;
    public SporeAnimation sporeAnimation;
    private bool wasGrounded = false;

    void Start()
    {
        sporeAnimation = transform.Find("Ani").GetComponent<SporeAnimation>();
        physicsCheck = GetComponent<PhysicsCheck>();
    }
    public void attack3(int direction)
    {
        transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * Random.Range(-10, startSpeed.x), Random.Range(10,startSpeed.y)), ForceMode2D.Impulse);
    }

    private void Update()
    {
        jump();
    }
    private void jump()
    {
        if (physicsCheck.isGround && !wasGrounded)  // 只有當剛落地時才執行
        {
            jumptime++;
            if (jumptime == 3)
            {
                sporeAnimation.sporeexplode();
            }
        }
        wasGrounded = physicsCheck.isGround;  // 更新狀態
    }


}
