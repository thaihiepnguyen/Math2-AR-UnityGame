using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragonHealthBar : MonoBehaviour
{
    public Image mask;
    public static DragonHealthBar instance { get; private set; }
    float originalSize;

    void Awake()
    {
    instance = this;
    }
    void Start()
    {
    originalSize = mask.rectTransform.rect.width;
    }

    public void SetValue(float value)
    {
    mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }
}