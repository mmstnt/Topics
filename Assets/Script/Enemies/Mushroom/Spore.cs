using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spore : MonoBehaviour
{
    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("角色參數")]
    public Vector2 startSpeed;  
    public float jumptime;
    private PhysicsCheck physicsCheck;
    public SporeAnimation sporeAnimation;
    private bool wasGrounded = false;

    void Awake()
    {
        sporeAnimation = transform.Find("Ani").GetComponent<SporeAnimation>();
        physicsCheck = GetComponent<PhysicsCheck>();
    }

    private void OnEnable()
    {
        afterSceneLoadEvent.onEventRaised += onAfterSceneLoadEvent;
    }

    private void OnDisable()
    {
        afterSceneLoadEvent.onEventRaised -= onAfterSceneLoadEvent;
    }

    private void onAfterSceneLoadEvent()
    {
        Destroy(this.gameObject);
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
