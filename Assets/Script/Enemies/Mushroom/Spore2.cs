using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
public class Spore2 : MonoBehaviour
{
    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("角色參數")]
    public Vector2 startSpeed;
    public float time;
    public float maxtime;

    private bool isExploded = false;
    public Spore2Animation sporeAnimation;
    
    private void Awake()
    {
        sporeAnimation = transform.Find("Ani").GetComponent<Spore2Animation>();
        time = maxtime;
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
        transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * Random.Range(-10, startSpeed.x), Random.Range(10, startSpeed.y)), ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isExploded) // 確保僅觸發一次
        {
            isExploded = true;
        }
    }

    private void Update()
    {
        if (!isExploded) return;

        if (time > 0)
        {
            time -= Time.deltaTime;
            return;
        }
        sporeAnimation.spore2explode();
    }
}
