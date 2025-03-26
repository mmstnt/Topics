using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CartoonVikingGunBullet : MonoBehaviour
{
    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("角色參數")]
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
