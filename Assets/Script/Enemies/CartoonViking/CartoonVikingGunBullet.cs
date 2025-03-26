using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CartoonVikingGunBullet : MonoBehaviour
{
    [Header("�ƥ��ť")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("����Ѽ�")]
    public float speed;
    public float time;

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

    private void Update()
    {
        if (time < 0)
        {
            destroyGameObject();
        }
        time -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        transform.Translate(new Vector2(transform.localScale.x, 0) * speed * Time.deltaTime);
    }

    public void destroyGameObject() 
    {
        Destroy(gameObject);
    }
}
