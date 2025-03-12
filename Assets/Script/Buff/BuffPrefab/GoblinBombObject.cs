using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinBombObject : MonoBehaviour
{
    [Header("°Ñ¼Æ")]
    public float explodeTime;

    private BombAnimation bombAnimation;
    private bool isExploded;
    private float time;

    private void Awake()
    {
        bombAnimation = transform.Find("Ani").GetComponent<BombAnimation>();
        time = explodeTime;
        isExploded = false;
    }

    private void Update()
    {
        time -= Time.deltaTime;
        if (time < 0 && !isExploded) 
        {
            isExploded = true;
            bombAnimation.bombexplode();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isExploded && collision.tag == "Enemies") 
        {
            
            isExploded = true;
            bombAnimation.bombexplode();
        }
    }
}
