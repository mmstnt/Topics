using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("�ƥ��ť")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("����Ѽ�")]
    public Vector2 startSpeedMin; // ���u��l�t��
    public Vector2 startSpeedMax;
    public GameObject bombCollider;
    private BombAnimation bombAnimation;
    private bool isExploded; // ����h��Ĳ�o�P��

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
        if (!isExploded) // �T�O��Ĳ�o�@��
        {
            isExploded = true;
            bombAnimation.bombexplode();
            bombCollider.SetActive(false);
        }
    }
}

