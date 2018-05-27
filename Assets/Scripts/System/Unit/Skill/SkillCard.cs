using UnityEngine;


public class SkillCard : ICard
{
    Skill m_skill;

    int m_level = 1;


    public Skill skill { get { return m_skill; } }
    public string key { get { return m_skill.key; }}
    public string name { get { return m_skill.name; }}
    public Sprite icon { get { return m_skill.icon; }}

    public int population { get { return 0; } }
    public int munitions { get { return 0; } }
    public int level { get { return m_level; } }
    public float waitTime { get { return m_skill.coolTime; }}
    public float range { get { return m_skill.range; } }
    public string contents { get { return m_skill.contents; } }
    public float size { get { return m_skill.size; } }
    public int actCnt { get { return m_skill.actorCnt; } }
    public TYPE_TEAM typeAlly { get { return m_skill.typeTeam; } }
    public Unit.TYPE_TARGETING typeTarget { get { return m_skill.typeTarget; } }
    public Skill.TYPE_SKILL_ACTIVE typeSkillPlay { get { return m_skill.typeSkillPlay; } }

//    public int skill { get { return m_skill.typeSkillPlay; } }

    public SkillCard(Skill skill)
    {
        m_skill = skill;
        m_level = 1;
        
    }

    public SkillCard(Skill skill, int level)
    {
        m_skill = skill;
        m_level = level;

    }


    public IActor skillAction(IActor iActor, IActor targetActor){
        return m_skill.skillAction(iActor, targetActor);
    }

}

