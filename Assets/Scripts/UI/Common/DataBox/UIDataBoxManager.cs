using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIDataBoxManager : MonoBehaviour
{

    [SerializeField]
    UINormalDataBox m_uiNormalBox;

    [SerializeField]
    UISkillDataBox m_uiSkillBox;

    [SerializeField]
    UIUnitDataBox m_uiUnitBox;


    void OnEnable()
    {
        close();
    }

    /// <summary>
    /// 보상 데이터박스 열기
    /// </summary>
    /// <param name="typeAwardCategory"></param>
    /// <param name="value"></param>
    public void setData(SinarioAward.TYPE_SINARIO_AWARD_CATEGORY typeAwardCategory, string value)
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        m_uiNormalBox.setData(typeAwardCategory, value);
        setPanel(m_uiNormalBox);
    }
    
    public void setData(SkillCard skillCard)
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        m_uiSkillBox.setSkillData(skillCard);
        setPanel(m_uiSkillBox);
    }

    public void setData(SkillCard skillCard, Vector3 pos)
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        m_uiSkillBox.setSkillData(skillCard, pos);
        setPanel(m_uiSkillBox);
    }

    public void setData(Skill skill, int level)
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        m_uiSkillBox.setSkillData(skill, level);
        setPanel(m_uiSkillBox);
    }

    public void setData(Skill skill, int level, Vector3 pos)
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        m_uiSkillBox.setSkillData(skill, level, pos);
        setPanel(m_uiSkillBox);
    }

    public void setData(UnitCard unitCard, Vector3 pos)
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        m_uiUnitBox.setData(unitCard, pos);
        setPanel(m_uiUnitBox);
    }

    void setPanel(UIDataBox uiDataBox)
    {
        if (uiDataBox is UINormalDataBox)
        {
            m_uiSkillBox.close();
            m_uiUnitBox.close();
        }
        else if (uiDataBox is UISkillDataBox)
        {
            m_uiNormalBox .close();
            m_uiUnitBox.close();
        }
        else if (uiDataBox is UIUnitDataBox)
        {
            m_uiNormalBox.close();
            m_uiSkillBox.close();
        }
    }

    public void close()
    {
        m_uiUnitBox.close();
        m_uiNormalBox.close();
        m_uiSkillBox.close();
    }




    
}

