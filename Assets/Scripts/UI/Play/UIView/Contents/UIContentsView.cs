using System;
using UnityEngine;
using UnityEngine.UI;

public class UIContentsView : MonoBehaviour
{
    [SerializeField]
    Text m_contentsText;

    [SerializeField]
    Image m_image;

    public void setContents(Contents contents)
    {
        m_image.sprite = null;
        m_contentsText.text = contents.contents;
    }
}

