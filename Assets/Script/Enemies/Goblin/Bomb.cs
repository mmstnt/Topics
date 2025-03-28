using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("角色參數")]
    public Vector2 startSpeedMin; // 炸彈初始速度
    public Vector2 startSpeedMax;
    public GameObject bombCollider;
    private BombAnimation bombAnimation;
    private bool isExploded; // 防止多次觸發銷毀

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

    private void Awake()
    {
        bombAnimation = transform.Find("Ani").GetComponent<BombAnimation>();
        isExploded = false;
    }

    public void isThrow(float direction) 
    {
        transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * Random.Range(startSpeedMin.x, startSpeedMax.x), Random.Range(startSpeedMin.y, startSpeedMax.y)), ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isExploded) // 確保僅觸發一次
        {
            isExploded = true;
            bombAnimation.bombexplode();
            bombCollider.SetActive(false);
        }
    }
}

