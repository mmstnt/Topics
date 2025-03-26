using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("ㄆン菏钮")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("à︹把计")]
    public Vector2 startSpeedMin; // 紆﹍硉
    public Vector2 startSpeedMax;
    public GameObject bombCollider;
    private BombAnimation bombAnimation;
    private bool isExploded; // ňゎΩ牟祇綪反

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
        if (!isExploded) // 絋玂度牟祇Ω
        {
            isExploded = true;
            bombAnimation.bombexplode();
            bombCollider.SetActive(false);
        }
    }
}

