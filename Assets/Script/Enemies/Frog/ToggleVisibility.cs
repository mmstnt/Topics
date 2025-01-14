using UnityEngine;
using System.Collections;

public class ToggleVisibility : MonoBehaviour
{
    public GameObject square; // Square物件
    private Rigidbody2D rg;
    private bool hasShown = false;

    private void Start()
    {
        rg = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Mathf.Abs(rg.velocity.y) < 0.01f)
        {
            StartCoroutine(ShowSquareTemporarily());
        }
        else
        {
            // 引藏Square物件
            square.SetActive(false);
            hasShown = true;
        }
    }

    private IEnumerator ShowSquareTemporarily()
    {
        if (hasShown == true)
        {
            square.SetActive(true);
            yield return new WaitForSeconds(0.1f); // 等待0.1秒
            square.SetActive(false);
            hasShown = false;
        }
    }
}






