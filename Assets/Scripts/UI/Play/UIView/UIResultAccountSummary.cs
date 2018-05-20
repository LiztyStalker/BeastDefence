using System;
using UnityEngine;
using UnityEngine.UI;

public class UIResultAccountSummary : MonoBehaviour
{
    [SerializeField]
    Text m_nameText;

    [SerializeField]
    Text m_levelText;

    [SerializeField]
    Text m_expText;

    [SerializeField]
    Slider m_expSlider;

    [SerializeField]
    Image m_image;

    public void setSummary(Account account)
    {
        m_nameText.text = account.name;
        m_levelText.text = string.Format("Lv {0}", account.level);
        m_expText.text = string.Format("{0}/{1}", account.nowExp, account.maxExp);
        m_expSlider.value = account.accData.expRate;// (float)account.nowExp / (float)account.maxExp;
        m_image.sprite = null;
    }

    public void setSummary(CommanderCard commanderCard)
    {
        m_nameText.text = commanderCard.name;
        m_levelText.text = string.Format("Lv {0}", commanderCard.level);
        m_expText.text = string.Format("{0}/{1}", commanderCard.nowExperiance, commanderCard.maxExperiance);
        m_expSlider.value = commanderCard.expRate();// (float)commanderCard.nowExperiance / (float)commanderCard.maxExperiance;
        m_image.sprite = commanderCard.icon;
    }

//    public void setExp(int exp)
//    {
//    }


    //현재 경험치와 다음 경험치를 합쳐야 함
}

