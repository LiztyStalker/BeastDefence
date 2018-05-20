using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//사용 안함
public class AccountSkill
{
    Skill[] m_skillArray = new Skill[Prep.maxSkillSlot];

//    string[] m_waitSkillArray = new string[Prep.maxSkillSlot];
    Dictionary<string, Skill> m_skillDic = new Dictionary<string, Skill>();

//    Dictionary<string, SkillCard> m_skillDic = new Dictionary<string, SkillCard>();

    public Skill[] skillArray { get { return m_skillArray; } }

//    public string[] waitSkillArray { get { return m_waitSkillArray; } }

    public IEnumerator skillEnumerator { get { return m_skillDic.Values.GetEnumerator(); } }

    public AccountSkill()
    {
        //초기화
        addSkill(SkillManager.GetInstance.getSkill("Artillery"));
        addSkill(SkillManager.GetInstance.getSkill("MoraleCharge"));

        setSkillArray(getSkill("Artillery"), 0);
        setSkillArray(getSkill("MoraleCharge"), 1);

//        SkillCard skillCard = new SkillCard();

    }

    public void addSkill(Skill skill, int level = 1)
    {
        if (!m_skillDic.ContainsKey(skill.key))
        {
            m_skillDic.Add(skill.key, skill);
//            m_skillDic.Add(skill.key, new SkillCard(skill));
        }
    }

    public void setSkillArray(Skill skill, int index)
    {
        if (index >= 0 && index < m_skillArray.Length)
            m_skillArray[index] = skill;

    }

     public Skill getSkill(string key)
     {
         if (m_skillDic.ContainsKey(key))
         {
             return m_skillDic[key];
         }

         Prep.LogError(key, "스킬을 찾을 수 없음", GetType());
         return null;
     }
}

