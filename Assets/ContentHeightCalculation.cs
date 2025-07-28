using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentHeightCalculation : MonoBehaviour
{
    public RectTransform contentTransform;
    public float SettingHeight = -10f;
    void LateUpdate()
    {
        float totalHeight = SettingHeight;
        foreach (RectTransform child in contentTransform)
        {
            totalHeight += child.sizeDelta.y;
        }

        contentTransform.sizeDelta = new Vector2(contentTransform.sizeDelta.x, totalHeight);
    }
}
