using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPUI : MonoBehaviour
{
    [Header("生命值物件")]
    public Image hpImage;
    public Image hpDelayImage;
    public float hpChangeTime;
    private float hpChangeDuration;

    private void Update()
    {
        if (hpChangeDuration > 0) 
        {
            hpChangeDuration -= Time.deltaTime;
        }
        else 
        {
            if (hpDelayImage.fillAmount > hpImage.fillAmount)
            {
                hpDelayImage.fillAmount -= Time.deltaTime;
            }
            else 
            {
                hpDelayImage.fillAmount = hpImage.fillAmount;
            }
        }
    }

    public void playerHealthChange(float persentage)
    {
        hpChangeDuration = hpChangeTime;
        hpImage.fillAmount = persentage;
    }
}
