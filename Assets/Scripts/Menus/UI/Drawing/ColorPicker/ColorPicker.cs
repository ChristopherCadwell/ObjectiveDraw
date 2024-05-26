using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

[Serializable] public class ColorEvent : UnityEvent<Color> { }

public class ColorPicker : MonoBehaviour
{
    public ColorEvent onColorPreview;
    public ColorEvent onColorSelect;
    private RectTransform Rect;
    private Texture2D colorTexture;
    private Color color;

    void Start()
    {
        Rect = GetComponent<RectTransform>();

        colorTexture = GetComponent<Image>().mainTexture as Texture2D;
    }

    void Update()
    {
        if (!RectTransformUtility.RectangleContainsScreenPoint(Rect, Input.mousePosition, Camera.main)) return;

        Vector2 delta;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(Rect, Input.mousePosition, Camera.main, out delta);

        float width = Rect.rect.width;
        float height = Rect.rect.height;
        delta += new Vector2(width * 0.5f, height * 0.5f);

        float x = Mathf.Clamp(delta.x / width, 0f, 1f);
        float y = Mathf.Clamp(delta.y / height, 0f, 1f);

        int texX = Mathf.RoundToInt(x * colorTexture.width);
        int texY = Mathf.RoundToInt(y * colorTexture.height);

        color = colorTexture.GetPixel(texX, texY);

        onColorPreview?.Invoke(color);
        if (Input.GetMouseButtonDown(0))
            onColorSelect?.Invoke(color);
    }

    public void ChangeColor()
    {
        GameManager.Instance.color = color;
    }
}
