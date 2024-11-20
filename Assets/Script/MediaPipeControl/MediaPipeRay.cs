using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediaPipeRay : MonoBehaviour
{
    public Camera _camera;
    public LayerMask backGroundLayer;
    public Vector3 hit;
    private Ray ray;

    private void Update()
    {
        ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit raycasHit, backGroundLayer))
        {
            hit = raycasHit.point;
            if (transform.name == "468")
            {
                Debug.DrawLine(transform.position, hit, transform.name == "LeftEve" ? Color.green : Color.red);
                Debug.DrawRay(transform.position, transform.forward);
            }
            if (transform.name == "473")
            {
                Debug.DrawLine(transform.position, hit, transform.name == "LeftEve" ? Color.green : Color.red);
                Debug.DrawRay(transform.position, transform.forward);
            }
            if (transform.name == "1")
            {
                Debug.DrawLine(transform.position, hit, Color.blue);
                Debug.DrawRay(transform.position, transform.forward);
            }
        }
    }

}
