using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    public bool isGround;
    public Vector2 bottomOffset;
    public float checkRaduis;
    public LayerMask groundLayer;
    public bool notChangeMaterial2D;
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;

    private void Update()
    {
        checkGround();
        checkState();
    }

    private void checkGround()
    {
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, checkRaduis, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        //繪製地面碰撞點
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRaduis);
    }

    private void checkState()
    {
        if (notChangeMaterial2D) return;
        GetComponent<CapsuleCollider2D>().sharedMaterial = isGround ? normal : wall;
    }
}
