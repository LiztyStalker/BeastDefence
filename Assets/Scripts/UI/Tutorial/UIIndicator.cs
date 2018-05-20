using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIIndicator : MonoBehaviour
{
    [SerializeField]
    Image m_image;


    public void setTexture(RectTransform rectTransform)
    {

        Texture2D texture = new Texture2D((int)rectTransform.rect.width, (int)rectTransform.rect.height, TextureFormat.RGBA32, true);

        Debug.LogWarning("Rect : " + rectTransform.rect);

        texture.ReadPixels(rectTransform.rect, 0, 0, true);

        texture.Apply();

        m_image.sprite = Sprite.Create(texture, rectTransform.rect, Vector2.one * 0.5f);

        gameObject.SetActive(true);

        m_image.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;

        m_image.GetComponent<RectTransform>().sizeDelta = rectTransform.sizeDelta;
    }
}

