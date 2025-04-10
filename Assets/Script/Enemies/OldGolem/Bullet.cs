using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 startSpeedMin;
    public Vector2 startSpeedMax;
    public GameObject bombCollider;
    private bool isExploded;

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
        if (!isExploded && collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isExploded = true;
            transform.Find("Ani").GetComponent<Animator>().SetTrigger("isExplode");
            bombCollider.SetActive(false);
        }
    }
}

