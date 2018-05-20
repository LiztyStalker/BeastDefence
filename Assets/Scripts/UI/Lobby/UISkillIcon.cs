using System;
using UnityEngine;
using UnityEngine.UI;

public class UISkillIcon : MonoBehaviour
{

    public delegate void DataBoxViewDelegate(Skill skill, int level);
    public event DataBoxViewDelegate dataBoxEvent;

    [SerializeField]
    Transform m_noneTransform;

    [SerializeField]
    Image m_image;

    [SerializeField]
    Text m_levelText;

    Skill m_skill;
//    Unit m_unit;
    int m_level;
//    UIDataBox m_uiDataBox;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => OnSkillClicked());
    }

    public void setSkill(Skill skill, int level)
    {
        m_skill = skill;
        m_level = level;

        if (m_skill == null)
        {
            m_image.sprite = null;
            m_noneTransform.gameObject.SetActive(false);
            GetComponent<Button>().interactable = false;
        }
        else
        {
            m_image.sprite = skill.icon;
            m_levelText.text = string.Format("Lv {0}", (level / 10) + 1);
            m_noneTransform.gameObject.SetActive(true);
            GetComponent<Button>().interactable = true;
        }
    }

    void OnSkillClicked()
    {
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.NONE);
        dataBoxEvent(m_skill, m_level);
    }
}

