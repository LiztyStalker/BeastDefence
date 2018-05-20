using UnityEngine;
using UnityEngine.UI;

public class UIMissionAwardButton : MonoBehaviour
{

    [SerializeField]
    Text m_lowValueText;

    [SerializeField]
    Text m_fullValueText;

    [SerializeField]
    Image m_awardImage;


    SinarioAward.TYPE_SINARIO_AWARD_CATEGORY m_typeCategory;
    string m_value;

//    void Award()
//    {
////        GetComponent<Button>().enabled = false;
//        Debug.Log("clicked");

//        GetComponent<Button>().onClick.AddListener(() => OnClicked());
//    }

    public void setAward(SinarioAward.TYPE_SINARIO_AWARD_CATEGORY typeCategory, string value, bool isSmall = false)
    {

        m_lowValueText.gameObject.SetActive(false);
        m_fullValueText.gameObject.SetActive(false);

        m_typeCategory = typeCategory;
        m_value = value;

        int data = 0;
        if (!int.TryParse(value, out data))
        {
            Unit unit = UnitManager.GetInstance.getUnit(value);
            m_awardImage.sprite = unit.icon;

            if (!isSmall){
                m_lowValueText.text = unit.name;
                m_fullValueText.gameObject.SetActive(true);
            }
        }
        else
        {
            m_awardImage.sprite = SinarioAwardManager.GetInstance.getSinarioAwardSprite(typeCategory);

            if (isSmall)
            {
                m_fullValueText.text = value;
                m_fullValueText.gameObject.SetActive(true);
            }
            else
            {
                m_lowValueText.text = value;
                m_lowValueText.gameObject.SetActive(true);
            }

        }

        if(!isSmall)
            GetComponent<Button>().onClick.AddListener(() => OnClicked());

    }

    void OnClicked()
    {
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.NONE);
        UIPanelManager.GetInstance.root.uiCommon.uiNormalBox.setData(m_typeCategory, m_value);
    }
}

