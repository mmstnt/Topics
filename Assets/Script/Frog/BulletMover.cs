using Mediapipe.Unity.Sample.FaceLandmarkDetection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMover : MonoBehaviour
{
    public float time;
    private float disTime;
    public float speed = 10f;             // 子彈移動速度
    public Vector3 direction;  // 子彈方向

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
        // 使用 Transform 移動子彈
        disTime -= Time.deltaTime;
        transform.position += direction * speed * Time.deltaTime;


    }
}
