using UnityEngine;
using UnityEngine.UI;

public class UISkillCard
{
    [SerializeField]
    Image m_icon;

    [SerializeField]
    Transform m_infoTransform;

    [SerializeField]
    Text m_nameText;

    [SerializeField]
    Text m_levelText;

    [SerializeField]
    Text m_populationText;

    [SerializeField]
    Text m_munitionsText;
    
    Skill m_skill;

}

