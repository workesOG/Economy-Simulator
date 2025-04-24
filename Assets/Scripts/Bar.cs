using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    private Color32 colorMin = new Color32(255, 133, 95, 255);
    private Color32 colorMax = new Color32(204, 255, 150, 255);
    private float maxLength;

    void Awake()
    {
        maxLength = GetComponent<RectTransform>().sizeDelta.x - 8;
    }

    public void UpdateBar(int current, int max)
    {
        float progress = Math.Min((float)current / max, 1f);
        float newLength = Mathf.Lerp(0, maxLength, progress);
        Color32 newColor = Color32.Lerp(colorMin, colorMax, progress);

        RectTransform rectTransform = transform.Find("Bar").GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(newLength, rectTransform.sizeDelta.y);
        transform.Find("Bar").GetComponent<Image>().color = newColor;
    }
}
