using UnityEngine;
using Defence.CommanderPackage;

public class CommanderCard : ICard
{

    Commander m_commander;
    int m_level;
    int m_maxExperiance;
    int m_nowExperiance;

    public Commander commander { get { return m_commander; } }
    public int level { get { return m_level; } }
    public int maxExperiance { get { return m_maxExperiance; } private set { m_maxExperiance = value; } }
    public int nowExperiance { get { return m_nowExperiance; } private set { m_nowExperiance = value; } }


    public string key { get { return m_commander.key; } }
    public string name { get { return m_commander.name; } }
    public Sprite icon { get { return m_commander.icon; } }
    public Sprite image { get { return m_commander.image; } }

    public TYPE_FORCE typeForce{get{return m_commander.typeForce;}}

    public int health{get{return m_commander.health;}}
    public int munitions{get{return m_commander.munitions;}}
    public int leadership{get{return m_commander.leadership;}}

    public string[] skills { get { return m_commander.skills; } }
    public string contents { get { return m_commander.contents; } }
   

    /// <summary>
    /// 유닛카드 초기화
    /// </summary>
    /// <param name="unit"></param>
    public CommanderCard(Commander commander)
    {
        setCommander(commander, 1, 0);
    }

    public CommanderCard(Commander commander, int level)
    {
        setCommander(commander, level, 0);
    }

    public CommanderCard(Commander commander, int level, int nowExp)
    {
        setCommander(commander, level, nowExp);
    }

    public CommanderCard(AccountCommanderCardSerial cardSerial)
    {
        Commander commander = CommanderManager.GetInstance.getCommander(cardSerial.key);
        if (commander != null)
        {
            setCommander(commander, cardSerial.level, cardSerial.nowExp);
        }
        else
        {
            Prep.LogError(cardSerial.key, "를 가져오지 못했습니다.", GetType());
        }
    }

    /// <summary>
    /// 유닛카드 삽입
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="level"></param>
    /// <param name="nowExperiance"></param>
    public void setCommander(Commander commander, int level, int nowExperiance)
    {
        m_commander = commander;
        m_level = level;
        m_nowExperiance = nowExperiance;
        m_maxExperiance = getMaxExperiance();


    }

    /// <summary>
    /// 경험치 값 가져오기
    /// </summary>
    /// <returns></returns>
    int getMaxExperiance()
    {
        //영웅
//        if (m_unit.typeUnit == Unit.TYPE_UNIT.HERO)
//            return m_level * 200;
        return level * 100;
    }

    /// <summary>
    /// 경험치 할당
    /// </summary>
    /// <param name="addExperiance"></param>
    /// <returns></returns>
    public int addExperiance(int addExperiance)
    {

        //최대 레벨 초과로 레벨업을 할 수 없음
        
        nowExperiance += addExperiance;
        while (nowExperiance >= maxExperiance)
        {
            //최대 레벨 미만이면
            //지휘관 최대 레벨 연구 필요
            if (m_level < Prep.commanderMaxLevel)
            {
                nowExperiance -= maxExperiance;
                m_level++;
                maxExperiance = getMaxExperiance();
            }
            else
            {
                //남은 exp는 골드로 전환
                //메시지 보내야 함
                Account.GetInstance.accData.addValue(nowExperiance, Shop.TYPE_SHOP_CATEGORY.Gold);
                nowExperiance = 0;
            }
        }
        return level;
    }
    
    public int population
    {
        get { return 1; }
    }

    public float waitTime
    {
        get { return 0f; }
    }

    public float expRate()
    {
        return (float)m_nowExperiance / (float)m_maxExperiance;
    }

    public AccountCommanderCardSerial getSerial()
    {
        AccountCommanderCardSerial accSerial = new AccountCommanderCardSerial();
        accSerial.key = key;
        accSerial.level = level;
        accSerial.nowExp = nowExperiance;
        return accSerial;
    }
}

