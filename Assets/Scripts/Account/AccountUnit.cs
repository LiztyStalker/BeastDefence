using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Defence.CommanderPackage;

public class AccountUnit
{
    //사용하는 지휘관
    string m_commander;

    //지휘관 리스트
    Dictionary<string, CommanderCard> m_commanderCardDic = new Dictionary<string, CommanderCard>();

    //사용 유닛 리스트 
    //세력에 따라서 리스트 배치
    Dictionary<TYPE_FORCE, List<string>> m_unitWaitDic = new Dictionary<TYPE_FORCE, List<string>>();

    //List<string> m_unitWaitList = new List<string>();

    //유닛 리스트
    Dictionary<string, UnitCard> m_unitCardDic = new Dictionary<string, UnitCard>();

    //사용 영웅 리스트
//    List<string> m_heroWaitList = new List<string>();

    //영웅 리스트
//    Dictionary<string, UnitCard> m_heroCardDic = new Dictionary<string, UnitCard>();

    //총 출전수
    //int m_totalHeroEngage;
    //int m_totalSoldierEngage;

    public int totalHeroEngage { get { return m_unitCardDic.Values.Where(unit => unit.typeUnit == Unit.TYPE_UNIT.Hero).Sum(unit => unit.engage); } }
    public int totalSoldierEngage { get { return m_unitCardDic.Values.Where(unit => unit.typeUnit == Unit.TYPE_UNIT.Soldier).Sum(unit => unit.engage); } }
    public int totalUnitEngage { get { return totalHeroEngage + totalSoldierEngage; } }

    //모집 시간
    DateTime m_lastTime;

    //모집 새로고침 타임
    TimeSpan m_getTime = new TimeSpan(0, 1, 0);

    public string nowCommander { 
        get {
            if (m_commander == null)
                m_commander = m_commanderCardDic.Values.First().key;
            return m_commander; 
        }
        set
        {
            m_commander = value;
        }
    }
//    public IEnumerable<CommanderCard> commanderCardEnumerator { get {return m_commanderCardDic.Values; } }

    /// <summary>
    /// 세력에 따른 지휘관 가져오기
    /// </summary>
    /// <param name="typeForce"></param>
    /// <returns></returns>
    public IEnumerator<CommanderCard> getCommanderCards(TYPE_FORCE typeForce = TYPE_FORCE.All)
    {
        return m_commanderCardDic.Values.Where(card => (typeForce & card.typeForce) == card.typeForce).GetEnumerator();
    }

    /// <summary>
    /// 보관하고 있는 유닛 가져오기
    /// </summary>
    /// <param name="typeUnit"></param>
    /// <param name="typeForce"></param>
    /// <returns></returns>
    //public IEnumerator<UnitCard> getUnitCard(Unit.TYPE_UNIT typeUnit, TYPE_FORCE typeForce)
    //{
    //    return m_unitCardDic.Values.Where(card => (card.typeForce & typeForce) == card.typeForce && card.typeUnit == typeUnit).GetEnumerator();
    //}

//    public List<string> unitWaitList { get { return m_unitWaitList; } }
//    public IEnumerable<UnitCard> unitCardEnumerator { get { return m_unitCardDic.Values; } }

    /// <summary>
    /// 세력 대기 유닛 가져오기
    /// </summary>
    /// <param name="typeForce"></param>
    /// <returns></returns>
    public List<string> getWaitUnitCards(Unit.TYPE_UNIT typeUnit, TYPE_FORCE typeForce)
    {

        if (m_unitWaitDic.ContainsKey(typeForce))
        {
            //교집합
            //두 시퀸스의 같은 데이터 반환
            return m_unitWaitDic[typeForce].Intersect(m_unitCardDic.Values.Where(card => card.typeUnit == typeUnit).Select(card => card.key)).ToList<string>();
        }

        return new List<string>();
    }


    /// <summary>
    /// 세력에 따른 카드 가져오기
    /// </summary>
    /// <param name="typeUnit"></param>
    /// <param name="typeForce"></param>
    /// <returns></returns>
    public IEnumerator<UnitCard> getUnitCards(Unit.TYPE_UNIT typeUnit, TYPE_FORCE typeForce = TYPE_FORCE.All)
    {
        return m_unitCardDic.Values.
            Where(unitCard => 
                (typeForce & unitCard.typeForce) == unitCard.typeForce &&
                unitCard.typeUnit == typeUnit).
                GetEnumerator();
    }

//    public List<string> heroWaitList { get { return m_heroWaitList; } }
//    public IEnumerable<UnitCard> heroCardEnumerator { get { return m_heroCardDic.Values; } }

    /// <summary>
    /// 유닛 수 가져오기
    /// </summary>
    /// <param name="typeUnit"></param>
    /// <returns></returns>
    public int getUnitCount(Unit.TYPE_UNIT typeUnit)
    {
        return m_unitCardDic.Values.Where(card => card.typeUnit == typeUnit).Count();
    }

    /// <summary>
    /// 세력 유닛 수 가져오기
    /// </summary>
    /// <param name="typeUnit"></param>
    /// <param name="typeForce"></param>
    /// <returns></returns>
    public int getUnitCount(Unit.TYPE_UNIT typeUnit, TYPE_FORCE typeForce)
    {
        return m_unitCardDic.Values.Where(card => card.typeUnit == typeUnit && (typeForce & card.typeForce) == card.typeForce).Count();
    }

    /// <summary>
    /// 총 유닛 수 가져오기
    /// </summary>
    /// <returns></returns>
    public int getTotalUnitCount()
    {
        return m_unitCardDic.Count;
    }

//    public int getUnitCnt { get { return getHeroCnt + getSoldierCnt; } }
//    public int getHeroCnt { get { return m_heroCardDic.Count; } }
//    public int getSoldierCnt { get { return m_unitCardDic.Count; } }

    /// <summary>
    /// 출전 기록
    /// </summary>
    /// <param name="typeForce"></param>
    /// <returns></returns>
    public bool setEngageReport(TYPE_FORCE typeForce)
    {
        if(m_unitWaitDic.ContainsKey(typeForce)){
            foreach (string key in m_unitWaitDic[typeForce])
            {
                UnitCard unitCard = getUnitCard(key);
                if (unitCard != null)
                {
                    unitCard.addEngage();
                }
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// 디폴트 생성자
    /// </summary>
    public AccountUnit()
    {

        //모든 유닛 가져오기
        if (Prep.isAllUnit)
        {
            IEnumerator enumerator = UnitManager.GetInstance.units;

            while (enumerator.MoveNext())
            {
                Unit unit = enumerator.Current as Unit;
                if (unit != null)
                    addUnit(unit);
            }
        }
        else
        {
            //기본 유닛
            addUnit(UnitManager.GetInstance.getUnit("SpearSoldier"));
        }






        //초기화
//        addUnit(UnitManager.GetInstance.getUnit("SpearSoldier"));
//        addUnit(UnitManager.GetInstance.getUnit("Archer"));
//        addUnit(UnitManager.GetInstance.getUnit("Engineer"));
//        addUnit(UnitManager.GetInstance.getUnit("SwordSoldier"));
        //addUnit(UnitManager.GetInstance.getUnit("CrossbowMan"));
        //addUnit(UnitManager.GetInstance.getUnit("Scout"));
        //addUnit(UnitManager.GetInstance.getUnit("Shielder"));
        //addUnit(UnitManager.GetInstance.getUnit("Skirmisher"));
        //addUnit(UnitManager.GetInstance.getUnit("FireMagician"));
        //addUnit(UnitManager.GetInstance.getUnit("LightningMagician"));
        //addUnit(UnitManager.GetInstance.getUnit("FrozenMagician"));
        //addUnit(UnitManager.GetInstance.getUnit("CrawMaster"));
        //addUnit(UnitManager.GetInstance.getUnit("Sniper"));
        //addUnit(UnitManager.GetInstance.getUnit("Medic"));
        //addUnit(UnitManager.GetInstance.getUnit("Assaulter"));
        //addUnit(UnitManager.GetInstance.getUnit("Grenadier"));
        //addUnit(UnitManager.GetInstance.getUnit("AirAssaulter"));
        //addUnit(UnitManager.GetInstance.getUnit("AirInterceptor"));
        //addUnit(UnitManager.GetInstance.getUnit("ArmoredSoldier"));
        //addUnit(UnitManager.GetInstance.getUnit("Ranger"));
        //addUnit(UnitManager.GetInstance.getUnit("ReinforcedCombat"));
        
        //영웅
        //addUnit(UnitManager.GetInstance.getUnit("Lizty"));

        //기본 지휘관 삽입
        if(!Prep.isAllCommander)
            addCommanderCard(CommanderManager.GetInstance.getCommanderCard("Cmd_Raty", 1));
//        addCommanderCard(CommanderManager.GetInstance.getCommanderCard("Cmd_Squirel", 1));
//        addCommanderCard(CommanderManager.GetInstance.getCommanderCard("Commander01", 1));
//        addCommanderCard(CommanderManager.GetInstance.getCommanderCard("Commander02", 1));
//        addCommanderCard(CommanderManager.GetInstance.getCommanderCard("Commander03", 1));
//        addCommanderCard(CommanderManager.GetInstance.getCommanderCard("Commander04", 1));


        for (int i = 0; i < (int)Enum.GetValues(typeof(TYPE_FORCE)).Length; i++)
        {
            m_unitWaitDic.Add((TYPE_FORCE)i, new List<string>());
        }

        m_lastTime = DateTime.UtcNow;

    }

    


    /// <summary>
    /// 유닛 삽입하기
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="level"></param>
    /// <returns>양수 - 현재 레벨 / 음수 : 남은 경험치</returns>
    public int addUnit(Unit unit, int level = 1)
    {
        //유닛이 있으면
        //경험치 삽입
        //
        if (m_unitCardDic.ContainsKey(unit.key))
        {
            return m_unitCardDic[unit.key].addExperiance(level * 100);
        }
        //유닛이 없으면
        else
        {
            m_unitCardDic.Add(unit.key, new UnitCard(unit));
            return 1;
        }        

    }

    /// <summary>
    /// 유닛 추가하기
    /// </summary>
    /// <param name="unitCard"></param>
    public void addUnitCard(UnitCard unitCard)
    {
        addUnit(unitCard.unit, unitCard.level);
    }

    /// <summary>
    /// 지휘관 카드 추가
    /// </summary>
    /// <param name="commanderCard"></param>
    public void addCommanderCard(CommanderCard commanderCard)
    {
        addCommanderCard(commanderCard.commander, commanderCard.level);
    }

    /// <summary>
    /// 지휘관 추가
    /// </summary>
    /// <param name="commander"></param>
    /// <param name="level"></param>
    public void addCommanderCard(Commander commander, int level = 1)
    {
        if (!m_commanderCardDic.ContainsKey(commander.key))
        {
            //지휘관 추가
            m_commanderCardDic.Add(commander.key, new CommanderCard(commander, level));
        }
        //지휘관이 있으면
        else
        {
            //경험치 추가
            m_commanderCardDic[commander.key].addExperiance(level * 50);
        }
    }

    /// <summary>
    /// 대기 유닛 삽입
    /// </summary>
    /// <param name="unitCard"></param>
    public void addWaitList(TYPE_FORCE typeForce, UnitCard unitCard)
    {


        //세력이 없으면 세력  추가 후 대기유닛 추가
        if (!m_unitWaitDic.ContainsKey(typeForce))
        {
            m_unitWaitDic.Add(typeForce, new List<string>());
        }

        m_unitWaitDic[typeForce].Add(unitCard.key);


//        switch (unitCard.unit.typeUnit)
//        {
//            case Unit.TYPE_UNIT.Hero:
//                m_heroWaitList.Add(unitCard.unit.key);
////                UnityEngine.Debug.LogWarning("card : " + m_heroWaitList.Count);
//                break;
//            case Unit.TYPE_UNIT.Soldier:
//                m_unitWaitList.Add(unitCard.unit.key);
//                break;
//        }
    }

    /// <summary>
    /// 지휘관 교체하기
    /// </summary>
    /// <param name="commanderCard"></param>
    public void changeCommander(CommanderCard commanderCard)
    {
        m_commander = commanderCard.key;
    }

    /// <summary>
    /// 대기 유닛 삭제
    /// </summary>
    /// <param name="unitCard"></param>
    /// <returns></returns>
    public bool removeWaitList(TYPE_FORCE typeForce, UnitCard unitCard)
    {

        //대기중인 유닛키 삭제
        if (m_unitWaitDic.ContainsKey(typeForce))
        {
            return m_unitWaitDic[typeForce].Remove(unitCard.key);
        }



        //switch (unitCard.unit.typeUnit)
        //{
        //    case Unit.TYPE_UNIT.Hero:
        //        if (m_heroWaitList.Contains(unitCard.unit.key))
        //        {
        //            return m_heroWaitList.Remove(unitCard.unit.key);
        //        }
        //        break;
        //    case Unit.TYPE_UNIT.Soldier:
        //        if (m_unitWaitList.Contains(unitCard.unit.key))
        //        {
        //            return m_unitWaitList.Remove(unitCard.unit.key);
        //        }
        //        break;
        //}

        return false;

    }

    /// <summary>
    /// 유닛 카드 가져오기
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public UnitCard getUnitCard(string key)
    {
        if (m_unitCardDic.ContainsKey(key))
        {
            return m_unitCardDic[key];
        }
        return null;
    }


    /// <summary>
    /// 유닛카드 유무
    /// </summary>
    /// <param name="unitkey"></param>
    /// <returns></returns>
    public bool isUnitCard(string unitkey)
    {
        return m_unitCardDic.ContainsKey(unitkey);
    }

    /// <summary>
    /// 영웅 카드 가져오기
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    //public UnitCard getHeroCard(string key)
    //{
    //    if (m_heroCardDic.ContainsKey(key))
    //    {
    //        return m_heroCardDic[key];
    //    }
    //    return null;
    //}

    /// <summary>
    /// 지휘관 카드 가져오기
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public CommanderCard getCommanderCard(string key)
    {
        if (m_commanderCardDic.ContainsKey(key))
        {
            return m_commanderCardDic[key];
        }
        return null;
    }

    /// <summary>
    /// 현재 지휘관 카드 가져오기
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public CommanderCard getNowCommanderCard()
    {
        return getCommanderCard(nowCommander);
    }

    /// <summary>
    /// 세력에 대한 첫번째 지휘관 가져오기
    /// </summary>
    /// <param name="typeForce"></param>
    /// <returns></returns>
    public CommanderCard getFirstCommanderCard(TYPE_FORCE typeForce)
    {
        return m_commanderCardDic.Values.Where(com => (typeForce & com.typeForce) == com.typeForce).FirstOrDefault();
    }

    /// <summary>
    /// 유닛 수 가져오기
    /// </summary>
    /// <param name="typeUnit"></param>
    /// <returns></returns>
    //public int getUnitCount(Unit.TYPE_UNIT typeUnit)
    //{
    //    switch (typeUnit)
    //    {
    //        case Unit.TYPE_UNIT.Hero:
    //            return m_heroCardDic.Count;
    //        case Unit.TYPE_UNIT.Soldier:
    //            return m_unitCardDic.Count;
    //    }
    //    return 0;
    //}

    /// <summary>
    /// 유닛 최대 수 가져오기
    /// </summary>
    /// <param name="typeUnit"></param>
    /// <returns></returns>
    public int getUnitMaxCount(Unit.TYPE_UNIT typeUnit) 
    {
        switch (typeUnit)
        {
            case Unit.TYPE_UNIT.Hero:
                return Prep.defaultHeroCount + (int)Account.GetInstance.accDevelop.getDevelopValue(Develop.TYPE_DEVELOP_VALUE_GROUP.HeroBarracks);
            case Unit.TYPE_UNIT.Soldier:
                return Prep.defaultSoldierCount + (int)Account.GetInstance.accDevelop.getDevelopValue(Develop.TYPE_DEVELOP_VALUE_GROUP.SoldierBarracks);
        }
        return 0;
    }

    /// <summary>
    /// 지휘관 수 가져오기
    /// </summary>
    /// <returns></returns>
    public int getCommanderCount()
    {
        return m_commanderCardDic.Count;
    }
    
    /// <summary>
    /// 영웅, 병사 통합 키 제공
    /// </summary>
    /// <returns></returns>
    //public string[] getUnitKeys()
    //{
    //   return m_heroWaitList.Union(m_unitWaitList).ToArray<string>();
    //}


    /// <summary>
    /// 총 인구수 가져오기 - 영웅 합산
    /// </summary>
    /// <param name="typeForce"></param>
    /// <returns></returns>
    public int getUnitTotalPopulation(TYPE_FORCE typeForce)
    {
        int count = 0;
        if (m_unitWaitDic.ContainsKey(typeForce))
        {
            foreach (string key in m_unitWaitDic[typeForce])
            {
                UnitCard unitCard = getUnitCard(key);
                if (unitCard != null)
                {
                    count += unitCard.population;
                }
            }
        }
        return count;
    }

    /// <summary>
    /// 총 인구수 가져오기
    /// </summary>
    /// <param name="typeForce"></param>
    /// <param name="typeUnit"></param>
    /// <returns></returns>
    public int getUnitTotalPopulation(TYPE_FORCE typeForce, Unit.TYPE_UNIT typeUnit)
    {
        int count = 0;
        if (m_unitWaitDic.ContainsKey(typeForce))
        {
            foreach (string key in m_unitWaitDic[typeForce])
            {
                UnitCard unitCard = getUnitCard(key);
                if (unitCard != null)
                {
                    if(unitCard.typeUnit == typeUnit)
                        count += unitCard.population;
                }
            }
        }
        return count;
    }

 

    /// <summary>
    /// 모집 타임 초기화
    /// </summary>
    public void setRefleshTime()
    {
        m_lastTime = DateTime.UtcNow;
    }

    /// <summary>
    /// 모집 타임 보기
    /// </summary>
    /// <returns></returns>
    public string nowTime()
    {
        if (nowTimeRate() >= 1f)
            return "";

        TimeSpan nowTime = m_getTime.Subtract(DateTime.UtcNow.Subtract(m_lastTime));
        return string.Format("\n({0:d2}:{1:d2})", nowTime.Minutes, nowTime.Seconds);
    }

    /// <summary>
    /// 타임 레이트
    /// 0~1f
    /// </summary>
    /// <returns></returns>
    public float nowTimeRate()
    {
        return (float)(DateTime.UtcNow.Subtract(m_lastTime).TotalSeconds / m_getTime.TotalSeconds);
    }

    /// <summary>
    /// 유닛 허용 최대 레벨 가져오기
    /// </summary>
    /// <param name="typeUnit"></param>
    /// <returns></returns>
    public int getUnitMaxLevel(Unit.TYPE_UNIT typeUnit)
    {
        switch (typeUnit)
        {
            case Unit.TYPE_UNIT.Soldier:
                return Prep.defaultSoldierLevel + (int)Account.GetInstance.accDevelop.getDevelopValue(Develop.TYPE_DEVELOP_VALUE_GROUP.SoldierLv);
            case Unit.TYPE_UNIT.Hero:
                return Prep.defaultHeroLevel + (int)Account.GetInstance.accDevelop.getDevelopValue(Develop.TYPE_DEVELOP_VALUE_GROUP.HeroLv);
        }
        return 0;
    }

    /// <summary>
    /// 지휘관 최고 레벨 가져오기
    /// </summary>
    /// <returns></returns>
    public int getHighCommanderLevel()
    {
        return m_commanderCardDic.Values.Max(card => card.level);
    }

    /// <summary>
    /// 유닛 최고 레벨 가져오기
    /// </summary>
    /// <param name="typeUnit"></param>
    /// <returns></returns>
    public int getHighUnit(Unit.TYPE_UNIT typeUnit)
    {
        return m_unitCardDic.Values.Where(card => card.typeUnit == typeUnit).Max(card => card.level);
    }


    #region ############################## 직렬화 ###################################

    /// <summary>
    /// 직렬화 하기
    /// </summary>
    /// <returns></returns>
    public AccountUnitSerial getSerial()
    {
        AccountUnitSerial accSerial = new AccountUnitSerial();

        foreach (CommanderCard card in m_commanderCardDic.Values)
        {
            AccountCommanderCardSerial cardSerial = card.getSerial();
            accSerial.commanderCardList.Add(cardSerial);
        }

        foreach (UnitCard card in m_unitCardDic.Values)
        {
            AccountUnitCardSerial cardSerial = card.getSerial();
            accSerial.unitCardList.Add(cardSerial);
        }

        for (int i = 0; i < Enum.GetValues(typeof(TYPE_FORCE)).Length; i++)
        {
            if (m_unitWaitDic.ContainsKey((TYPE_FORCE)i))
            {
                accSerial.unitWaitList.Add(m_unitWaitDic[(TYPE_FORCE)i]);
            }
        }

//        accSerial.totalHeroEngage = m_totalHeroEngage;
//        accSerial.totalSoldierEngage = m_totalSoldierEngage;

        accSerial.lastTime = m_lastTime.ToString();

        return accSerial;
    }

    /// <summary>
    /// 직렬화된 데이터 변환하기
    /// </summary>
    /// <param name="accSerial"></param>
    public AccountUnit(AccountUnitSerial accSerial)
    {
        //유닛 역직렬화
        foreach (AccountUnitCardSerial cardSerial in accSerial.unitCardList)
        {
            UnitCard card = UnitManager.GetInstance.getUnitCard(cardSerial);
            if (card != null) m_unitCardDic.Add(card.key, card);
            else Prep.LogError(cardSerial.key, "유닛을 가져오지 못했습니다.", GetType());
        }

        //지휘관 역직렬화
        foreach (AccountCommanderCardSerial cardSerial in accSerial.commanderCardList)
        {
            CommanderCard card = CommanderManager.GetInstance.getCommanderCard(cardSerial);
            if (card != null) m_commanderCardDic.Add(card.key, card);
            else Prep.LogError(cardSerial.key, "유닛을 가져오지 못했습니다.", GetType());
        }

        //대기유닛 역직렬화
        for (int i = 0; i < accSerial.unitWaitList.Count; i++)
        {
            m_unitWaitDic.Add((TYPE_FORCE)i, accSerial.unitWaitList[i]);
        }

//        m_totalHeroEngage = accSerial.totalHeroEngage;
//        m_totalSoldierEngage = accSerial.totalSoldierEngage;

        DateTime.TryParse(accSerial.lastTime, out m_lastTime);
    }
    #endregion

}

