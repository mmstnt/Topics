using Mediapipe.Unity.Sample.FaceLandmarkDetection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMover : MonoBehaviour
{
    public Vector3 startSite;
    public Vector3 endSite;
    public bool back;
    public float speed;             // 子彈移動速度
    private LineRenderer lineRenderer;

    private void Start() 
    {
        lineRenderer = transform.GetComponent<LineRenderer>();
        
        back = false;
    }

    void Update()
    {
        startSite = transform.parent.position;
        if (endSite == null) return;
        if (!back && transform.position == endSite)
        {
            back = true;
        }
        if (back && transform.position == startSite)
        {
            transform.parent.GetComponent<Frog>().action = false;
            Destroy(this.gameObject);
        }
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, startSite);
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, back ? startSite : endSite, (back ? 1.5f * speed : speed) * Time.deltaTime);
    }

    public void frogAttack(Vector3 Site) 
    {
        endSite = Site;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.GetComponent<Character>()) 
        {
            back = true;
        }
    }
}
