using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Transform site;

    private void Update()
    {
        transform.position = site.position;
        transform.localScale = site.localScale;
    }
}
