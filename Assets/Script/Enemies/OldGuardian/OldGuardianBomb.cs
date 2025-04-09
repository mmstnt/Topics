using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldGuardianBomb : MonoBehaviour
{
    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("角色參數")]
    public Vector2 startSpeedMin;
    public Vector2 startSpeedMax;
    public GameObject bombCollider;
    private bool isExploded;

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
        isExploded = false;
    }

    public void isThrow(float direction)
    {
        transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * Random.Range(startSpeedMin.x, startSpeedMax.x), Random.Range(startSpeedMin.y, startSpeedMax.y)), ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isExploded)
        {
            isExploded = true;
            transform.Find("Ani").GetComponent<Animator>().SetTrigger("isExplode");
            bombCollider.SetActive(false);
        }
    }
}
