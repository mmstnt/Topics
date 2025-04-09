using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CthuluSkill03 : MonoBehaviour
{
    public float gravityForce;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<Rigidbody2D>() && other.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
            Vector2 direction = transform.position - other.transform.position;
            float distance = direction.magnitude;
            float forceMagnitude = Mathf.Min(gravityForce / (distance + 5), gravityForce/5);
            Debug.Log(forceMagnitude);
            Vector2 force = direction.normalized * forceMagnitude;
            other.GetComponent<Rigidbody2D>().AddForce(force);
        }
    }
}
