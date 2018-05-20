using UnityEngine;

public class UnitCard : ICard
{
    Unit m_unit;
    int m_level;
    int m_maxExperiance;
    int m_nowExperiance;

    /// <summary>
    /// 출전 수
    /// </summary>
    int m_engage;

    public Unit unit { get { return m_unit; } }
    public int level { get { return m_level; } }
    public int engage { get { return m_engage; } }
    public int maxExperiance { get { return m_maxExperiance; } private set { m_maxExperiance = value; } }
    public int nowExperiance { get { return m_nowExperiance; } private set { m_nowExperiance = value; } }


    public string key { get { return m_unit.key; } }
    public string name { get { return m_unit.name; } }
    public string effectKey { get { return m_unit.effectKey; } }
    public Sprite icon { get { return m_unit.icon; } }

    public int attack { get { return m_unit.attack + (level - 1) * m_unit.increaseAttack; } }
    public float attackSpeed { get { return m_unit.attackSpeed + (float)(level - 1) * m_unit.increaseAttackSpeed; } }
    public float moveSpeed { get { return m_unit.moveSpeed + (float)(level - 1) * m_unit.increaseMoveSpeed; } }
    public float range { get { return m_unit.range + (float)(level - 1) * m_unit.increaseRange; } }
    public float health { 
        get {
            float nowHP = m_unit.health + (float)(level - 1) * m_unit.increaseHealth; 
            //성은 레벨에 대한 추가 퍼센트 체력을 획득
            if (m_unit.typeUnit == Unit.TYPE_UNIT.Building)
            {
                nowHP += nowHP * level * 0.01f;
            }
            return nowHP;
        } 
    }

    public float waitTime { 
        get 
        {
            float downTime = 0f;
            switch (unit.typeUnit)
            {
                case Unit.TYPE_UNIT.Soldier:
                    downTime = Account.GetInstance.accDevelop.getDevelopValue(Develop.TYPE_DEVELOP_VALUE_GROUP.SoldierBatch) * 0.01f;
                    break;
                case Unit.TYPE_UNIT.Hero:
                    downTime = Account.GetInstance.accDevelop.getDevelopValue(Develop.TYPE_DEVELOP_VALUE_GROUP.HeroBatch) * 0.01f;
                    break;
            }
            return m_unit.waitTime - downTime; 
        } 
    }

    public int population { get { return m_unit.population; } }
    public int munitions { get { return m_unit.munitions; } }
    public string contents { get { return m_unit.contents; } }
    public int cost { get { return m_unit.cost; } }

    public Unit.TYPE_UNIT typeUnit { get { return m_unit.typeUnit; } }
    public Unit.TYPE_LINE typeLine { get { return m_unit.typeLine; } }
    public Unit.TYPE_MOVEMENT typeMovement { get { return m_unit.typeMovement; } }
    public int typeRange { get { return m_unit.typeRange; } }
    public Unit.TYPE_TARGETING typeTargeting { get { return m_unit.typeTargeting; } }
    public TYPE_FORCE typeForce { get { return m_unit.typeForce; } }

    /// <summary>
    /// 훈련비용
    /// </summary>
    public int trainingCost { get { return m_maxExperiance - m_nowExperiance; } }

    /// <summary>
    /// 유닛카드 초기화
    /// </summary>
    /// <param name="unit"></param>
    public UnitCard(Unit unit)
    {
        setUnit(unit, 1, 0);
    }

    public UnitCard(Unit unit, int level)
    {
        setUnit(unit, level, 0);
    }

    public UnitCard(Unit unit, int level, int nowExp)
    {
        setUnit(unit, level, nowExp);
    }
       

    /// <summary>
    /// 유닛카드 삽입
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="level"></param>
    /// <param name="nowExperiance"></param>
    public void setUnit(Unit unit, int level, int nowExperiance)
    {
        m_unit = unit;
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
    /// <returns>양수 : 레벨 / 음수 : 남은 경험치</returns>
    public int addExperiance(int addExperiance)
    {

        //최대 레벨 초과로 레벨업을 할 수 없음
        //int maxLevel = Account.GetInstance.accUnit.getUnitMaxLevel(unit.typeUnit);

        //switch(unit.typeUnit){
        //    case Unit.TYPE_UNIT.Soldier:
        //        maxLevel = Prep.defaultSoldierLevel + (int)Account.GetInstance.accDevelop.getDevelopValue(Develop.TYPE_DEVELOP_VALUE_GROUP.SoldierLv);
        //        addExperiance += (int)((float)addExperiance * Account.GetInstance.accDevelop.getDevelopValue(Develop.TYPE_DEVELOP_VALUE_GROUP.SoldierExp) * 0.01f);
        //        break;
        //    case Unit.TYPE_UNIT.Hero:
        //        maxLevel = Prep.defaultHeroLevel + (int)Account.GetInstance.accDevelop.getDevelopValue(Develop.TYPE_DEVELOP_VALUE_GROUP.HeroLv);
        //        addExperiance += (int)((float)addExperiance * Account.GetInstance.accDevelop.getDevelopValue(Develop.TYPE_DEVELOP_VALUE_GROUP.HeroExp) * 0.01f);
        //        break;
        //}
        
        nowExperiance += addExperiance;
        while (nowExperiance >= maxExperiance)
        {
            //최대 레벨 미만이면
            if (!isMaxLevel())
            {
                nowExperiance -= maxExperiance;
                m_level++;
                maxExperiance = getMaxExperiance();
            }
            else
            {
                //남은 exp는 골드로 전환 - 음수로 출력
                //1경험치당 1골드
                int surplusExp = nowExperiance;
                Account.GetInstance.accData.addValue(nowExperiance, Shop.TYPE_SHOP_CATEGORY.Gold);
                nowExperiance = 0;
                return -surplusExp;
            }
        }
        return level;
    }

    /// <summary>
    /// 최대 레벨 도달 여부
    /// </summary>
    /// <returns></returns>
    public bool isMaxLevel()
    {
        return (m_level >= Account.GetInstance.accUnit.getUnitMaxLevel(m_unit.typeUnit));
    }

    /// <summary>
    /// 출전 횟수 증가하기
    /// </summary>
    /// <param name="cnt"></param>
    public void addEngage(int cnt = 1)
    {
        m_engage += cnt;
    }


    #region ########################### 직렬화 #################################

    /// <summary>
    /// 직렬화
    /// </summary>
    /// <returns></returns>
    public AccountUnitCardSerial getSerial()
    {
        AccountUnitCardSerial accSerial = new AccountUnitCardSerial();
        accSerial.key = key;
        accSerial.level = level;
        accSerial.nowExp = nowExperiance;
        accSerial.engage = m_engage;
        return accSerial;
    }

    /// <summary>
    /// 역직렬화
    /// </summary>
    /// <param name="cardSerial"></param>
    public UnitCard(AccountUnitCardSerial cardSerial)
    {
        Unit unit = UnitManager.GetInstance.getUnit(cardSerial.key);
        if (unit != null)
        {
            setUnit(unit, cardSerial.level, cardSerial.nowExp);
            m_engage = cardSerial.engage;
        }
        else
        {
            Prep.LogError(cardSerial.key, "를 가져오지 못했습니다.", GetType());
        }
    }

    #endregion
}

