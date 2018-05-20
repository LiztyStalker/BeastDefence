using UnityEngine;
using UnityEngine.UI;

public class UIConscriptButton : MonoBehaviour
{

    [SerializeField]
    Text m_nameText;

    [SerializeField]
    Text m_costText;

    [SerializeField]
    Image m_image;

    public void setConscript(Sprite sprite, int index)
    {
        m_image.sprite = sprite;
        m_nameText.text = Prep.conscriptText[index].ToString();
        m_costText.text = Prep.conscriptPay[index].ToString();

    }

}

