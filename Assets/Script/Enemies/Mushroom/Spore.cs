using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spore : MonoBehaviour
{
    [Header("�ƥ��ť")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("����Ѽ�")]
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
        if (physicsCheck.isGround && !wasGrounded)  // �u����踨�a�ɤ~����
        {
            jumptime++;
            if (jumptime == 3)
            {
                sporeAnimation.sporeexplode();
            }
        }
        wasGrounded = physicsCheck.isGround;  // ��s���A
    }
}
