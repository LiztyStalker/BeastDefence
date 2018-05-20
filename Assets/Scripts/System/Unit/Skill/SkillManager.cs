using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

public class SkillManager : SingletonClass<SkillManager>
{

    enum TYPE_SKILL_DATA {Key, Name, TypeActive, TypeTeam, ActorCount, Size, CoolTime, IncCoolTime, Rate, IncRate, SkillActorKey, Contents}
 

    Dictionary<string, Skill> m_skillDic = new Dictionary<string, Skill>();

    public SkillManager()
    {
        initParse();
    }

    void initParse()
    {



        Sprite[] iconArray = Resources.LoadAll<Sprite>(Prep.skillIconPath);


        TextAsset textAsset = Resources.Load<TextAsset>(Prep.skillDataPath);

        if (textAsset != null)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(textAsset.text);

            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(Prep.getXmlDataPath(GetType()));

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                string key = xmlNode.SelectSingleNode(TYPE_SKILL_DATA.Key.ToString()).InnerText;
                string name = xmlNode.SelectSingleNode(TYPE_SKILL_DATA.Name.ToString()).InnerText;

                Sprite icon = iconArray.Where(spr => spr.name == key).SingleOrDefault();
                if (icon == null) Prep.LogError(key, "아이콘을 찾을 수 없음", GetType());

                //string active = xmlNode.SelectSingleNode(TYPE_SKILL_DATA.TypeActive.ToString()).InnerText;
                //Skill.TYPE_SKILL_ACTIVE typeActive = Skill.TYPE_SKILL_ACTIVE.PASSIVE;
                //switch (active)
                //{
                //    case "P":
                //        typeActive = Skill.TYPE_SKILL_ACTIVE.PASSIVE;
                //        break;
                //    case "A":
                //        typeActive = Skill.TYPE_SKILL_ACTIVE.ACTIVE;
                //        break;
                //}


                Skill.TYPE_SKILL_ACTIVE typeSkillActive = (Skill.TYPE_SKILL_ACTIVE)Enum.Parse(typeof(Skill.TYPE_SKILL_ACTIVE), xmlNode.SelectSingleNode(TYPE_SKILL_DATA.TypeActive.ToString()).InnerText);

                //int i_type = 0;
                //if (!int.TryParse(xmlNode.SelectSingleNode(TYPE_SKILL_DATA.TypeActive.ToString()).InnerText, out i_type))
                //{
                //    i_type = 0;
                //}

                //Skill.TYPE_SKILL_ACTIVE typeActive = Skill.TYPE_SKILL_ACTIVE.PASSIVE;


                //if ((i_type & 0x4) == 0x4)
                //    typeActive |= Skill.TYPE_SKILL_ACTIVE.ACTIVE;
                //if ((i_type & 0x3) == 0x3)
                //    typeActive |= Skill.TYPE_SKILL_ACTIVE.MOVE;
                //if ((i_type & 0x2) == 0x2)
                //    typeActive |= Skill.TYPE_SKILL_ACTIVE.HIT;
                //if ((i_type & 0x1) == 0x1)
                //    typeActive |= Skill.TYPE_SKILL_ACTIVE.ATTACK;


                TYPE_TEAM typeTeam = (TYPE_TEAM)Enum.Parse(typeof(TYPE_TEAM), xmlNode.SelectSingleNode(TYPE_SKILL_DATA.TypeTeam.ToString()).InnerText);



                int actorCnt = 1;
                if (!int.TryParse(xmlNode.SelectSingleNode(TYPE_SKILL_DATA.ActorCount.ToString()).InnerText, out actorCnt))
                {
                    actorCnt = 1;
                }

                float size = 1f;
                if (!float.TryParse(xmlNode.SelectSingleNode(TYPE_SKILL_DATA.Size.ToString()).InnerText, out size))
                {
                    size = 1f;
                }


                float coolTime;
                if (!float.TryParse(xmlNode.SelectSingleNode(TYPE_SKILL_DATA.CoolTime.ToString()).InnerText, out coolTime))
                {
                    coolTime = 1f;
                }

                float incCoolTime;
                if (!float.TryParse(xmlNode.SelectSingleNode(TYPE_SKILL_DATA.IncCoolTime.ToString()).InnerText, out incCoolTime))
                {
                    incCoolTime = 0f;
                }

                float rate;
                if (!float.TryParse(xmlNode.SelectSingleNode(TYPE_SKILL_DATA.Rate.ToString()).InnerText, out rate))
                {
                    rate = 0f;
                }


                float incRate;
                if (!float.TryParse(xmlNode.SelectSingleNode(TYPE_SKILL_DATA.IncRate.ToString()).InnerText, out incRate))
                {
                    incRate = 0f;
                }

                //string skillActorKey = xmlNode.SelectSingleNode(TYPE_SKILL_DATA.SkillActorKey.ToString()).InnerText;
                SkillActor skillActor = SkillActorManager.GetInstance.getSkillActor(key);
                if (key != "-" && skillActor == null)
                {
                    Prep.LogError(key, "를 찾을 수 없음", GetType());
                }

                string contents = xmlNode.SelectSingleNode(TYPE_SKILL_DATA.Contents.ToString()).InnerText;


                Skill skill = new Skill(
                    key, 
                    name, 
                    icon, 
                    typeTeam,
                    actorCnt,
                    size,
                    typeSkillActive, 
                    coolTime, 
                    incCoolTime, 
                    rate, 
                    incRate, 
                    skillActor, 
                    contents
                    );
                m_skillDic.Add(key, skill);
            }
        }
        else
        {
            Prep.LogError(Prep.skillDataPath, "를 찾을 수 없음", GetType());
        }


        //Skill skill = new Skill();

        //skill.key = "Bomb";
        //skill.name = "폭탄투척";
        //skill.coolTime = 5f;
        //skill.skillRate = 100f;

        //skill.typeSkillPlay = Skill.TYPE_SKILL_ACTIVE.ACTIVE;

        ////스킬 행동
        //ISkillActor skillActor = new DrawAttackSkillActor();
        //((DrawAttackSkillActor)skillActor).attack = 100;

        //skill.skillActor = skillActor;


        //m_skillDic.Add("Bomb", skill);

        //Skill skill1 = new Skill();

        //skill1.key = "Assaulting";
        //skill1.name = "급습";
        //skill1.coolTime = 5f;
        //skill1.skillRate = 100f;

        //skill1.typeSkillPlay = Skill.TYPE_SKILL_ACTIVE.ACTIVE;

        ////스킬 행동
        //ISkillActor skillActor1 = new MoveAttackSkillActor();
        //((MoveAttackSkillActor)skillActor1).attack = 50;

        //skill1.skillActor = skillActor1;

        //m_skillDic.Add("Assaulting", skill1);



        //Skill skill2 = new Skill();

        //skill2.key = "Defence";
        //skill2.name = "방어";
        //skill2.coolTime = 0f;
        //skill2.skillRate = 100f;

        //skill2.typeSkillPlay = Skill.TYPE_SKILL_PLAY.PASSIVE;

        ////스킬 행동
        //ISkillActor skillActor2 = new MyselfBuffSkillActor();

        //IStateControl[] stateControls = new IStateControl[1];

        //stateControls[0] = new DefenceStateControl();
        //stateControls[0].value = 0.2f;
        //stateControls[0].typeStateValue = StateControl.TYPE_STATE_VALUE.RATE;

        //Buff buff = new Buff("Defence", "방어", 0f, Buff.TYPE_BUFF_CONSTRAINT.ALWAYS, stateControls);
        
        //BuffActor buffActor = new BuffActor();
        //buffActor.setBuff(buff);

        //((BuffSkillActor)skillActor2).buffActor = buffActor;

        //skill2.skillActor = skillActor2;

        //m_skillDic.Add("Defence", skill2);


    }

    /// <summary>
    /// 스킬 가져오기
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Skill getSkill(string key)
    {
        if (m_skillDic.ContainsKey(key))
        {
            return m_skillDic[key];
        }
        
        if(key != "-")
            Prep.LogError(key, "를 찾을 수 없음", GetType());

        return null;
    }

    /// <summary>
    /// 스킬 카드 가져오기
    /// </summary>
    /// <param name="key"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    public SkillCard getSkillCard(string key, int level = 1)
    {
        Skill skill = getSkill(key);
        if (skill != null)
        {
            return new SkillCard(skill, level);
        }
        return null;
    }
}

