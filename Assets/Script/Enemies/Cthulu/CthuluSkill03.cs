using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CthuluSkill03 : MonoBehaviour
{
    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("角色參數")]
    public float gravityForce;
    public float speed;
    public float times;
    private Vector2 dir;
    private GameObject player;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Vector3 direction = player.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        dir = new Vector2(
        Mathf.Cos(angle * Mathf.Deg2Rad),
        Mathf.Sin(angle * Mathf.Deg2Rad)
        ).normalized;
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

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + 180.0f * Time.deltaTime);
        times -= Time.deltaTime;
        if (times <= 0) 
        {
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(dir * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<Rigidbody2D>() && other.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
            Vector2 direction = transform.position - other.transform.position;
            float distance = direction.magnitude;
            float forceMagnitude = Mathf.Min(gravityForce / (distance + 10), gravityForce/10);
            Debug.Log(forceMagnitude);
            Vector2 force = direction.normalized * forceMagnitude;
            other.GetComponent<Rigidbody2D>().AddForce(force);
        }
    }
}
