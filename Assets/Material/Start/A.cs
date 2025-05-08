using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class A : MonoBehaviour
{
    public ScrollRect SR;
    public RectTransform viewPort_rtf;
    public RectTransform content_rtf;
    public HorizontalLayoutGroup HLG;
    public RectTransform[] ItemList;
    public float speed;


    private bool isUpdated;
    private Vector2 Oldvelocity;

    void Start()
    {
        isUpdated = false;
        Oldvelocity = Vector2.zero;//靜止不滾動

        int AddNum = Mathf.CeilToInt(viewPort_rtf.rect.width / (ItemList[0].rect.width + HLG.spacing));

        //右邊補充A B C...
        for (int i = 0; i < AddNum; i++)
        {
            RectTransform rtf = Instantiate(ItemList[i % ItemList.Length], content_rtf);
            rtf.SetAsLastSibling();
        }

        //左邊補充Z Y X...
        for (int i = 0; i < AddNum; i++)
        {
            //倒數循環
            int j = ItemList.Length - 1 - i;
            while (j < 0)
            {
                j += ItemList.Length;
            }
            RectTransform rtf = Instantiate(ItemList[j], content_rtf);
            rtf.SetAsFirstSibling();
        }
        content_rtf.localPosition = new Vector3(-(ItemList[0].rect.width + HLG.spacing) * AddNum,
                                                content_rtf.localPosition.y,
                                                content_rtf.localPosition.z);
    }

    void Update()
    {
        if (isUpdated)
        {
            isUpdated = false;
            SR.velocity = Oldvelocity;
        }
        //左邊補充用完時跳轉原始組
        if (content_rtf.localPosition.x > 0)
        {
            //強制刷新
            Canvas.ForceUpdateCanvases();
            Oldvelocity = SR.velocity;
            content_rtf.localPosition -= new Vector3(ItemList.Length * (ItemList[0].rect.width + HLG.spacing), 0, 0);
            isUpdated = true;
        }
        //右邊補充用完時跳轉原始組
        if (content_rtf.localPosition.x < -(ItemList[0].rect.width + HLG.spacing) * ItemList.Length)
        {
            //強制刷新
            Canvas.ForceUpdateCanvases();
            Oldvelocity = SR.velocity;
            content_rtf.localPosition += new Vector3(ItemList.Length * (ItemList[0].rect.width + HLG.spacing), 0, 0);
            isUpdated = true;
        }

        
        void SimulateScrollWithVelocity(Vector3 velocity)
        {
            SR.velocity = velocity;
        }

        // 示例调用
        SimulateScrollWithVelocity(new Vector3(-speed, 0)); // 向下滚动
    }
}