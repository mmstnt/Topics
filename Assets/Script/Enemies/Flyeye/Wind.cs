using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [Header("�ƥ��ť")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("����Ѽ�")]
    public float speed;
    public Vector2 direction;

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
        float angle = transform.eulerAngles.z;

        direction = new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad), 
            Mathf.Sin(angle * Mathf.Deg2Rad)  
        );

        Destroy(gameObject, 1.0f);
    }

    private void FixedUpdate()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }
}

 
