using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICommanderInfo : MonoBehaviour
{
    [SerializeField]
    Text m_nameText;

    [SerializeField]
    Image m_image;

    [SerializeField]
    Text m_levelText;

    [SerializeField]
    Text m_expText;

    [SerializeField]
    Text m_forceText;

    [SerializeField]
    Text m_healthText;

    [SerializeField]
    Text m_leadershipText;

    [SerializeField]
    Text m_munitionsText;

    [SerializeField]
    Text m_contentsText;

    [SerializeField]
    UISkillIcon[] m_uiSkillIcons;

    UIDataBox m_uiDataBox;

    void Awake()
    {
        m_uiDataBox = UIPanelManager.GetInstance.root.uiCommon.uiDataBox;

        for (int i = 0; i < m_uiSkillIcons.Length; i++)
        {
            m_uiSkillIcons[i].dataBoxEvent += m_uiDataBox.setSkillData;
        }
    }

    public void setInfo(CommanderCard commanderCard)
    {
        if (commanderCard != null)
        {
            m_nameText.text = commanderCard.name;
            m_image.sprite = commanderCard.image;
            m_contentsText.text = commanderCard.contents;

            m_levelText.text = string.Format("{0}", commanderCard.level);
            m_expText.text = string.Format("{0}/{1}", commanderCard.nowExperiance, commanderCard.maxExperiance);
            m_forceText.text = string.Format("{0}", commanderCard.typeForce);
            m_healthText.text = string.Format("{0}", commanderCard.health);
            m_leadershipText.text = string.Format("{0}", commanderCard.leadership);
            m_munitionsText.text = string.Format("{0}", commanderCard.munitions);
            
            for (int i = 0; i < m_uiSkillIcons.Length; i++)
            {
                Skill skill = SkillManager.GetInstance.getSkill(commanderCard.skills[i]);
                m_uiSkillIcons[i].setSkill(skill, commanderCard.level);
            }
        }
    }


}

