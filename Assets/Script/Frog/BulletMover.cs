using Mediapipe.Unity.Sample.FaceLandmarkDetection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMover : MonoBehaviour
{
    public float time;
    private float disTime;
    public float speed = 10f;             // �l�u���ʳt��
    public Vector3 direction;  // �l�u��V

    private void Start() 
    {
        disTime = time;
    }

    void Update()
    {
        if (disTime < 0) 
        {
            Destroy(this.gameObject);
        }
        // �ϥ� Transform ���ʤl�u
        disTime -= Time.deltaTime;
        transform.position += direction * speed * Time.deltaTime;


    }
}
