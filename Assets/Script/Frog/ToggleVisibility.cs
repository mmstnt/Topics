using UnityEngine;
using System.Collections;

public class ToggleVisibility : MonoBehaviour
{
    public GameObject square; // Square����
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
            // ����Square����
            square.SetActive(false);
            hasShown = true;
        }
    }

    private IEnumerator ShowSquareTemporarily()
    {
        if (hasShown == true)
        {
            square.SetActive(true);
            yield return new WaitForSeconds(0.1f); // ����0.1��
            square.SetActive(false);
            hasShown = false;
        }
    }
}






